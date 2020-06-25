﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewJobWizardViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the NewJobWizardViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using DrawingModule.Views;
using CanvasDrawing = DrawingModule.CustomControl.CanvasControl.CanvasDrawing;

namespace DrawingModule.ViewModels
{
    using System.Windows.Controls;
    using System.Windows.Input;

    using ApplicationCore.BaseModule;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Regions;
    using Unity;

    /// <summary>
    /// The new job wizard view model.
    /// </summary>
    
    public class DrawingWindowViewModel : ApplicationWindowBaseViewModel
    {
        #region private field

        /// <summary>
        /// The drawin model.
        /// </summary>
        private CanvasDrawing _drawingModel;

        private DrawingWindowView _window;
        private bool _isOrthorMode;
        private bool _isDimByObject;

        #endregion
        #region public Property
        //public ObservableCollection<ListViewModelItem> Layers { get; set; }
        public bool IsOrthorMode
        {
            get => this._isOrthorMode;
            set => this.SetProperty(ref this._isOrthorMode, value);
        }

        public bool IsDimByObject
        {
            get => this._isDimByObject;
            set => this.SetProperty(ref this._isDimByObject, value);
        }

        #endregion
        #region Command

        /// <summary>
        /// Gets the control loaded command.
        /// </summary>
        public ICommand ControlLoadedCommand { get; private set; }
        public ICommand WindowLoadedCommand { get; private set; }
        public ICommand DrawLineCommand { get; private set; }
        public ICommand DrawRectangleCommand { get; private set; }
        public ICommand DrawRayCommand { get; private set; }
        public ICommand DrawXlineCommand { get; private set; }
        public ICommand MoveToolCommand { get; private set; }
        public ICommand DrawLinePathCommand { get; private set; }
        public ICommand DrawLinearDimCommand { get; private set; }
        public ICommand DrawAlignDimCommand { get; private set; }
        public ICommand DrawAngularDimCommand { get; private set; }
        public ICommand DrawTextCommand { get; private set; }
        public ICommand DrawLeaderCommand { get; private set; }
        public ICommand CopyCommand { get; private set; }
        public ICommand OffsetCommand { get; private set; }
        public ICommand RotateCommand { get; private set; }
        public ICommand MirrorCommand { get; private set; }
        public ICommand ScaleCommand { get; private set; }
        public ICommand TrimCommand { get; private set; }
        public ICommand ExtendCommand { get; private set; }
        public ICommand FilletCommand { get; private set; }
        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingWindowViewModel"/> class.
        /// </summary>
        public DrawingWindowViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingWindowViewModel"/> class. 
        /// </summary>
        /// <param name="unityContainer">
        /// The unity container.
        /// </param>
        /// <param name="regionManager">
        /// The region manager.
        /// </param>
        /// <param name="eventAggregator">
        /// The event Aggregator.
        /// </param>
        public DrawingWindowViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(unityContainer, regionManager, eventAggregator)
        {
            this.ControlLoadedCommand = new DelegateCommand<Grid>(this.OnLoaded);
            this.WindowLoadedCommand = new DelegateCommand<DrawingWindowView>(this.WindowLoaded);
            //this.DrawLineCommand = new DelegateCommand(this.OnDrawLine);
            //this.DrawRectangleCommand = new DelegateCommand(this.OnDrawRectangle);
            //this.DrawRayCommand = new DelegateCommand(this.OnDrawRay);
            //this.DrawXlineCommand = new DelegateCommand(this.OnDrawXLine);
            //this.DrawLinePathCommand = new DelegateCommand(this.OnDrawLinePath);
            //this.MoveToolCommand = new DelegateCommand(this.OnMoveObject);
            //this.DrawLinearDimCommand = new DelegateCommand(this.OnDrawLinearDim);
            //this.DrawAlignDimCommand = new DelegateCommand(this.OnDrawALignDim);
            //this.DrawAngularDimCommand = new DelegateCommand(this.OnDrawAngularDim);
            //this.DrawTextCommand = new DelegateCommand(this.OnDrawText);
            //this.DrawLeaderCommand = new DelegateCommand(this.OnDrawLeader);
            //this.CopyCommand = new DelegateCommand(this.OnCoppy);
            //this.OffsetCommand = new DelegateCommand(this.OnOffset);
            //this.RotateCommand = new DelegateCommand(this.OnRotate);
            //this.MirrorCommand = new DelegateCommand(this.OnMirror);
            //this.ScaleCommand = new DelegateCommand(this.OnScale);
            //this.TrimCommand = new DelegateCommand(this.OnTrimCommand);
            //this.ExtendCommand = new DelegateCommand(this.OnExtendCommand);
            //this.FilletCommand = new DelegateCommand(this.OnFilletCommand);
            this.RegionManager = RegionManager.CreateRegionManager();
            this.IsOrthorMode = true;
            this.IsDimByObject = true;
           
        }

        #endregion

       

       

        #region Private Function

        /// <summary>
        /// The on loaded.
        /// </summary>
        /// <param name="rootGrid">
        /// The root grid.
        /// </param>
        private void OnLoaded(Grid rootGrid)
        {
            /*var parrentWindow = WindowHelper.GetWindowParent(rootGrid);
            if (parrentWindow == null) return;
            if (parrentWindow is Window window)
            {
                var rawInputManager = new WPFRawInputManager(window,RawInputCaptureMode.Foreground);
                rawInputManager.KeyPress += RawInputManager_KeyPress;
                rawInputManager.MousePress += RawInputManager_MousePress;
            }*/

            //this.RegionManager.RequestNavigate("CommandLineRegion",nameof(CommandLineView));
        }
        private void WindowLoaded(DrawingWindowView window)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));
            this._window = window ?? throw new ArgumentNullException(nameof(window));
            this._drawingModel = window.CanvasDrawing.CanvasDrawing;
            this.SetRegionManager();
            //this._drawingModel = window.FindName("CanvasDrawing") as CanvasDrawing;
            //var drawingModel = this._drawingModel;
            //if (drawingModel != null)
            //{
            //    drawingModel.SetView(viewType.Top);
            //    drawingModel.Camera.ProjectionMode = projectionType.Orthographic;
            //    //drawingModel.SetDynamicInput(this._window.CursorEntry, this._window.LengthDimension,
            //    //    this._window.AngleDimension, this._window.WidthDimension, this._window.HeightDimension,
            //    //    this._window.HDim, this._window.VDim);
            //    //drawingModel.ActiveViewport.SmallSizeRatio = 0;
            //    drawingModel.Invalidate();
            //    drawingModel.Focus();
            //}

            //this._window.PreviewKeyDown += OnPreviewKeyDown;
            //this._window.PreviewKeyUp += OnPreviewKeyUp;
            //Application.DrawingModel = this._window.CanvasDrawing;
            //this.PrepareLayers();
            //this._drawingModel.MouseMove += _drawingModel_MouseMove;
        }

        private void SetRegionManager()
        {
            this.RegionManager = this.RegionManager.CreateRegionManager();
            var drawingWindowRegionManager = this.RegionManager.CreateRegionManager();
            Prism.Regions.RegionManager.SetRegionManager(this._window, drawingWindowRegionManager);
            this.RegionManager = drawingWindowRegionManager;
            this.LoadLayerManger();

        }

        private void LoadLayerManger()
        {
            var parameters = new NavigationParameters { { "Layers", _drawingModel.Layers } };
            //if (this.Job.Info != null)
            //{
                this.RegionManager.RequestNavigate("LayerManagerRegion", nameof(LayerManagerView),parameters);
            //}

        }

        //public bool ProcessMessages(ref MSG msg, ref bool handled)
        //{
        //    return false;
        //}


    #endregion

    }
}