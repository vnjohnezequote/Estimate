using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        public int AdditionalStuds { get; set; }
        public double RoofPitch { get; set; }

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
            RoofPitch = wall.CeilingPitch;
            if (wall.WallType.IsRaked)
            {
                if (NumberOfSameWalls == 0)
                {
                    NumberOfSameWalls = 1;
                }
                WallLength = (int) (WallLength * 1000 / NumberOfSameWalls);
                WallHeight = (int)Math.Ceiling(wall.FinalWallHeight-WallLength* Math.Tan(Utility.DegToRad(wall.CeilingPitch)));
            }
            else
            {
                WallLength = (int)wall.WallLength;
                WallHeight = wall.FinalWallHeight;
            }
            
            
            StudSize = wall.Stud.SizeGrade;
            TopPlate = wall.TopPlate.SizeGrade;
            if (IsLoadbearingWall)
            {
                RibbonPlate = wall.RibbonPlate.SizeGrade;
            }
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

        public void ExportWallDataToExcel( WarnervaleExcelDataRangeIndex wallDataRange)
        {
            if (IsLoadbearingWall)
            {
                if (IsRakedWall)
                {
                    wallDataRange.StudSize.Value = StudSize;
                    wallDataRange.WallSpacing.Value = WallSpacing;
                    wallDataRange.WallLength.Value = WallLength;
                    wallDataRange.WallHeight.Value = WallHeight;
                    wallDataRange.BeamPockets.Value = BeamPockets;
                    wallDataRange.Corners.Value = Corners;
                    wallDataRange.TCorners.Value = TCorners;
                    wallDataRange.Supports.Value = InwallSupport;
                    wallDataRange.TopPlate.Value = TopPlate;
                    wallDataRange.BottomPlate.Value = BottomPlate;
                    wallDataRange.RibbonPlate.Value = RibbonPlate;
                    wallDataRange.WetAreaStud.Value = AdditionalStuds;
                    wallDataRange.RoofPitch.Value = RoofPitch;
                    wallDataRange.NumberOfSameWall.Value = NumberOfSameWalls;
                    ExportDoorDatatoExcel( wallDataRange);
                }
                else
                {
                    wallDataRange.StudSize.Value = StudSize;
                    wallDataRange.WallSpacing.Value = WallSpacing;
                    wallDataRange.WallLength.Value = WallLength;
                    wallDataRange.WallHeight.Value = WallHeight;
                    wallDataRange.BeamPockets.Value = BeamPockets;
                    wallDataRange.Corners.Value = Corners;
                    wallDataRange.TCorners.Value = TCorners;
                    wallDataRange.Supports.Value = InwallSupport;
                    wallDataRange.TopPlate.Value = TopPlate;
                    wallDataRange.BottomPlate.Value = BottomPlate;
                    wallDataRange.RibbonPlate.Value = RibbonPlate;
                    wallDataRange.WetAreaStud.Value = AdditionalStuds;
                    ExportDoorDatatoExcel(wallDataRange);   
                }
            }
            else
            {
                if (IsRakedWall)
                {
                    wallDataRange.StudSize.Value = StudSize;
                    wallDataRange.WallSpacing.Value = WallSpacing;
                    wallDataRange.WallLength.Value = WallLength;
                    wallDataRange.WallHeight.Value = WallHeight;
                    wallDataRange.Corners.Value = Corners;
                    wallDataRange.TCorners.Value = TCorners;
                    wallDataRange.TopPlate.Value = TopPlate;
                    wallDataRange.BottomPlate.Value = BottomPlate;
                    wallDataRange.WetAreaStud.Value = AdditionalStuds;
                    wallDataRange.BathCheckout.Value = BathCheckOuts;
                    wallDataRange.RoofPitch.Value = RoofPitch;
                    wallDataRange.NumberOfSameWall.Value = NumberOfSameWalls;
                    ExportDoorDatatoExcel(wallDataRange);
                }
                else
                {
                    wallDataRange.StudSize.Value = StudSize;
                    wallDataRange.WallSpacing.Value = WallSpacing;
                    wallDataRange.WallLength.Value = WallLength;
                    wallDataRange.WallHeight.Value = WallHeight;
                    wallDataRange.Corners.Value = Corners;
                    wallDataRange.TCorners.Value = TCorners;
                    wallDataRange.TopPlate.Value = TopPlate;
                    wallDataRange.BottomPlate.Value = BottomPlate;
                    wallDataRange.WetAreaStud.Value = AdditionalStuds;
                    wallDataRange.BathCheckout.Value = BathCheckOuts;
                    ExportDoorDatatoExcel(wallDataRange);
                }
            }
        }

        public void ExportDoorDatatoExcel(WarnervaleExcelDataRangeIndex wallDataRange)
        {
            if (IsLoadbearingWall)
            {
                if (Windows.Count>16)
                {
                    MessageBox.Show(
                        "Your Wall Contains Two Much Window, exceed Window range in Excel please check againt ");
                    return;
                }
                else
                {
                    var i = 0;
                    foreach (var window in Windows)
                    {
                        wallDataRange.StartWindowHeight.Offset[i,0].Value = window.Height;
                        wallDataRange.StartWindowWidth.Offset[i, 0].Value = window.Width;
                        wallDataRange.StartWindowQty.Offset[i, 0].Value = window.Qty;
                        wallDataRange.StartWindowTrussSpan.Offset[i, 0].Value = window.TrussSpan;
                        i++;
                    }

                    
                }

                if (Doors.Count>11)
                {
                    MessageBox.Show(
                        "Your Wall Contains Two Much Door, exceed Door range in Excel please check againt ");
                    return;
                }
                else
                {
                    var i = 0;
                    foreach (var door in Doors)
                    {
                        wallDataRange.StartDoorWidth.Offset[i, 0].Value =door.Width;
                        wallDataRange.StartDoorQty.Offset[i, 0].Value = door.Qty;
                        wallDataRange.StartDoorTrussSpan.Offset[i, 0].Value = door.TrussSpan;
                        i++;
                    }

                }
                
            }
            else
            {
                if (Doors.Count > 11)
                {
                    MessageBox.Show(
                        "Your Wall Contains Two Much Door, exceed Door range in Excel please check againt ");
                    return;
                }
                else
                {
                    var i = 0;
                    foreach (var door in Doors)
                    {
                        wallDataRange.StartDoorWidth.Offset[i, 0].Value = door.Width;
                        wallDataRange.StartDoorQty.Offset[i, 0].Value = door.Qty;
                        i++;
                    }
                }

            }

        }

    }
}
