using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using devDept.Serialization;

namespace AppModels.CustomEntity
{
    public class Joist2D : Text
    {
        private Point3D _startPoint1;
        private Point3D _startPoint2;
        private Point3D _endPoint1;
        private Point3D _endPoint2;
        private bool _isOuttrigger;
        public Point3D StartPoint { get; set; }
        public Point3D EndPoint { get; set; }
        public int Thickness { get; set; }
        public int Depth { get; set; }
        public bool IsShowHanger { get; set; }
        public bool IsDoubleJoits { get; set; }
        public Segment2D CenterLine { get; set; }
        public Segment2D Line1 { get; set; }
        public Segment2D Line2 { get; set; }
        
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

        public Joist2D(Plane wallPlan, Point3D startPoint, Point3D endPoint, Joist joistReference,
            int thickness = 90, bool isShowHanger = false, bool isDoubleJoits = false,bool isShowJoistName= false, double textHeight = 90) :
            base(wallPlan, startPoint, textHeight, Text.alignmentType.BaselineCenter)
        {
            Thickness = thickness;
            IsShowHanger = isShowHanger;
            IsDoubleJoits = isDoubleJoits;
            this.JoistReference = joistReference;
            InitializerWallLine(startPoint, endPoint);
            this.LineTypeScale = 10;
        }
        private void InitializerWallLine(Point3D startPoint, Point3D endPoint)
        {
            StartPoint1 = startPoint;
            EndPoint1 = endPoint;
            var thickness = IsDoubleJoits ? Thickness * 2 : Thickness;
            Line1 = new Segment2D(StartPoint1, EndPoint1);
            Line2 = Line1.Offset(Thickness);
            StartPoint2 = Line2.P0.ConvertPoint2DtoPoint3D();
            EndPoint2 = Line2.P1.ConvertPoint2DtoPoint3D();

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
    }
}
