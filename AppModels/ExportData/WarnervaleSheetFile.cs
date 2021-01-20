using System.Collections.Generic;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Openings;

namespace AppModels.ExportData
{
    public class WarnervaleSheetFile
    {
        public string SheetName { get; set; }
        public string SheetFile { get; set; }

        public Dictionary<string, WarnervaleWallDict> WarnervaleWallDiscs
        {
            get;
            set;
        }
        //public List<WarnervaleWallDict> WallWorksheetDicts { get; set; }
        public WarnervaleSheetFile(string sheetName,string sheetFile,List<WallBase> walls,List<Opening> openings)
        {
            SheetName = sheetName;
            SheetFile = sheetFile;
            WarnervaleWallDiscs = new Dictionary<string, WarnervaleWallDict>();
            var floorWallList = new List<WallBase>();
            var rakeWallList = new List<WallBase>();
            var upperWallList = new List<WallBase>();
            foreach (var wallBase in walls)
            {
                if (wallBase.WallType.IsRaked)
                {
                    rakeWallList.Add(wallBase);
                }
                else if (wallBase.IsExportToUpper)
                {
                    upperWallList.Add(wallBase);
                }
                else
                {
                    floorWallList.Add(wallBase);
                }
            }

            if (floorWallList.Count>0)
            {
                WarnervaleWallDiscs.Add("Normal Wall2D", new WarnervaleWallDict(floorWallList, openings));
            }

            if (rakeWallList.Count>0)
            {
                WarnervaleWallDiscs.Add("Raked Wall2D", new WarnervaleWallDict(rakeWallList, openings));
            }

            if (upperWallList.Count>0)
            {
                WarnervaleWallDiscs.Add("Upper Wall2D", new WarnervaleWallDict(upperWallList,openings));
            }
        }

        public void ExportDataToExcel(IJob jobModel,LevelWall level,bool includeFloorName)
        {
            var excel = new Microsoft.Office.Interop.Excel.Application();
            var workbook = excel.Workbooks.Open(SheetFile+".xlsm");
            var sheets = workbook.Worksheets;
            Microsoft.Office.Interop.Excel.Worksheet infoSheet = sheets["Job Particulars Input"];
            ExportWallInfoToExcel(infoSheet,jobModel,level.LevelName,level,includeFloorName);
            var isUpperLevel = level == jobModel.Levels[jobModel.Levels.Count - 1];
            ExportWallDataToExcel(sheets,isUpperLevel);
            workbook.Save();
            excel.Quit();
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excel);
        }
        private void ExportWallInfoToExcel(Microsoft.Office.Interop.Excel.Worksheet infoSheet,IJob jobModel, string levelName = "", LevelWall level = null, bool includeFloorName = false)
        {
            infoSheet.Range["B1"].Value = jobModel.Info.BuilderName;
            infoSheet.Range["B2"].Value = jobModel.Info.JobAddress;
            var subAddress = jobModel.Info.SubAddress;
            if (!string.IsNullOrEmpty(jobModel.Info.UnitNumber))
            {
                subAddress = subAddress + "-Unit " + jobModel.Info.UnitNumber;
            }

            if (includeFloorName)
            {
                subAddress = subAddress + "(" + levelName + ")";
            }

            if (SheetName.Contains("Sheet"))
            {
                subAddress = subAddress.Replace(")", " - " + SheetName + ")");
            }
            infoSheet.Range["B3"].Value = subAddress;

            infoSheet.Range["F6"].Value = jobModel.Info.JobNumber;
            infoSheet.Range["F7"].Formula = "=TODAY()";
            infoSheet.Range["F19"].Formula = jobModel.Info.WindRate;
            if (jobModel.Info.RoofMaterial == "Tiles")
            {
                infoSheet.Range["A6"].Value = "LOWER STOREY TILES";
                infoSheet.Range["A15"].Value = "SINGLE & UPPER STOREY TILES";
                infoSheet.Range["C25"].Value = "Not Req'd";
                infoSheet.Range["C27"].Value = "Not Fitted";
            }
            else
            {
                infoSheet.Range["A6"].Value = "LOWER STOREY SHEET";
                infoSheet.Range["A15"].Value = "SINGLE & UPPER STOREY SHEET";
                infoSheet.Range["C25"].Value = "Req'd";
                infoSheet.Range["C27"].Value = "Fitted";

            }

            if (jobModel.Levels.Count>1)
            {
                var lowerLevel = level;
                var upperLevel = jobModel.Levels[jobModel.Levels.Count - 1];
                // lower
                infoSheet.Range["C7"].Value = lowerLevel.LevelInfo.ExternalWallThickness + "x" + lowerLevel.LevelInfo.ExternalWallTimberDepth + " " + lowerLevel.LevelInfo.ExternalWallTimberGrade;
                infoSheet.Range["C8"].Value = lowerLevel.LevelInfo.InternalWallThickness + "x" + lowerLevel.LevelInfo.InternalWallTimberDepth + " " + lowerLevel.LevelInfo.InternalWallTimberGrade;
                infoSheet.Range["C11"].Value = lowerLevel.LevelInfo.WallHeight;
                //Upper
                infoSheet.Range["C16"].Value = upperLevel.LevelInfo.ExternalWallThickness + "x" + upperLevel.LevelInfo.ExternalWallTimberDepth + " " + upperLevel.LevelInfo.ExternalWallTimberGrade;
                infoSheet.Range["C17"].Value = upperLevel.LevelInfo.InternalWallThickness + "x" + upperLevel.LevelInfo.InternalWallTimberDepth + " " + upperLevel.LevelInfo.InternalWallTimberGrade;
                infoSheet.Range["C20"].Value = upperLevel.LevelInfo.WallHeight;
            }
            else
            {
                var singleLevel = level;
                infoSheet.Range["C16"].Value = singleLevel.LevelInfo.ExternalWallThickness + "x" + singleLevel.LevelInfo.ExternalWallTimberDepth + " " + singleLevel.LevelInfo.ExternalWallTimberGrade;
                infoSheet.Range["C17"].Value = singleLevel.LevelInfo.InternalWallThickness + "x" + singleLevel.LevelInfo.InternalWallTimberDepth + " " + singleLevel.LevelInfo.InternalWallTimberGrade;
                infoSheet.Range["C20"].Value = singleLevel.LevelInfo.WallHeight;
            }

        }

        private void ExportWallDataToExcel(Microsoft.Office.Interop.Excel.Sheets worksheets,bool isUpperLevel)
        {
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
            bool inputToRakedWall = false;
            foreach (var warnervaleWallDisc in WarnervaleWallDiscs)
            {
                if (warnervaleWallDisc.Key== "Normal Wall2D")
                {
                    if (isUpperLevel)
                    {
                        worksheet = worksheets["Input(Single-UpperStorey)"];
                        inputToRakedWall = false;
                    }
                    else
                    {
                        worksheet = worksheets["Input(LowerStorey)"];
                        inputToRakedWall = false;
                    }
                }
                else if(warnervaleWallDisc.Key == "Upper Wall2D")
                {
                    worksheet = worksheets["Input(Single-UpperStorey)"];
                    inputToRakedWall = false;
                }
                else
                {
                    worksheet = worksheets["Input Raking Walls"];
                    inputToRakedWall = true;
                }
                warnervaleWallDisc.Value.ExportWallDataToExcel(worksheet,inputToRakedWall);
            }
            
        }
    }
}
