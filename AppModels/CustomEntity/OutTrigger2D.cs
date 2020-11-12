using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.ViewModelEntity;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;

namespace AppModels.CustomEntity
{
    public class OutTrigger2D: Text, IEntityVmCreateAble
    {
        private int _thickness;
        private Point3D _outerStartPoint;
        private Point3D _innerStartPoint;
        private Point3D _outerEndPoint;
        private Point3D _innerEndPoint;
        private int _insizeLength;
        private int _outSizeLength;
        private bool _flipped;

        public Guid Id { get; set; }
        public Point3D StartPoint { get; set; }
        public Point3D EndPoint { get; set; }


        public Point3D InnerStartPoint
        {
            get => _innerStartPoint;
            set
            {
                _innerStartPoint = value;
                this.RegenMode = regenType.RegenAndCompile;
            }
        }

        public Point3D InnerEndPoint
        {
            get => this._innerEndPoint;
            set
            {
                this._innerEndPoint = value;
                this.RegenMode = regenType.RegenAndCompile;
            }
        }

        public Point3D OuterStartPoint
        {
            get => _outerStartPoint;
            set
            {
                this._outerStartPoint = value;
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

        public int InSizeLength
        {
            get => _insizeLength;
            set
            {
                var extendLength = value - _insizeLength;
                _insizeLength = value;
                OuterLine.ExtendBy(0,extendLength);
                this._outerEndPoint = OuterLine.P1.ConvertPoint2DtoPoint3D();
                this.RegenGeometry(_outerStartPoint,_outerEndPoint);
                this.RegenMode = regenType.RegenAndCompile;
            }
        }

        public int OutSizeLength
        {
            get => _outSizeLength;
            set
            {
                var extendLength = value - _outSizeLength;
                _outSizeLength = value;
                OuterLine.ExtendBy(extendLength, 0);
                this._outerStartPoint = OuterLine.P0.ConvertPoint2DtoPoint3D();
                this.RegenGeometry(_outerStartPoint, _outerEndPoint);
                this.RegenMode = regenType.RegenAndCompile;
            }
        }

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
        public Segment2D CenterLine { get; set; }
        public Segment2D OuterLine { get; set; }
        public Segment2D InterLine { get; set; }
        public List<Segment2D> BoxLines { get; set; } = new List<Segment2D>();
        public List<Point3D> JoistVertices { get; set; } = new List<Point3D>();
        public List<Point3D> CenterlineVertices { get; set; } = new List<Point3D>();

        public int FullLength { get; set; }
        public OutTrigger OutTriggerReference { get; private set; }
        public OutTrigger2D(Plane plane, Point3D outerStartPoint,Point3D outerEndPoint,OutTrigger outrigger,bool flipped = false, int thickness= 90,int  outSizeLength = 450,int inSizeLength = 900, double textHeight = 50) : base(plane, outerStartPoint, textHeight, Text.alignmentType.MiddleCenter)
        {
            Id = Guid.NewGuid();
            _flipped = flipped;
            _thickness = thickness;
            _outerStartPoint = outerStartPoint;
            _outerEndPoint = outerEndPoint;
            _insizeLength = inSizeLength;
            _outSizeLength = outSizeLength;
            OutTriggerReference = outrigger;
            OutTriggerReference.PropertyChanged+= OutTriggerReferenceOnPropertyChanged;
            RegenGeometry(outerStartPoint, outerEndPoint);
        }

        private void OutTriggerReferenceOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OutTriggerReference.FramingInfo))
            {
                if (OutTriggerReference.FramingInfo!= null)
                {
                    Thickness = OutTriggerReference.FramingInfo.Depth * OutTriggerReference.FramingInfo.NoItem;
                }
                
            }
        }

        private void RegenGeometry(Point3D startPoint, Point3D endPoint, bool outTriggerA = false, bool outTriggerB = false, bool flipperOutriggA = false, bool flippedOutriggerB = false)
        {
            var thickness = Thickness;
            var flippedFactor = -1;
            if (_flipped)
            {
                flippedFactor = 1;
            }
            OuterLine = new Segment2D(OuterStartPoint, OuterEndPoint);
            InterLine = OuterLine.Offset(Thickness*flippedFactor);
            _innerStartPoint = InterLine.P0.ConvertPoint2DtoPoint3D();
            _innerEndPoint = InterLine.P1.ConvertPoint2DtoPoint3D();
            CenterLine = OuterLine.Offset(flippedFactor*(double)Thickness / 2);
            CenterLine.ExtendBy(-(double)Thickness / 2, -(double)Thickness / 2);
            StartPoint = CenterLine.P0.ConvertPoint2DtoPoint3D();
            EndPoint = CenterLine.P1.ConvertPoint2DtoPoint3D();
            CenterlineVertices.Clear();
            CenterlineVertices.Add(StartPoint);
            CenterlineVertices.Add( Point3D.MidPoint(StartPoint, EndPoint));
            CenterlineVertices.Add(EndPoint);
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
        private void UpdateDistance()
        {
            if (this.OuterStartPoint != null && this.OuterEndPoint != null)
            {
                this.FullLength = (int)OuterStartPoint.DistanceTo(OuterEndPoint);
                if (this.OutTriggerReference!=null)
                {
                    OutTriggerReference.FullLength = (int) FullLength;
                }

            }
        }

        protected override void Draw(DrawParams data)
        {
            this.DrawOutrigger(data,true);
        }

        protected virtual void DrawOutrigger(DrawParams data, bool disableCulling)
        {
            base.PreDraw(data, disableCulling);
            //this.DrawExtraGeometry(data);
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

        public override void Regen(RegenParams data)
        {
            this.TextString = String.Empty;
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
            JoistVertices.Clear();
            JoistVertices.Add(OuterStartPoint);
            JoistVertices.Add(InnerStartPoint);
            JoistVertices.Add(InnerEndPoint);
            JoistVertices.Add(OuterEndPoint);
            base.UpdateBoundingBox(data);
            this.RegenMode = regenType.CompileOnly;
        }
        public IEntityVm CreateEntityVm(IEntitiesManager entitiesManager)
        {
            return new OutTrigger2dVm(this, entitiesManager);
        }
        protected override void DrawForSelection(GfxDrawForSelectionParams data)
        {
            this.DrawOutrigger(data,false);
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

        public void Flipper(Point3D startPoint,Point3D endPoint)
        {
            _flipped = !_flipped;
            var line = new Line(this._outerStartPoint, this._outerEndPoint);
            var p0 = startPoint;
            var p1 = endPoint;
            if (endPoint.X < startPoint.X || endPoint.Y < startPoint.Y)
            {
                Utility.Swap(ref p0, ref p1);

                //_clickPoints[0] = p0;
                //_clickPoints[1] = p1;
            }
            var axisX = new Vector3D(p0, p1);
            var mirrorPlane = new Plane(startPoint, axisX, Vector3D.AxisZ);
            var mirror = new Mirror(mirrorPlane);
            line.TransformBy(mirror);
            this._outerStartPoint = line.StartPoint;
            this._outerEndPoint = line.EndPoint;
            this.RegenGeometry(OuterStartPoint,OuterEndPoint);
            this.RegenMode = regenType.RegenAndCompile;
        }
    }
}
