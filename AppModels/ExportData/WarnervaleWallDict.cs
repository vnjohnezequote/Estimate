using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Openings;

namespace AppModels.ExportData
{
    public class WarnervaleWallDict
    {
        public Dictionary<string, List<ExportWallData>> WallDictionary { get; set; }

        public WarnervaleWallDict(List<WallBase> walls,List<Opening> openings)
        {
            WallDictionary = new Dictionary<string, List<ExportWallData>>();
            var extWalls = new List<ExportWallData>();
            var intWalls=new List<ExportWallData>();
            foreach (var wallBase in walls)
            {
                var openingList = openings.Where(opening => opening.WallReference == wallBase).ToList();
                if (wallBase.WallType.IsLoadBearingWall)
                {
                    extWalls.Add(new ExportWallData(wallBase,openingList));
                }
                else
                {
                    intWalls.Add(new ExportWallData(wallBase,openingList));
                }
            }

            if (extWalls.Count>0)
            {
                WallDictionary.Add("Ext", extWalls);
            }

            if (intWalls.Count>0)
            {
                WallDictionary.Add("Int", intWalls);
            }
            
        }

        public void ExportWallDataToExcel(Microsoft.Office.Interop.Excel.Worksheet levelSheet, bool inputToRakedWall = false)
        {
            int offsetValue = 0;
            var wallDataRange =new WarnervaleExcelDataRangeIndex();
            //var frameSize = levelSheet.Range["C9"];

            foreach (var wallKey in WallDictionary)
            {
                if (wallKey.Key=="Ext")
                {
                    if (inputToRakedWall)
                    {
                        offsetValue = 0;
                        foreach (var exportWallData in wallKey.Value)
                        {
                            exportWallData.ExportWallDataToExcel(levelSheet,offsetValue,wallDataRange);
                            offsetValue += 43;
                        }
                    }
                    else
                    {
                        offsetValue = 0;
                        foreach (var exportWallData in wallKey.Value)
                        {
                            exportWallData.ExportWallDataToExcel(levelSheet,offsetValue,wallDataRange);
                            offsetValue += 43;
                        }
                        
                    }
                }
                else
                {
                    if (inputToRakedWall)
                    {
                        
                    }
                    else
                    {
                        
                    }
                   
                }
            }
        }

    }
}
