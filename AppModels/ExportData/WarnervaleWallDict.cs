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
                            wallDataRange.StudSize = levelSheet.Range["C9"].Offset[offsetValue,0];
                            wallDataRange.TopPlate = levelSheet.Range["F14"].Offset[offsetValue, 0];
                            wallDataRange.BottomPlate = levelSheet.Range["F15"].Offset[offsetValue, 0];
                            wallDataRange.RibbonPlate = levelSheet.Range["F16"].Offset[offsetValue, 0];
                            wallDataRange.WallSpacing = levelSheet.Range["C10"].Offset[offsetValue, 0];
                            wallDataRange.WallLength = levelSheet.Range["G6"].Offset[offsetValue, 0];
                            wallDataRange.WallHeight = levelSheet.Range["G7"].Offset[offsetValue, 0];
                            wallDataRange.BeamPockets = levelSheet.Range["G8"].Offset[offsetValue, 0];
                            wallDataRange.Corners = levelSheet.Range["G9"].Offset[offsetValue, 0];
                            wallDataRange.TCorners = levelSheet.Range["G10"].Offset[offsetValue, 0];
                            wallDataRange.Supports = levelSheet.Range["G11"].Offset[offsetValue, 0];
                            wallDataRange.RoofPitch = levelSheet.Range["J6"].Offset[offsetValue, 0];
                            wallDataRange.NumberOfSameWall = levelSheet.Range["L6"].Offset[offsetValue, 0];
                            wallDataRange.StartWindowHeight = levelSheet.Range["I16"].Offset[offsetValue, 0];
                            wallDataRange.StartWindowWidth = levelSheet.Range["J16"].Offset[offsetValue, 0];
                            wallDataRange.StartWindowQty = levelSheet.Range["L16"].Offset[offsetValue, 0];
                            wallDataRange.StartWindowTrussSpan = levelSheet.Range["M16"].Offset[offsetValue, 0];
                            wallDataRange.StartDoorWidth = levelSheet.Range["J35"].Offset[offsetValue, 0];
                            wallDataRange.StartDoorQty = levelSheet.Range["L35"].Offset[offsetValue, 0];
                            wallDataRange.StartDoorTrussSpan = levelSheet.Range["M35"].Offset[offsetValue, 0];
                            wallDataRange.WetAreaStud = levelSheet.Range["F39"].Offset[offsetValue, 0];
                            exportWallData.ExportWallDataToExcel(wallDataRange);
                            offsetValue += 43;
                        }
                    }
                    else
                    {
                        offsetValue = 0;
                        foreach (var exportWallData in wallKey.Value)
                        {
                            wallDataRange.StudSize = levelSheet.Range["C9"].Offset[offsetValue, 0];
                            wallDataRange.TopPlate = levelSheet.Range["F14"].Offset[offsetValue, 0];
                            wallDataRange.BottomPlate = levelSheet.Range["F15"].Offset[offsetValue, 0];
                            wallDataRange.RibbonPlate = levelSheet.Range["F16"].Offset[offsetValue, 0];
                            wallDataRange.WallSpacing = levelSheet.Range["C10"].Offset[offsetValue, 0];
                            wallDataRange.WallLength = levelSheet.Range["C13"].Offset[offsetValue, 0];
                            wallDataRange.WallHeight = levelSheet.Range["G7"].Offset[offsetValue, 0];
                            wallDataRange.BeamPockets = levelSheet.Range["G8"].Offset[offsetValue, 0];
                            wallDataRange.Corners = levelSheet.Range["G9"].Offset[offsetValue, 0];
                            wallDataRange.TCorners = levelSheet.Range["G10"].Offset[offsetValue, 0];
                            wallDataRange.Supports = levelSheet.Range["G11"].Offset[offsetValue, 0];
                            wallDataRange.StartWindowHeight = levelSheet.Range["I16"].Offset[offsetValue, 0];
                            wallDataRange.StartWindowWidth = levelSheet.Range["J16"].Offset[offsetValue, 0];
                            wallDataRange.StartWindowQty = levelSheet.Range["L16"].Offset[offsetValue, 0];
                            wallDataRange.StartWindowTrussSpan = levelSheet.Range["M16"].Offset[offsetValue, 0];
                            wallDataRange.StartDoorWidth = levelSheet.Range["J35"].Offset[offsetValue, 0];
                            wallDataRange.StartDoorQty = levelSheet.Range["L35"].Offset[offsetValue, 0];
                            wallDataRange.StartDoorTrussSpan = levelSheet.Range["M35"].Offset[offsetValue, 0];
                            wallDataRange.WetAreaStud = levelSheet.Range["F39"].Offset[offsetValue, 0];
                            exportWallData.ExportWallDataToExcel(wallDataRange);
                            offsetValue += 43;
                        }
                        
                    }
                }
                else
                {

                    if (inputToRakedWall)
                    {
                        offsetValue = 0;
                        foreach (var exportWallData in wallKey.Value)
                        {
                            wallDataRange.StudSize = levelSheet.Range["C181"].Offset[offsetValue, 0];
                            wallDataRange.TopPlate = levelSheet.Range["F185"].Offset[offsetValue, 0];
                            wallDataRange.BottomPlate = levelSheet.Range["F186"].Offset[offsetValue, 0];
                            wallDataRange.WallSpacing = levelSheet.Range["C1182"].Offset[offsetValue, 0];
                            wallDataRange.WallLength = levelSheet.Range["G179"].Offset[offsetValue, 0];
                            wallDataRange.WallHeight = levelSheet.Range["G180"].Offset[offsetValue, 0];
                            wallDataRange.TCorners = levelSheet.Range["G181"].Offset[offsetValue, 0];
                            wallDataRange.Corners = levelSheet.Range["G182"].Offset[offsetValue, 0];
                            wallDataRange.RoofPitch = levelSheet.Range["J179"].Offset[offsetValue, 0];
                            wallDataRange.NumberOfSameWall = levelSheet.Range["L179"].Offset[offsetValue, 0];
                            wallDataRange.StartDoorWidth = levelSheet.Range["J186"].Offset[offsetValue, 0];
                            wallDataRange.StartDoorQty = levelSheet.Range["K186"].Offset[offsetValue, 0];
                            wallDataRange.WetAreaStud = levelSheet.Range["F198"].Offset[offsetValue, 0];
                            wallDataRange.BathCheckout = levelSheet.Range["L198"].Offset[offsetValue, 0];
                            exportWallData.ExportWallDataToExcel(wallDataRange);
                            offsetValue += 24;
                        }
                    }
                    else
                    {
                        offsetValue = 0;
                        foreach (var exportWallData in wallKey.Value)
                        {
                            wallDataRange.StudSize = levelSheet.Range["C181"].Offset[offsetValue, 0];
                            wallDataRange.TopPlate = levelSheet.Range["F185"].Offset[offsetValue, 0];
                            wallDataRange.BottomPlate = levelSheet.Range["F186"].Offset[offsetValue, 0];
                            wallDataRange.WallSpacing = levelSheet.Range["C1182"].Offset[offsetValue, 0];
                            wallDataRange.WallLength = levelSheet.Range["C185"].Offset[offsetValue, 0];
                            wallDataRange.WallHeight = levelSheet.Range["G180"].Offset[offsetValue, 0];
                            wallDataRange.TCorners = levelSheet.Range["G181"].Offset[offsetValue, 0];
                            wallDataRange.Corners = levelSheet.Range["G182"].Offset[offsetValue, 0];
                            wallDataRange.StartDoorWidth = levelSheet.Range["J186"].Offset[offsetValue, 0];
                            wallDataRange.StartDoorQty = levelSheet.Range["K186"].Offset[offsetValue, 0];
                            wallDataRange.WetAreaStud = levelSheet.Range["F198"].Offset[offsetValue, 0];
                            wallDataRange.BathCheckout = levelSheet.Range["L198"].Offset[offsetValue, 0];
                            exportWallData.ExportWallDataToExcel(wallDataRange);
                            offsetValue += 24;
                        }

                    }

                }
            }
        }

    }
}
