using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppModels.Enums;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Openings;
using devDept.Geometry;

namespace AppModels.ExportData
{
    public class ExportWallData
    {
        public string WallType { get; set; }
        public bool IsLoadbearingWall {get; set; }
        public bool IsRakedWall { get; set; }
        public int WallTypeId { get; set; }
        public int WallLength { get; set; }
        public int WallHeight { get; set; }
        public string StudSize { get; set; }
        public string TopPlate { get; set; }
        public string RibbonPlate { get; set; }
        public string BottomPlate { get; set; }
        public int WallSpacing { get; set; }
        public int NumberOfSameWalls { get; set; }
        public int BeamPockets { get; set; }
        public int Corners { get; set; }
        public int TCorners { get; set; }
        public int InwallSupport { get; set; }
        public int BathCheckOuts { get; set; }
        public int WallRunLength { get; set; }
        public int AdditionalStuds { get; set; }

        public ObservableCollection<ExportDoorData> Doors { get; set; }
        public ObservableCollection<ExportDoorData> Windows { get; set; }

        public ExportWallData(WallBase wall,List<Opening> openings)
        {
            WallType = wall.WallType.AliasName;
            IsLoadbearingWall = wall.WallType.IsLoadBearingWall;
            IsRakedWall = wall.WallType.IsRaked;
            WallTypeId = wall.TypeId;
            NumberOfSameWalls = wall.NumberOfSameWall;
            BeamPockets = wall.BeamPockets;
            Corners = wall.Corners;
            TCorners = wall.TCorners;
            InwallSupport = wall.InWallSupports;
            BathCheckOuts = wall.BathCheckout;
            if (wall.WallType.IsRaked)
            {
                if (NumberOfSameWalls == 0)
                {
                    NumberOfSameWalls = 1;
                }
                WallLength = (int) WallLength * 1000 / NumberOfSameWalls;
                WallHeight = (int)Math.Ceiling(wall.FinalWallHeight-WallLength* Math.Tan(Utility.DegToRad(wall.CeilingPitch)));
            }
            else
            {
                WallLength = (int)wall.WallLength;
                WallHeight = wall.FinalWallHeight;
            }
            
            
            StudSize = wall.Stud.SizeGrade;
            TopPlate = wall.TopPlate.SizeGrade;
            RibbonPlate = wall.RibbonPlate.SizeGrade;
            BottomPlate = wall.BottomPlate.SizeGrade;
            WallSpacing = wall.WallSpacing;
            AdditionalStuds = (int)Math.Ceiling(wall.WetAreaLength / 0.45 - wall.WetAreaLength / 0.6);
            Doors = new ObservableCollection<ExportDoorData>();
            Windows = new ObservableCollection<ExportDoorData>();
            InitializerOpeningList(openings);
        }

        private void InitializerOpeningList(List<Opening> openings)
        {
            foreach (var opening in openings)
            {
                var doorIndex = 0;
                if (opening.OpeningType ==OpeningType.Door )
                {
                    if (DoorContaintDoor(opening,ref doorIndex))
                    {
                        Doors[doorIndex].Qty++;
                    }
                    else
                    {
                        var exportDoor = new ExportDoorData(opening);
                        exportDoor.Qty = 1;
                        Doors.Add(exportDoor);

                    }
                }
                else
                {
                    if (WindowContainWindow(opening,ref doorIndex))
                    {
                        Windows[doorIndex].Qty++;
                    }
                    else
                    {
                        var exportDoor = new ExportDoorData(opening);
                        exportDoor.Qty = 1;
                        Windows.Add(exportDoor);
                    }
                }
            }
        }
        public bool DoorContaintDoor(Opening door, ref int doorIndex)
        {
            foreach (var exportDoorData in Doors)
            {
                if (exportDoorData.Width == door.Width && exportDoorData.Height == door.Height)
                {
                    doorIndex = Doors.IndexOf(exportDoorData);
                    return true;
                }
            }

            return false;
        }
        public bool WindowContainWindow(Opening window,ref int windowIndex)
        {
            foreach (var exportDoorData in Windows)
            {
                if (exportDoorData.Width != window.Width || exportDoorData.Height != window.Height) continue;
                windowIndex = Doors.IndexOf(exportDoorData);
                return true;
            }
            return false;
        }

        public void ExportWallDataToExcel(Microsoft.Office.Interop.Excel.Worksheet levelSheet,int offsetValue, WarnervaleExcelDataRangeIndex wallDataRange)
        {
            ExportDoorDatatoExcel(levelSheet,wallDataRange,offsetValue);
        }

        public void ExportDoorDatatoExcel(Microsoft.Office.Interop.Excel.Worksheet levelSheet, WarnervaleExcelDataRangeIndex wallDataRange, int offsetValue)
        {

        }

    }
}
