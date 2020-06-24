using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.Entities;
using DrawingModule.Enums;
using DrawingModule.Views;

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
        #endregion

        #region Properties

        #endregion

        #region Constructor

        public CustomSheet(linearUnitsType units, double width, double height, string name) : base(units, width, height, name)
        {
        }

        protected CustomSheet(Sheet another) : base(another)
        {
        }

        public CustomSheet(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        #endregion

        #region Build Page

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
            double[] tableWidths = new double[] { 42 };
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
            double textHeight = 3 * unitsConversionFactor;

            Color myColor = Color.Black;

           //double textHeight = 1.3;
            double attributeHeight = 3*unitsConversionFactor;
            //double height = 3.0 * unitsConversionFactor;
            //CustomTable table = new CustomTable(Plane.XY, 9, 1, tableHeights, columnsWidths, textHeight);
            CustomTable table = new CustomTable(Plane.XY, 9, 1, tableHeights, tableWidths, textHeight);
            table.HorzCellMargin = 0.9;
            table.VertCellMargin = 0.9;

            //table.Translate(pageWidth - 180, 40);
            table.Translate(pageWidth - tableWidth, tableHeight);

            //#region Cells texts + attributes

            table.SetTextString(0, 0, "Wind Rate:");
            table.SetAlignment(0, 0, Text.alignmentType.TopLeft);
            var baseAtributePoint = table.GetCenter(0, 0);
            var basePoint2 = new Point3D(baseAtributePoint.X+5*pageSize.ScaleFactor*unitsConversionFactor,baseAtributePoint.Y);
            lists.Add(new devDept.Eyeshot.Entities.Attribute(basePoint2, "WindRate", String.Empty, attributeHeight)
            {
                Alignment = Text.alignmentType.MiddleCenter
            });

            table.SetTextString(1, 0, "Roof Material:");
            table.SetAlignment(1, 0, Text.alignmentType.TopLeft);
            baseAtributePoint = table.GetCenter(1, 0);
            basePoint2 = new Point3D(baseAtributePoint.X + 13 * pageSize.ScaleFactor * unitsConversionFactor, baseAtributePoint.Y);
            lists.Add(new devDept.Eyeshot.Entities.Attribute(basePoint2, "RoofMaterial", String.Empty, attributeHeight)
            {
                Alignment = Text.alignmentType.MiddleCenter
            });
            table.SetTextString(3,0,"PLATE HAS BEEN DESIGNED AT ");
            table.SetAlignment(3, 0, Text.alignmentType.TopLeft);
            table.SetTextHeight(3,0,2.3);
            baseAtributePoint = table.GetBottomLeftCorner(3, 0);
            basePoint2 = new Point3D((baseAtributePoint.X+5) * pageSize.ScaleFactor * unitsConversionFactor, (baseAtributePoint.Y+1.2)* pageSize.ScaleFactor*unitsConversionFactor);
            lists.Add(new devDept.Eyeshot.Entities.Attribute(basePoint2, "TieDown", String.Empty, 2.3)
            {
                Alignment = Text.alignmentType.BottomLeft
            });

            //table.SetTextString(1, 0, "SUBTITLE:");
            //table.SetAlignment(1, 0, Text.alignmentType.TopLeft);

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
            lists.Add(table);


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
        private string GetBlockNameString(string string_0, string string_1)
        {
            return string_1 ?? string.Format("{0} {1}", this.Name, string_0);
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
