using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
    public class Joist2D : Text, IEntityVmCreateAble
    {
        private Point3D _outerStartPoint;
        private Point3D _innerStartPoint;
        private Point3D _outerEndPoint;
        private Point3D _innerEndPoint;
        private bool _outTriggerAFlipped;
        private bool _outTriggerBFlipped;

        private int _thickness;
        private int _depth;
        public Guid Id { get; set; }
        public string SheetName { get; set; }
        public Point3D StartPoint { get; set; }
        public Point3D EndPoint { get; set; }
        public Point3D MidPoint => Point3D.MidPoint(StartPoint, EndPoint);
        public int Thickness
        {
            get => _thickness;
            set
            {
                _thickness = value;
                this.RegenGeometry(this.OuterStartPoint, this.OuterEndPoint);
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
        public bool IsShowHanger { get; set; }
        //public bool IsDoubleJoits { get; set; }
        public Segment2D CenterLine { get; set; }
        public Segment2D OuterLine { get; set; }
        public Segment2D InterLine { get; set; }
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
                this.RegenGeometry(_outerStartPoint, _outerEndPoint);
                this.RegenMode = regenType.RegenAndCompile;
            }
        }
        public Point3D InnerStartPoint
        {
            get => _innerStartPoint;
            set
            {
                _innerStartPoint = value;
                this.RegenGeometry(_outerStartPoint, _outerEndPoint);
                this.RegenMode = regenType.RegenAndCompile;
            }
        }
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
        public bool IsShowJoistName
        {
            get;
            set;
        }
        public List<Segment2D> BoxLines { get; set; } = new List<Segment2D>();
        public List<Point3D> JoistVertices { get; set; } = new List<Point3D>();
        public List<Point3D> CenterlineVertices { get; set; } = new List<Point3D>();
        public string HangerAId { get; set; }
        public string HangerBId { get; set; }
        public Hanger2D HangerA { get; set; }
        public Hanger2D HangerB { get; set; }
        public bool IsHangerA { get; set; }
        public bool IsHangerB { get; set; }
        public Joist JoistReference { get; private set; }
        public string OutTriggerAId { get; set; }
        public string OutTriggerBId { get; set; }
        public OutTrigger2D OutTriggerA { get; set; }
        public OutTrigger2D OutTriggerB { get; set; }
        public bool IsOutTriggerA { get; set; }
        public bool IsOutTriggerB { get; set; }
        public double FullLength
        {
            get;
            set;
        }
        public bool OutTriggerAFlipped
        {
            get => _outTriggerAFlipped;
            set
            {
                _outTriggerAFlipped = value;
                
                if (this.OutTriggerA!=null)
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
                if (this.OutTriggerB!=null)
                {
                    _outTriggerBFlipped = value;

                    if (this.OutTriggerB != null)
                    {
                        OutTriggerB.Flipper(StartPoint, EndPoint);
                    }
                }
            }
        }

        public Joist2D(Plane wallPlan, Point3D outerStartPoint, Point3D outerEndPoint, Joist joistReference,
            int thickness = 90, bool isShowHanger = false, bool isShowJoistName = false, double textHeight = 50) :
            base(wallPlan, outerStartPoint, textHeight, Text.alignmentType.MiddleCenter)
        {
            Id = Guid.NewGuid();
            _thickness = thickness;
            IsShowHanger = isShowHanger;
            this.JoistReference = joistReference;
            _outerStartPoint = outerStartPoint;
            _outerEndPoint = outerEndPoint;
            JoistReference.PropertyChanged += JoistReferenceOnPropertyChanged;
            RegenGeometry(outerStartPoint, outerEndPoint);
        }

        public Joist2D(Joist2D another) : base(another)
        {
            this._thickness = another.Thickness;
            this._depth = another.Depth;
            this._outerStartPoint = (Point3D)another.OuterStartPoint.Clone();
            this._outerEndPoint = (Point3D)another.OuterEndPoint.Clone();
            this.IsShowHanger = another.IsShowHanger;
            this.JoistReference = new Joist();
            FramingSheet currentSheet = another.JoistReference.FloorSheet;
            JoistReference.SheetName = currentSheet.Name;
            JoistReference.FramingSpan = another.JoistReference.FramingSpan;
            JoistReference.FloorSheet = currentSheet;
            if (another.JoistReference.FramingInfo != null)
            {
                JoistReference.FramingInfo = another.JoistReference.FramingInfo;
            }
            JoistReference.PropertyChanged += JoistReferenceOnPropertyChanged;
            this.LineTypeScale = 10;
            RegenGeometry(_outerStartPoint, _outerEndPoint);


        }

        private void UpdateDistance()
        {
            if (this.OuterStartPoint != null && this.OuterEndPoint != null)
            {
                this.FullLength = OuterStartPoint.DistanceTo(OuterEndPoint);
                if (this.JoistReference != null)
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
                    if (JoistReference.FramingInfo != null)
                    {
                        Thickness = JoistReference.FramingInfo.Depth * JoistReference.FramingInfo.NoItem;
                        Depth = JoistReference.FramingInfo.Thickness;
                    }
                    break;
                default:
                    break;
            }
        }

        private void RegenGeometry(Point3D startPoint, Point3D endPoint, bool outTriggerA = false, bool outTriggerB = false, bool flipperOutriggA = false, bool flippedOutriggerB = false)
        {
            var thickness = Thickness;
            OuterLine = new Segment2D(OuterStartPoint, OuterEndPoint);
            InterLine = OuterLine.Offset(Thickness);
            _innerStartPoint = InterLine.P0.ConvertPoint2DtoPoint3D();
            _innerEndPoint = InterLine.P1.ConvertPoint2DtoPoint3D();
            CenterLine = OuterLine.Offset((double)Thickness / 2);
            CenterLine.ExtendBy(-(double)Thickness / 2, -(double)Thickness / 2);
            StartPoint = CenterLine.P0.ConvertPoint2DtoPoint3D();
            EndPoint = CenterLine.P1.ConvertPoint2DtoPoint3D();

            //var offsetLine = OuterLine.Offset(-(double)Thickness / 2);
            //var segment1 = new Segment2D((Point3D)OuterStartPoint.Clone(), (Point3D)OuterEndPoint.Clone());
            //segment1.ExtendBy(-Thickness, -Thickness);
            //var segment2 = new Segment2D((Point3D)InnerStartPoint.Clone(), (Point3D)InnerEndPoint.Clone());
            //segment2.ExtendBy(-Thickness, -Thickness);
            //var p1 = segment1.P0;
            //var p2 = segment2.P0;
            //var p3 = segment1.P1;
            //var p4 = segment2.P1;
            this.UpdateDistance();

        }

        private Color HangerColor
        {
            get;
            set;
        } = Color.DarkOliveGreen;
        protected override void Draw(DrawParams data)
        {
           this.DrawJoist(data,true,false);
        }

        private void DrawBackground(DrawParams data)
        {
            var points = new Point3D[] { OuterStartPoint, OuterEndPoint, InnerEndPoint, InnerStartPoint };
            Color prevColorWireframe = data.RenderContext.CurrentWireColor;
            data.RenderContext.SetColorWireframe(HangerColor);
            data.RenderContext.PushRasterizerState();
            data.RenderContext.SetState(rasterizerStateType.CCW_PolygonFill_NoCullFace_NoPolygonOffset);
            data.RenderContext.DrawQuads(points, new Vector3D[] { Plane.AxisZ });
            data.RenderContext.PopRasterizerState();
            data.RenderContext.SetColorWireframe(prevColorWireframe);
        }
        private Transformation GetTextMatrix(Point3D point3D)
        {
            return new Transformation(point3D, base.Plane.AxisX, base.Plane.AxisY, base.Plane.AxisZ) *
                   this.UnscaledTransform * this.ScaleTransform;
        }
        //protected override void DrawEdges(DrawParams data)
        //{
        //    base.DrawEdges(data);
        //    data.RenderContext.SetLineSize(2);
        //    //data.RenderContext.SetColorWireframe(Color.Crimson);
        //    data.RenderContext.PushModelView();
        //    //data.RenderContext.SetColorWireframe(Color.Crimson);
        //        data.RenderContext.DrawBufferedLine(OuterStartPoint, InnerStartPoint);
        //        //data.RenderContext.SetColorWireframe(Color.Crimson);
        //        data.RenderContext.DrawBufferedLine(OuterEndPoint, InnerEndPoint);
        //        //data.RenderContext.SetColorWireframe(Color.Crimson);
        //        data.RenderContext.DrawBufferedLine(OuterStartPoint, OuterEndPoint);
        //        //data.RenderContext.SetColorWireframe(Color.Crimson);
        //        data.RenderContext.DrawBufferedLine(InnerStartPoint, InnerEndPoint);
        //        data.RenderContext.PopModelView();
        //}

        public override void Regen(RegenParams data)
        {
            //var distance = (int)StartPoint.DistanceTo(EndPoint) - Thickness;
            this.TextString = string.Empty;
            //this.InsertionPoint = new Point3D(DimensionLine.MidPoint.X, DimensionLine.MidPoint.Y, 0);
            base.Regen(data);
            //var widthFactor = 0.0;
            //var exacWidth = 0.0;
            //var excacDescent = 0.0;
            //this.PrepareText(TextString, data, ref widthFactor, out exacWidth, out excacDescent, out var point3D,
            //    out var point3D2);

            //this.PrepareText();
            //Vertices
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
            JoistVertices.Clear();
            JoistVertices.Add(OuterStartPoint);
            JoistVertices.Add(InnerStartPoint);
            JoistVertices.Add(InnerEndPoint);
            JoistVertices.Add(OuterEndPoint);
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
            return new Joist2dVm(this,entitiesManager);
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
            Joist2D joist = (Joist2D)this.Clone();
            Line joistline = new Line(joist.OuterStartPoint, joist.OuterEndPoint);
            Vector3D vector3D = Vector3D.Cross(joistline.Tangent, planeNormal);
            vector3D.Normalize();
            Vector3D v = vector3D * amount;
            joistline.Translate(v);
            joist.OuterStartPoint = joistline.StartPoint;
            joist.OuterEndPoint = joistline.EndPoint;
            joist.InsertionPoint = OuterStartPoint;
            return joist;
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

        protected override void DrawForSelection(GfxDrawForSelectionParams data)
        {
            this.DrawJoist(data, false, true);
        }

        protected virtual void DrawJoist(DrawParams data,bool disableCulling, bool drawingForSelection)
        {
            base.PreDraw(data,disableCulling);
            this.DrawExtraGeometry(data);
            RenderContextBase renderContext = data.RenderContext;
            renderContext.PushModelView();
            if (renderContext.IsDirect3D)
            {
                this.PushLineSize(data);
                this.DrawEntity(renderContext,null);
                this.PopLineSize(data);
            }
            else
            {
                renderContext.Draw(this.drawData,primitiveType.Undefined);
            }

            //if (HangerA)
            //{
            //    DrawHangers(data,disableCulling,drawingForSelection,StartPoint);
            //}

            //if (HangerB)
            //{
            //    DrawHangers(data,disableCulling,drawingForSelection,EndPoint);
            //}
            renderContext.PopModelView();
            PostDraw(data);

        }

        protected virtual void DrawHangers(DrawParams data,bool disableCulling, bool drawingForSelection,Point3D hangerPoint)
        {
            var textMatrix = this.GetTextMatrix(hangerPoint);
            data.RenderContext.MultMatrixModelView(textMatrix);
            var currentColor = data.RenderContext.CurrentWireColor;
            if (data.Viewport.DisplayMode != displayType.HiddenLines && !drawingForSelection)
            {
                data.RenderContext.SetColorWireframe(HangerColor, false);
            }
            data.RenderContext.PushRasterizerState();
            data.RenderContext.SetState(rasterizerStateType.CCW_PolygonFill_NoCullFace_PolygonOffset_Minus3Minus2);
            var currentState = data.RenderContext.CurrentDepthStencilState;
            this.DrawChars(TextString, data);
            DrawCircle(hangerPoint,data);
            data.RenderContext.PopRasterizerState();
            if (data.Viewport.DisplayMode != displayType.HiddenLines && !drawingForSelection)
            {
                data.RenderContext.SetColorWireframe(currentColor);
            }
        }
        internal void PopLineSize(DrawParams drawParams_0)
        {
            if (drawParams_0.RenderContext.CurrentLineWidth > 1f)
            {
                drawParams_0.RenderContext.PopShader();
            }
        }
        internal void PushLineSize(DrawParams drawParams_0)
        {
            if (drawParams_0.RenderContext.CurrentLineWidth > 1f)
            {
                drawParams_0.RenderContext.PushShader();
                drawParams_0.RenderContext.EnableThickLines();
            }
        }

        private void DrawCircle(Point3D centerPoint,DrawParams data)
        {
            var radius = 50;
            var drawingPlane = Plane.XY;

            if (radius > 1e-3)
            {
                Circle tempCircle = new Circle(drawingPlane, centerPoint, radius);
                DrawCurve(tempCircle,data);
            }
        }

        private void DrawCurve(ICurve theCurve,DrawParams data)
        {
            if (theCurve is CompositeCurve)
            {
                CompositeCurve compositeCurve = theCurve as CompositeCurve;
                Entity[] explodedCurves = compositeCurve.Explode();
                foreach (Entity ent in explodedCurves)

                    DrawScreenCurve((ICurve)ent,data);
            }
            else
            {
                DrawScreenCurve(theCurve,data);
            }
        }

        private void DrawScreenCurve(ICurve curve,DrawParams data)
        {
            const int subd = 100;

            Point3D[] pts = new Point3D[subd + 1];

            for (int i = 0; i <= subd; i++)
            {
                pts[i] = curve.PointAt(curve.Domain.ParameterAt((double) i / subd));
                //pts[i] = data.WorldToScreen(curve.PointAt(curve.Domain.ParameterAt((double)i / subd)));
            }

            for (int i = 0; i < pts.Length; i++)
            {
                if (i!=pts.Length-1)
                {
                    data.RenderContext.DrawBufferedLine(pts[i], pts[i + 1]);
                }
                else
                {
                    data.RenderContext.DrawBufferedLine(pts[0],pts[i]);
                }
                
            }
        }


        protected override void DrawEntity(RenderContextBase context, object myParams)
        {
            if (base.Compiling)
            {
                context.DrawLine(StartPoint,EndPoint);
            }
            else
            {
                this.drawData.DrawD3D(context, 0);
            }
            //Transformation transform = new Transformation(base.Plane.Origin, base.Plane.AxisX, base.Plane.AxisY, base.Plane.AxisZ);
            context.PushModelView();
            //context.MultMatrixModelView(transform);
            DrawBackGroundRectangle(context);
            context.PopModelView();
        }

        protected void DrawBackGroundRectangle(RenderContextBase context)
        {
            if (context.CurrentLineWidth > 1f)
            {
                context.SetLineSize(1f, true, false);
            }
            this.DrawArrow(context);
        }
        protected void DrawArrow(RenderContextBase context)
        {
            context.PushModelView();
                var points = new Point3D[] { OuterStartPoint, OuterEndPoint, InnerEndPoint, InnerStartPoint };
                context.DrawQuads(points, new Vector3D[]
                {
                    Vector3D.AxisZ
                });
            //if (this.Compiling)
            //{
            //    var points = new Point3D[] { OuterStartPoint, OuterEndPoint, InnerEndPoint, InnerStartPoint };
            //    context.DrawQuads(points, new Vector3D[]
            //    {
            //        Vector3D.AxisZ
            //    });
            //    //context.DrawTriangles2D(points);
            //}
            //else
            //{
            //    this.drawData.DrawD3D(context,true);
//        }
        context.PopModelView();
        }

        protected virtual void DrawExtraGeometry(DrawParams data)
        {

        }
    }
}
