using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Framings;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.ViewModelEntity;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using devDept.Serialization;

namespace AppModels.CustomEntity
{
    public class Joist2D : Text,IEntityVmCreateAble
    {
        private Point3D _startPoint1;
        private Point3D _startPoint2;
        private Point3D _endPoint1;
        private Point3D _endPoint2;
        private bool _isOuttrigger;
        private int _thickness;
        private int _depth;
        public Point3D StartPoint { get; set; }
        public Point3D EndPoint { get; set; }
        public int Thickness
        {
            get => _thickness;
            set
            {
                _thickness = value;
                this.RegenGeometry(this.StartPoint1, this.EndPoint1);
                this.RegenMode = regenType.RegenAndCompile;
            }
        }
        public int Depth
        {
            get => _depth;
            set
            {
                _depth = value;
                this.RegenGeometry(this.StartPoint1,this.EndPoint1);
                this.RegenMode = regenType.RegenAndCompile;
            }
        }
        public bool IsShowHanger { get; set; }
        //public bool IsDoubleJoits { get; set; }
        public Segment2D CenterLine { get; set; }
        public Segment2D Line1 { get; set; }
        public Segment2D Line2 { get; set; }
        public Point3D StartPoint1
        {
            get => _startPoint1;
            set
            {
                _startPoint1 = value;
                if (_startPoint1 ==null|| _endPoint1 ==null )
                {
                    return;
                }
                this.RegenGeometry(_startPoint1,_endPoint1);
                this.RegenMode = regenType.RegenAndCompile;
            }
        }
        public Point3D StartPoint2
        {
            get => _startPoint2;
            set
            {
                _startPoint2 = value;
                this.RegenGeometry(_startPoint1,_endPoint1);
                this.RegenMode = regenType.RegenAndCompile;
            }
        }
        public Point3D EndPoint1
        {
            get => _endPoint1;
            set
            {
                _endPoint1 = value;
                if (_startPoint1 == null || _endPoint1 == null)
                {
                    return;
                }
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
        public bool IsShowJoistName
        {
            get;
            set;
        }
        public List<Segment2D> BoxLines { get; set; } = new List<Segment2D>();
        public List<Point3D> JoistVertices { get; set; } = new List<Point3D>();
        public List<Point3D> CenterlineVertices { get; set; } = new List<Point3D>();
        public bool IsOutrigger
        {
            get => _isOuttrigger;
            set
            {
                _isOuttrigger = value;
                this.RegenMode = regenType.RegenAndCompile;
            }
        }
        public Joist JoistReference { get; private set; }

        public double FullLength
        {
            get;
            set;
        }

        public Joist2D(Plane wallPlan, Point3D startPoint, Point3D endPoint, Joist joistReference,
            int thickness = 90, bool isShowHanger = false, bool isShowJoistName= false, double textHeight = 90) :
            base(wallPlan, startPoint, textHeight, Text.alignmentType.BaselineCenter)
        {
            _thickness = thickness;
            IsShowHanger = isShowHanger;
            this.JoistReference = joistReference;
            _startPoint1 = startPoint;
            _endPoint1 = endPoint;
            JoistReference.PropertyChanged+=JoistReferenceOnPropertyChanged;

            RegenGeometry(startPoint, endPoint);
            //this.LineTypeScale = 10;
        }

        public Joist2D(Joist2D another): base(another)
        {
            this._thickness = another.Thickness;
            this._depth = another.Depth;
            this.IsShowHanger = another.IsShowHanger;
            this.JoistReference = new Joist();
            FramingSheet currentSheet = another.JoistReference.FloorSheet;
            var index = currentSheet.Joists.Count;
            JoistReference.Id = index;
            JoistReference.SheetName = currentSheet.Name;
            JoistReference.FramingSpan = another.JoistReference.FramingSpan;
            JoistReference.FloorSheet = currentSheet;
            if (another.JoistReference.FramingInfo!=null)
            {
                JoistReference.FramingInfo = another.JoistReference.FramingInfo;
            }
            this._startPoint1 = (Point3D)another.StartPoint1.Clone();
            this._endPoint1 = (Point3D)another.EndPoint1.Clone();
            this.LineTypeScale = 10;
            RegenGeometry(_startPoint1,_endPoint1);


        }

        private void UpdateDistance()
        {
            if (this.StartPoint1!=null && this.EndPoint1 !=null)
            {
                this.FullLength = StartPoint1.DistanceTo(EndPoint1);
                if (this.JoistReference!=null)
                {
                    this.JoistReference.FullLength = (int)FullLength;
                }
            }
        }
        private void JoistReferenceOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(JoistReference.FramingInfo):
                    if (JoistReference.FramingInfo!=null)
                    {
                        Thickness = JoistReference.FramingInfo.Depth * JoistReference.FramingInfo.NoItem;
                        Depth = JoistReference.FramingInfo.Thickness;
                    }
                    break;
                default:
                    break;
            }
        }

        private void RegenGeometry(Point3D startPoint, Point3D endPoint)
        {
            //if (startPoint == null || endPoint == null) return;
            
            //StartPoint1 = startPoint;
            //EndPoint1 = endPoint;
            var thickness = Thickness;
            Line1 = new Segment2D(StartPoint1, EndPoint1);
            Line2 = Line1.Offset(Thickness);
            _startPoint2 = Line2.P0.ConvertPoint2DtoPoint3D();
            _endPoint2 = Line2.P1.ConvertPoint2DtoPoint3D();

            CenterLine = Line1.Offset((double)Thickness / 2);
            CenterLine.ExtendBy(-(double)Thickness / 2, -(double)Thickness / 2);

            StartPoint = CenterLine.P0.ConvertPoint2DtoPoint3D();
            EndPoint = CenterLine.P1.ConvertPoint2DtoPoint3D();
            
            var offsetLine = Line1.Offset(-(double)Thickness / 2);
            var segment1 = new Segment2D((Point3D)StartPoint1.Clone(), (Point3D)EndPoint1.Clone());
            segment1.ExtendBy(-Thickness, -Thickness);
            var segment2 = new Segment2D((Point3D)StartPoint2.Clone(), (Point3D)EndPoint2.Clone());
            segment2.ExtendBy(-Thickness, -Thickness);
            var p1 = segment1.P0;
            var p2 = segment2.P0;
            var p3 = segment1.P1;
            var p4 = segment2.P1;
            this.UpdateDistance();
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
            if (IsShowJoistName)
            {
                this.DrawText(data);
            }

        }
        //protected override void DrawEdges(DrawParams data)
        //{
        //    base.DrawEdges(data);
        //    data.RenderContext.SetLineSize(2);
        //    //data.RenderContext.SetColorWireframe(Color.Crimson);
        //    data.RenderContext.PushModelView();
        //    //data.RenderContext.SetColorWireframe(Color.Crimson);
        //        data.RenderContext.DrawBufferedLine(StartPoint1, StartPoint2);
        //        //data.RenderContext.SetColorWireframe(Color.Crimson);
        //        data.RenderContext.DrawBufferedLine(EndPoint1, EndPoint2);
        //        //data.RenderContext.SetColorWireframe(Color.Crimson);
        //        data.RenderContext.DrawBufferedLine(StartPoint1, EndPoint1);
        //        //data.RenderContext.SetColorWireframe(Color.Crimson);
        //        data.RenderContext.DrawBufferedLine(StartPoint2, EndPoint2);
        //        data.RenderContext.PopModelView();
        //}

        public override void Regen(RegenParams data)
        {
            var distance = (int)StartPoint.DistanceTo(EndPoint) - Thickness;
            this.TextString = distance.ToString();
            //this.InsertionPoint = new Point3D(DimensionLine.MidPoint.X, DimensionLine.MidPoint.Y, 0);
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
            JoistVertices.Clear();
            JoistVertices.Add(StartPoint1);
            JoistVertices.Add(StartPoint2);
            JoistVertices.Add(EndPoint2);
            JoistVertices.Add(EndPoint1);
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
            return new Joist2dVm(this);
        }

        public override object Clone()
        {
            return new Joist2D(this);
        }

        public Joist2D Offset(double amount, Vector3D planeNormal)
        {
            return this.Offset(amount, planeNormal, 0.0, false);
        }

        public Joist2D Offset(double amount, Vector3D planeNormal, double tolerance, bool sharp)
        {
            Joist2D joist = (Joist2D) this.Clone();
            Line joistline = new Line(joist.StartPoint1, joist.EndPoint1);
            Vector3D vector3D = Vector3D.Cross(joistline.Tangent,planeNormal);
            vector3D.Normalize();
            Vector3D v = vector3D * amount;
            joistline.Translate(v);
            joist.StartPoint1 = joistline.StartPoint;
            joist.EndPoint1 = joistline.EndPoint;
            joist.InsertionPoint = StartPoint1;
            return joist;
        }

        public bool Project(Point3D point, out double t)
        {
            Segment3D segment = new Segment3D(this.StartPoint1, this.EndPoint1);
            Line joistLine = new Line(this.StartPoint1, this.EndPoint1);
            return joistLine.Project(point, out t);
        }

        public Point3D PointAt(double t)
        {
            var joitsLine = new Line(StartPoint1, EndPoint1);
            return joitsLine.PointAt(t);
        }
    }
}
