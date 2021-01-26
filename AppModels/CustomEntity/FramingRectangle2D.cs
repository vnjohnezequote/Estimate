using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using Point = System.Drawing.Point;

namespace AppModels.CustomEntity
{
    public enum CrossTypeLocation
    {
        AtStartPoint,
        AtEndPoint,
        None
    }
    public abstract class FramingRectangle2D : PlanarEntity, IRectangleSolid, IFraming2D
    {
        private Point3D _outerStartPoint;
        //private Point3D _innerStartPoint;
        private Point3D _outerEndPoint;
        //private Point3D _innerEndPoint;
        private IFraming _framingReference;
        private int _thickness;
        public bool IsShowLeader { get; set; }
        public Guid? FramingNameId { get; set; }
        public FramingNameEntity FramingName { get; set; }
        public Guid Id { get; set; }
        public Guid LevelId { get; set; }
        public Guid FramingSheetId { get; set; }
        public Point3D StartPoint { get; private set; }
        public Point3D EndPoint { get; private set; }
        public Point3D OuterStartPoint
        {
            get => _outerStartPoint;
            set
            {
                _outerStartPoint = value;
                if (_outerStartPoint == null || _outerEndPoint == null)
                {
                    return;
                }
                this.RegenFramingGeometry(_outerStartPoint, _outerEndPoint);
            }
        }
        public Point3D InnerStartPoint { get; set; }
        //{
        //    get => _innerStartPoint;
        //    set
        //    {
        //        _innerStartPoint = value;
        //        this.RegenFramingGeometry(_outerStartPoint, _outerEndPoint);
        //    }
        //}
        public Point3D OuterEndPoint
        {
            get => _outerEndPoint;
            set
            {
                _outerEndPoint = value;
                if (_outerStartPoint == null || _outerEndPoint == null)
                {
                    return;
                }
                this.RegenFramingGeometry(_outerStartPoint, _outerEndPoint);
            }
        }
        public Point3D InnerEndPoint { get; set; }
        //{
        //    get => _innerEndPoint;
        //    set
        //    {
        //        _innerEndPoint = value;
        //        this.RegenMode = regenType.RegenAndCompile;
        //    }

        //}
        public Point3D StartTopPoint { get; set; }
        public Point3D EndTopPoint { get; set; }
        public Point3D MidPoint => Point3D.MidPoint(StartPoint, EndPoint);
        public int Thickness
        {
            get => _thickness;
            set
            {
                _thickness = value;
                this.RegenFramingGeometry(_outerStartPoint, _outerEndPoint,Flipped);
            }
        }
        public bool Flipped { get; set; }
        public int Depth { get; set; }
        public Point3D HangerACenterPoint { get; set; }
        public Point3D HangerBCenterPoint { get; set; }
        //public FramingTypes FramingType
        //{
        //    get => FramingReference.FramingType;
        //    set => FramingReference.FramingType = value;
        //}
        public IFraming FramingReference
        {
            get => _framingReference;
            set
            {
                _framingReference = value;
                if (_framingReference != null)
                {
                    _framingReference.PropertyChanged += FramingPropertiesChanged;
                }
            }
        }
        public Guid FramingReferenceId { get; set; }
        public List<Point3D> FramingVertices { get; set; } = new List<Point3D>();
        //public List<Point3D> CenterlineVertices { get; set; } = new List<Point3D>();
        public double FullLength
        {
            get
            {
                if (OuterStartPoint != null && OuterEndPoint != null)
                {
                    return OuterStartPoint.DistanceTo(OuterEndPoint);
                }

                return 0.0;
            }
            set{}
        }
        public bool IsShowFramingName { get; set; }
        public bool IsShowFramingLeader { get; set; }

        #region Constructor

        public FramingRectangle2D(Point3D outerStartPoint, Point3D outerEndPoint, FramingBase framingReference, int thickness = 90, bool flipped = false) : base(Plane.XY)
        {
            Id = Guid.NewGuid();
            LevelId = framingReference.LevelId;
            FramingSheetId = framingReference.FramingSheetId;
            _thickness = thickness;
            _outerStartPoint = outerStartPoint;
            _outerEndPoint = outerEndPoint;
            Flipped = flipped;
            FramingReference = framingReference;
            FramingReferenceId = framingReference.Id;
            InitFramingGeometry(_outerStartPoint, _outerEndPoint, flipped);
        }
        public FramingRectangle2D(Point3D outerStartPoint, Point3D outerEndPoint, int thickness,bool flipped) : base(Plane.XY)
        {
            this._outerStartPoint = outerStartPoint;
            this._outerEndPoint = outerEndPoint;
            _thickness = thickness;
            InitFramingGeometry(_outerStartPoint, _outerEndPoint,flipped);
        }

        protected FramingRectangle2D(FramingRectangle2D another) : base(another)
        {
            Id = Guid.NewGuid();
            LevelId = another.LevelId;
            FramingSheetId = another.FramingSheetId;
           _thickness = another.Thickness;
            _outerStartPoint = (Point3D)another.OuterStartPoint.Clone();
            _outerEndPoint = (Point3D)another.OuterEndPoint.Clone();
            InnerStartPoint = (Point3D)another.InnerStartPoint.Clone();
            InnerEndPoint = (Point3D)another.InnerEndPoint.Clone();
            StartPoint = (Point3D)another.StartPoint.Clone();
            EndPoint = (Point3D)another.EndPoint.Clone();
            FramingReference = (FramingBase)another.FramingReference.Clone();
            FramingReferenceId = FramingReference.Id;
        }


        #endregion

        protected virtual void FramingPropertiesChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(FramingReference.FramingInfo):
                    if (FramingReference.FramingInfo != null)
                    {
                        Thickness = FramingReference.FramingInfo.Depth * FramingReference.FramingInfo.NoItem;
                        Depth = FramingReference.FramingInfo.Thickness;
                        SetFramingColor(Thickness);
                        
                    }
                    break;
                default: break;

            }
        }

        protected abstract void SetFramingColor(int thickness);

        public bool IntersectWith(FramingRectangle2D other, out Point3D farPoint, out Point3D closerPoint)
        {
            var outerSegment = new Segment2D(OuterStartPoint, OuterEndPoint);
            var otherOuterSegment = new Segment2D(other.OuterStartPoint, other.OuterEndPoint);
            farPoint = null;
            closerPoint = null;
            var otherInnerSegment = new Segment2D(other.InnerStartPoint, other.InnerEndPoint);
            var isFirstIntersection = Segment2D.IntersectionLine(outerSegment, otherOuterSegment, out var p1);
            var isSecondIntersection = Segment2D.IntersectionLine(outerSegment, otherInnerSegment, out var p2);
            if (!isFirstIntersection)
            {
                return false;
            }
            else
            {
                if (p1!=null && p2!=null)
                {
                    var distance1 = OuterStartPoint.DistanceTo(p1);
                    var distance2 = OuterStartPoint.DistanceTo(p2);
                    if (distance1>distance2)
                    {
                        closerPoint = p2.ConvertPoint2DtoPoint3D();
                        farPoint = p1.ConvertPoint2DtoPoint3D();
                    }
                    else
                    {
                        closerPoint = p1.ConvertPoint2DtoPoint3D();
                        farPoint = p2.ConvertPoint2DtoPoint3D();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
                
            }

        }

        public bool IntersectWith(FramingRectangle2D other, List<Point3D> intersectPoints, out segmentIntersectionType intersectType)
        {
            var outerSegment = new Segment2D(OuterStartPoint, OuterEndPoint);
            //var innerSegment = new Segment2D(InnerStartPoint, InnerEndPoint);
            var otherOuterSegment = new Segment2D(other.OuterStartPoint, other.OuterEndPoint);
            var otherInnerSegment = new Segment2D(other.InnerStartPoint, other.InnerEndPoint);
            
            var firstInterSectype =
                Segment2D.Intersection(outerSegment, otherOuterSegment, out var p1, out var p2, 0.0001);
            var secondInterSecType =
                Segment2D.Intersection(outerSegment, otherInnerSegment, out var p3, out var p4, 0.0001);
            if (firstInterSectype == segmentIntersectionType.Cross && secondInterSecType == segmentIntersectionType.Cross)
            {
                intersectType = segmentIntersectionType.Cross;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                return true;
            }

            if (firstInterSectype==segmentIntersectionType.Cross&& (secondInterSecType==segmentIntersectionType.Touch|| secondInterSecType == segmentIntersectionType.Disjoint))
            {
                intersectType = segmentIntersectionType.Touch;                                                             
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                return true;
            }

            if ((firstInterSectype== segmentIntersectionType.Touch|| firstInterSectype == segmentIntersectionType.Disjoint) && secondInterSecType == segmentIntersectionType.Cross)
            {
                intersectType = segmentIntersectionType.Touch;
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                return true;
            }

            if (firstInterSectype==segmentIntersectionType.Touch && secondInterSecType == segmentIntersectionType.Touch)
            {
                intersectType = segmentIntersectionType.Cross;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                return true;
            }

            if (firstInterSectype==segmentIntersectionType.Touch && (secondInterSecType == segmentIntersectionType.EndPointTouch || secondInterSecType == segmentIntersectionType.Disjoint))
            {
                intersectType = segmentIntersectionType.EndPointTouch;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                return true;
            }

            if ((firstInterSectype == segmentIntersectionType.Disjoint || firstInterSectype == segmentIntersectionType.EndPointTouch) && secondInterSecType == segmentIntersectionType.Touch)
            {
                intersectType = segmentIntersectionType.EndPointTouch;
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                return true;
            }

            intersectType = segmentIntersectionType.Disjoint;
            return false;
        }

        public bool IntersectWith(Line line, out Point3D intersectPoint, out segmentIntersectionType intersctionType)
        {
            var thisSegment = new Segment2D(OuterStartPoint, OuterEndPoint);
            var otherSegment = new Segment2D(line.StartPoint, line.EndPoint);
            intersctionType = Segment2D.Intersection(thisSegment, otherSegment, out var p1, out var p2, 0.0001);
            if (intersctionType == segmentIntersectionType.Cross)
            {
                intersectPoint = p1.ConvertPoint2DtoPoint3D();
                return true;
            }

            if (intersctionType == segmentIntersectionType.EndPointTouch)
            {
                intersectPoint = p1.ConvertPoint2DtoPoint3D();
                return true;

            }

            if (intersctionType == segmentIntersectionType.Touch)
            {
                intersectPoint = p1.ConvertPoint2DtoPoint3D();
                return true;
            }

            intersectPoint = null;
            return false;

        }
        public bool IntersectWith(Line line, out Point3D intersectPoint)
        {
            var thisSegment = new Segment2D(OuterStartPoint, OuterEndPoint);
            var otherSegment = new Segment2D(line.StartPoint, line.EndPoint);
            var result = Segment2D.IntersectionLine(thisSegment, otherSegment, out var tempIntersectPoint);
            intersectPoint = tempIntersectPoint.ConvertPoint2DtoPoint3D();
            return result;

        }
        public bool IsPointInside(Point3D point)
        {
            var linearPath = new List<Point3D>()
                {OuterStartPoint, OuterEndPoint, InnerEndPoint, InnerStartPoint, OuterStartPoint};
            var quad = Mesh.CreatePlanar(linearPath, Mesh.natureType.Plain);
            return quad.IsPointInside(point);
        }

        private void Extend(double atStart, double atEnd)
        {
            var segmentFirstLine = new Segment2D(_outerStartPoint, _outerEndPoint);
            segmentFirstLine.ExtendBy(atStart,atEnd);
            _outerStartPoint = segmentFirstLine.P0.ConvertPoint2DtoPoint3D();
            _outerEndPoint = segmentFirstLine.P1.ConvertPoint2DtoPoint3D();
            this.RegenFramingGeometry(_outerStartPoint,_outerEndPoint);
        }
        private void InitFramingGeometry(Point3D outerStartPoint, Point3D outerEndPoint, bool flipped = false)
        {
            RegenFramingGeometry(outerStartPoint, outerEndPoint, flipped);
        }
        protected virtual void RegenFramingGeometry(Point3D outerStartPoint, Point3D outerEndPoint, bool flipped = false)
        {
            var flippedFactor = 1;
            if (flipped)
            {
                flippedFactor = -1;
            }
            var outerLine = new Segment2D(OuterStartPoint, OuterEndPoint);
            var interLine = outerLine.Offset(Thickness * flippedFactor);
            InnerStartPoint = interLine.P0.ConvertPoint2DtoPoint3D();
            InnerEndPoint = interLine.P1.ConvertPoint2DtoPoint3D();
            var centerLine = outerLine.Offset((double)Thickness * flippedFactor / 2);
            var centerCircleLine = outerLine.Offset((double)Thickness * flippedFactor / 2);
            StartTopPoint = centerLine.P0.ConvertPoint2DtoPoint3D();
            EndTopPoint = centerLine.P1.ConvertPoint2DtoPoint3D();
            centerLine.ExtendBy(-(double)Thickness / 2, -(double)Thickness / 2);
            StartPoint = centerLine.P0.ConvertPoint2DtoPoint3D();
            EndPoint = centerLine.P1.ConvertPoint2DtoPoint3D();
            centerCircleLine.ExtendBy(-140,-140);
            HangerACenterPoint = centerCircleLine.P0.ConvertPoint2DtoPoint3D();
            HangerBCenterPoint = centerCircleLine.P1.ConvertPoint2DtoPoint3D();
            this.UpdateDistance();
            this.SetFramingColor(Thickness);
            this.RegenMode = regenType.RegenAndCompile;
        }
        protected void UpdateDistance()
        {
            if (this.OuterStartPoint != null && this.OuterEndPoint != null)
            {
                if (this.FramingReference != null)
                {
                    this.FramingReference.FullLength = (int)FullLength;
                }
            }
        }
        protected override void Draw(DrawParams data)
        {
            DrawFraming(data, true, false);
        }
        protected virtual void DrawFraming(DrawParams data, bool disableCulling, bool drawingForSelection)
        {
            PreDraw(data);
            this.DrawExtraGeometry(data);
            RenderContextBase renderContext = data.RenderContext;
            renderContext.PushModelView();
            if (renderContext.IsDirect3D)
            {
                this.PushLineSize(data);
                this.DrawEntity(renderContext, null);
                this.PopLineSize(data);
            }
            else
            {
                renderContext.Draw(this.drawData, primitiveType.Undefined);
            }

            renderContext.PopModelView();

            PostDraw(data);
        }
        protected override void DrawEntity(RenderContextBase context, object myParams)
        {
            if (base.Compiling)
            {
                context.DrawLine(StartPoint, EndPoint);
            }
            else
            {
                this.drawData.DrawD3D(context, 0);
            }

            context.PushModelView();
            DrawBackGroundRectangle(context);
            context.PopModelView();
        }
        protected virtual void DrawBackGroundRectangle(RenderContextBase context)
        {
            if (context.CurrentLineWidth > 1f)
            {
                context.SetLineSize(1f, true, false);
            }
            this.DrawRectangle(context);
        }
        protected virtual void DrawRectangle(RenderContextBase context)
        {
            context.PushModelView();
            var points = new Point3D[] { OuterStartPoint, OuterEndPoint, InnerEndPoint, InnerStartPoint };
            context.DrawQuads(points, new Vector3D[]
            {
                Vector3D.AxisZ
            });
            //if (this.Compiling)
            //{
            //    context.DrawQuads(points, new Vector3D[]
            //    {
            //        Vector3D.AxisZ
            //    });
            //}
            //else
            //{
            //    this.drawData.DrawD3D(context,true);
            //}
            context.PopModelView();
        }
        protected override void DrawForSelection(GfxDrawForSelectionParams data)
        {
            this.DrawFraming(data, false, true);
        }
        private void PreDraw(DrawParams data)
        {
            data.RenderContext.PushRasterizerState();
            data.RenderContext.SetRasterizerState(rasterizerPolygonDrawingType.Fill, rasterizerCullFaceType.None);
        }
        private void PostDraw(DrawParams data)
        {
            data.RenderContext.PopRasterizerState();
        }
        internal void PopLineSize(DrawParams drawParams)
        {
            if (drawParams.RenderContext.CurrentLineWidth > 1f)
            {
                drawParams.RenderContext.PopShader();
            }
        }
        internal void PushLineSize(DrawParams drawParams)
        {
            if (drawParams.RenderContext.CurrentLineWidth > 1f)
            {
                drawParams.RenderContext.PushShader();
                drawParams.RenderContext.EnableThickLines();
            }
        }
        protected virtual void DrawExtraGeometry(DrawParams data)
        {

        }
        public override void Regen(RegenParams data)
        {
            base.Regen(data);
            var listPoint = new List<Point3D>();
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
            FramingVertices.Clear();

            FramingVertices.Add(OuterStartPoint);
            FramingVertices.Add(InnerStartPoint);
            FramingVertices.Add(InnerEndPoint);
            FramingVertices.Add(OuterEndPoint);
            base.UpdateBoundingBox(data);
            this.RegenMode = regenType.CompileOnly;
        }
        public override void TransformBy(Transformation transform)
        {
            var joistLine = new Line(this.OuterStartPoint, this.OuterEndPoint);
            joistLine.TransformBy(transform);
            this._outerStartPoint = joistLine.StartPoint;
            this._outerEndPoint = joistLine.EndPoint;
            this.RegenFramingGeometry(_outerStartPoint, _outerEndPoint, false);
        }
        public override void Translate(double dx, double dy, double dz = 0)
        {
            var joistLine = new Line((Point3D)this.OuterStartPoint.Clone(), (Point3D)this.OuterEndPoint.Clone());
            joistLine.Translate(dx, dy, dz);
            this._outerStartPoint = joistLine.StartPoint;
            this._outerEndPoint = joistLine.EndPoint;
            this.RegenFramingGeometry(OuterStartPoint, OuterEndPoint);
        }
        public bool Project(Point3D point, out double t)
        {
            Segment3D segment = new Segment3D(this.OuterStartPoint, this.OuterEndPoint);
            Line joistLine = new Line(this.OuterStartPoint, this.OuterEndPoint);
            return joistLine.Project(point, out t);
        }
        public Point3D PointAt(double t)
        {
            var joitsLine = new Line(OuterStartPoint, OuterEndPoint);
            return joitsLine.PointAt(t);
        }
        public FramingRectangle2D Offset(double amount, Vector3D planeNormal, double tolerance, bool sharp)
        {
            FramingRectangle2D framing = (FramingRectangle2D)Clone();
            Line framingLine = new Line(framing.OuterStartPoint, framing.OuterEndPoint);
            Vector3D vector3D = Vector3D.Cross(framingLine.Tangent, planeNormal);
            vector3D.Normalize();
            Vector3D v = vector3D * amount;
            framingLine.Translate(v);
            framing.OuterStartPoint = framingLine.StartPoint;
            framing.OuterEndPoint = framingLine.EndPoint;
            return framing;
        }
        public FramingRectangle2D Offset(double amount, Vector3D planeNormal)
        {
            return this.Offset(amount, planeNormal, 0.0, false);
        }
        protected void SetThickness(int thickness)
        {
            _thickness = thickness;
        }
        protected void SetOuterStartPoint(Point3D point)
        {
            _outerStartPoint = point;
        }
        protected void SetOuterEndPoint(Point3D point)
        {
            _outerEndPoint = point;
        }
        //public void SetFramingType(FramingTypes framingType)
        //{
        //    FramingType = framingType;
        //}

    }
}
