using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using AppModels.CustomEntity;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using Prism.Mvvm;
using Syncfusion.XlsIO;

namespace AppModels.ExportData.FrameData
{
    public class FrameJob: BindableBase
    {
        #region private

        private string _createDate;
        private string _refNo;
        private string _clientName;
        private string _jobName;
        private string _address;
        private string _windRate;
        private string _roofType;
        private string _treatement;
        //private string JobLocation
        
        #endregion
        public string CreateDate { get; set; }
        public string RefNo { get; set; }
        public string ClientName { get; set; }
        public string JobName { get; set; }
        public string Address { get; set; }
        public string WindRate { get; set; }
        public string RoofType { get; set; }
        public string Treatement { get; set; }

        public string JobLocation {get;set;}
        public FramingSheetTypes FramingSheetType { get; set; }
        public string FrameSheetname { get; set; }
        public string FrameName { get; set; }
        public FramingList FrameList { get; set; } = new FramingList();
        #region Constructor

        public FrameJob(IJob job,FramingSheet framingSheet,string levelName = "")
        {
            CreateDate = DateTime.Now.ToShortDateString();
            ClientName = job.Info.Client.Name;
            JobName = job.Info.JobNumber;
            JobLocation = job.Info.JobLocation;
            FramingSheetType = framingSheet.FramingSheetType;
            FrameName = framingSheet.Name;
            switch (framingSheet.FramingSheetType)
            {
                case FramingSheetTypes.FloorFraming when string.IsNullOrEmpty(levelName):
                    JobName += " - Floor";
                    FrameSheetname = "FLOOR";
                    break;
                case FramingSheetTypes.FloorFraming:
                    JobName += " - " + levelName + " Floor";
                    FrameSheetname = "FLOOR";
                    break;
                case FramingSheetTypes.CeilingJoistFraming when string.IsNullOrEmpty(levelName):
                    JobName += " - Ceiling";
                    FrameSheetname = "CEILING";
                    break;
                case FramingSheetTypes.CeilingJoistFraming:
                    JobName += " - " + levelName + " Ceiling";
                    FrameSheetname = "CEILING";
                    break;
                case FramingSheetTypes.RafterFraming when string.IsNullOrEmpty(levelName):
                    JobName += " - Rafter";
                    FrameSheetname = "ROOF";
                    break;
                case FramingSheetTypes.RafterFraming:
                    JobName += " - " + levelName + " Rafter";
                    FrameSheetname = "ROOF";
                    break;
                case FramingSheetTypes.PurlinFraming when string.IsNullOrEmpty(levelName):
                    JobName += " - Purlin";
                    FrameSheetname = "PURLIN";
                    break;
                case FramingSheetTypes.PurlinFraming:
                    JobName += " - " + levelName + " Purlin";
                    FrameSheetname = "PURLIN";
                    break;
            }

            Address = job.Info.FullAddress;
            WindRate = job.Info.WindRate;
            RoofType = job.Info.RoofMaterial;
            Treatement = job.Info.Treatment;
            foreach (var framing in framingSheet.Joists)
            {
                if (framing.FramingType == FramingTypes.SteelBeam || framing.IsExisting)
                {
                    continue;
                }
                var framingData = new FrameData(framing, framing.Name);
                FrameList.Add(framingData);
            }

            foreach (var framing in framingSheet.OutTriggers)
            {
                if (framing.FramingType == FramingTypes.SteelBeam || framing.IsExisting)
                {
                    continue;
                }
                var framingData = new FrameData(framing,framing.Name);
                FrameList.Add(framingData);
            }

            foreach (var framing in framingSheet.Beams )       
            {
                if (framing.FramingType == FramingTypes.SteelBeam || framing.IsExisting)
                {
                    continue;
                }
                var framingData = new FrameData(framing,framing.Name);
                FrameList.Add(framingData);
            }

            foreach (var framing in framingSheet.Blockings)
            {
                if (framing.FramingType == FramingTypes.SteelBeam || framing.IsExisting)
                {
                    continue;
                }
                var framingData = new FrameData(framing, framing.Name);
                FrameList.Add(framingData);
            }

            foreach (var framing in framingSheet.Hangers)
            {
                if (framing.FramingType == FramingTypes.SteelBeam || framing.IsExisting)
                {
                    continue;
                }
                var framingData = new FrameData(framing, framing.Name);
                FrameList.Add(framingData);
            }
        }

        #endregion

        public void CreateTable(Block block)
        {
            var dicFrameData = new Dictionary<string, List<FrameData>>();
            var listDoubleFramingDataDict = new Dictionary<string, List<FrameData>>();
            var listHangerAndTieDownData = new List<FrameData>();
            this.GeneralFrameQtyListForFrameJob(dicFrameData, listDoubleFramingDataDict, listHangerAndTieDownData);
            var floorQtyTableRow = 2;
            var floorQtyTableColumn = 5;

            if (dicFrameData.Count>0)
            {
                foreach (var frameData in dicFrameData)
                {
                    floorQtyTableRow += frameData.Value.Count;
                    floorQtyTableRow += 3;
                }
            }

            if (listDoubleFramingDataDict.Count>0)
            {
                foreach (var doubleData in listDoubleFramingDataDict)
                {
                    floorQtyTableRow += doubleData.Value.Count;
                    floorQtyTableRow += 3;
                }
            }
            floorQtyTableRow += listHangerAndTieDownData.Count;

            var tableHeights = new double[floorQtyTableRow];
            for (int i = 0; i < tableHeights.Length; i++)
            {
                tableHeights[i] = 3;
            }
            var tableWidth = new double[]
            {
                14,30,9,9,9
            };

            var tableQty = new CustomTable(Plane.XY, floorQtyTableRow, floorQtyTableColumn, tableHeights,
                tableWidth, 1.1);
            tableQty.LineWeight = 0.1f;
            tableQty.LineWeightMethod = colorMethodType.byEntity;
            var startIndex = 0;

            tableQty.MergeCells(startIndex, 0, startIndex, 4);

            var baseAtributePoint = tableQty.GetCenter(0, 0);
            var basePoint2 = new Point3D(baseAtributePoint.X, baseAtributePoint.Y);
            block.Entities.Add(new devDept.Eyeshot.Entities.Attribute(basePoint2, "Title", String.Empty, 2.2)
            {
                Alignment = Text.alignmentType.MiddleCenter
            });
            startIndex+=2;
            AutoFillDataToTableEntities(ref startIndex,dicFrameData,tableQty,block);
            AutoFillDataToTableEntities(ref startIndex,listDoubleFramingDataDict,tableQty,block,true,false);
            AutofilOrderDataToTableEtities(startIndex,listHangerAndTieDownData,tableQty);
            block.Entities.Add(tableQty);
            block.Entities.Regen();
        }

        public void AutoFillDataToTableEntities(ref int startIndex, Dictionary<string, List<FrameData>> frameDataDict, CustomTable table,Block block,bool isDrawDoubleArea = false,bool isFillTotalLength = true)
        {
           
            foreach (var dataFrame in frameDataDict)
            {
                var totalLength = 0.0;
                AutoFillDataToTableEntities(ref startIndex,dataFrame.Value,table,ref totalLength,block,isDrawDoubleArea,isFillTotalLength);
                startIndex++;
                table.MergeCells(startIndex, 0, startIndex, 3);
                table.SetTextString(startIndex,0,"Total Length: ");
                table.SetStyleName(startIndex,0, "TotalHeader");
                if (isFillTotalLength)
                {
                    table.SetTextString(startIndex, 4, totalLength.ToString());
                }
                else
                {
                    table.SetTextString(startIndex, 4, "0.0");
                }
                
                startIndex+=2;
            }
        }

        public void AutofilOrderDataToTableEtities(int startIndex, List<FrameData> otherFrameData, CustomTable table)
        {
            foreach (var frameData in otherFrameData)
            {
                table.SetTextString(startIndex,0,frameData.FullName);
                table.SetTextString(startIndex,1,frameData.Material);
                table.SetTextString(startIndex,2,frameData.Quantity.ToString());
                startIndex++;
            }

            
        }
        public void AutoFillDataToTableEntities(ref int startIndex, List<FrameData> frameDatas, CustomTable table, ref double sumtotalLength,Block block, bool isDrawDoubleArea = false, bool isFilltotalQty = true)
        {
            var points = new List<Point3D>();
            if (isDrawDoubleArea)
            {
                var topLeft = table.GetTopLeftCorner(startIndex, 0);
                var topRight = table.GetTopRightCorner(startIndex, 3);
                points.Add(topLeft);
                points.Add(topRight);

            }
            foreach (var frame in frameDatas)
            {
                table.SetTextString(startIndex,0,frame.FullName);
                table.SetTextString(startIndex, 1, frame.Material);
                table.SetTextString(startIndex, 2, frame.Quantity.ToString());
                table.SetTextString(startIndex, 3, frame.Length.ToString());
                var totalLength = frame.Quantity * frame.Length;
                if (isFilltotalQty)
                {
                    table.SetTextString(startIndex, 4, frame.TotalLength.ToString());
                }
                sumtotalLength += frame.TotalLength;
                startIndex++;
            }

            if (isDrawDoubleArea)
            {
                var row = startIndex - 1;
                var bottomLeft = table.GetBottomLeftCorner(row, 0);
                var bottomRight = table.GetBottomRightCorner(row, 3);
                points.Add(bottomRight);
                points.Add(bottomLeft);
                points.Add(points[0]);
                var quad = Mesh.CreatePlanar(points, Mesh.natureType.Plain);
                //quad.PrintOrder = 1;
                quad.ColorMethod = colorMethodType.byEntity;
                quad.Color = Color.FromArgb(125,Color.Yellow);
                quad.ColorMethod = colorMethodType.byEntity;
                quad.LineWeight = 0.1f;
                quad.LineWeightMethod = colorMethodType.byEntity;

                block.Entities.Add(quad);
            }
        }
        public void ExportToExcel()
        {
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2016;
                IWorkbook workBook = application.Workbooks.Create(1);
                workBook.StandardFont = "Arial";
                IWorksheet workSheet = workBook.Worksheets[0];
                GeneralFormatStyleForWorkbook(workBook);
                GeneralFramingWorkSheet(workSheet,workBook);
                FillDataToFrameSheet(workSheet);
                var fileSave = JobName + " " + FrameName;
                if (this.JobLocation.EndsWith("\\"))
                {
                    fileSave = JobLocation + fileSave;
                }
                else
                {
                    fileSave = JobLocation + "\\" + fileSave;
                }

                fileSave += ".xlsx";
                workBook.SaveAs(fileSave);
            }
        }

        private void GeneralFormatStyleForWorkbook(IWorkbook workbook)
        {
            IStyle header1Style = workbook.Styles.Add("Header1");
            header1Style.Font.Bold = true;
            header1Style.Font.FontName = "Arial";
            header1Style.Font.Size = 12;
            header1Style.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            header1Style.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            header1Style.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            header1Style.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
        }
        private void GeneralFramingWorkSheet(IWorksheet workSheet,IWorkbook workbook)
        {
            workSheet.Range["A3:G10"].CellStyle.Font.Bold = true;
            workSheet.Range["B3:G10"].CellStyle.Font.Size = 10;
            workSheet.Range["F3:G4"].CellStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            workSheet.Range["F3:G4"].CellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            workSheet.Range["F3:G4"].CellStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            workSheet.Range["F3:G4"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;

            workSheet.Range["B5:G5"].BorderAround(ExcelLineStyle.Thin);
            workSheet.Range["B6:E6"].BorderAround(ExcelLineStyle.Thin);
            workSheet.Range["B7:E7"].BorderAround(ExcelLineStyle.Thin);

            workSheet.Range["F6:G9"].CellStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            workSheet.Range["F6:G9"].CellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            workSheet.Range["F6:G9"].CellStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            workSheet.Range["F6:G9"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            workSheet.Range["A9:E9"].BorderAround(ExcelLineStyle.Thin);

            workSheet.Range["A10:G10"].CellStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            workSheet.Range["A10:G10"].CellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            workSheet.Range["A10:G10"].CellStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            workSheet.Range["A10:G10"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            workSheet.Range["A9"].CellStyle.Font.Size = 11;
            workSheet.Range["A9"].CellStyle.Font.Underline = ExcelUnderline.Single;
            workSheet.Range["A9"].CellStyle.Font.Italic = true;

            workSheet.Name = "QUOTE";
            workSheet.Range["F3"].Value = "DATE";
            workSheet.Range["F4"].Value = "REF No";
            workSheet.Range["G3"].DateTime = DateTime.Now;
            workSheet.Range["G3"].NumberFormat = "mm/dd/yyyy";

            workSheet.Range["A5"].Value = "CLIENT";
            workSheet.Range["A5"].CellStyle = workbook.Styles["Header1"];

            workSheet.Range["B5"].Value = ClientName.ToUpper();

            workSheet.Range["A6"].Value = "JOB";
            workSheet.Range["A6"].CellStyle = workbook.Styles["Header1"];

            workSheet.Range["B6"].Value = this.JobName;

            workSheet.Range["A7"].Value = "ADDRESS";
            workSheet.Range["A7"].CellStyle = workbook.Styles["Header1"];

            workSheet.Range["B7"].Value = this.Address.ToUpper();
            workSheet.Range["F6"].Value = "WIND CAT.";
            workSheet.Range["G6"].Value = this.WindRate;
            workSheet.Range["F7"].Value = FrameSheetname;
            workSheet.Range["G7"].Value = "SHEET";
            workSheet.Range["F8"].Value = "Treatment";
            workSheet.Range["G8"].Value = Treatement;
            workSheet.Range["F10"].Value = "Lm Price";
            workSheet.Range["G10"].Value = "Total Price";
            if (this.FramingSheetType == FramingSheetTypes.FloorFraming)
            {
                workSheet.Range["A9"].Value = "Floor Materials";
            }
            else if (this.FramingSheetType == FramingSheetTypes.RafterFraming)
            {
                workSheet.Range["A9"].Value = "Rafter Materials";
            }
            else if (this.FramingSheetType == FramingSheetTypes.CeilingJoistFraming)
            {
                workSheet.Range["A9"].Value = "Ceiling Materials";
            }
            else
            {
                workSheet.Range["A9"].Value = "Purlin Materials";
            }
        }

        private void GeneralFrameQtyListForFrameJob(Dictionary<string, List<FrameData>> dicFrameData, Dictionary<string, List<FrameData>> listDoubleFramingDataDict, List<FrameData> listHangerAndTieDownData)
        {
            //var dicFrameData = new Dictionary<string, List<FrameData>>();
            //var listDoubleFramingDataDict = new Dictionary<string, List<FrameData>>();
            //var listHangerAndTieDownData = new List<FrameData>();
            //var tieDowns = new List<FrameData>();
            var frames = FrameList.FrameList.ToList();
            frames.Sort();
            foreach (var frameData in frames)
            {
                if (frameData.IsDouble)
                {
                    if (listDoubleFramingDataDict.ContainsKey((frameData.Material)))
                    {
                        listDoubleFramingDataDict.TryGetValue(frameData.Material, out var doubleFrameList);
                        if (doubleFrameList != null)
                        {
                            doubleFrameList.Add(frameData);
                        }
                    }
                    else
                    {
                        var doubleFrameList = new List<FrameData>();
                        doubleFrameList.Add(frameData);
                        listDoubleFramingDataDict.Add(frameData.Material, doubleFrameList);
                    }
                }
                else
                {
                    switch (frameData.FramingType)
                    {
                        case FramingTypes.Hanger:
                            listHangerAndTieDownData.Add(frameData);
                            break;
                        case FramingTypes.TieDown:
                            listHangerAndTieDownData.Add(frameData);
                            break;
                        default:
                            {
                                if (dicFrameData.ContainsKey(frameData.Material))
                                {
                                    dicFrameData.TryGetValue(frameData.Material, out var frameList);
                                    frameList?.Add(frameData);
                                }
                                else
                                {
                                    var frameList = new List<FrameData> { frameData };
                                    dicFrameData.Add(frameData.Material, frameList);
                                }

                                break;
                            }
                    }
                }

            }
            listHangerAndTieDownData.Sort();
        }
        private void FillDataToFrameSheet(IWorksheet worksheet)
        {
            var dicFrameData = new Dictionary<string, List<FrameData>>();
            var listDoubleFramingDataDict = new Dictionary<string,List<FrameData>>();
            var listHangerAndTieDownData = new List<FrameData>();
            this.GeneralFrameQtyListForFrameJob(dicFrameData,listDoubleFramingDataDict,listHangerAndTieDownData);
            
            var startIndex = 11;
            if (dicFrameData.Count>0)
            {
                AutoFillFrameDataDic(ref startIndex, dicFrameData, worksheet);
            }

            if (listDoubleFramingDataDict.Count > 0) 
            {
                AutoFillFrameDataDic(ref startIndex, listDoubleFramingDataDict, worksheet);
            }

            if (listHangerAndTieDownData.Count>0)
            {
                AutoFillOtherdata(ref startIndex, listHangerAndTieDownData, worksheet);
            }

            startIndex++;
            var skipedRange = "A" + startIndex + ":G" + startIndex;
            worksheet.Range[skipedRange].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            worksheet.Range[skipedRange].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            worksheet.Range[skipedRange].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            worksheet.Range[skipedRange].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            startIndex++;
            skipedRange = "A" + startIndex + ":G" + startIndex;
            worksheet.Range[skipedRange].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            worksheet.Range[skipedRange].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            worksheet.Range[skipedRange].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            worksheet.Range[skipedRange].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            startIndex++;
            

            var endIndexRange = startIndex;
            var noteRange = "A" + startIndex;
            var skipNoteStringRange = "G" + startIndex;
            startIndex++;
            var sumRange = "G11:G"+endIndexRange;
            var sumFormularRange = "G" + startIndex;
            startIndex++;
            var gstRagne = "A" + startIndex;
            var gstRangePrice = "G" + startIndex;
            startIndex++;
            var nextRange = "G" + startIndex;
            startIndex++;
            var totalInclude = "A" + startIndex;
            var formulartTotalInclueGstRange = "G" + startIndex;
            var bottomBorderQuoteRange = totalInclude + ":" + formulartTotalInclueGstRange;

            worksheet.Range[noteRange].Value = "Note:";
            worksheet.Range[sumFormularRange].Value = "=sum(" + sumRange + ")";
            worksheet.Range[sumFormularRange].CellStyle.Font.Bold = true;
            worksheet.Range[sumFormularRange].NumberFormat= " _ -$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";


            worksheet.Range[gstRagne].Value = "GST";
            worksheet.Range[gstRangePrice].Value = "="+ sumFormularRange + "*0.1";
            worksheet.Range[gstRangePrice].CellStyle.Font.Bold = true;
            worksheet.Range[gstRangePrice].CellStyle.Font.Size = 11;
            worksheet.Range[gstRangePrice].NumberFormat = " _ -$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

            worksheet.Range[totalInclude].Value = "Total including GST";
            worksheet.Range[formulartTotalInclueGstRange].Value ="="+ sumFormularRange + "+" + gstRangePrice;
            worksheet.Range[formulartTotalInclueGstRange].CellStyle.Font.Bold = true;
            worksheet.Range[formulartTotalInclueGstRange].CellStyle.Font.Size = 11;
            worksheet.Range[formulartTotalInclueGstRange].NumberFormat = " _ -$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

            worksheet.UsedRange.AutofitColumns();

            var formatQuoteRange = noteRange + ":" + formulartTotalInclueGstRange;
            
            worksheet.Range[formatQuoteRange].BorderAround(ExcelLineStyle.Thin);

            var borderQuoteRang = skipNoteStringRange + ":" + formulartTotalInclueGstRange;

            worksheet.Range[borderQuoteRang].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            worksheet.Range[borderQuoteRang].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            worksheet.Range[borderQuoteRang].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            worksheet.Range[borderQuoteRang].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;

            worksheet.Range[bottomBorderQuoteRange].Borders[ExcelBordersIndex.EdgeBottom].LineStyle =
                ExcelLineStyle.Double;
            startIndex += 2;
            var estimateQuoteNote = "A" + startIndex;
            worksheet.Range[estimateQuoteNote].Value =
                "Estimate Price only client to confirm all details prior to placing order.";
            worksheet.Range[estimateQuoteNote].CellStyle.Font.Bold = true;
            worksheet.Range[estimateQuoteNote].CellStyle.Font.Italic = true;
            worksheet.Range[estimateQuoteNote].CellStyle.Font.Size = 10;

            worksheet.Columns[0].ColumnWidth = 20;
            worksheet.Columns[1].ColumnWidth = 31;
            worksheet.Columns[2].ColumnWidth = 7;
            worksheet.Columns[3].ColumnWidth = 7;
            worksheet.Columns[4].ColumnWidth = 7;
            worksheet.Columns[5].ColumnWidth = 13;
            worksheet.Columns[6].ColumnWidth = 13;

        }

        private void AutoFillOtherdata(ref int startIndex, List<FrameData> listData, IWorksheet worksheet)
        {
            if (startIndex>11)
            {
                startIndex++;
                var skipedRange = "A" + startIndex + ":G" + startIndex;
                worksheet.Range[skipedRange].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[skipedRange].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[skipedRange].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[skipedRange].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                startIndex++;
            }
            foreach (var frameData in listData)
            {
                var nameRange = "A" + startIndex;
                var materialRange = "B" + startIndex;
                var qtyRange = "C" + startIndex;
                var nextQty = "D" + startIndex;
                var nextnextQty = "E" + startIndex;
                var lmRange = "F" + startIndex;
                var totalRange = "G" + startIndex;
                worksheet.Range[nameRange].Value = frameData.FullName;
                worksheet.Range[nameRange].BorderAround(ExcelLineStyle.Thin);

                worksheet.Range[materialRange].Value = frameData.Material;
                worksheet.Range[materialRange].BorderAround(ExcelLineStyle.Thin);

                worksheet.Range[qtyRange].Number = frameData.Quantity;
                worksheet.Range[qtyRange].BorderAround(ExcelLineStyle.Thin);

                worksheet.Range[nextQty].BorderAround(ExcelLineStyle.Thin);
                worksheet.Range[nextnextQty].BorderAround(ExcelLineStyle.Thin);


                worksheet.Range[totalRange].Value = "=" + qtyRange + "*" + lmRange;

                worksheet.Range[lmRange].BorderAround(ExcelLineStyle.Thin);
                worksheet.Range[lmRange].CellStyle.Font.Bold = true;
                worksheet.Range[lmRange].CellStyle.Font.Size = 11;
                worksheet.Range[lmRange].Number =0;
                worksheet.Range[lmRange].NumberFormat = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                worksheet.Range[totalRange].BorderAround(ExcelLineStyle.Thin);
                worksheet.Range[totalRange].CellStyle.Font.Bold = true;
                worksheet.Range[totalRange].CellStyle.Font.Size = 11;

                worksheet.Range[totalRange].NumberFormat = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                startIndex++;
            }
        }
        private void AutoFillFrameDataDic(ref int startIndex,Dictionary<string,List<FrameData>> listDataDic, IWorksheet worksheet)
        {
            foreach (var framek in listDataDic)
            {
                if (startIndex > 11)
                {
                    startIndex++;
                    var skipedRange = "A" + startIndex + ":G" + startIndex;
                    worksheet.Range[skipedRange].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                    worksheet.Range[skipedRange].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                    worksheet.Range[skipedRange].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                    worksheet.Range[skipedRange].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                    startIndex ++;
                }
                var startRange = startIndex;
                foreach (var frameData in framek.Value)
                {
                    var nameRange = "A" + startIndex;
                    var materialRange = "B" + startIndex;
                    var qtyRange = "C" + startIndex;
                    var lengthRange = "D" + startIndex;
                    var totalRange = "E" + startIndex;
                    var lmPrice = "F" + startIndex;
                    var totalPrice = "G" + startIndex;
                    worksheet.Range[nameRange].Value = frameData.FullName;
                    worksheet.Range[nameRange].BorderAround(ExcelLineStyle.Thin);
                    
                    worksheet.Range[materialRange].Value = frameData.Material;
                    worksheet.Range[materialRange].BorderAround(ExcelLineStyle.Thin);

                    worksheet.Range[qtyRange].Number = frameData.Quantity;
                    worksheet.Range[qtyRange].BorderAround(ExcelLineStyle.Thin);


                    worksheet.Range[lengthRange].Number = frameData.Length;
                    worksheet.Range[lengthRange].BorderAround(ExcelLineStyle.Thin);

                    if (frameData.IsDouble)
                    {
                        worksheet.Range[nameRange].CellStyle.Color = Color.Yellow;
                        worksheet.Range[materialRange].CellStyle.Color = Color.Yellow;
                        worksheet.Range[qtyRange].CellStyle.Color = Color.Yellow;
                        worksheet.Range[lengthRange].CellStyle.Color = Color.Yellow;
                    }
                    if (!frameData.IsDouble)
                    {
                        worksheet.Range[totalRange].Value = "=" + qtyRange + "*" + lengthRange;
                    }

                    worksheet.Range[totalRange].BorderAround(ExcelLineStyle.Thin);
                    worksheet.Range[lmPrice].BorderAround(ExcelLineStyle.Thin);
                    worksheet.Range[totalPrice].BorderAround(ExcelLineStyle.Thin);
                    
                    startIndex++;
                }
                var endIndex = startIndex;

                var skipRange = "A" + startIndex + ":G" + startIndex;
                worksheet.Range[skipRange].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[skipRange].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[skipRange].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[skipRange].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;

                startIndex += 1;
                var totalLengthHeaderRange = "A" + startIndex;

                var totalLengthRange = "A" + startIndex + ":D" + startIndex;
                worksheet.Range[totalLengthRange].BorderAround(ExcelLineStyle.Thin);

                var sumLengthRange = "E" + startIndex;
                worksheet.Range[sumLengthRange].BorderAround(ExcelLineStyle.Thin);

                var totalLenghPriceRange = "F" + startIndex;
                worksheet.Range[totalLenghPriceRange].BorderAround(ExcelLineStyle.Thin);
                worksheet.Range[totalLenghPriceRange].CellStyle.Font.Bold = true;
                worksheet.Range[totalLenghPriceRange].CellStyle.Font.Size = 11;
                worksheet.Range[totalLenghPriceRange].NumberFormat = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                worksheet.Range[totalLenghPriceRange].Number = 0;


                var totalPriceRange = "G" + startIndex;
                worksheet.Range[totalPriceRange].BorderAround(ExcelLineStyle.Thin);
                worksheet.Range[totalPriceRange].CellStyle.Font.Bold = true;

                worksheet.Range[totalPriceRange].NumberFormat = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                worksheet.Range[totalPriceRange].CellStyle.Font.Size = 11;
                worksheet.Range[totalLengthHeaderRange].Value = "Total length:";
                worksheet.Range[totalLengthHeaderRange].CellStyle.Font.Bold = true;
                worksheet.Range[totalLengthHeaderRange].Merge(false);
                worksheet.Range[totalLengthHeaderRange].HorizontalAlignment = ExcelHAlign.HAlignCenter;


                var startSumRange = "E" + startRange + ":E" + endIndex;
                worksheet.Range[sumLengthRange].Value = "=Sum(" + startSumRange + ")";
                worksheet.Range[sumLengthRange].CellStyle.Font.Bold = true;

                worksheet.Range[totalPriceRange].Value = "=E" + startIndex + "*F" + startIndex;

                
                
            }
        }
    }
}
