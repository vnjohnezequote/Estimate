using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using AppModels.EntityCreator.Interface;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings;
using AppModels.Undo;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Milling;
using devDept.Geometry;
using devDept.Graphics;
using Point = System.Drawing.Point;
using Region = System.Drawing.Region;

namespace AppModels.CustomEntity
{
    public enum CrossTypeLocation
    {
        AtStartPoint,
        AtEndPoint,
        None
    }
    public abstract class FramingRectangle2D : PlanarEntity, IRectangleSolid, IFraming2D,IDependencyUndoEntity
    {
        private Point3D _outerStartPoint;
        //private Point3D _innerStartPoint;
        private Point3D _outerEndPoint;
        //private Point3D _innerEndPoint;
        private IFraming _framingReference;
        private int _thickness;
        public bool IsShowLeader { get; set; }
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
                this.RegenFramingGeometry(_outerStartPoint, _outerEndPoint,Flipped);
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
                this.RegenFramingGeometry(_outerStartPoint, _outerEndPoint,Flipped);
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
        public Point3D StartCenterLinePoint { get; set; }
        public Point3D EndCenterLinePoint { get; set; }
        public Point3D MidPoint => Point3D.MidPoint(StartPoint, EndPoint);
        public int Thickness
        {
            get => _thickness;
            set
            {
                _thickness = value;
                this.RegenFramingGeometry(_outerStartPoint, _outerEndPoint, Flipped);
            }
        }
        public bool Flipped { get; set; }
        public int Depth { get; set; }
        public Point3D HangerACenterPoint { get; set; }
        public Point3D HangerBCenterPoint { get; set; }
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
            set { }
        }
        public bool IsShowFramingLeader { get; set; }

        public FramingRectangeDirectionType FramingDirectionType
        {
            get
            {
                if (StartCenterLinePoint == null || EndCenterLinePoint == null)
                {
                    return FramingRectangeDirectionType.Freedom;
                }
                if (Math.Abs(StartCenterLinePoint.X - EndCenterLinePoint.X) < 0.01)
                {
                    return FramingRectangeDirectionType.Horizontal;
                }
                else if(Math.Abs(StartCenterLinePoint.Y - EndCenterLinePoint.Y)<0.01)
                {
                    return FramingRectangeDirectionType.Vertical;
                }

                return FramingRectangeDirectionType.Freedom;
            }
        }

        #region Constructor

        public FramingRectangle2D(Point3D outerStartPoint, Point3D outerEndPoint, IFraming framingReference, int thickness = 90, bool flipped = false,bool centerCreator = false) : base(Plane.XY)
        {
            Id = Guid.NewGuid();
            LevelId = framingReference.LevelId;
            FramingSheetId = framingReference.FramingSheetId;
            _thickness = thickness;
            if (centerCreator)
            {
                var centerLine = new Segment2D(outerStartPoint, outerEndPoint);
                var outnerLine = centerLine.Offset(-(double)thickness / 2);
                _outerStartPoint = outnerLine.P0.ConvertPoint2DtoPoint3D();
                _outerEndPoint = outnerLine.P1.ConvertPoint2DtoPoint3D();

            }
            else
            {
                _outerStartPoint = outerStartPoint;
                _outerEndPoint = outerEndPoint;
            }
            
            Flipped = flipped;
            FramingReference = framingReference;
            FramingReferenceId = framingReference.Id;
            InitFramingGeometry(_outerStartPoint, _outerEndPoint, flipped);
        }
        public FramingRectangle2D(Point3D outerStartPoint, Point3D outerEndPoint, int thickness, bool flipped,bool centerCreator) : base(Plane.XY)
        {
            if (centerCreator)
            {
                var centerLine = new Segment2D(outerStartPoint, outerEndPoint);
                var outnerLine = centerLine.Offset(-thickness / 2);
                _outerStartPoint = outnerLine.P0.ConvertPoint2DtoPoint3D();
                _outerEndPoint = outnerLine.P1.ConvertPoint2DtoPoint3D();
            }
            else
            {
                this._outerStartPoint = outerStartPoint;
                this._outerEndPoint = outerEndPoint;
            }
            
            _thickness = thickness;
            InitFramingGeometry(_outerStartPoint, _outerEndPoint, flipped);
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
            //Flipped = another.Flipped;
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
                if (p1 != null && p2 != null)
                {
                    var distance1 = OuterStartPoint.DistanceTo(p1);
                    var distance2 = OuterStartPoint.DistanceTo(p2);
                    if (distance1 > distance2)
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
        public bool IntersectWith(Line lineBreak, out Point3D intersectPoint, out segmentIntersectionType intersectType)
        {
            var centerLine = new Segment2D(StartCenterLinePoint, EndCenterLinePoint);
            var lineBreakSegment = new Segment2D(lineBreak.StartPoint,lineBreak.EndPoint);
            intersectType =
                Segment2D.Intersection(centerLine, lineBreakSegment, out var p1, out var p2, 0.0001);
            if (intersectType == segmentIntersectionType.Disjoint)
            {
                intersectPoint = null;
                return false;
            }

            intersectPoint = p1.ConvertPoint2DtoPoint3D();
            return true;
            
        }
        public bool IntersectWith(FramingRectangle2D other, List<Point3D> intersectPoints, out FramingIntersectionType intersectType)
        {
            var outerSegment = new Segment2D(OuterStartPoint, OuterEndPoint);
            var interSegment = new Segment2D(InnerStartPoint, InnerEndPoint);
            //var innerSegment = new Segment2D(InnerStartPoint, InnerEndPoint);
            var otherOuterSegment = new Segment2D(other.OuterStartPoint, other.OuterEndPoint);
            var otherInnerSegment = new Segment2D(other.InnerStartPoint, other.InnerEndPoint);

            var firstIntersectType =
                Segment2D.Intersection(outerSegment, otherOuterSegment, out var p1, out var p2, 0.0001);
            var secondInterSecType =
                Segment2D.Intersection(outerSegment, otherInnerSegment, out var p3, out var p4, 0.0001);
            var thirstIntersectType =
                Segment2D.Intersection(interSegment, otherOuterSegment, out var p5, out var p6, 0.0001);
            var fourIntersectType =
                Segment2D.Intersection(interSegment, otherInnerSegment, out var p7, out var p8, 0.0001);

            //TH1
            if (firstIntersectType == segmentIntersectionType.Cross &&
                secondInterSecType == segmentIntersectionType.Cross &&
                thirstIntersectType == segmentIntersectionType.Cross &&
                fourIntersectType == segmentIntersectionType.Cross)
            {
                intersectType = FramingIntersectionType.Cross;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                return true;
            }

            //TH2

            //2.1

            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Touch &&
                fourIntersectType == segmentIntersectionType.Touch)
            {
                intersectType = FramingIntersectionType.TTouch;
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                return true;
            }
            //2.2
            if (firstIntersectType == segmentIntersectionType.Touch &&
                secondInterSecType == segmentIntersectionType.Touch &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.TTouch;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                return true;
            }
            //TH3
            //3.1
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Cross &&
                fourIntersectType == segmentIntersectionType.Cross)
            {
                intersectType = FramingIntersectionType.TOverlap;
                Segment2D.IntersectionLine(outerSegment, otherOuterSegment, out var intersecP1);
                Segment2D.IntersectionLine(outerSegment, otherInnerSegment, out var intersecP2);
                intersectPoints.Add(intersecP1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(intersecP2.ConvertPoint2DtoPoint3D());
                return true;
            }
            //3.2
            if (firstIntersectType == segmentIntersectionType.Cross &&
                secondInterSecType == segmentIntersectionType.Cross &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.TOverlap;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                return true;
            }
            //TH4
            //4.1
            if (firstIntersectType == segmentIntersectionType.Touch &&
                secondInterSecType == segmentIntersectionType.Touch &&
                thirstIntersectType == segmentIntersectionType.Cross &&
                fourIntersectType == segmentIntersectionType.Cross)
            {
                intersectType = FramingIntersectionType.TCross;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                return true;
            }
            //4.2
            if (firstIntersectType == segmentIntersectionType.Cross &&
                secondInterSecType == segmentIntersectionType.Cross &&
                thirstIntersectType == segmentIntersectionType.Touch &&
                fourIntersectType == segmentIntersectionType.Touch)
            {
                intersectType = FramingIntersectionType.TCross;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                return true;
            }
            //TH5
            //5.1
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Touch &&
                fourIntersectType == segmentIntersectionType.EndPointTouch)
            {
                intersectType = FramingIntersectionType.LTouchTouch;
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                return true;
            }
            //5.2
            if (firstIntersectType == segmentIntersectionType.Touch &&
                secondInterSecType == segmentIntersectionType.EndPointTouch &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.LTouchTouch;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                return true;
            }
            //5.3
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.EndPointTouch &&
                fourIntersectType == segmentIntersectionType.Touch)
            {
                intersectType = FramingIntersectionType.LTouchTouch;
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                return true;
            }
            //5.4
            if (firstIntersectType == segmentIntersectionType.EndPointTouch &&
                secondInterSecType == segmentIntersectionType.Touch &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.LTouchTouch;
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                return true;
            }
            // TH6
            //6.1
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Cross &&
                fourIntersectType == segmentIntersectionType.Touch)
            {
                intersectType = FramingIntersectionType.LOverlap;
                Segment2D.IntersectionLine(outerSegment, otherOuterSegment, out var iP1);
                Segment2D.IntersectionLine(outerSegment, otherInnerSegment, out var iP2);
                intersectPoints.Add(iP1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(iP2.ConvertPoint2DtoPoint3D());
                return true;
            }
            //6.2

            if (firstIntersectType == segmentIntersectionType.Cross &&
                secondInterSecType == segmentIntersectionType.Touch &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.LOverlap;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                return true;
            }

            //6.3
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Touch &&
                fourIntersectType == segmentIntersectionType.Cross)
            {
                intersectType = FramingIntersectionType.LOverlap;
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                return true;
            }
            //6.4
            if (firstIntersectType == segmentIntersectionType.Touch &&
                secondInterSecType == segmentIntersectionType.Cross &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.LOverlap;
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                return true;
            }
            //TH7
            //7.1
            if (firstIntersectType == segmentIntersectionType.Touch &&
                secondInterSecType == segmentIntersectionType.EndPointTouch &&
                thirstIntersectType == segmentIntersectionType.Cross &&
                fourIntersectType == segmentIntersectionType.Touch)
            {
                intersectType = FramingIntersectionType.LEndCrossTouch;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                return true;
            }
            //7.2
            if (firstIntersectType == segmentIntersectionType.Cross &&
                secondInterSecType == segmentIntersectionType.Touch &&
                thirstIntersectType == segmentIntersectionType.Touch &&
                fourIntersectType == segmentIntersectionType.EndPointTouch)
            {
                intersectType = FramingIntersectionType.LEndCrossTouch;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                return true;
            }
            //7.3
            if (firstIntersectType == segmentIntersectionType.EndPointTouch &&
                secondInterSecType == segmentIntersectionType.Touch &&
                thirstIntersectType == segmentIntersectionType.Touch &&
                fourIntersectType == segmentIntersectionType.Cross)
            {
                intersectType = FramingIntersectionType.LEndCrossTouch;
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                return true;
            }
            //7.4
            if (firstIntersectType == segmentIntersectionType.Touch &&
                secondInterSecType == segmentIntersectionType.Cross &&
                thirstIntersectType == segmentIntersectionType.EndPointTouch &&
                fourIntersectType == segmentIntersectionType.Touch)
            {
                intersectType = FramingIntersectionType.LEndCrossTouch;
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                return true;
            }
            // TH8
            //8.1
            if (firstIntersectType == segmentIntersectionType.Cross &&
                secondInterSecType == segmentIntersectionType.Touch &&
                thirstIntersectType == segmentIntersectionType.Cross &&
                fourIntersectType == segmentIntersectionType.Touch)
            {
                intersectType = FramingIntersectionType.TEndCross;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                return true;
            }
            //8.2
            if (firstIntersectType == segmentIntersectionType.Touch &&
                secondInterSecType == segmentIntersectionType.Cross &&
                thirstIntersectType == segmentIntersectionType.Touch &&
                fourIntersectType == segmentIntersectionType.Cross)
            {
                intersectType = FramingIntersectionType.TEndCross;
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                return true;
            }
            //TH9
            //9.1
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Touch &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.LTTouch;
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                return true;
            }
            //9.2

            if (firstIntersectType == segmentIntersectionType.Touch &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.LTTouch;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                return true;
            }
            //9.3
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Touch)
            {
                intersectType = FramingIntersectionType.LTTouch;
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                return true;
            }
            //9.4
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Touch &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.LTTouch;
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                return true;
            }

            //TH10

            //10.1
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Cross &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.LEndCross;
                Segment2D.IntersectionLine(outerSegment, otherOuterSegment, out var ip1);
                //Segment2D.IntersectionLine(outerSegment, otherInnerSegment, out var ip2);
                intersectPoints.Add(ip1.ConvertPoint2DtoPoint3D());
                //intersectPoints.Add(ip2.ConvertPoint2DtoPoint3D());
                return true;
            }
            //10.2

            if (firstIntersectType == segmentIntersectionType.Cross &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.LEndCross;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                return true;
            }
            //10.3
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Cross)
            {
                intersectType = FramingIntersectionType.LEndCross;
                Segment2D.IntersectionLine(outerSegment, otherInnerSegment, out var ip2);
                intersectPoints.Add(ip2.ConvertPoint2DtoPoint3D());
                return true;
            }
            //10.4
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Cross &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.LEndCross;
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                return true;
            }
            //TH11
            //11.1
            if (firstIntersectType == segmentIntersectionType.Touch &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Cross &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.LEndOverlap;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                return true;
            }
            //11.2
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Touch &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Cross)
            {
                intersectType = FramingIntersectionType.LEndOverlap;
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                return true;
            }
            //11.3
            if (firstIntersectType == segmentIntersectionType.Cross &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Touch &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.LEndOverlap;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                return true;
            }
            //11.4
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Cross &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Touch)
            {
                intersectType = FramingIntersectionType.LEndOverlap;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                return true;
            }
            //TH12
            //12.1
            if (firstIntersectType == segmentIntersectionType.Cross &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Cross &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.TEndOverlap;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                return true;
            }
            //12.2
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Cross &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Cross)
            {
                intersectType = FramingIntersectionType.TEndOverlap;
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                return true;
            }
            //TH13
            //13.1
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.EndPointTouch &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.LEndTouch;
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                return true;
            }
            //13.2

            if (firstIntersectType == segmentIntersectionType.EndPointTouch &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.LEndTouch;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                return true;
            }
            //13.3
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.EndPointTouch)
            {
                intersectType = FramingIntersectionType.LEndTouch;
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                return true;
            }
            //13.4
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.EndPointTouch &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.LEndTouch;
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                return true;
            }
            //TH14
            //14.1
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Touch &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.LEndTTouch;
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                return true;
            }
            //14.2
            if (firstIntersectType == segmentIntersectionType.Touch &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.LEndTTouch;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                return true;
            }
            //14.3
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Touch)
            {
                intersectType = FramingIntersectionType.LEndTTouch;
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                return true;
            }
            //14.4
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Touch &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.LEndTTouch;
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                return true;
            }
            //TH15
            //15.1
            if (firstIntersectType == segmentIntersectionType.EndPointTouch &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Touch &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.LEndTouchTouch;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                return true;
            }
            //15.2
            if (firstIntersectType == segmentIntersectionType.Touch &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.EndPointTouch &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.LEndTouchTouch;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                return true;
            }
            //15.3
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.EndPointTouch &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Touch)
            {
                intersectType = FramingIntersectionType.LEndTouchTouch;
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                return true;
            }
            //15.4
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Touch &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.EndPointTouch)
            {
                intersectType = FramingIntersectionType.LEndTouchTouch;
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                return true;
            }
            //TH16
            //16.1
            if (firstIntersectType == segmentIntersectionType.Touch &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Touch && 
                fourIntersectType ==segmentIntersectionType.Disjoint )
            {
                intersectType = FramingIntersectionType.TEndTouch;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                return true;
            }
            //16.2
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Touch &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Touch)
            {
                intersectType = FramingIntersectionType.TEndTouch;
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                return true;
            }

            
            if (this.IsOverlapWith(other,out var ovelapIntersectPoint))
            {
                intersectPoints.Add(ovelapIntersectPoint);
                intersectType = FramingIntersectionType.ParallelOverlap;
                return true;
            }

            if (IsOverLapTouch(other,out var overlapCount,segmentIntersectionType.OverlapInSegment) )
            {
                if (overlapCount == 1)
                {
                    intersectType = FramingIntersectionType.ParallelOverlapTouch;
                    return true;
                }
                intersectType = FramingIntersectionType.ParallelOverlapOverlap;
                return true;

            }

            var intersectPs = new List<Point3D>();
            if (IsColinear(other,out var intersecLinearType,intersectPs))
            {
                if (intersecLinearType == segmentIntersectionType.CollinearEndPointTouch)
                {
                    intersectType = FramingIntersectionType.ColinearTouch;
                    intersectPoints.AddRange(intersectPs);
                    return true;
                }
                intersectType = FramingIntersectionType.ColinearOverlap;
                intersectPoints.AddRange(intersectPs);
                return true;
            }
            if (firstIntersectType == segmentIntersectionType.Disjoint &&
                secondInterSecType == segmentIntersectionType.Disjoint &&
                thirstIntersectType == segmentIntersectionType.Disjoint &&
                fourIntersectType == segmentIntersectionType.Disjoint)
            {
                intersectType = FramingIntersectionType.Disjoint;
                return false;
            }

            if (firstIntersectType == segmentIntersectionType.CollinearEndPointTouch )
            {
                intersectType = FramingIntersectionType.ParallelTouch;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                return true;
            }

            if (secondInterSecType == segmentIntersectionType.CollinearEndPointTouch)
            {
                intersectType = FramingIntersectionType.ParallelTouch;
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                return true;
            }

            if (thirstIntersectType == segmentIntersectionType.CollinearEndPointTouch)
            {
                intersectType = FramingIntersectionType.ParallelTouch;
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                return true;
            }

            if (fourIntersectType == segmentIntersectionType.CollinearEndPointTouch)
            {
                intersectType = FramingIntersectionType.ParallelTouch;
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                return true;
            }

            if (firstIntersectType == segmentIntersectionType.OverlapInSegment)
            {
                intersectType = FramingIntersectionType.ParallelTouchOverlap;
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p2.ConvertPoint2DtoPoint3D());
                return true;
            }

            if (secondInterSecType == segmentIntersectionType.OverlapInSegment)
            {
                intersectType = FramingIntersectionType.ParallelTouchOverlap;
                intersectPoints.Add(p3.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p4.ConvertPoint2DtoPoint3D());
                return true;
            }

            if (thirstIntersectType == segmentIntersectionType.OverlapInSegment)
            {
                intersectType = FramingIntersectionType.ParallelTouchOverlap;
                intersectPoints.Add(p5.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p6.ConvertPoint2DtoPoint3D());
                return true;
            }

            if (fourIntersectType == segmentIntersectionType.OverlapInSegment)
            {
                intersectType = FramingIntersectionType.ParallelTouchOverlap;
                intersectPoints.Add(p7.ConvertPoint2DtoPoint3D());
                intersectPoints.Add(p8.ConvertPoint2DtoPoint3D());
                return true;
            }

            intersectType = FramingIntersectionType.Disjoint;
            return false;
        }
        private bool IsColinear(FramingRectangle2D framing, out segmentIntersectionType intersectionType,List<Point3D> intersectPoints)
        {
            var centerLine = new Segment2D(StartCenterLinePoint, EndCenterLinePoint);
            var otherCenterLine = new Segment2D(framing.StartCenterLinePoint, framing.EndCenterLinePoint);
            intersectionType = Segment2D.Intersection(centerLine, otherCenterLine, out var p1, out var p2, 0.0001);
            if (intersectionType == segmentIntersectionType.CollinearEndPointTouch ||
                intersectionType == segmentIntersectionType.OverlapInSegment)
            {
                intersectPoints.Add(p1.ConvertPoint2DtoPoint3D());
                if (p2!=null)
                {
                    intersectPoints.Add(p2.ConvertPoint2DtoPoint3D());
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool IsOverLapTouch(FramingRectangle2D framing,out int count,segmentIntersectionType intersectType)
        {
            var outStartSegment = new Segment2D(OuterStartPoint, InnerStartPoint);
            var outEndSegment = new Segment2D(OuterEndPoint, InnerEndPoint);
            var otherStartSegment = new Segment2D(framing.OuterStartPoint, framing.InnerStartPoint);
            var otherEndSegment = new Segment2D(framing.OuterEndPoint, framing.InnerEndPoint);
            var outSeg = new List<Segment2D>() {outStartSegment, outEndSegment};
            var otherSeg = new List<Segment2D>() { otherStartSegment,otherEndSegment};
            count = 0;
            foreach (var segment2D in outSeg)
            {
                foreach (var segment2D1 in otherSeg)
                {
                    if (Segment2D.Intersection(segment2D,segment2D1,out var p1,out var p2,0.0001) == intersectType)
                    {
                        count++;
                    }
                }
            }

            return count == 1 || count == 2;
        }
        private bool IsOverlapWith(FramingRectangle2D other,out Point3D overlapIntersectPoint)
        {
            var points = new List<Point2D>(){other.OuterStartPoint,other.OuterEndPoint,other.InnerEndPoint,other.InnerStartPoint};
            //points.Add(Vertices[0]);
            //var region = devDept.Eyeshot.Entities.Region.CreatePolygon(points.ToArray());
            var polygons = new List<Point2D>() {OuterStartPoint, OuterEndPoint, InnerEndPoint, InnerStartPoint,OuterStartPoint};

            foreach (var framingVertex in points)
            {
                if (Utility.PointInPolygon(framingVertex,polygons))
                {
                    var ounerSegment = new Segment2D(OuterStartPoint, OuterEndPoint);
                    var disc = ounerSegment.Project(framingVertex);
                    overlapIntersectPoint = ounerSegment.PointAt(disc).ConvertPoint2DtoPoint3D();
                    return true;
                }
            }

            overlapIntersectPoint = null;
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
            var linearPath = new List<Point2D>()
                {OuterStartPoint, OuterEndPoint, InnerEndPoint, InnerStartPoint,OuterStartPoint};
            return Utility.PointInPolygon(point, linearPath);
        }


        public bool IsPointIn(Point3D point)
        {
            for (int i = 0; i < 4; i++)
            {
                if (UtilityEx.IsPointOnSegment(point,Vertices[i], Vertices[i + 1], 0.0001))
                    return true;
            }

            return false;


        }
        private void Extend(double atStart, double atEnd)
        {
            var segmentFirstLine = new Segment2D(_outerStartPoint, _outerEndPoint);
            segmentFirstLine.ExtendBy(atStart, atEnd);
            _outerStartPoint = segmentFirstLine.P0.ConvertPoint2DtoPoint3D();
            _outerEndPoint = segmentFirstLine.P1.ConvertPoint2DtoPoint3D();
            this.RegenFramingGeometry(_outerStartPoint, _outerEndPoint);
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
            StartCenterLinePoint = centerLine.P0.ConvertPoint2DtoPoint3D();
            EndCenterLinePoint = centerLine.P1.ConvertPoint2DtoPoint3D();
            centerLine.ExtendBy(-(double)Thickness / 2, -(double)Thickness / 2);
            StartPoint = centerLine.P0.ConvertPoint2DtoPoint3D();
            EndPoint = centerLine.P1.ConvertPoint2DtoPoint3D();
            centerCircleLine.ExtendBy(-140, -140);
            HangerACenterPoint = centerCircleLine.P0.ConvertPoint2DtoPoint3D();
            HangerBCenterPoint = centerCircleLine.P1.ConvertPoint2DtoPoint3D();
            HangerACenterPointChanged();
            HangerBCenterPointChanged();
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

        protected abstract void HangerACenterPointChanged();
        protected abstract void HangerBCenterPointChanged();
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
            this.RegenFramingGeometry(OuterStartPoint, OuterEndPoint,Flipped);
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
        public virtual IRectangleSolid Offset(double amount, Vector3D planeNormal, double tolerance, bool sharp)
        {
            var framing = (IRectangleSolid)Clone();
            Line framingLine = new Line(framing.OuterStartPoint, framing.OuterEndPoint);
            Vector3D vector3D = Vector3D.Cross(framingLine.Tangent, planeNormal);
            vector3D.Normalize();
            Vector3D v = vector3D * amount;
            framingLine.Translate(v);
            framing.OuterStartPoint = framingLine.StartPoint;
            framing.OuterEndPoint = framingLine.EndPoint;
            return framing;
        }
        public IRectangleSolid Offset(double amount, Vector3D planeNormal)
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
        public bool IsColinearTouchWithOther(FramingRectangle2D other)
        {
            var outSegment = new Segment2D(StartCenterLinePoint, EndCenterLinePoint);
            var otherSegment = new Segment2D(other.StartCenterLinePoint, other.EndCenterLinePoint);
            var intersectType = Segment2D.Intersection(outSegment, otherSegment, out var p1, out var p2, 0.1);
            if (intersectType == segmentIntersectionType.CollinearEndPointTouch)
            {
                return true;
            }
            else
            {
                if (Segment2D.AreCollinear(outSegment,otherSegment))
                {
                    return StartCenterLinePoint == other.StartCenterLinePoint ||
                           StartCenterLinePoint == other.EndCenterLinePoint ||
                           EndCenterLinePoint == other.StartCenterLinePoint ||
                           EndCenterLinePoint == other.EndCenterLinePoint;
                }
            }

            return false;
        }
        //public void SetFramingType(FramingTypes framingType)
        //{
        //    FramingType = framingType;
        //}
        public bool IsParallelWith(FramingRectangle2D other)
        {
            var thisCenterVector = new Vector2D(StartCenterLinePoint, EndCenterLinePoint);
            var otherCenterVector = new Vector2D(other.StartCenterLinePoint, other.EndCenterLinePoint);

            return Vector2D.AreParallel(thisCenterVector, otherCenterVector, 0.1);
        }

        public bool IsCoaxialCenterWith(FramingRectangle2D other)
        {
            var centeraxisVector = new Vector2D(this.MidPoint, other.MidPoint);
            var thiscenterVector = new Vector2D(StartCenterLinePoint, EndCenterLinePoint);
            var otherCenterVector = new Vector2D(other.StartCenterLinePoint, EndCenterLinePoint);
            return Vector2D.AreOrthogonal(centeraxisVector, thiscenterVector) &&
                   Vector2D.AreOrthogonal(centeraxisVector, otherCenterVector);

        }

        public void ResetPoint(Point3D outerStartPoint, Point3D outerEndPoint)
        {
            _outerStartPoint = outerStartPoint;
            _outerEndPoint = outerEndPoint;
            this.RegenFramingGeometry(_outerStartPoint, _outerEndPoint,Flipped);
        }

        public void RollBackDependency(UndoList undoItem, IEntitiesManager entitiesManager)
        {
            
        }
    }
}
