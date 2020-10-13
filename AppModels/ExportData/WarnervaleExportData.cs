using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using Excel = Microsoft.Office.Interop.Excel;

namespace AppModels.ExportData
{
    public class WarnervaleExportData
    {
        public LevelWall Level { get; set; }
        public List<WarnervaleSheetFile> Sheets { get; set; }
        
        public WarnervaleExportData(LevelWall level,string fileSave)
        {
            Level = level;
            Sheets = new List<WarnervaleSheetFile>();
            if (CheckForSecondSheetFile(level))
            {
                var file1 = fileSave + "-sheet 1";
                var file2 = fileSave + "-sheet 2";
                var listWallSheet1 = new List<WallBase>();
                var listWallSheet2 = new List<WallBase>();
                foreach (var levelWallLayer in level.WallLayers)
                {
                    if (levelWallLayer.TypeId > 4)
                    {
                        listWallSheet2.Add(levelWallLayer);
                    }
                    else
                        listWallSheet1.Add(levelWallLayer);
                }
                var sheet1 = new WarnervaleSheetFile("Sheet 1",file1,listWallSheet1,level.Openings.ToList());
                var sheet2 = new WarnervaleSheetFile("Sheet 2",file2,listWallSheet2,level.Openings.ToList());
                Sheets.Add(sheet1);
                Sheets.Add(sheet2);
            }
            else
            {
                var sheet = new WarnervaleSheetFile("SingleFile",fileSave,level.WallLayers.ToList(),level.Openings.ToList());
                Sheets.Add(sheet);
            }
        }

        private bool CheckForSecondSheetFile(LevelWall level)
        {
            var maximumWallType = level.WallLayers.Select(wallLayer => wallLayer.TypeId).Prepend(0).Max();

            return maximumWallType > 4;
        }

        public void SaveFile(Excel.Workbook workbook)
        {
            foreach (var warnervaleSheetFile in Sheets)
            {
                try
                {
                    workbook.SaveAs(warnervaleSheetFile.SheetFile);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Please closed your excel file before export data");
                    return;
                }
                

            }
        }

        public void ExportToExcel(IJob jobModel)
        {
            bool includeFloorName = jobModel.Levels.Count>1;
            foreach (var warnervaleSheetFile in Sheets)
            {
                warnervaleSheetFile.ExportDataToExcel(jobModel,Level,includeFloorName);
            }
        }

    }
}
