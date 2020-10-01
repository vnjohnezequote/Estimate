using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace AppModels.ExportData
{
    public class WarnervaleExcelDataRangeIndex
    {
        public Excel.Range WallSpacing { get; set; }
        public Excel.Range WallLength { get; set; }
        public Excel.Range WallHeight { get; set; }
        public Excel.Range BeamPockets { get; set; }
        public Excel.Range Corners { get; set; }
        public Excel.Range TCorners { get; set; }
        public Excel.Range Supports { get; set; }
        public Excel.Range StudSize { get; set; }
        public Excel.Range TopPlate { get; set; }
        public Excel.Range BottomPlate { get; set; }
        public Excel.Range RibbonPlate { get; set; }
        public Excel.Range StartWindowHeight { get; set; }
        public Excel.Range StartWindowWidth { get; set; }
        public Excel.Range StartWindowQty { get; set; }
        public Excel.Range StartWindowTrussSpan { get; set; }
        public Excel.Range StartDoorWidth { get; set; }
        public Excel.Range StartDoorQty { get; set; }
        public Excel.Range StartDoorTrussSpan { get; set; }
        public Excel.Range WetAreaStud { get; set; }
        public Excel.Range BathCheckout { get; set; }
        //public Excel.Range RunLength { get; set; }
        public Excel.Range NumberOfSameWall { get; set; }
        public Excel.Range RoofPitch { get; set; }




    }
}
