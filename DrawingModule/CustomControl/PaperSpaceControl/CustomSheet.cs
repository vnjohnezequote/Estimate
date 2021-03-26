using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using AppModels.CustomEntity;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Framings;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.Enums;
using DrawingModule.Views;
using Point = System.Drawing.Point;

namespace DrawingModule.CustomControl.PaperSpaceControl
{
    [Serializable]
    public class CustomSheet : Sheet
    {
        #region Fields

        private Dictionary<formatType, PageSize> _pageFormatTypes = new Dictionary<formatType, PageSize>()
        {
            {formatType.A3_ISO,new PageSize(390, 277,2,PageLayout.Landscape)},
            {formatType.A4_ISO,new PageSize(180.0, 277.0,1,PageLayout.Portrait)},
            {formatType.A4_LANDSCAPE_ISO,new PageSize(267.0, 190.0,1,PageLayout.Landscape)},
        };

        private IJob _job;
        private Point3D _logoInsertPoint;
        private PictureEntity _logoEntity;
        private FramingSheet _framingSheet;
        #endregion

        #region Properties
        //public Guid FramingSheetId { get; set; }
        //public string FramingSheetName { get; set; }

        public FramingSheet FramingSheet
        {
            get => _framingSheet;
            set
            {
                _framingSheet = value;
                _framingSheet.PropertyChanged+=FramingSheetOnPropertyChanged;
            }
            
        }

        private void FramingSheetOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FramingSheet.Name))
            {
                //FramingSheetName = FramingSheet.Name;
                this.Name = FramingSheet.Name;
                RebuildTitle();
            }
        }
        private void RebuildTitle()
        {
            foreach (var entity in this.Entities)
            {
                if(entity is BlockReference blockRef)
                {
                    if (blockRef.Attributes.ContainsKey("Title"))
                        blockRef.Attributes["Title"].Value = Name;
                }

            }
        }
        public IJob Job
        {
            get => _job;
            set
            {
                _job = value;
            }
        }
        #endregion

        #region Constructor

        public CustomSheet(linearUnitsType units, double width, double height, string name) : base(units, width, height, name)
        {
        }
        public CustomSheet(linearUnitsType units, double width, double height, FramingSheet framingSheet) : base(units, width, height, framingSheet.Name)
        {
        }

        public CustomSheet(linearUnitsType units, double width, double height, FramingSheet framingSheet, IJob jobInfo) : base(units, width,
            height, framingSheet.Name)
        {
            FramingSheet = framingSheet;
            //FramingSheetId = framingSheet.Id;
            //FramingSheetName = framingSheet.Name;

        }
        public CustomSheet(linearUnitsType units, double width, double height, string name,IJob jobInfo) : base(units, width, height, name)
        {
            Job = jobInfo;
            Job.Info.PropertyChanged += JobInfo_PropertyChanged;
        }

        private void JobInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(JobInfo.Customer))
            {
                this.RebuildLogo();
            }
        }

        #endregion

        #region Build Page

        private PictureEntity BuildLogo(Point3D logoInsertPoint)
        {
            if (this.Job==null || logoInsertPoint==null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(Job.Info.Customer))
            {
                return null; 
            }
            var curFile = Job.Info.Customer+".png";
            if (File.Exists(curFile))
            {
                var img = Image.FromFile(curFile);
                _logoEntity = new PictureEntity(Plane.XY, logoInsertPoint, 37.648, 6.136, img);
                return _logoEntity;
            }
            return null;

        }

        private void RebuildLogo()
        {
            if (string.IsNullOrEmpty(Job.Info.Customer))
            {
                if (this.Entities.Contains(_logoEntity))
                {
                    _logoEntity.Image = null;
                    this.Entities.Remove(_logoEntity);
                    return;
                }
            }
            var curFile = Job.Info.Customer+".png";
            if (File.Exists(curFile))
            {
                if (this.Entities.Contains(_logoEntity))
                    this.Entities.Remove(_logoEntity);
                var img = Image.FromFile(curFile);
                if (_logoEntity!=null)
                {
                    _logoEntity.Image = img;
                }
                else
                {
                    _logoEntity = new PictureEntity(Plane.XY, _logoInsertPoint,37.648,6.136,img);
                }
                this.Entities.Add(_logoEntity);

            }
            else
            {
                if (this.Entities.Contains(_logoEntity))
                    this.Entities.Remove(_logoEntity);
            }


        }
        protected Entity[] CreateTitleBlocks(PageSize pageSize, Color color, float lineWeight = 0.3f)
        {
            List<Entity> list = new List<Entity>();
            var unitsConversionFactor = Utility.GetLinearUnitsConversionFactor(linearUnitsType.Millimeters, this.Units);
            var borderGap = 10;
            var pageBorder = 0.3f;
            Color color2 = this.GetColorIfNull(color);
            if (pageSize.PageLayout == PageLayout.Landscape)
            {
                this.CreateLandscapePageBorder(pageSize,unitsConversionFactor,color2,lineWeight,list,pageBorder);
                
            }
            else
            {
                this.CreatePortraitPageBorder(pageSize,unitsConversionFactor,color2,lineWeight,list,borderGap);
            }
            
            return list.ToArray();
        }
        public virtual void CreateLandscapePageBorder(PageSize pageSize, double unitsConversionFactor, Color color, float lineWeight, List<Entity> lists, float pageBorder)
        {
            double borderWidth = pageBorder * unitsConversionFactor;
            var pageWidth = pageSize.PageWidth * unitsConversionFactor;
            double[] tableHeights = new double[]
            {
                5.0,
                5.0,
                95.0,
                10.0,
                15.0,
                15.0,
                15.0,
                15.0,
                15.0
            };
            double[] tableWidths = new double[] { 21,21 };
            double tableWidth = 0;
            foreach (var width in tableWidths)
            {
                tableWidth += width;
            }
            for (int i = 0; i < tableHeights.Length; i++)
            {
                tableHeights[i] *= unitsConversionFactor;
            }

            double tableHeight = 0;
            foreach (var height in tableHeights)
            {
                tableHeight += height;
            }
            tableWidths[0] *= unitsConversionFactor;
            double textHeight = 2.5 * unitsConversionFactor;

            Color myColor = Color.Black;

           //double textHeight = 1.3;
            double attributeHeight = 2.25*unitsConversionFactor;
            //double height = 3.0 * unitsConversionFactor;
            //CustomTable table = new CustomTable(Plane.XY, 9, 1, tableHeights, columnsWidths, textHeight);
            CustomTable table = new CustomTable(Plane.XY, 9, 2, tableHeights, tableWidths, textHeight);
            table.HorzCellMargin = 0.9;
            table.VertCellMargin = 0.9;
            table.MergeCells(0, 0, 0, 1);
            table.MergeCells(1, 0, 1, 1);
            table.MergeCells(2, 0, 2, 1);
            table.MergeCells(3, 0, 3, 1);
            table.MergeCells(4, 0, 4, 1);
            table.MergeCells(5, 0, 5, 1);
            table.MergeCells(6, 0, 6, 1);
            table.MergeCells(7, 0, 7, 1);
            //table.Translate(pageWidth - 180, 40);
            table.Translate(pageWidth - tableWidth, tableHeight);

            //#region Cells texts + attributes

            table.SetTextString(0, 0, "Wind Rate = ");
            table.SetAlignment(0, 0, Text.alignmentType.MiddleLeft);
            var baseAtributePoint = table.GetCenter(0, 0);
            var basePoint2 = new Point3D(baseAtributePoint.X*pageSize.ScaleFactor*unitsConversionFactor,baseAtributePoint.Y);
            lists.Add(new devDept.Eyeshot.Entities.Attribute(basePoint2, "WindRate", String.Empty, attributeHeight)
            {
                Alignment = Text.alignmentType.MiddleLeft
            });

            table.SetTextString(1, 0, "Roof Material = ");
            table.SetAlignment(1, 0, Text.alignmentType.MiddleLeft);
            baseAtributePoint = table.GetCenter(1, 0);
            basePoint2 = new Point3D((baseAtributePoint.X+4) + pageSize.ScaleFactor * unitsConversionFactor, baseAtributePoint.Y);
            lists.Add(new devDept.Eyeshot.Entities.Attribute(basePoint2, "RoofMaterial", String.Empty, attributeHeight)
            {
                Alignment = Text.alignmentType.MiddleLeft
            });
            table.SetTextString(3,0,"PLATE HAS BEEN DESIGNED ");
            table.SetAlignment(3, 0, Text.alignmentType.TopLeft);
            table.SetTextHeight(3,0,attributeHeight);
            baseAtributePoint = table.GetCenter(3, 0);
            var aT = new Text((baseAtributePoint.X - 20) * pageSize.ScaleFactor * unitsConversionFactor, baseAtributePoint.Y-2.25,0,"AT",attributeHeight);
            lists.Add(aT);
            basePoint2 = new Point3D((baseAtributePoint.X-15) * pageSize.ScaleFactor * unitsConversionFactor, baseAtributePoint.Y);
            lists.Add(new devDept.Eyeshot.Entities.Attribute(basePoint2, "TieDown", String.Empty, attributeHeight)
            {
                Alignment = Text.alignmentType.TopLeft
            });

            baseAtributePoint = table.GetBottomLeftCorner(4, 0);
            _logoInsertPoint = new Point3D(baseAtributePoint.X+22,baseAtributePoint.Y+14);
            //_logoInsertPoint = baseAtributePoint;
            var logo = BuildLogo(_logoInsertPoint);
            if (logo!=null)
            {
                this.Entities.Add(logo);
            }
            
            //lists.Add(logo);
            // lists.Add(new );


            baseAtributePoint = table.GetBottomLeftCorner(4, 0);
            basePoint2 = new Point3D((baseAtributePoint.X+2) * pageSize.ScaleFactor * unitsConversionFactor, baseAtributePoint.Y-2);
            Text text = new Text(basePoint2, "CLIENT", 2,Text.alignmentType.TopLeft);
            text.Color = Color.Chartreuse;
            text.ColorMethod = colorMethodType.byEntity;
            lists.Add(text);
            lists.Add(new devDept.Eyeshot.Entities.Attribute(new Point3D(basePoint2.X,basePoint2.Y-5), "Client", String.Empty, attributeHeight/1.2)
            {
                Alignment = Text.alignmentType.MiddleLeft
            });

            baseAtributePoint = table.GetBottomLeftCorner(5, 0);
            basePoint2 = new Point3D((baseAtributePoint.X+2) * pageSize.ScaleFactor * unitsConversionFactor, baseAtributePoint.Y-2);
            Text text2 = new Text(basePoint2, "PROJECT", 2, Text.alignmentType.TopLeft);
            text2.Color = Color.Chartreuse;
            text2.ColorMethod = colorMethodType.byEntity;
            lists.Add(text2);
            lists.Add(new devDept.Eyeshot.Entities.Attribute(new Point3D(basePoint2.X, basePoint2.Y - 5), "Address", String.Empty, attributeHeight)
            {
                Alignment = Text.alignmentType.MiddleLeft
            });
            lists.Add(new devDept.Eyeshot.Entities.Attribute(new Point3D(basePoint2.X, basePoint2.Y - 8.5), "City", String.Empty, attributeHeight)
            {
                Alignment = Text.alignmentType.MiddleLeft
            });

            baseAtributePoint = table.GetBottomLeftCorner(6, 0);
            basePoint2 = new Point3D((baseAtributePoint.X + 2) * pageSize.ScaleFactor * unitsConversionFactor, baseAtributePoint.Y - 2);
            Text text3 = new Text(basePoint2, "TITLE", 2, Text.alignmentType.TopLeft);
            text3.Color = Color.Chartreuse;
            text3.ColorMethod = colorMethodType.byEntity;
            lists.Add(text3);

            lists.Add(new devDept.Eyeshot.Entities.Attribute(new Point3D(basePoint2.X, basePoint2.Y - 5), "Title", String.Empty, attributeHeight)
            {
                Alignment = Text.alignmentType.MiddleLeft
            });

            var textHeigthProp = 1;
            baseAtributePoint = table.GetBottomLeftCorner(7, 0);
            basePoint2 = new Point3D((baseAtributePoint.X + 2) * pageSize.ScaleFactor * unitsConversionFactor, baseAtributePoint.Y - 1.25);
            Text text4 = new Text(basePoint2, "DESIGNED BY", textHeigthProp, Text.alignmentType.TopLeft);
            text4.Color = Color.Chartreuse;
            text4.ColorMethod = colorMethodType.byEntity;
            lists.Add(text4);
            Line seperateLine = new Line(new Point3D(basePoint2.X,basePoint2.Y-2.5),new Point3D(basePoint2.X+17,basePoint2.Y-2.5));
            seperateLine.ColorMethod = colorMethodType.byEntity;
            seperateLine.Color = Color.DimGray;
            lists.Add(seperateLine);

            basePoint2 = new Point3D((baseAtributePoint.X + 2) * pageSize.ScaleFactor * unitsConversionFactor, baseAtributePoint.Y - 4.5);
            Text text5 = new Text(basePoint2, "DRAWN BY", textHeigthProp, Text.alignmentType.TopLeft);
            text5.Color = Color.Chartreuse;
            text5.ColorMethod = colorMethodType.byEntity;
            lists.Add(text5);


            Line seperateLine2 = new Line(new Point3D(basePoint2.X, basePoint2.Y - 2.5), new Point3D(basePoint2.X + 17, basePoint2.Y - 2.5));
            seperateLine2.ColorMethod = colorMethodType.byEntity;
            seperateLine2.Color = Color.DimGray;
            lists.Add(seperateLine2);

            basePoint2 = new Point3D((baseAtributePoint.X + 2) * pageSize.ScaleFactor * unitsConversionFactor, baseAtributePoint.Y - 7.75);
            Text text6 = new Text(basePoint2, "AUTHORISED", textHeigthProp, Text.alignmentType.TopLeft);
            text6.Color = Color.Chartreuse;
            text6.ColorMethod = colorMethodType.byEntity;
            lists.Add(text6);

            Line seperateLine3 = new Line(new Point3D(basePoint2.X, basePoint2.Y - 2.5), new Point3D(basePoint2.X + 17, basePoint2.Y - 2.5));
            seperateLine3.ColorMethod = colorMethodType.byEntity;
            seperateLine3.Color = Color.DimGray;
            lists.Add(seperateLine3);

            basePoint2 = new Point3D((baseAtributePoint.X + 2) * pageSize.ScaleFactor * unitsConversionFactor, baseAtributePoint.Y - 11);
            Text text7 = new Text(basePoint2, "DATE", textHeigthProp, Text.alignmentType.TopLeft);
            text7.Color = Color.Chartreuse;
            text7.ColorMethod = colorMethodType.byEntity;
            lists.Add(text7);

            lists.Add(new devDept.Eyeshot.Entities.Attribute(new Point3D(basePoint2.X+1.5, basePoint2.Y - 2.25), "Date", String.Empty, textHeigthProp)
            {
                Alignment = Text.alignmentType.MiddleLeft
            });

            baseAtributePoint = table.GetBottomLeftCorner(7, 1);
            basePoint2 = new Point3D((baseAtributePoint.X + 2) * pageSize.ScaleFactor * unitsConversionFactor, baseAtributePoint.Y - 1.25);
            Text text8 = new Text(basePoint2, "SCALE", textHeigthProp, Text.alignmentType.TopLeft);
            text8.Color = Color.Chartreuse;
            text8.ColorMethod = colorMethodType.byEntity;
            lists.Add(text8);

            lists.Add(new devDept.Eyeshot.Entities.Attribute(new Point3D(basePoint2.X+5, basePoint2.Y - 2.25), "Scale", String.Empty, textHeigthProp*2)
            {
                Alignment = Text.alignmentType.MiddleLeft
            });
            Line seperateLine4 = new Line(new Point3D(basePoint2.X, basePoint2.Y - 4), new Point3D(basePoint2.X + 17, basePoint2.Y - 4));
            seperateLine4.ColorMethod = colorMethodType.byEntity;
            seperateLine4.Color = Color.DimGray;
            lists.Add(seperateLine4);

            basePoint2 = new Point3D((baseAtributePoint.X + 2) * pageSize.ScaleFactor * unitsConversionFactor, baseAtributePoint.Y - 6);
            Text text9 = new Text(basePoint2, "JOB NO", textHeigthProp, Text.alignmentType.TopLeft);
            text9.Color = Color.Chartreuse;
            text9.ColorMethod = colorMethodType.byEntity;
            lists.Add(text9);

            lists.Add(new devDept.Eyeshot.Entities.Attribute(new Point3D(basePoint2.X, basePoint2.Y - 2.25), "JobNo", String.Empty, textHeigthProp)
            {
                Alignment = Text.alignmentType.MiddleLeft
            });

            Line seperateLine5 = new Line(new Point3D(basePoint2.X, basePoint2.Y - 4), new Point3D(basePoint2.X + 17, basePoint2.Y - 4));
            seperateLine5.ColorMethod = colorMethodType.byEntity;
            seperateLine5.Color = Color.DimGray;
            lists.Add(seperateLine5);

            basePoint2 = new Point3D((baseAtributePoint.X + 2) * pageSize.ScaleFactor * unitsConversionFactor, baseAtributePoint.Y - 10.75);
            Text text10 = new Text(basePoint2, "DRAWING NO", textHeigthProp, Text.alignmentType.TopLeft);
            text10.Color = Color.Chartreuse;
            text10.ColorMethod = colorMethodType.byEntity;
            lists.Add(text10);


            //table.SetTextString(4, 0, "CLIENT:");
            //table.SetAlignment(4, 0, Text.alignmentType.TopLeft);
            //table.SetTextHeight(4,0,2);

            //lists.Add(new devDept.Eyeshot.Entities.Attribute(new Point3D(20, 25), "Subtitle", String.Empty, attributeHeight)
            //{
            //    Alignment = Text.alignmentType.MiddleCenter
            //});

            //#endregion Cells texts

            table.ColorMethod = colorMethodType.byEntity;
            table.Color = myColor;
            table.LineWeightMethod = colorMethodType.byEntity;
            table.LineWeight = lineWeight;
            lists.Add(table);

            //table.Translate(pageWidth - tableWidth, tableHeight);
            //table.SetTextString(0, 0, "TITLE:");
            //table.SetAlignment(0, 0, Text.alignmentType.TopLeft);

            //double[] tableHeights2 = new double[]
            //{
            //    15.0
            //};
            //double[] tableWidths2 = new double[] { 21,21 };
            //double tableHeight2 = 0;
            //foreach (var height in tableHeights2)
            //{
            //    tableHeight2 += height;
            //}

            //lists.Add(table);

            //CustomTable table2 = new CustomTable(Plane.XY, 1, 2, tableHeights2, tableWidths2, textHeight);
            //table2.HorzCellMargin = 0.9;
            //table2.VertCellMargin = 0.9;

            ////table.Translate(pageWidth - 180, 40);
            //table2.Translate(pageWidth - tableWidth, tableHeight2);
            //lists.Add(table2);


        }
        public virtual void CreatePortraitPageBorder(PageSize pageSize, double unitsConversionFactor,Color color, float lineWeight,List<Entity> lists, double borderGap)
        {
            //var y = (42 * pageSize.ScaleFactor) * unitsConversionFactor;
            //var x = pageSize.PageWidth * unitsConversionFactor;
            //var rightSeperatorPoint = new Point3D(x, y);
            //var leftSeperatorPoint = new Point3D(-borderGap, y);
            //var seperateLine = this.BuildLineBorder(rightSeperatorPoint, leftSeperatorPoint, color, lineWeight);
            //lists.Add(seperateLine);
        }
        protected Entity[] CreateBorders(PageSize pageSize, Color? color = null, float lineWeight = 0.5f,
            double textHeight = 3, double externalBorderGap = 10, bool createExternalBorder = true,
            float externalBorderLineWeight = 0.3f)
        {
            List<Entity> list = new List<Entity>();
            var unitsConversionFactor = Utility.GetLinearUnitsConversionFactor(linearUnitsType.Millimeters, this.Units);
            double width = pageSize.PageWidth*unitsConversionFactor;
            double height = pageSize.PageHeight*unitsConversionFactor;
            double gapBorder = externalBorderGap*unitsConversionFactor;
            double textHeight2 = textHeight*unitsConversionFactor;
            Color color2 = this.GetColorIfNull(color);
            var bottomLeftPoint = new Point3D(-gapBorder,0);
            var bottomRigthPoint = new Point3D(width,0);
            var topRightPoint = new Point3D(width,height);
            var topLeftPoint = new Point3D(-gapBorder,height);
            if (createExternalBorder)
            {
                Line item = this.BuildLineBorder(bottomLeftPoint,bottomRigthPoint, color2,externalBorderLineWeight);
                Line item2 = this.BuildLineBorder(bottomRigthPoint,topRightPoint, color2, externalBorderLineWeight);
                Line item3 = this.BuildLineBorder(topRightPoint,topLeftPoint , color2, externalBorderLineWeight);
                Line item4 = this.BuildLineBorder(topLeftPoint,bottomLeftPoint, color2, externalBorderLineWeight);
                list.AddRange(new List<Entity>
                {
                    item,
                    item2,
                    item3,
                    item4
                });
            }
            return list.ToArray();
        }

        #endregion

        #region Private method

        private Line BuildLineBorder(Point3D starP, Point3D endP, Color color_0, float float_0)
        {
            return new Line(starP,endP)
            {
                ColorMethod = colorMethodType.byEntity,
                Color = color_0,
                LineWeightMethod = colorMethodType.byEntity,
                LineWeight = float_0
            };
        }

        #endregion

        #region Public method

        public BlockReference BuildPaper(formatType pageFormateType,out Block block, string blockName = null, Color? color = null)
        {
            var pageSize = (from pageFormat in _pageFormatTypes 
                where pageFormat.Key == pageFormateType select pageFormat.Value).FirstOrDefault();
            IList<Entity> ents = this.BuildBlockEntities(pageSize, color);
            
            return this.BuildFormatBlock(this.GetBlockNameString("A3", blockName), ents, out block);
            
        }
        internal IList<Entity> BuildBlockEntities(PageSize pageSize, Color? color)
        {
            List<Entity> list = new List<Entity>();
            Color colorOut = this.GetColorIfNull(color);
            list.AddRange(this.CreateBorders(pageSize, color, 0.5f, 3.0));
            list.AddRange(this.CreateTitleBlocks(pageSize, colorOut, 0.15f));
            return list;
        }
        private string GetBlockNameString(string paperSizeName, string blockName)
        {
            return blockName ?? string.Format("{0} {1}", this.Name, paperSizeName);
        }

        public void AddWallPlaceHolder(WallName wallName, Drawings drawings,string placeHolderText)
        {
            if (drawings.Blocks.Contains(wallName.BlockName)) return;
            var block = new Block(wallName.BlockName);
            block.Entities.Add(new Line(new Point3D(0,0,0),new Point3D(50,0,0)){LayerName = drawings.WiresLayerName,ColorMethod = colorMethodType.byEntity,Color = Color.Red,LineWeightMethod = colorMethodType.byEntity});
            drawings.Blocks.Add(block);
            if (this == drawings.ActiveSheet)
            {
                drawings.Entities.Add(wallName);
                return;
            }
            this.Entities.Add(wallName);
        }

        public void AddFloorQtyPaceHolder(FloorQuantity floorQty, Drawings drawings,string placeHolderText)
        {
            if (drawings.Blocks.Contains(floorQty.BlockName) ) return;
            var block = new Block(floorQty.BlockName);
            block.Entities.Add(new Line(new Point3D(0, 0, 0), new Point3D(50, 0, 0)) { LayerName = drawings.WiresLayerName, ColorMethod = colorMethodType.byEntity, Color = Color.Red, LineWeightMethod = colorMethodType.byEntity });
            drawings.Blocks.Add(block);
            if (this==drawings.ActiveSheet)
            {
                drawings.Entities.Add(floorQty);
                return;
            }
            this.Entities.Add(floorQty);
        }
        #endregion

        #region MyRegion

        private Color GetColorIfNull(Color? nullable_0)
        {
            if (nullable_0 != null && !nullable_0.Value.IsEmpty)
            {
                return nullable_0.Value;
            }
            return Color.Black;
        }

        #endregion


        
    }
}
