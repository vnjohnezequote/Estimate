using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.Openings;
using AppModels.ViewModelEntity;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Graphics;
using Plane = devDept.Geometry.Plane;
using Point3D = devDept.Geometry.Point3D;
using Segment2D = devDept.Geometry.Segment2D;
using Vector3D = devDept.Geometry.Vector3D;

namespace AppModels.CustomEntity
{
    public class Beam2D: Text,IEntityVmCreateAble
    {
        #region private Field
        private Point3D _startPoint1;
        private Point3D _startPoint2;
        private Point3D _endPoint1;
        private Point3D _endPoint2;
        private Beam _beamReference;
        private int _thickness;
        private int _depth;
        private bool _isBeamUnder;


        #endregion

        #region Properties
        public Point3D StartPoint { get; set; }
        public Point3D EndPoint { get; set; }
        public int Thickness
        {
            get => _thickness;
            set
            {
                _thickness = value;
                this.RegenGeometry(this.StartPoint1,this.EndPoint1);
                this.RegenMode = regenType.RegenAndCompile;
            }
        }
        public int Depth
        {
            get => _depth;
            set
            {
                _depth = value;
                this.RegenGeometry(this.StartPoint1, this.EndPoint1);
                this.RegenMode = regenType.RegenAndCompile;
            }
        }
        public bool IsBeamUnder {
            get=>_isBeamUnder;
            set
            {
                _isBeamUnder = value;
                if (_isBeamUnder )
                {
                    Color = Color.FromArgb(100, 255, 0, 127);
                }

                else
                {
                    Color = Color.Crimson;
                }
            }
        }
        public bool ShowDimension { get; set; }
        public Segment2D CenterLine { get; set; }
        public Segment2D Line1 { get; set; }
        public Segment2D Line2 { get; set; }
        public Segment2D DimensionLine { get; set; }
        public Beam BeamReference { get; private set; }
        public List<Point3D> StartVerticesBox { get; set; }
        public List<Point3D> EndVerticesBox { get; set; }
        public Point3D StartPoint1
        {
            get => _startPoint1;
            set
            {
                _startPoint1 = value;
                this.RegenMode = regenType.RegenAndCompile;
            }
        }
        public Point3D StartPoint2
        {
            get => _startPoint2;
            set
            {
                _startPoint2 = value;
                this.RegenMode = regenType.RegenAndCompile;
            }
        }
        public Point3D EndPoint1
        {
            get => _endPoint1;
            set
            {
                _endPoint1 = value;
                this.RegenMode = regenType.RegenAndCompile;
            }
        }
        public Point3D EndPoint2
        {
            get => _endPoint2;
            set
            {
                _endPoint2 = value;
                this.RegenMode = regenType.RegenAndCompile;
            }

        }
        public List<Segment2D> BoxLines { get; set; } = new List<Segment2D>();
        public List<Point3D> BeamVertices { get; set; } = new List<Point3D>();
        public List<Point3D> CenterlineVertices { get; set; } = new List<Point3D>();
        #endregion
        public Beam2D(Plane wallPlan, Point3D startPoint, Point3D endPoint,Beam beamRef,
            int thickness = 45, bool isBeamUnder = true, bool showDimension = false, double textHeight = 90) :
            base(wallPlan, startPoint, textHeight, Text.alignmentType.BaselineCenter)
        {
            ColorMethod = colorMethodType.byEntity;
            if (isBeamUnder)
            {
                Color = Color.FromArgb(100,255,0,127);
            }

            else
            {
                Color = Color.Crimson;
            }
            _thickness = thickness;
            IsBeamUnder = isBeamUnder;
            ShowDimension = showDimension;
            this.BeamReference = beamRef;
            if (BeamReference.TimberInfo!=null )
            {
                _depth = BeamReference.Thickness;
            }
            BeamReference.PropertyChanged += BeramReferencePropertiesChanged;
            RegenGeometry(startPoint, endPoint);
            this.LineTypeScale = 10;
        }
        private void BeramReferencePropertiesChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(BeamReference.TimberInfo):
                    if (BeamReference.TimberInfo!=null)
                    {
                        Thickness = BeamReference.Depth * BeamReference.NoItem;
                        Depth = BeamReference.Thickness;
                    }
                    
                    break;

                default: break;
            }
        }

        private void RegenGeometry(Point3D startPoint, Point3D endPoint)
        {
            StartPoint1 = startPoint;
            EndPoint1 = endPoint;
            Line1 = new Segment2D(StartPoint1,EndPoint1);
            Line2 = Line1.Offset(Thickness);
            StartPoint2 = Line2.P0.ConvertPoint2DtoPoint3D();
            EndPoint2 = Line2.P1.ConvertPoint2DtoPoint3D();

            CenterLine = Line1.Offset((double)Thickness / 2);
            CenterLine.ExtendBy(-(double)Thickness/2,-(double)Thickness/2);

            StartPoint = CenterLine.P0.ConvertPoint2DtoPoint3D();
            EndPoint = CenterLine.P1.ConvertPoint2DtoPoint3D();

            var offsetLine = Line1.Offset(-(double)Thickness / 2);
            DimensionLine = offsetLine;

            var segment1 = new Segment2D((Point3D)StartPoint1.Clone(), (Point3D)EndPoint1.Clone());
            segment1.ExtendBy(-Thickness, -Thickness);
            var segment2 = new Segment2D((Point3D)StartPoint2.Clone(), (Point3D)EndPoint2.Clone());
            segment2.ExtendBy(-Thickness, -Thickness);
            var p1 = segment1.P0;
            var p2 = segment2.P0;
            var p3 = segment1.P1;
            var p4 = segment2.P1;

            StartVerticesBox = new List<Point3D>() { StartPoint1, p1.ConvertPoint2DtoPoint3D(), p2.ConvertPoint2DtoPoint3D(), StartPoint2 };
            EndVerticesBox = new List<Point3D>() { EndPoint1, EndPoint2, p4.ConvertPoint2DtoPoint3D(), p3.ConvertPoint2DtoPoint3D() };

            var tempLine = new Segment2D(StartPoint1, StartPoint2);
            StartVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            tempLine = new Segment2D(p1, p2);
            StartVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            tempLine = new Segment2D(StartPoint1, p1);
            StartVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            tempLine = new Segment2D(StartPoint2, p2);
            StartVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            tempLine = new Segment2D(EndPoint1, EndPoint2);
            EndVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            tempLine = new Segment2D(p3, p4);
            EndVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            tempLine = new Segment2D(EndPoint1, p3);
            EndVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            tempLine = new Segment2D(EndPoint2, p4);
            EndVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
        }
        protected override void Draw(DrawParams data)
        {
            var points = new Point3D[] { StartPoint1, EndPoint1, EndPoint2, StartPoint2 };
            //data.RenderContext.SetState(depthStencilStateType.DepthTestOff);
            data.RenderContext.PushRasterizerState();
            data.RenderContext.SetRasterizerState(rasterizerPolygonDrawingType.Fill, rasterizerCullFaceType.None);
            data.RenderContext.PushModelView();
            data.RenderContext.DrawQuads(points, new Vector3D[]
                {
                    Vector3D.AxisZ
                });
            data.RenderContext.PopModelView();
            data.RenderContext.PopRasterizerState();
            if (ShowDimension)
            {
                this.DrawText(data);
            }

        }
        private void DrawDashWallLine(DrawParams data, Point3D startPoint, Point3D endPoint)
        {
            var lineTypePattername = this.LineTypeName;
            var lineTypePattern = data.LineTypes[lineTypePattername];
            if (lineTypePattern.Length * this.LineTypeScale > data.ScreenToWorld4Times && data.ScreenToWorld > 0f)
            {
                float num = 1f;
                if (data.FullParents != null && data.FullParents.Count > 0)
                {
                    num = (float)data.FullParents.Peek().GetFullTransformation(data.Blocks).ScaleFactorX;
                }

                List<Point3D> list;
                List<Point3D> list2;
                var vertices = new Point3D[] { startPoint, endPoint };
                lineTypePattern.GetPatternVertices(data.MaxPatternRepetitions, vertices, this.LineTypeScale / num, out list, out list2);
                for (int i = 0; i < list.Count; i += 2)
                {
                    data.RenderContext.DrawBufferedLine(list[i], list[i + 1]);
                }
                data.RenderContext.DrawPointsOnTheFly(list2.ToArray());
            }

        }

        //protected override void DrawEdges(DrawParams data)
        //{
        //    base.DrawEdges(data);
        //    data.RenderContext.SetLineSize(2);
        //    //data.RenderContext.SetColorWireframe(Color.Crimson);
        //    data.RenderContext.PushModelView();
        //    if (this.IsBeamUnder)
        //    {
        //        //data.RenderContext.SetColorWireframe(Color.Crimson);
        //        data.RenderContext.DrawBufferedLine(StartPoint1, StartPoint2);
        //        //data.RenderContext.SetColorWireframe(Color.Crimson);
        //        data.RenderContext.DrawBufferedLine(EndPoint1, EndPoint2);
        //        //data.RenderContext.SetColorWireframe(Color.Crimson, true);
        //        this.DrawDashWallLine(data, StartPoint1, EndPoint1);
        //        //data.RenderContext.SetColorWireframe(Color.Crimson);
        //        this.DrawDashWallLine(data, StartPoint2, EndPoint2);
        //        data.RenderContext.DrawBufferedLine(StartVerticesBox[1], StartVerticesBox[2]);
        //        data.RenderContext.DrawBufferedLine(EndVerticesBox[2], EndVerticesBox[3]);
        //    }
        //    else
        //    {
        //        //data.RenderContext.SetColorWireframe(Color.Crimson);
        //        data.RenderContext.DrawBufferedLine(StartPoint1, StartPoint2);
        //        //data.RenderContext.SetColorWireframe(Color.Crimson);
        //        data.RenderContext.DrawBufferedLine(EndPoint1, EndPoint2);
        //        //data.RenderContext.SetColorWireframe(Color.Crimson);
        //        data.RenderContext.DrawBufferedLine(StartPoint1, EndPoint1);
        //        //data.RenderContext.SetColorWireframe(Color.Crimson);
        //        data.RenderContext.DrawBufferedLine(StartPoint2, EndPoint2);
        //        data.RenderContext.DrawBufferedLine(StartVerticesBox[1], StartVerticesBox[2]);
        //        data.RenderContext.DrawBufferedLine(EndVerticesBox[2], EndVerticesBox[3]);
        //    }
        //    data.RenderContext.PopModelView();
        //}
        public override void Regen(RegenParams data)
        {
            var distance = (int)StartPoint.DistanceTo(EndPoint) - Thickness;
            this.TextString = distance.ToString();
            this.InsertionPoint = new Point3D(DimensionLine.MidPoint.X, DimensionLine.MidPoint.Y, 0);
            base.Regen(data);
            var listPoint = new List<Point3D>(Vertices);
            if (!listPoint.Contains(this.StartPoint))
            {
                listPoint.Add(StartPoint);
            }
            if (!listPoint.Contains(this.EndPoint))
            {
                listPoint.Add(EndPoint);
            }

            if (!listPoint.Contains(StartPoint1))
            {
                listPoint.Add(StartPoint1);
            }
            if (!listPoint.Contains(StartPoint2))
            {
                listPoint.Add(StartPoint2);
            }
            if (!listPoint.Contains(EndPoint1))
            {
                listPoint.Add(EndPoint1);
            }
            if (!listPoint.Contains(EndPoint2))
            {
                listPoint.Add(EndPoint2);
            }
            Vertices = listPoint.ToArray();
            BeamVertices.Clear();
            BeamVertices.Add(StartPoint1);
            BeamVertices.Add(StartPoint2);
            BeamVertices.Add(EndPoint2);
            BeamVertices.Add(EndPoint1);
            base.UpdateBoundingBox(data);
            this.RegenMode = regenType.CompileOnly;
        }
        public WallIntersectionType GetInterSection(Wall2D other, out Point3D intersectionPoint, out LinearPath centerLinePath)
        {
            intersectionPoint = null;
            centerLinePath = null;
            foreach (var otherCenterlineVertex in other.CenterlineVertices)
            {
                if (this.CenterlineVertices.Contains(otherCenterlineVertex))
                {
                    intersectionPoint = otherCenterlineVertex;
                    if (intersectionPoint == this.StartPoint && intersectionPoint == other.StartPoint)
                    {
                        if (intersectionPoint == other.StartPoint)
                        {
                            var points = new List<Point3D>() { this.EndPoint, intersectionPoint, other.EndPoint };
                            centerLinePath = new LinearPath(points.ToArray());
                        }
                        else
                        {
                            var points = new List<Point3D>() { this.EndPoint, intersectionPoint, other.StartPoint };
                            centerLinePath = new LinearPath(points.ToArray());
                        }
                    }
                    else if (intersectionPoint == this.EndPoint)
                    {
                        if (intersectionPoint == other.StartPoint)
                        {
                            var points = new List<Point3D>() { this.StartPoint, intersectionPoint, other.EndPoint };
                            centerLinePath = new LinearPath(points.ToArray());
                        }
                        else
                        {
                            var points = new List<Point3D>() { this.StartPoint, intersectionPoint, other.StartPoint };
                            centerLinePath = new LinearPath(points.ToArray());
                        }
                    }
                    return WallIntersectionType.AtVertice;
                }
            }
            return WallIntersectionType.None;
        }
        public IEntityVm CreateEntityVm()
        {
            return new Beam2DVm(this);
        }
    }
}
