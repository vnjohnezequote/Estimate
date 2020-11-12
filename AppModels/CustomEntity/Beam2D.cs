using System;
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
        private Point3D _outerStartPoint;
        private Point3D _innerStartPoint;
        private Point3D _outerEndPoint;
        private Point3D _innerEndPoint;
        private Beam _beamReference;
        private int _thickness;
        private int _depth;
        private bool _isBeamUnder;
        private bool _outTriggerAFlipped;
        private bool _outTriggerBFlipped;
        #endregion

        #region Properties
        public Guid Id { get; set; }
        public string SheetName { get; set; }
        public Point3D StartPoint { get; set; }
        public Point3D EndPoint { get; set; }
        public int Thickness
        {
            get => _thickness;
            set
            {
                _thickness = value;
                this.RegenGeometry(this.OuterStartPoint,this.OuterEndPoint);
                this.RegenMode = regenType.RegenAndCompile;
            }
        }
        public int Depth
        {
            get => _depth;
            set
            {
                _depth = value;
                this.RegenGeometry(this.OuterStartPoint, this.OuterEndPoint);
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
        public Point3D OuterStartPoint
        {
            get => _outerStartPoint;
            set
            {
                _outerStartPoint = value;
                this.RegenMode = regenType.RegenAndCompile;
            }
        }
        public Point3D InnerStartPoint
        {
            get => _innerStartPoint;
            set
            {
                _innerStartPoint = value;
                this.RegenMode = regenType.RegenAndCompile;
            }
        }
        public Point3D OuterEndPoint
        {
            get => _outerEndPoint;
            set
            {
                _outerEndPoint = value;
                this.RegenMode = regenType.RegenAndCompile;
            }
        }
        public Point3D InnerEndPoint
        {
            get => _innerEndPoint;
            set
            {
                _innerEndPoint = value;
                this.RegenMode = regenType.RegenAndCompile;
            }

        }
        public string HangerAId { get; set; }
        public string HangerBId { get; set; }
        public Hanger2D HangerA { get; set; }
        public Hanger2D HangerB { get; set; }
        public bool IsHangerA { get; set; }
        public bool IsHangerB { get; set; }
        public string OutTriggerAId { get; set; }
        public string OutTriggerBId { get; set; }
        public OutTrigger2D OutTriggerA { get; set; }
        public OutTrigger2D OutTriggerB { get; set; }
        public bool IsOutTriggerA { get; set; }
        public bool IsOutTriggerB { get; set; }
        public bool OutTriggerAFlipped
        {
            get => _outTriggerAFlipped;
            set
            {
                _outTriggerAFlipped = value;

                if (this.OutTriggerA != null)
                {
                    OutTriggerA.Flipper(StartPoint, EndPoint);
                }
            }
        }
        public bool OutTriggerBFlipped
        {
            get => _outTriggerBFlipped;
            set
            {
                if (this.OutTriggerB != null)
                {
                    _outTriggerBFlipped = value;

                    if (this.OutTriggerB != null)
                    {
                        OutTriggerB.Flipper(StartPoint, EndPoint);
                    }
                }
            }
        }
        public double FullLength { get; set; }

        public List<Segment2D> BoxLines { get; set; } = new List<Segment2D>();
        public List<Point3D> BeamVertices { get; set; } = new List<Point3D>();
        public List<Point3D> CenterlineVertices { get; set; } = new List<Point3D>();

        #endregion
        public Beam2D(Plane wallPlan, Point3D startPoint, Point3D endPoint,Beam beamRef,
            int thickness = 45, bool isBeamUnder = true, bool showDimension = false, double textHeight = 90) :
            base(wallPlan, startPoint, textHeight, Text.alignmentType.BaselineCenter)
        {
            Id = new Guid();
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
            OuterStartPoint = startPoint;
            OuterEndPoint = endPoint;
            Line1 = new Segment2D(OuterStartPoint,OuterEndPoint);
            Line2 = Line1.Offset(Thickness);
            InnerStartPoint = Line2.P0.ConvertPoint2DtoPoint3D();
            InnerEndPoint = Line2.P1.ConvertPoint2DtoPoint3D();

            CenterLine = Line1.Offset((double)Thickness / 2);
            CenterLine.ExtendBy(-(double)Thickness/2,-(double)Thickness/2);

            StartPoint = CenterLine.P0.ConvertPoint2DtoPoint3D();
            EndPoint = CenterLine.P1.ConvertPoint2DtoPoint3D();

            var offsetLine = Line1.Offset(-(double)Thickness / 2);
            DimensionLine = offsetLine;

            var segment1 = new Segment2D((Point3D)OuterStartPoint.Clone(), (Point3D)OuterEndPoint.Clone());
            segment1.ExtendBy(-Thickness, -Thickness);
            var segment2 = new Segment2D((Point3D)InnerStartPoint.Clone(), (Point3D)InnerEndPoint.Clone());
            segment2.ExtendBy(-Thickness, -Thickness);
            var p1 = segment1.P0;
            var p2 = segment2.P0;
            var p3 = segment1.P1;
            var p4 = segment2.P1;

            StartVerticesBox = new List<Point3D>() { OuterStartPoint, p1.ConvertPoint2DtoPoint3D(), p2.ConvertPoint2DtoPoint3D(), InnerStartPoint };
            EndVerticesBox = new List<Point3D>() { OuterEndPoint, InnerEndPoint, p4.ConvertPoint2DtoPoint3D(), p3.ConvertPoint2DtoPoint3D() };

            var tempLine = new Segment2D(OuterStartPoint, InnerStartPoint);
            StartVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            tempLine = new Segment2D(p1, p2);
            StartVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            tempLine = new Segment2D(OuterStartPoint, p1);
            StartVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            tempLine = new Segment2D(InnerStartPoint, p2);
            StartVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            tempLine = new Segment2D(OuterEndPoint, InnerEndPoint);
            EndVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            tempLine = new Segment2D(p3, p4);
            EndVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            tempLine = new Segment2D(OuterEndPoint, p3);
            EndVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            tempLine = new Segment2D(InnerEndPoint, p4);
            EndVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
            this.UpdateDistance();
        }
        protected override void Draw(DrawParams data)
        {
            var points = new Point3D[] { OuterStartPoint, OuterEndPoint, InnerEndPoint, InnerStartPoint };
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
        //        data.RenderContext.DrawBufferedLine(OuterStartPoint, InnerStartPoint);
        //        //data.RenderContext.SetColorWireframe(Color.Crimson);
        //        data.RenderContext.DrawBufferedLine(OuterEndPoint, InnerEndPoint);
        //        //data.RenderContext.SetColorWireframe(Color.Crimson, true);
        //        this.DrawDashWallLine(data, OuterStartPoint, OuterEndPoint);
        //        //data.RenderContext.SetColorWireframe(Color.Crimson);
        //        this.DrawDashWallLine(data, InnerStartPoint, InnerEndPoint);
        //        data.RenderContext.DrawBufferedLine(StartVerticesBox[1], StartVerticesBox[2]);
        //        data.RenderContext.DrawBufferedLine(EndVerticesBox[2], EndVerticesBox[3]);
        //    }
        //    else
        //    {
        //        //data.RenderContext.SetColorWireframe(Color.Crimson);
        //        data.RenderContext.DrawBufferedLine(OuterStartPoint, InnerStartPoint);
        //        //data.RenderContext.SetColorWireframe(Color.Crimson);
        //        data.RenderContext.DrawBufferedLine(OuterEndPoint, InnerEndPoint);
        //        //data.RenderContext.SetColorWireframe(Color.Crimson);
        //        data.RenderContext.DrawBufferedLine(OuterStartPoint, OuterEndPoint);
        //        //data.RenderContext.SetColorWireframe(Color.Crimson);
        //        data.RenderContext.DrawBufferedLine(InnerStartPoint, InnerEndPoint);
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

            if (!listPoint.Contains(OuterStartPoint))
            {
                listPoint.Add(OuterStartPoint);
            }
            if (!listPoint.Contains(InnerStartPoint))
            {
                listPoint.Add(InnerStartPoint);
            }
            if (!listPoint.Contains(OuterEndPoint))
            {
                listPoint.Add(OuterEndPoint);
            }
            if (!listPoint.Contains(InnerEndPoint))
            {
                listPoint.Add(InnerEndPoint);
            }
            Vertices = listPoint.ToArray();
            BeamVertices.Clear();
            BeamVertices.Add(OuterStartPoint);
            BeamVertices.Add(InnerStartPoint);
            BeamVertices.Add(InnerEndPoint);
            BeamVertices.Add(OuterEndPoint);
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
        public IEntityVm CreateEntityVm(IEntitiesManager entitiesManager)
        {
            return new Beam2DVm(this,entitiesManager);
        }

        private void UpdateDistance()
        {
            if (this.OuterStartPoint != null && this.OuterEndPoint != null)
            {
                this.FullLength = OuterStartPoint.DistanceTo(OuterEndPoint);
                if (this.BeamReference != null)
                {
                    //this.JoistReference.FullLength = (int)FullLength;
                }
            }
        }
    }
}
