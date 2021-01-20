using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using AppModels.CustomEntity.CustomEntitySurrogate;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings;
using AppModels.ViewModelEntity;
using devDept.Geometry;
using devDept.Serialization;

namespace AppModels.CustomEntity
{
    public sealed class Beam2D : FramingRectangleContainHangerAndOutTrigger,IEntityVmCreateAble
    {
        #region private Field
        //private Point3D _outerStartPoint;
        ////private Point3D _innerStartPoint;
        //private Point3D _outerEndPoint;
        ////private Point3D _innerEndPoint;
        //private IFraming _framingReference;
        //private int _thickness;
        private bool _isBeamUnder;
        //private bool _outTriggerAFlipped;
        //private bool _outTriggerBFlipped;
        #endregion

        #region Properties
        //public Guid Id { get; set; }
        //public Guid LevelId { get; set; }
        //public Guid FramingSheetId { get; set; }

        //public Point3D StartPoint { get; set; }
        //public Point3D EndPoint { get; set; }
        //public Point3D OuterStartPoint
        //{
        //    get => _outerStartPoint;
        //    set
        //    {
        //        _outerStartPoint = value;
        //        if (_outerStartPoint == null || _outerEndPoint == null)
        //        {
        //            return;
        //        }
        //        this.RegenFramingGeometry(_outerStartPoint, _outerEndPoint);
        //    }
        //}
        //public Point3D InnerStartPoint { get; set; }
        ////{
        ////    get => _innerStartPoint;
        ////    set
        ////    {
        ////        _innerStartPoint = value;
        ////        this.RegenMode = regenType.RegenAndCompile;
        ////    }
        ////}
        //public Point3D OuterEndPoint
        //{
        //    get => _outerEndPoint;
        //    set
        //    {
        //        _outerEndPoint = value;
        //        if (_outerStartPoint == null || _outerEndPoint == null)
        //        {
        //            return;
        //        }
        //        this.RegenFramingGeometry(_outerStartPoint, _outerEndPoint);
        //    }
        //}
        //public Point3D InnerEndPoint { get; set; }
        ////{
        ////    get => _innerEndPoint;
        ////    set
        ////    {
        ////        _innerEndPoint = value;
        ////        this.RegenMode = regenType.RegenAndCompile;
        ////    }

        ////}
        //public Point3D MidMPoint3D => Point3D.MidPoint(StartPoint, EndPoint);
        //public int Thickness
        //{
        //    get => _thickness;
        //    set

        //    {
        //        _thickness = value;
        //        if (_outerStartPoint == null || _outerEndPoint == null)
        //        {
        //            return;
        //        }
        //        this.RegenFramingGeometry(this.OuterStartPoint, this.OuterEndPoint);
        //    }
        //}
        //public int Depth { get; set; }

        //public IFraming FramingReference
        //{
        //    get => _framingReference;
        //    set
        //    {
        //        _framingReference = value;
        //        if(_framingReference!=null)
        //        {
        //            _framingReference.PropertyChanged += FramingPropertiesChanged;
        //        }
        //    }
        //}
        //public Guid FramingReferenceId { get; set; }
        //public List<Point3D> FramingVertices { get; set; } = new List<Point3D>();
        ////public List<Point3D> CenterlineVertices { get; set; } = new List<Point3D>();
        //public double FullLength
        //{
        //    get
        //    {
        //        if (OuterStartPoint != null && OuterEndPoint != null)
        //        {
        //            return OuterStartPoint.DistanceTo(OuterEndPoint);
        //        }

        //        return 0.0;
        //    }
        //    set
        //    {

        //    }
        //}

        //public Guid? HangerAId { get; set; }
        //public Guid? HangerBId { get; set; }
        //public Hanger2D HangerA { get; set; }
        //public Hanger2D HangerB { get; set; }
        //public bool IsHangerA { get; set; }
        //public bool IsHangerB { get; set; }
        //public Guid? OutTriggerAId { get; set; }
        //public Guid? OutTriggerBId { get; set; }
        //public OutTrigger2D OutTriggerA { get; set; }
        //public OutTrigger2D OutTriggerB { get; set; }
        //public bool IsOutTriggerA { get; set; }
        //public bool IsOutTriggerB { get; set; }
        //public bool OutTriggerAFlipped
        //{
        //    get => _outTriggerAFlipped;
        //    set
        //    {
        //        if (this.OutTriggerA == null) return;
        //        _outTriggerAFlipped = value;

        //        OutTriggerA?.Flipper(StartPoint, EndPoint);

        //    }
        //}
        //public bool OutTriggerBFlipped
        //{
        //    get => _outTriggerBFlipped;
        //    set
        //    {
        //        if (this.OutTriggerB == null) return;
        //        _outTriggerBFlipped = value;

        //        OutTriggerB?.Flipper(StartPoint, EndPoint);
        //    }
        //}
        public bool IsBeamUnder
        {
            get => _isBeamUnder;
            set
            {
                _isBeamUnder = value;
                {
                    if (FramingReference!=null&& FramingReference.FramingType!=FramingTypes.SteelBeam)
                    {
                        Color = _isBeamUnder ? Color.FromArgb(255, 0, 127) : Color.Crimson;
                    }
                }
                
            }
        }
        public bool ShowDimension { get; set; }
        public List<Point3D> StartVerticesBox { get; set; }
        public List<Point3D> EndVerticesBox { get; set; }
        public Segment2D DimensionLine { get; set; }

        #endregion

        #region Constructor

        protected override void FramingPropertiesChanged(object sender, PropertyChangedEventArgs e)
        {
            base.FramingPropertiesChanged(sender, e);
            if (e.PropertyName=="FramingType")
            {
                if (FramingReference.FramingType == FramingTypes.SteelBeam)
                {
                    Color = Color.Blue;
                }
                else
                {
                    IsBeamUnder = _isBeamUnder;
                }
            }
        }

        protected override void SetFramingColor(int thickness)
        {
            
        }

        public Beam2D(Point3D outerStartPoint, Point3D outerEndPoint, FramingBase framingReference, int thickness = 90, bool flipped = false,bool isBeamUnder = false) : base(outerStartPoint, outerEndPoint, framingReference, thickness, flipped)
        {
            IsBeamUnder = isBeamUnder;
        }

        public Beam2D(Point3D outerStartPoint, Point3D outerEndPoint, int thickness, bool flipped) : base(outerStartPoint, outerEndPoint, thickness, flipped)
        {
        }

        public Beam2D(FramingRectangle2D another) : base(another)
        {
        }

        #endregion
        //public Beam2D(Plane wallPlan, Point3D outerstartPoint, Point3D outerEndPoint, FBeam beamRef, int thickness = 45, bool isBeamUnder = true, bool showDimension = false, double textHeight = 90) :
        //    base(wallPlan, outerstartPoint, textHeight, Text.alignmentType.BaselineCenter)
        //{
        //    //Id = new Guid();
        //    //LevelId = beamRef.LevelId;
        //    //FramingSheetId = beamRef.FramingSheetId;
        //    //_thickness = thickness;
        //    //_outerStartPoint = outerstartPoint;
        //    //_outerEndPoint = outerEndPoint;
        //    //ColorMethod = colorMethodType.byEntity;
        //    //IsBeamUnder = isBeamUnder;
        //    //ShowDimension = showDimension;
        //    //this.LineTypeScale = 10;
        //    //FramingReference = beamRef;
        //    //FramingReferenceId = beamRef.Id;
        //    //RegenFramingGeometry(_outerStartPoint, _outerEndPoint);
        //}
        //public Beam2D(Text another):base(another)
        //{ }
        //public Beam2D(Beam2D another) : base(another)
        //{
        //    Id = Guid.NewGuid();
        //    LevelId = another.LevelId;
        //    FramingSheetId = another.FramingSheetId;
        //    _thickness = another.Thickness;
        //    _outerStartPoint = (Point3D)another.OuterStartPoint.Clone();
        //    _outerEndPoint = (Point3D)another.OuterEndPoint.Clone();
        //    InnerStartPoint = (Point3D)another.InnerStartPoint.Clone();
        //    InnerEndPoint = (Point3D)another.InnerEndPoint.Clone();
        //    StartPoint = (Point3D)another.StartPoint.Clone();
        //    EndPoint = (Point3D)another.EndPoint.Clone();
        //    FramingReference = (FramingBase)another.FramingReference.Clone();
        //    FramingReferenceId = FramingReference.Id;

        //}


        //private void FramingPropertiesChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    switch (e.PropertyName)
        //    {
        //        case nameof(FramingReference.FramingInfo):
        //            if (FramingReference.FramingInfo != null)
        //            {
        //                Thickness = FramingReference.FramingInfo.Depth * FramingReference.FramingInfo.NoItem;
        //                Depth = FramingReference.FramingInfo.Thickness;
        //            }
        //            break;
        //        default: break;

        //    }
        //}
        //private void RegenFramingGeometry(Point3D startPoint, Point3D endPoint, bool flipped = false)
        //{
        //    var flippedFactor = 1;
        //    if (flipped)
        //    {
        //        flippedFactor = -1;
        //    }

        //    var outerLine = new Segment2D(OuterStartPoint, OuterEndPoint);
        //    var interLine = outerLine.Offset(Thickness * flippedFactor);
        //    InnerStartPoint = interLine.P0.ConvertPoint2DtoPoint3D();
        //    InnerEndPoint = interLine.P1.ConvertPoint2DtoPoint3D();

        //    var centerLine = outerLine.Offset((double)Thickness * flippedFactor / 2);
        //    centerLine.ExtendBy(-(double)Thickness / 2, -(double)Thickness / 2);

        //    StartPoint = centerLine.P0.ConvertPoint2DtoPoint3D();
        //    EndPoint = centerLine.P1.ConvertPoint2DtoPoint3D();

        //    var offsetLine = outerLine.Offset(-(double)Thickness * flippedFactor / 2);
        //    DimensionLine = offsetLine;

        //    var segment1 = new Segment2D((Point3D)OuterStartPoint.Clone(), (Point3D)OuterEndPoint.Clone());
        //    segment1.ExtendBy(-Thickness, -Thickness);
        //    var segment2 = new Segment2D((Point3D)InnerStartPoint.Clone(), (Point3D)InnerEndPoint.Clone());
        //    segment2.ExtendBy(-Thickness, -Thickness);
        //    var p1 = segment1.P0;
        //    var p2 = segment2.P0;
        //    var p3 = segment1.P1;
        //    var p4 = segment2.P1;

        //    StartVerticesBox = new List<Point3D>() { OuterStartPoint, p1.ConvertPoint2DtoPoint3D(), p2.ConvertPoint2DtoPoint3D(), InnerStartPoint };
        //    EndVerticesBox = new List<Point3D>() { OuterEndPoint, InnerEndPoint, p4.ConvertPoint2DtoPoint3D(), p3.ConvertPoint2DtoPoint3D() };

        //    var tempLine = new Segment2D(OuterStartPoint, InnerStartPoint);
        //    StartVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
        //    tempLine = new Segment2D(p1, p2);
        //    StartVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
        //    tempLine = new Segment2D(OuterStartPoint, p1);
        //    StartVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
        //    tempLine = new Segment2D(InnerStartPoint, p2);
        //    StartVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
        //    tempLine = new Segment2D(OuterEndPoint, InnerEndPoint);
        //    EndVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
        //    tempLine = new Segment2D(p3, p4);
        //    EndVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
        //    tempLine = new Segment2D(OuterEndPoint, p3);
        //    EndVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
        //    tempLine = new Segment2D(InnerEndPoint, p4);
        //    EndVerticesBox.Add(tempLine.MidPoint.ConvertPoint2DtoPoint3D());
        //    this.UpdateDistance();
        //    this.RegenMode = regenType.RegenAndCompile;
        //}
        protected override void RegenFramingGeometry(Point3D outerStartPoint, Point3D outerEndPoint, bool flipped = false)
        {
            base.RegenFramingGeometry(outerStartPoint, outerEndPoint, flipped);
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
        }
        //private void UpdateDistance()
        //{
        //    if (this.OuterStartPoint != null && this.OuterEndPoint != null)
        //    {
        //        if (this.FramingReference != null)
        //        {
        //            this.FramingReference.FullLength = (int)FullLength;
        //        }
        //    }
        //}

        //protected override void Draw(DrawParams data)
        //{
        //    DrawFraming(data, true, false);
        //}

        //private void DrawFraming(DrawParams data, bool disableCulling, bool drawForSelection)
        //{
        //    base.PreDraw(data);
        //    DrawExtraGeometry(data);
        //    RenderContextBase renderContext = data.RenderContext;
        //    renderContext.PushModelView();
        //    if (renderContext.IsDirect3D)
        //    {
        //        this.PushLineSize(data);
        //        this.DrawEntity(renderContext,null);
        //        this.PopLineSize(data);
        //    }
        //    else
        //    {
        //        renderContext.Draw(this.drawData,primitiveType.Undefined);
        //    }

        //    if (ShowDimension)
        //    {
        //        this.DrawText(data);
        //    }
        //    renderContext.PopModelView();
        //    PostDraw(data);
        //}

        //protected override void DrawEntity(RenderContextBase context, object myParams)
        //{
        //    if (base.Compiling)
        //    {
        //        context.DrawLine(StartPoint, EndPoint);
        //    }
        //    else
        //    {
        //        this.drawData.DrawD3D(context, 0);
        //    }

        //    context.PushModelView();
        //    DrawBackGroundRectangle(context);
        //    context.PopModelView();
        //}
        //private void DrawBackGroundRectangle(RenderContextBase context)
        //{
        //    if (context.CurrentLineWidth > 1f)
        //    {
        //        context.SetLineSize(1f, true, false);
        //    }
        //    this.DrawRectangle(context);
        //}
        //private void DrawRectangle(RenderContextBase context)
        //{
        //    context.PushModelView();
        //    var points = new Point3D[] { OuterStartPoint, OuterEndPoint, InnerEndPoint, InnerStartPoint };
        //    context.DrawQuads(points, new Vector3D[]
        //    {
        //        Vector3D.AxisZ
        //    });
        //    context.PopModelView();
        //}
        //protected override void DrawForSelection(GfxDrawForSelectionParams data)
        //{
        //    this.DrawFraming(data, false, true);
        //}
        //internal void PopLineSize(DrawParams drawParams)
        //{
        //    if (drawParams.RenderContext.CurrentLineWidth > 1f)
        //    {
        //        drawParams.RenderContext.PopShader();
        //    }
        //}
        //internal void PushLineSize(DrawParams drawParams)
        //{
        //    if (drawParams.RenderContext.CurrentLineWidth > 1f)
        //    {
        //        drawParams.RenderContext.PushShader();
        //        drawParams.RenderContext.EnableThickLines();
        //    }
        //}
        //private void DrawExtraGeometry(DrawParams data)
        //{

        //}
        //private void DrawDashWallLine(DrawParams data, Point3D startPoint, Point3D endPoint)
        //{
        //    var lineTypePattername = this.LineTypeName;
        //    var lineTypePattern = data.LineTypes[lineTypePattername];
        //    if (lineTypePattern.Length * this.LineTypeScale > data.ScreenToWorld4Times && data.ScreenToWorld > 0f)
        //    {
        //        float num = 1f;
        //        if (data.FullParents != null && data.FullParents.Count > 0)
        //        {
        //            num = (float)data.FullParents.Peek().GetFullTransformation(data.Blocks).ScaleFactorX;
        //        }

        //        List<Point3D> list;
        //        List<Point3D> list2;
        //        var vertices = new Point3D[] { startPoint, endPoint };
        //        lineTypePattern.GetPatternVertices(data.MaxPatternRepetitions, vertices, this.LineTypeScale / num, out list, out list2);
        //        for (int i = 0; i < list.Count; i += 2)
        //        {
        //            data.RenderContext.DrawBufferedLine(list[i], list[i + 1]);
        //        }
        //        data.RenderContext.DrawPointsOnTheFly(list2.ToArray());
        //    }

        //}

        //public override void Regen(RegenParams data)
        //{
        //    var distance = this.FullLength;
        //    this.TextString = distance.ToString(CultureInfo.InvariantCulture);
        //    if (DimensionLine!=null)
        //    {
        //        this.InsertionPoint = new Point3D(DimensionLine.MidPoint.X, DimensionLine.MidPoint.Y, 0);
        //    }
        //    base.Regen(data);
        //    var listPoint = new List<Point3D>(Vertices);
        //    if (!listPoint.Contains(this.StartPoint))
        //    {
        //        listPoint.Add(StartPoint);
        //    }
        //    if (!listPoint.Contains(this.EndPoint))
        //    {
        //        listPoint.Add(EndPoint);
        //    }

        //    if (!listPoint.Contains(OuterStartPoint))
        //    {
        //        listPoint.Add(OuterStartPoint);
        //    }
        //    if (!listPoint.Contains(InnerStartPoint))
        //    {
        //        listPoint.Add(InnerStartPoint);
        //    }
        //    if (!listPoint.Contains(OuterEndPoint))
        //    {
        //        listPoint.Add(OuterEndPoint);
        //    }
        //    if (!listPoint.Contains(InnerEndPoint))
        //    {
        //        listPoint.Add(InnerEndPoint);
        //    }
        //    Vertices = listPoint.ToArray();
        //    FramingVertices.Clear();
        //    FramingVertices.Add(OuterStartPoint);
        //    FramingVertices.Add(InnerStartPoint);
        //    FramingVertices.Add(InnerEndPoint);
        //    FramingVertices.Add(OuterEndPoint);
        //    base.UpdateBoundingBox(data);
        //    this.RegenMode = regenType.CompileOnly;
        //}
        //public override void TransformBy(Transformation transform)
        //{
        //    //base.TransformBy(transform);
        //    var beamLine = new Line(this.OuterStartPoint, this.OuterEndPoint);
        //    beamLine.TransformBy(transform);
        //    this._outerStartPoint = beamLine.StartPoint;
        //    this._outerEndPoint = beamLine.EndPoint;
        //    this.RegenFramingGeometry(_outerStartPoint, _outerEndPoint, false);
        //}
        //public override void Translate(double dx, double dy, double dz = 0)
        //{
        //    var joistLine = new Line((Point3D)this.OuterStartPoint.Clone(), (Point3D)this.OuterEndPoint.Clone());
        //    joistLine.Translate(dx, dy, dz);
        //    this._outerStartPoint = joistLine.StartPoint;
        //    this._outerEndPoint = joistLine.EndPoint;
        //    this.RegenFramingGeometry(OuterStartPoint, OuterEndPoint);
        //}
        //public bool Project(Point3D point, out double t)
        //{
        //    Segment3D segment = new Segment3D(this.OuterStartPoint, this.OuterEndPoint);
        //    Line joistLine = new Line(this.OuterStartPoint, this.OuterEndPoint);
        //    return joistLine.Project(point, out t);
        //}
        //public Point3D PointAt(double t)
        //{
        //    var joitsLine = new Line(OuterStartPoint, OuterEndPoint);
        //    return joitsLine.PointAt(t);
        //}
        //public Beam2D Offset(double amount, Vector3D planeNormal, double tolerance, bool sharp)
        //{
        //    Beam2D framing = (Beam2D)Clone();
        //    Line framingLine = new Line(framing.OuterStartPoint, framing.OuterEndPoint);
        //    Vector3D vector3D = Vector3D.Cross(framingLine.Tangent, planeNormal);
        //    vector3D.Normalize();
        //    Vector3D v = vector3D * amount;
        //    framingLine.Translate(v);
        //    framing.OuterStartPoint = framingLine.StartPoint;
        //    framing.OuterEndPoint = framingLine.EndPoint;
        //    return framing;
        //}
        //public Beam2D Offset(double amount, Vector3D planeNormal)
        //{
        //    return this.Offset(amount, planeNormal, 0.0, false);
        //}
        
        public IEntityVm CreateEntityVm(IEntitiesManager entitiesManager)
        {
            return new Beam2DVm(this, entitiesManager);
        }

        public override EntitySurrogate ConvertToSurrogate()
        {
            return new Beam2DSurrogate(this);
        }

        //public void SetFlippedOutriggerA(bool outTriggerAFlipped)
        //{
        //    _outTriggerAFlipped = outTriggerAFlipped;
        //}
        //public void SetFlippedOutriggerB(bool outTriggerBFlipped)
        //{
        //    _outTriggerBFlipped = outTriggerBFlipped;
        //}
        public override object Clone()
        {
            return new Beam2D(this);
        }

        //internal void SetInnerStartPoint(Point3D point)
        //{
        //    _innerStartPoint = point;
        //}

        //internal void SetOuterStartPoint(Point3D point)
        //{
        //    _outerEndPoint = point;
        //}

    }
}
