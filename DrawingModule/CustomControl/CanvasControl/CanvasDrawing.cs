// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CanvasDrawing.cs" company="John Nguyen">
// John Nguyen
// </copyright>
// <summary>
//   Defines the CanvasDrawing type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using AppDataBase.DataBase;
using ApplicationInterfaceCore;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels;
using AppModels.CustomEntity;
using AppModels.EventArg;
using AppModels.Interaface;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Graphics;
using DrawingModule.Application;
using DrawingModule.CommandClass;
using DrawingModule.Control;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.EditingTools;
using DrawingModule.Enums;
using DrawingModule.Helper;
using DrawingModule.Views;
using Size = System.Drawing.Size;

namespace DrawingModule.CustomControl.CanvasControl
{
    /// <summary>
    /// The my model.
    /// </summary>
    public partial class CanvasDrawing : Model, ICadDrawAble
    {

        #region Field

        public static readonly DependencyProperty ActiveLayerNameProperty =
            DependencyProperty.Register("ActiveLayerName", typeof(string), typeof(CanvasDrawing),
                new PropertyMetadata("Default"));
        public static readonly DependencyProperty EntitiesManagerProperty =
            DependencyProperty.Register("EntitiesManager", typeof(IEntitiesManager), typeof(CanvasDrawing),
                new PropertyMetadata(null, EntitiesManagerChangedCallBack));
        public static readonly DependencyProperty LayersManagerProperty =
            DependencyProperty.Register("LayersManager", typeof(ILayerManager), typeof(CanvasDrawing),
                new PropertyMetadata(null, LayersManagerChangedCallBack));
        public static readonly DependencyProperty JobModelProperty =
            DependencyProperty.Register("JobModel", typeof(IJob), typeof(CanvasDrawing),
                new PropertyMetadata(null));
        private bool _cursorOutSide;
        private bool _isUserInteraction;
        private bool _isSnappingEnable;
        private bool _isOrthoModeEnable;
        private readonly Plane _drawingPlane = Plane.XY;
        private SelectTool _selectTool;
        private bool _waitingForSelection;
        private bool _waitingForPickSelection;
        private Entity _entityUnderMouse;
        private Point3D _currentPoint;
        private bool _isUserClicked = false;
        private Point3D _lastClickPoint;
        
        private Point3D _basePoint;

        #endregion

        #region Properties
        public IEntitiesManager EntitiesManager
        {
            get => (IEntitiesManager)GetValue(EntitiesManagerProperty);
            set => SetValue(EntitiesManagerProperty, value);
        }

        public ILayerManager LayersManager
        {
            get => (ILayerManager)GetValue(LayersManagerProperty);
            set => SetValue(LayersManagerProperty, value);
        }

        public IJob JobModel
        {
            get => (IJob)GetValue(JobModelProperty);
            set => SetValue(JobModelProperty, value);
        }
        //public DrawEntitiesType DrawEntitiesType { get; set; }
        public int CurrentIndex { get; set; }
        public bool IsUserInteraction
        {
            get => _isUserInteraction;
            private set
            {
                _isUserInteraction = value;
                OnUserInteractionMessage();
            }
        }
        public string ActiveLayerName
        {
            get => (string)GetValue(ActiveLayerNameProperty);
            set => SetValue(ActiveLayerNameProperty, value);
        }
        public int DimTextHeight { get; set; }
        public bool IsAbsotuleInput { get; set; }
        public bool IsDrawEntityUnderMouse { get; set; }
        public bool IsDrawingMode { get; private set; }
        public Plane DrawingPlane => this._drawingPlane;
        public double ScaleFactor
        {
            get
            {
                if (this.CurrentTool == null)
                {
                    return 1;
                }
                else
                {
                    return this.CurrentTool.ScaleFactor.Round();
                }
            }
        }
        public double CurrentLengthDimension
        {
            get
            {
                if (BasePoint3D == null || CurrentPoint == null)
                {
                    return 0;
                }
                return BasePoint3D.DistanceTo(CurrentPoint).Round();
            }
        }
        public double CurrentAngleDimension
        {
            get
            {
                if (this.BasePoint3D == null || this.CurrentPoint == null) return 0;
                var vector = new Vector2D(this.BasePoint3D, this.CurrentPoint);
                var angle = 0.0;
                if (this.CurrentTool != null && this.CurrentTool.ReferencePoint != null)
                {
                    angle = this.CurrentTool.CurrentAngle;
                }
                else
                {
                    angle = -Vector2D.SignedAngleBetween(vector, Vector2D.AxisX) * (180 / Math.PI);
                }

                angle = angle.Round();
                return angle;
            }
        }
        public double CurrentHeightDimension
        {
            get
            {
                if (this.CurrentTool != null && Math.Abs(this.CurrentTool.CurrentHeight) > 0.0001)
                {

                    return this.CurrentTool.CurrentHeight;
                }
                if (this.LastClickPoint == null || this.CurrentPoint == null) return 0;
                var height = CurrentPoint.Y - LastClickPoint.Y;
                if (height < 0)
                {
                    height = (-1) * height;
                }

                return height.Round();

            }
        }
        public double CurrentWidthDimension
        {
            get
            {
                if (this.CurrentTool != null && Math.Abs(this.CurrentTool.CurrentWidth) > 0.0001)
                {

                    return this.CurrentTool.CurrentWidth;
                }
                if (this.LastClickPoint == null || this.CurrentPoint == null) return 0;
                var width = CurrentPoint.X - LastClickPoint.X;
                if (width < 0)
                {
                    width = (-1) * width;

                }

                return width.Round();

            }
        }
        //public string CurrentText => "Test";
        //public double CurrentTextHeight => 100;
        //public double CurrentTextAngle => 0;

        public string CurrentText
        {
            get
            {
                switch (CurrentTool)
                {
                    case null:
                        return "";
                    case ITextTool textTool:
                        return textTool.TextInput;
                    default:
                        return "";
                }
            }
            set
            {
                switch (CurrentTool)
                {
                    case null:
                        return;
                    case ITextTool textTool:
                        textTool.TextInput = value;
                        break;
                    default:
                        break;
                }
            }
        }

        public double CurrentTextHeight
        {

            get
            {
                switch (CurrentTool)
                {
                    case null:
                        return 0;
                    case ITextTool textTool:
                        return textTool.TextHeight;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (CurrentTool)
                {
                    case null:
                        return;
                    case ITextTool textTool:
                        textTool.TextHeight = value;
                        break;
                    default:
                        break;
                }
            }

        }
        public double CurrentTextAngle
        {
            get
            {
                switch (CurrentTool)
                {
                    case null:
                        return 0;
                    case ITextTool textTool:
                        return textTool.TextAngle;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (CurrentTool)
                {
                    case null:
                        return;
                    case ITextTool textTool:
                        textTool.TextAngle = value;
                        break;
                    default:
                        break;
                }
            }
        }

        public int LeaderSegmentNumber
        {
            get
            {
                switch (CurrentTool)
                {
                    case null:
                        return 0;
                    case ILeaderTool leaderTool:
                        return leaderTool.LeaderSegment;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (CurrentTool)
                {
                    case null:
                        return;
                    case ILeaderTool leaderTool:
                        leaderTool.LeaderSegment = value;
                        break;
                    default:
                        break;
                }
            }
        }

        public int ArrowSize
        {
            get
            {
                switch (CurrentTool)
                {
                    case null:
                        return 0;
                    case ILeaderTool leaderTool:
                        return leaderTool.ArrowSize;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (CurrentTool)
                {
                    case null:
                        return;
                    case ILeaderTool leaderTool:
                        leaderTool.ArrowSize = value;
                        break;
                    default:
                        break;
                }
            }
        }

        //private void UpdateFocusDynamicInput()
        //{
        //    if (this._dynamicInput!=null)
        //    {
        //        _dynamicInput.FocusPreviousTextBox(CurrentTool.DefaultDynamicInputTextBoxToFocus);
        //    }
        //}


        public bool IsSnappingEnable
        {
            get => this._isSnappingEnable;
            set => this.SetProperty(ref _isSnappingEnable, value);
        }
        public bool IsOrthoModeEnable
        {
            get => this._isOrthoModeEnable;
            set
            {
                this.SetProperty(ref _isOrthoModeEnable, value);
            }
        }
        public bool IsProcessingTool { get; private set; }
        public PromptStatus PromptStatus { get; set; }
        public Point3D CurrentPoint
        {
            get => _currentPoint;
            set
            {
                SetProperty(ref _currentPoint, value);
                this.RaisePropertiesChangedForDynamicInput();
                RaisePropertyChanged(nameof(TempLineType));
                _isMouseMove = true;
            }
        }
        public Point3D LastClickPoint
        {
            get => _lastClickPoint;
            set
            {
                SetProperty(ref _lastClickPoint, value);
                this.RaisePropertiesChangedForDynamicInput();
                RaisePropertyChanged(nameof(TempLineType));
            }
        }
        public Point3D BasePoint3D
        {
            get => _basePoint;
            set
            {
                SetProperty(ref _basePoint, value);
                RaisePropertiesChangedForDynamicInput();
            }
        }

        #endregion

        #region Constructor
        public CanvasDrawing()

        {
            //PickBoxSize = 16;
            DimTextHeight = 500;
            IsUserInteraction = false;
            _waitingForSelection = false;
            _waitingForPickSelection = false;
            IsSnappingEnable = true;
            IsOrthoModeEnable = true;
            IsAbsotuleInput = false;
            this._selectTool = new SelectTool();
            this.PromptStatus = PromptStatus.None;
            this.IsDrawingMode = false;
            this._entityUnderMouse = null;
            this.CurrentIndex = -1;
            Loaded += CanvasDrawing_Loaded;
            this.PrepareLineTypes();

            //SelectionColor = null;
            //SelectionColor = Color.FromArgb(50,Color.Chocolate);


        }

        private void PrepareLineTypes()
        {
            LineTypes.Add("Dash Dot", new float[] { 5f, -1f, 1f, -1f });
            LineTypes.Add("Dash Space", new float[] { 5f, -5f });
        }
        private void CanvasDrawing_Loaded(object sender, RoutedEventArgs e)
        {
            _selectTool.SetEntitiesManager(EntitiesManager);
            _selectTool.SetLayersManager(LayersManager);

        }
        #endregion

        #region Override Method
        protected override void DrawOverlay(DrawSceneParams data)
        {

            //if (!_firstTime)
            //{
            //    _firstTime = true;

            //    Font font = new Font("Tahoma", 50, System.Drawing.FontStyle.Italic);

            //    Image image1 = GetTextImage("My Text", font, Color.Red, Color.Transparent, ContentAlignment.TopRight, RotateFlipType.RotateNoneFlipY);
            //    texture = renderContext.CreateTexture2D(image1, textureFilteringFunctionType.Nearest, textureFilteringFunctionType.Nearest, true, true);
            //    image1.Dispose();
            //    font.Dispose();
            //}

            //if (texture !=null)
            //{
            //    DrawTexture(texture, 100, 100, ContentAlignment.BottomLeft);
            //    Draw
            //}

            var drawInteractiveArgs = new DrawInteractiveArgs(this.CurrentPoint, this.LastClickPoint, data, _mousePosition);
            if (IsProcessingTool && this._currentTool != null)
            {
                //this._drawInteractiveHandler.Invoke(this, drawInteractiveArgs);
                DrawOverlay_Drawing(drawInteractiveArgs);
                if (this._waitingForSelection)
                {
                    this._selectTool.DrawInteractiveHandler.Invoke(this, drawInteractiveArgs);
                    if (this.CurrentTool != null && this.CurrentTool.IsSnapEnable && this.IsSnappingEnable)
                    {
                        if (SnapPoint != null)
                        {
                            DrawSnappPointUtl.DisplaySnappedVertex(this, SnapPoint, renderContext);
                        }

                        if (PolaTrackingPoints.Count !=0)
                        {
                            DrawSnappPointUtl.DisplayTrackingPoint(this, PolaTrackingPoints, renderContext);
                            //DrawInteractiveUntilities.DrawInteractiveSpotLine(LastClickPoint,CurrentPoint,this);
                            FindingPolaPoint();
                            if (PolaTrackedPoint != null)
                            {
                                CurrentPoint = PolaTrackedPoint;
                                PolaTrackedPoint = null;
                            }
                        }
                    }
                }
                else if (IsSnappingEnable)
                {
                    if (this._currentTool != null)
                    {
                        if (this._currentTool.IsSnapEnable)
                        {
                            if (SnapPoint != null)
                            {
                                DrawSnappPointUtl.DisplaySnappedVertex(this, SnapPoint, renderContext);
                            }
                            if (PolaTrackingPoints.Count != 0)
                            {
                                DrawSnappPointUtl.DisplayTrackingPoint(this, PolaTrackingPoints, renderContext);
                                //DrawInteractiveUntilities.DrawInteractiveSpotLine(LastClickPoint,CurrentPoint,this);
                                FindingPolaPoint();
                                if (PolaTrackedPoint!=null)
                                {
                                    CurrentPoint = PolaTrackedPoint;
                                    PolaTrackedPoint = null;
                                }
                            }

                        }
                    }

                }
            }
            else
            {
                this._selectTool.DrawInteractiveHandler.Invoke(this, drawInteractiveArgs);
            }
            if (_entityUnderMouse != null)
            {
                this.DrawInteractiveEntityUnderMouse(_entityUnderMouse);
            }
            base.DrawOverlay(data);
        }

        private void FindingPolaPoint()
        {
            if (TempLineType == AppModels.Enums.LineTypes.None || TempLineType == AppModels.Enums.LineTypes.SlantingLine)
            {
                return;
            }

            if (TempLineType == AppModels.Enums.LineTypes.HorizontalLine)
            {
                foreach (var polaTrackingPoint in PolaTrackingPoints)
                {
                    FindingVerticalTrackedPoint(polaTrackingPoint);
                }
            }
            else
            {
                foreach (var polaTrackingPoint in PolaTrackingPoints)
                {
                    FindingHorizontaltrackedPoint(polaTrackingPoint);
                }
            }
        }

        private void FindingVerticalTrackedPoint(Point3D polaTrackingPoint)
        {
            var newPoint = new Point3D(polaTrackingPoint.X,polaTrackingPoint.Y+1);
            var verticalSegment2d = new Segment2D(polaTrackingPoint,newPoint);
            var curentTempLineSegment = new Segment2D(LastClickPoint,CurrentPoint);
            Segment2D.IntersectionLine(verticalSegment2d, curentTempLineSegment, out var intersecPoint);
            if (intersecPoint!=null)
            {
                DrawInteractiveUntilities.DrawInteractiveSpotLine(polaTrackingPoint,intersecPoint.ConvertPoint2DtoPoint3D(),this);
                //var polaLine= new Segment2D(polaTrackingPoint,intersecPoint);
                var dist = CurrentPoint.DistanceTo(intersecPoint);
                if (dist<=10)
                {
                    PolaTrackedPoint = intersecPoint.ConvertPoint2DtoPoint3D();
                    DrawSnappPointUtl.DisplayTrackedPoint(this,PolaTrackedPoint,renderContext);
                }
            }
        }

        private void FindingHorizontaltrackedPoint(Point3D polaTrackingPoint)
        {
            var newPoint = new Point3D(polaTrackingPoint.X+1, polaTrackingPoint.Y );
            var horizontalSegment2d = new Segment2D(polaTrackingPoint, newPoint);
            var curentTempLineSegment = new Segment2D(LastClickPoint, CurrentPoint);
            Segment2D.IntersectionLine(horizontalSegment2d, curentTempLineSegment, out var intersecPoint);
            if (intersecPoint != null)
            {
                DrawInteractiveUntilities.DrawInteractiveSpotLine(polaTrackingPoint, intersecPoint.ConvertPoint2DtoPoint3D(), this);
                //var polaLine= new Segment2D(polaTrackingPoint,intersecPoint);
                var dist = CurrentPoint.DistanceTo(intersecPoint);
                if (dist <= 10)
                {
                    PolaTrackedPoint = intersecPoint.ConvertPoint2DtoPoint3D();
                    DrawSnappPointUtl.DisplayTrackedPoint(this, PolaTrackedPoint, renderContext);
                }
            }
        }

        private void DrawInteractiveEntityUnderMouse(Entity entity)
        {
            renderContext.SetLineSize(1.5f);
            renderContext.SetColorWireframe(Color.CornflowerBlue);
            List<Point3D> points = new List<Point3D>();

            if (entity is Line line)
            {
                DrawInteractiveUntilities.DrawCurveOrBlockRef(line, this);
            }
            else if (entity is LinearPath linearPath)
            {
                if (this.CurrentTool != null && this.CurrentTool.EntityUnderMouseDrawingType == UnderMouseDrawingType.BySegment)
                {
                    var tempLine = Utils.GetClosestSegment(linearPath, _mousePosition, DrawingPlane, this);
                    points.AddRange(tempLine.Vertices);
                }
                else
                {
                    points.AddRange(linearPath.Vertices);
                }
                var screenVertex = Utils.GetScreenVertices(points, this);
                renderContext.DrawLineStrip(screenVertex);

            }
            else if (entity is Wall2D wall)
            {
                var wallLine = new Line(wall.StartPoint,wall.EndPoint);
                DrawInteractiveUntilities.DrawCurveOrBlockRef(wallLine,this);
            }
            else
            {
                DrawInteractiveUntilities.DrawCurveOrBlockRef(entity, this);
                return;
            }

        }

        #endregion
        #region Private Method

        private void ProcessMouseDownForSelectionTool(MouseButtonEventArgs e, bool isSelected)
        {
            this._selectTool.ProcessMouseDownForSelection(e, isSelected, this);
        }

        private Point3D GetCurrentPoint(System.Drawing.Point mouseLocations)
        {
            this.ScreenToPlane(mouseLocations, this._drawingPlane, out var currentPoint);
            currentPoint = new Point3D((int)currentPoint.X,(int)currentPoint.Y,(int)currentPoint.Z);
            return currentPoint;
        }

        private void RaisePropertiesChangedForDynamicInput()
        {
            this.RaisePropertyChanged(nameof(CurrentLengthDimension));
            this.RaisePropertyChanged(nameof(CurrentAngleDimension));
        }
        #endregion
        #region Public Method
        public string ActiveLevel { get; set; }

        public void SetActivelevel(string activeLevel)
        {
            this.ActiveLevel = activeLevel;
            if (this.CurrentTool!=null)
            {
                this.CurrentTool.ActiveLevel = activeLevel;
            }
        }
        public void UpdateCurrentPointByLengthAndAngle(double length, double angle, double scaleFactor)
        {
            if (this.BasePoint3D == null & this.CurrentPoint == null)
            {
                return;
            }

            length = length * scaleFactor;
            if (this.CurrentTool != null && this.CurrentTool.ReferencePoint != null)
            {
                this.CurrentPoint = Utils.CalculatorPointWithLengthAndAngle(this.BasePoint3D, this.CurrentTool.ReferencePoint, length, angle);
            }
            else
            {
                this.CurrentPoint = Utils.CalculatorPointWithLengthAndAngle(this.BasePoint3D, null, length, angle);
            }

            this.Invalidate();
            //UpdateFocusDynamicInput();

        }


        public void UpdateCurrentPointByWidthAndHeight(double width, double height, SetDimensionType setDimensionType)
        {
            var xFactor = 1;
            var yFactor = 1;
            switch (setDimensionType)
            {
                case SetDimensionType.Width:
                    if (this.CurrentTool != null)
                        this.CurrentTool.CurrentWidth = width;
                    break;
                case SetDimensionType.Height:
                    if (this.CurrentTool != null)
                        this.CurrentTool.CurrentHeight = height;
                    break;
            }

            if (this.LastClickPoint.X > this.CurrentPoint.X)
            {
                xFactor = -1;
            }

            if (this.LastClickPoint.Y > this.CurrentPoint.Y)
            {
                yFactor = -1;
            }

            width *= xFactor;
            height *= yFactor;
            this.CurrentPoint = new Point3D(LastClickPoint.X + width, LastClickPoint.Y + height);
            this.Invalidate();
        }

        public Size DrawTextString(int x, int y, string text, Font textFont, Color textColor, ContentAlignment textAlign)
        {
            return this.DrawText(x, y, text, textFont, textColor, textAlign);
        }
        public void SetDynamicInput(DynamicInputView dynamicInput)
        {
            DynamicInput = dynamicInput;
            this.RefreshEntities();
        }
        //public void RegisterDrawInteractive(IDrawInteractive drawObject)
        //{
        //    if (drawObject != null && drawObject.DrawInteractiveHandler != null)
        //    {
        //        this.RegisterDrawInteractiveHandler(drawObject);
        //    }
        //}

        //public void UnRegisterDrawInteractive(IDrawInteractive drawObject)
        //{
        //    if (drawObject != null && drawObject.DrawInteractiveHandler != null)
        //    {
        //        this.UnRegisterDrawInteractiveHandler(drawObject);
        //    }
        //}
        public void ProcessCommand(string commandString)
        {
            CommandThunk tempCommandThunk = null;
            if (ApplicationHolder.CommandClass.CommandThunks.Count > 0)
            {
                foreach (var commandThunk in ApplicationHolder.CommandClass.CommandThunks.Where(commandThunk =>
                    commandThunk.GlobalName == commandString))
                {
                    tempCommandThunk = commandThunk;
                    break;
                }
            }

            if (tempCommandThunk?.Invoker == null) return;
            tempCommandThunk.Invoker.DynamicInvoke();
            this.Focus();
        }
        private void SetFreeTool()
        {
            this.IsProcessingTool = false;
        }
        private void SetBusyTool()
        {
            this.IsProcessingTool = true;
        }
        public List<Entity> GetEntities()
        {
            //return this._selectTool.SelectedEntities.ToList();

            return Dispatcher.Invoke((Func<List<Entity>>)(() => this.EntitiesManager.SelectedEntities.ToList()));
            //return this.EntitiesManager.SelectedEntities.ToList();
            //return this._selectTool.SelectedEntities.ToList();
        }

        public Entity GetSelectionEntity()
        {
            return Dispatcher.Invoke((Func<Entity>)(() => this.EntitiesManager.SelectedEntity?.Entity as Entity));
        }
        

        private void ClearSelectionEntity()
        {
            Dispatcher.Invoke((Action)(() =>
            {//this refer to form in WPF application 
                this.EntitiesManager.ClearSelectedEntities();
            }));
        }
        public PromptStatus GetSelection(out string stringResult, out Point3D clickedPoint, out Entity entity, bool isClearAtTheEndSelecion = true)
        {
            ClearSelectionEntity();
            _waitingForPickSelection = true;
            _waitingForSelection = true;
            while (_waitingForPickSelection)
            {// no do any thing until _isClicked is true
            }

            var promptStatus = PromptStatus.None;
            if (this.PromptStatus == PromptStatus.OK)
            {
                entity = GetSelectionEntity();
                if (entity is LinearPath linearPath)
                {
                    if (this.CurrentTool.EntityUnderMouseDrawingType == UnderMouseDrawingType.BySegment)
                    {
                        entity = Utils.GetClosestSegment(linearPath, _mousePosition, DrawingPlane, this);
                    }
                }

                clickedPoint = this.LastClickPoint;
                stringResult = "Get Point Complete";
                promptStatus = PromptStatus.OK;
            }
            else if (this.PromptStatus == PromptStatus.SwitchMode)
            {
                entity = GetSelectionEntity();
                if (entity != null && entity is LinearPath linearPath)
                {
                    if (this.CurrentTool.EntityUnderMouseDrawingType == UnderMouseDrawingType.BySegment)
                    {
                        entity = Utils.GetClosestSegment(linearPath, _mousePosition, DrawingPlane, this);
                    }
                }
                else entity = null;

                if (_isUserClicked)
                {
                    clickedPoint = this.LastClickPoint;
                }
                else
                {
                    clickedPoint = null;
                }
                stringResult = "You have changed SelectionMode";
                promptStatus = PromptStatus.SwitchMode;

            }
            else
            {
                entity = null;
                clickedPoint = null;
                stringResult = "You canceled Tool";
                promptStatus = PromptStatus.Cancel;
            }
            this.PromptStatus = PromptStatus.None;
            IsUserInteraction = false;
            _isUserClicked = false;
            _waitingForSelection = false;
            _waitingForPickSelection = false;
            if (isClearAtTheEndSelecion)
            {
                this.ClearSelectionEntity();
            }
            return promptStatus;

        }
        public PromptStatus GetEntities(out string stringResult, out Point3D clickedPoint, List<Entity> entities)
        {
            var seletedEntities = GetEntities();
            if (seletedEntities.Count <= 0)
            {
                _waitingForSelection = true;
            }
            else
            {
                _waitingForSelection = false;
                this.PromptStatus = PromptStatus.OK;
            }

            while (_waitingForSelection)
            {

            }
            var promptStatus = PromptStatus.None;
            seletedEntities = GetEntities();
            if (seletedEntities.Count != 0 && this.PromptStatus == PromptStatus.OK)
            {
                _waitingForSelection = false;
                IsUserInteraction = false;
                clickedPoint = LastClickPoint;
                stringResult = "Select Entities Complete";
                entities.Clear();
                entities.AddRange(seletedEntities);
                promptStatus = PromptStatus.OK;
            }
            else
            {
                var selectToolSelectedEntities = seletedEntities;
                if ((selectToolSelectedEntities.Count == 0 && this.PromptStatus == PromptStatus.OK))
                {
                    clickedPoint = CurrentPoint;
                    stringResult = "You have not yet select entities";
                    entities.Clear();
                    promptStatus = PromptStatus.None;
                }
                else
                {
                    clickedPoint = null;
                    stringResult = "You canceled Tool";
                    entities.Clear();
                    promptStatus = PromptStatus.Cancel;
                }
            }

            this.PromptStatus = PromptStatus.None;
            return promptStatus;

        }
        public PromptStatus GetPoint(Point3D basePoint, out string stringResult, out Point3D resultPoint)
        {
            if (basePoint != null)
            {
                this.BasePoint3D = basePoint;
            }
            else if (this.CurrentTool != null && this.CurrentTool.BasePoint != null)
            {
                this.BasePoint3D = this.CurrentTool.BasePoint;
            }
            else if (this.LastClickPoint != null)
            {
                this.BasePoint3D = this.LastClickPoint;
            }
            while (!_isUserInteraction)
            {// no do any thing until _isClicked is true
            }

            var promptStatus = PromptStatus.None;
            IsUserInteraction = false;
            _isUserClicked = false;
            if (this.PromptStatus == PromptStatus.OK)
            {
                //if (PolaTrackedPoint !=null)
                //{
                //    resultPoint = PolaTrackedPoint;
                //    PolaTrackedPoint = null;
                //}
                //else
                //{
                    resultPoint = CurrentPoint;    
                //}
                
                stringResult = "Get Point Complete";
                promptStatus = PromptStatus.OK;
            }
            else if (this.PromptStatus == PromptStatus.SwitchMode)
            {
                resultPoint = null;
                stringResult = "You Have Changed Tool Mode";
                promptStatus = PromptStatus.SwitchMode;
            }
            else
            {
                resultPoint = null;
                stringResult = "You canceled Tool";
                promptStatus = PromptStatus.Cancel;
            }
            this.PromptStatus = PromptStatus.None;
            return promptStatus;
        }
        public void AddAndRefresh(Entity entity)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {//this refer to form in WPF application 
                Entities.Add(entity, ActiveLayerName);
                
                Entities.Regen();
                Invalidate();
            }));
            //Entities.Add(entity, Color.Blue);
            //Entities.Regen();
            //Invalidate();
        }
        public void ResetProcessingTool(PromptStatus promptStatus = PromptStatus.None)
        {
            this.PromptStatus = promptStatus;
            IsUserInteraction = true;
            _isUserClicked = false;
            this._waitingForSelection = false;
            this._waitingForPickSelection = false;

            this.Dispatcher.Invoke((Action)(() =>
            {//this refer to form in WPF application 
                foreach (var selectToolSelectedEntity in this.EntitiesManager.SelectedEntities)
                {
                    selectToolSelectedEntity.Selected = false;
                }
                this.EntitiesManager.SelectedEntities.Clear();
                Invalidate();
            }));
        }
        public void RefreshEntities()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {//this refer to form in WPF application 
                Entities.Regen();
                Invalidate();
                //Refresh();
            }));
        }
        private void ProcessCancelTool()
        {
            this.ResetProcessingTool(PromptStatus.Cancel);

            if (this._selectTool != null && this._selectTool.StartPoint != null)
            {
                this._selectTool.ProcessEscapeTool();
                this.Invalidate();
            }

        }
        #endregion
        #region Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;
            RaisePropertyChanged(propertyName);

            return true;
        }
        protected virtual bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;
            onChanged?.Invoke();
            RaisePropertyChanged(propertyName);

            return true;
        }
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }
        #endregion

        #region Dependency Change Callback

        private static void EntitiesManagerChangedCallBack(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vp = (ICadDrawAble)d;
            var newValue = (IEntitiesManager)e.NewValue;
            if (newValue != null)
            {
                newValue.SetEntitiesList(vp.Entities);
                newValue.SetBlocks(vp.Blocks);
            }

            newValue.SetCanvasDrawing(vp);

            // do something
        }
        private static object EntitiesManagerCoerceCallback(DependencyObject d, object baseValue)
        {
            if (baseValue is IEntitiesManager entitiesManager)
            {
                return entitiesManager;
            }
            return new EntitiesManager() as IEntitiesManager;
        }
        private static void LayersManagerChangedCallBack(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vp = (ICadDrawAble)d;
            var newValue = (ILayerManager)e.NewValue;
            if (newValue != null)
                newValue.SetLayerList(vp.Layers);

            // do something
        }
        private static object LayersManagerCoerceCallback(DependencyObject d, object baseValue)
        {
            if (baseValue is ILayerManager layersManager)
            {
                return layersManager;
            }
            return new LayerManager() as LayerManager;
        }

        #endregion
    }
}