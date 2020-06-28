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
using ApplicationService;
using AppModels;
using AppModels.EventArg;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.Application;
using DrawingModule.CommandClass;
using DrawingModule.Control;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.EditingTools;
using DrawingModule.Helper;
using DrawingModule.Interface;
using DrawingModule.Views;

namespace DrawingModule.CustomControl.CanvasControl
{
    /// <summary>
    /// The my model.
    /// </summary>
    public partial class CanvasDrawing : Model, ICadDrawAble
    {
        
        #region Delegate
        private DrawInteractiveDelegate _drawInteractiveHandler;
        

        #endregion
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

        private bool _cursorOutSide;
        private bool _isUserInteraction;
        private bool _isSnappingEnable;
        private bool _isOrthoModeEnable;
        private readonly Plane _drawingPlane = Plane.XY;
        private SelectTool _selectTool;
        private bool _waitingForSelection;
        private Entity _entityUnderMouse;

        private Point3D _currentPoint;

        private Point3D _lastClickPoint;

        private Point3D _basePoint;
        //private 
        //private List<Entity> _selectedEntities; 

        #endregion

        #region Properties
        public IEntitiesManager EntitiesManager {
            get => (IEntitiesManager) GetValue(EntitiesManagerProperty);
            set => SetValue(EntitiesManagerProperty,value);
        }

        public ILayerManager LayersManager
        {
            get => (ILayerManager)GetValue(LayersManagerProperty);
            set => SetValue(LayersManagerProperty, value);
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
            get=>(string)GetValue(ActiveLayerNameProperty);
            set=>SetValue(ActiveLayerNameProperty,value);
        }
        public bool IsDrawEntityUnderMouse { get; set; }
        public bool IsDrawingMode { get; private set; }
        public Plane DrawingPlane => this._drawingPlane;
        public double CurrentLengthDimension
        {
            get
            {
                if (BasePoint3D==null|| CurrentPoint==null)
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
                var angle = -Vector2D.SignedAngleBetween(vector, Vector2D.AxisX) * (180 / Math.PI);
                angle = angle.Round();
                return angle;
            }
        }

        public void UpdateCurrentPointByLengthAndAngle(double length, double angle)
        {
            if (this.BasePoint3D ==null& this.CurrentPoint == null)
            {
                return;
            }

            this.CurrentPoint = Utils.CalculatorPointWithLengthAndAngle(this.BasePoint3D,length, angle);
            this.Invalidate();
            //UpdateFocusDynamicInput();
            
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
            set => this.SetProperty(ref _isOrthoModeEnable, value);
        }
        public bool IsProcessingTool { get; private set; }
        public PromptStatus PromptStatus { get; set; }
        public Point3D CurrentPoint
        {
            get=>_currentPoint;
            set
            {
                SetProperty(ref _currentPoint, value);
                this.RaisePropertiesChangedForDynamicInput();
            }
        }
        public Point3D LastClickPoint
        {
            get => _lastClickPoint;
            set
            {
                SetProperty(ref _lastClickPoint, value);
                this.RaisePropertiesChangedForDynamicInput();
            }
        }
        public Point3D BasePoint3D
        {
            get=>_basePoint;
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
            IsUserInteraction = false;
            _waitingForSelection = false;
            IsSnappingEnable = true;
            IsOrthoModeEnable = true;
            this._selectTool = new SelectTool();
            this.PromptStatus = PromptStatus.None;
            this.IsDrawingMode = false;
            this._entityUnderMouse = null;
            this.CurrentIndex = -1;
            Loaded += CanvasDrawing_Loaded;
        }

        private void CanvasDrawing_Loaded(object sender, RoutedEventArgs e)
        {
            _selectTool.SetEntitiesManager(EntitiesManager);
            _selectTool.SetLayersManager(LayersManager);
        }
        #endregion

        #region Override Method


        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    base.OnKeyDown(e);
        //    switch (e.Key)
        //    {
        //        case Key.Enter:
        //            if (this.IsProcessingTool & this._waitingForSelection)
        //            {
        //                this._waitingForSelection = false;
        //                this.PromptStatus = PromptStatus.OK;
        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //}
        protected override void DrawOverlay(DrawSceneParams data)
        {
            var drawInteractiveArgs = new DrawInteractiveArgs(this.CurrentPoint,this.LastClickPoint,data) ;
            if (IsProcessingTool && this._currentTool !=null)
            {
                //this._drawInteractiveHandler.Invoke(this, drawInteractiveArgs);
                DrawOverlay_Drawing(drawInteractiveArgs);
                if (this._waitingForSelection)
                {
                    this._selectTool.DrawInteractiveHandler.Invoke(this, drawInteractiveArgs);
                }
                else if (IsSnappingEnable)
                {
                    if (this._currentTool.IsSnapEnable)
                    {
                        if (SnapPoint != null)
                        {
                            DrawSnappPointUtl.DisplaySnappedVertex(this, SnapPoint, renderContext);
                        }
                        
                    }
                }
            }
            else
            {
                this._selectTool.DrawInteractiveHandler.Invoke(this,drawInteractiveArgs);   
            }
            base.DrawOverlay(data);
        }
        
        #endregion
        #region Private Method

        private void OnUserInteraction()
        {

        }
        private void ProcessMouseDownForSelectionTool(MouseButtonEventArgs e, bool isSelected)
        {
            this._selectTool.ProcessMouseDownForSelection(e,isSelected,this);
        }

        private Point3D GetCurrentPoint(System.Drawing.Point mouseLocations)
        {
            this.ScreenToPlane(mouseLocations, this._drawingPlane, out var currentPoint);
            return currentPoint;
        }
        
        //private void RegisterDrawInteractiveHandler(IDrawInteractive subscriber)
        //{
        //    this._drawInteractiveHandler += subscriber.DrawInteractiveHandler;
        //}
        //private void UnRegisterDrawInteractiveHandler(IDrawInteractive subscriber)
        //{
        //    _drawInteractiveHandler =null;
        //    this.Invalidate();
        //}


        //private void ProcessEnterKey()
        //{
        //    if (this.FuncCommand!= null)
        //    {
        //        IAsyncResult asysnResult = FuncCommand.BeginInvoke(new AsyncCallback(DoSomeThingCompleteAsyncComplete), null);
        //    }
        //}

        private void RaisePropertiesChangedForDynamicInput()
        {
            this.RaisePropertyChanged(nameof(CurrentLengthDimension));
            this.RaisePropertyChanged(nameof(CurrentAngleDimension));
            //this.RaisePropertyChanged(nameof(TextHeightVisibility));
            //this.RaisePropertyChanged(nameof(TextWidthVisibility));
            //this.RaisePropertyChanged(nameof(IsSelectedForEditingTool));
            //this.RaisePropertyChanged(nameof(CurrentLength));
            //this.RaisePropertyChanged(nameof(CurrentAngle));
            //this.RaisePropertyChanged(nameof(CurrentHeight));
            //this.RaisePropertyChanged(nameof(CurrentWidth));
            //this.RaisePropertyChanged(nameof(TestPropertiesValue));
            //this.RaisePropertyChanged(nameof(DrawingMode));
            //this.RaisePropertyChanged(nameof(LengthVisibility));
            //this.RaisePropertyChanged(nameof(AngleVisibility));
            //this.RaisePropertyChanged(nameof(HeightVisibility));
            //this.RaisePropertyChanged(nameof(WidthVisibility));
            //this.RaisePropertyChanged(nameof(VDimDimension));
            //this.RaisePropertyChanged(nameof(HDimDimension));
            //this.RaisePropertyChanged(nameof(VDimVisibility));
            //this.RaisePropertyChanged(nameof(HDimVisibility));
            //this.RaisePropertyChanged(nameof(TextInputContentVisibility));
            //this.RaisePropertyChanged(nameof(TextStringAngleVisibility));
            //this.RaisePropertyChanged(nameof(LeaderSegmentVisibility));
            //this.RaisePropertyChanged(nameof(ArrowSizeVisibility));
        }
        #endregion
        #region Public Method
        public void SetDynamicInput(DynamicInputView dynamicInput)
        {
            _dynamicInput = dynamicInput;
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
            return this.EntitiesManager.SelectedEntities.ToList();
        }
        public PromptStatus GetEntities(out string stringResult, out Point3D clickedPoint, List<Entity> entities)
        {
            if (this.EntitiesManager.SelectedEntities.Count <= 0)
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
            if (this.EntitiesManager.SelectedEntities != null && this.EntitiesManager.SelectedEntities.Count != 0 && this.PromptStatus == PromptStatus.OK)
            {
                _waitingForSelection = false;
                IsUserInteraction = false;
                clickedPoint = LastClickPoint;
                stringResult = "Select Entities Complete";
                entities.Clear();
                entities.AddRange(this.EntitiesManager.SelectedEntities);
                promptStatus = PromptStatus.OK;
            }
            else
            {
                var selectToolSelectedEntities = this.EntitiesManager.SelectedEntities;
                if (selectToolSelectedEntities != null && (selectToolSelectedEntities.Count == 0 && this.PromptStatus == PromptStatus.OK))
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
            if (basePoint!=null)
            {
                this.BasePoint3D = basePoint;
            }
            else if (this.CurrentTool!=null && this.CurrentTool.BasePoint!=null)
            {
                this.BasePoint3D = this.CurrentTool.BasePoint;
            }
            else if (this.LastClickPoint!=null)
            {
                this.BasePoint3D = this.LastClickPoint;
            }
            while (!_isUserInteraction)
            {// no do any thing until _isClicked is true
            }

            var promptStatus = PromptStatus.None;
            IsUserInteraction = false;
            if (this.PromptStatus == PromptStatus.OK)
            {
                resultPoint = CurrentPoint;
                stringResult = "Get Point Complete";
                promptStatus = PromptStatus.OK;
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
            this._waitingForSelection = false;
           

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
                Refresh();
            }));
        }
        private void ProcessCancelTool()
        {
            this.ResetProcessingTool(PromptStatus.Cancel);
            
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
                newValue.SetEntitiesList(vp.Entities);
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