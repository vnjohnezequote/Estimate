﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewJobWizardViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the NewJobWizardViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media.Animation;
using ApplicationInterfaceCore;
using ApplicationService;
using AppModels;
using AppModels.AppData;
using AppModels.CustomEntity;
using AppModels.FileSerializer;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using devDept.Diagnostic;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Serialization;
using DrawingModule.Views;
using DynamicData;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using CanvasDrawing = DrawingModule.CustomControl.CanvasControl.CanvasDrawing;

namespace DrawingModule.ViewModels
{
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
        private string _selectedLevel;
        private ObservableCollection<LevelWall> _levels;
        private Timer _autoSaveTimer = new Timer(1 * 60 * 1000);
        private Entity _selectedEntity;
        private bool _isSaving;
        private bool _isPropertyBarOpened = false;
        private int _lastPropertyWidth;
        private string _selectedLayout;
        #endregion
        #region public Property
        public IEntitiesManager EntitiesManager { get; }

        //public Entity SelectedEntity
        //{
        //    get => _selectedEntity;
        //    set => SetProperty(ref _selectedEntity, value);
        //}
        public ObservableCollection<LevelWall> Levels
        {
            get=>_levels;
            set
            {
                SetProperty(ref _levels, value);
                if (string.IsNullOrEmpty(_selectedLayout))
                {
                    return;
                }
                this.RaisePropertyChanged(SelectedLevel);
            }
        }

        public ObservableCollection<string> Layouts { get; set; } = new ObservableCollection<string>();

        public string SelectedLayout
        {
            get => _selectedLayout;
            set
            {
                SetProperty(ref _selectedLayout, value);
                ChangedLayout(_selectedLayout);
            }
        }

        public List<int> WallThicknessList { get; set; } = new List<int>() { 70, 90, 140,180,190, 200, 230 };
        public List<int> WallSpacingList { get; set; } = new List<int>() { 300,450,600,900,1200};
        public string SelectedLevel
        {
            get
            {
                if (!string.IsNullOrEmpty(_selectedLevel)) return _selectedLevel;
                if (this.Levels!=null && this.Levels.Count>0)
                {
                    return this.Levels[0].LevelName;
                }

                return _selectedLevel;
            }
            set
            {
                if(string.IsNullOrEmpty(value))
                    return;
                SetProperty(ref _selectedLevel, value);
                EventAggregator?.GetEvent<LevelNameService>().Publish(_selectedLevel);
            } 
        }

        public bool IsOrthorMode
        {
            get => this._isOrthorMode;
            set => this.SetProperty(ref this._isOrthorMode, value);
        }

        public int LastPropertyPanelWidth
        {
            get => _lastPropertyWidth == 0 ? 250 : this._lastPropertyWidth;
            private set => this.SetProperty(ref this._lastPropertyWidth, value);
        }

        #endregion
        #region Command
        
        /// <summary>
        /// Gets the control loaded command.
        /// </summary>
        public ICommand ExportDWGCommand { get; private set; }
        public ICommand ImportDWGCommand { get; private set; }
        public ICommand ImportPDFCommand { get; private set; }
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
        public ICommand SaveCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand ExpanderPropertyPanelCommamd { get; private set; }
        public ICommand PropertyPanelGridSplitterDragDeltaCommand { get; private set; }
        public ICommand PropertyPanelGridSplitterDragCompleteCommand { get; private set; }
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
        public DrawingWindowViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager,IEntitiesManager entitiesManager,IJob jobModel)
            : base(unityContainer, regionManager, eventAggregator, layerManager,jobModel)
        {
            
            if (jobModel!=null)
            {
                this.Levels = jobModel.Levels;
            }
            EntitiesManager = entitiesManager;
            this.WindowLoadedCommand = new DelegateCommand<DrawingWindowView>(this.WindowLoaded);
            ExportDWGCommand = new DelegateCommand(this.OnExportDWG);
            ImportDWGCommand = new DelegateCommand(this.OnImportDWG);
            
            SaveCommand = new DelegateCommand(this.SaveJob);
            OpenCommand = new DelegateCommand(this.OnOpenDrawing);
            EventAggregator.GetEvent<OpenJobEvent>().Subscribe(OnOpenDrawingEventSubcribe);
            EventAggregator.GetEvent<AutoSaveDrawingEvent>().Subscribe(OnAutoSaveDrawing);
            EventAggregator.GetEvent<SaveDrawingEvent>().Subscribe(OnSaveDrawing);
            ExpanderPropertyPanelCommamd = new DelegateCommand(OnOpenPropertiesPanelCommand);
            PropertyPanelGridSplitterDragDeltaCommand = new DelegateCommand(this.OnPropertyPanelSizeChanged);
             this.PropertyPanelGridSplitterDragCompleteCommand = new DelegateCommand(this.OnPropertyPanelGridSplitterDragComplete);

            //ImportPDFCommand = new DelegateCommand(OnImportPDF);
            //_autoSaveTimer.Elapsed += AutoSaveTimer_Elapsed;
            //_autoSaveTimer.Start();

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
            // LastPropertyPanelWidth = 0;

        }
        

        //private void OnSaveButtorPressed()
        //{
        //    //EventAggregator
        //}
        private void _drawingModel_WorkCompleted(object sender, WorkCompletedEventArgs e)
        {
            if (_isSaving)
            {
                _isSaving = false;
            }
        }

        private void OnPropertyPanelGridSplitterDragComplete()
        {
            if ((int)this._window.PropertyPanel.ActualWidth <= 0
                || (int)this._window.PropertyPanel.ActualWidth >= 150) return;
            this._isPropertyBarOpened = false;
            this._window.ButtonOpenPropertyPanel.Visibility = Visibility.Visible;
            this._window.ButtonClosePropertyPanel.Visibility = Visibility.Collapsed;
            this._window.PropertyPanel.Width = new GridLength(0);
            this.LastPropertyPanelWidth = 150;
        }

        private void ChangedLayout(string layout)
        {
            EventAggregator?.GetEvent<LayoutNameService>().Publish(layout);
        }

        public void OnOpenPropertiesPanelCommand()
        {

            if (this._isPropertyBarOpened == false)
            {
                this._isPropertyBarOpened = true;
                 var openProperty = this._window.FindResource("OpenPropertyPanel") as Storyboard;
                this._window.ButtonOpenPropertyPanel.Visibility = Visibility.Collapsed;
                this._window.ButtonClosePropertyPanel.Visibility = Visibility.Visible;
                // LastPropertyPanelWidth = 150;
                 openProperty?.Begin();
            }
            else
            {
                this._isPropertyBarOpened = false;
                 var closeProperty = this._window.FindResource("ClosePropertyPanel") as Storyboard;
                this._window.ButtonOpenPropertyPanel.Visibility = Visibility.Visible;
                this._window.ButtonClosePropertyPanel.Visibility = Visibility.Collapsed;
                 closeProperty?.Begin();
                // LastPropertyPanelWidth = 0;

            }


        }
        private void OnPropertyPanelSizeChanged()
        {
            if ((int)this._window.PropertyPanel.ActualWidth > 0)
            {
                this._isPropertyBarOpened = true;
                this._window.ButtonOpenPropertyPanel.Visibility = Visibility.Collapsed;
                this._window.ButtonClosePropertyPanel.Visibility = Visibility.Visible;
                this.LastPropertyPanelWidth = (int)this._window.PropertyPanel.ActualWidth;
            }
            else
            {
                this._isPropertyBarOpened = false;
                this._window.ButtonOpenPropertyPanel.Visibility = Visibility.Visible;
                this._window.ButtonClosePropertyPanel.Visibility = Visibility.Collapsed;
                this.LastPropertyPanelWidth = 150;
            }
        }


        public void SaveJob()
        {
            if (this.EventAggregator!=null)
            {
                EventAggregator.GetEvent<DrawingSaveJobEvent>().Publish();
            }
        }
        private void OnAutoSaveDrawing()
        {
            System.Windows.Application.Current.Dispatcher.Invoke((Action)(OnAutoSaveDrawingExcute));
        }

        private void AutoSaveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
            {//this refer to form in WPF application 
                //OnSaveDrawing();
            }));
            
        }

        
        private void OnAutoSaveDrawingExcute()
        {
            if (!_isSaving)
            {
                _isSaving = true;
                if (this.JobModel != null)
                {
                    _isSaving = true;
                    var fileName = JobModel.Info.JobLocation + "\\" + JobModel.Info.JobNumber + "_back" + ".eye";
                    WriteFile writeFile = new WriteFile(
                        new WriteFileParams(_drawingModel)
                        , fileName,
                        new EzequoteFileSerializer()
                    );
                    _drawingModel.StartWork(writeFile);
                }

            }
           
        }

        private void OnOpenDrawingEventSubcribe(string drawingPath)
        {
            if (_drawingModel!=null)
            {
                _drawingModel.Clear();
                ReadFile readFile = new ReadFile(drawingPath, new EzequoteFileSerializer());
                _drawingModel.StartWork(readFile);
            }
        }

        #endregion





        #region Private Function

        /// <summary>
        /// The on loaded.
        /// </summary>
        /// <param name="rootGrid">
        /// The root grid.
        /// </param>
        private void OnExportDWG()
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter =
                "CAD drawings (*.dwg)|*.dwg|Drawing Exchange Format (*.dxf)|*.dxf|3D PDF (*.pdf)|*.pdf";

            saveFileDialog.Title = "Export";
            saveFileDialog.AddExtension = true;
            saveFileDialog.CheckPathExists = true;

            var result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                //EnableControls(false);

                WriteFileAsync wfa = null;
                GereralDatatoExport(_drawingModel,_canvasExport);
                switch (saveFileDialog.FilterIndex)
                {
                    case 1:
                    case 2:
                        wfa = new WriteAutodesk(new WriteAutodeskParams(_canvasExport), saveFileDialog.FileName);
                        break;
                    case 3:
                        wfa = new Write3DPDF(
                            new Write3DPdfParams(_canvasExport, new Size(595, 842), new Rect(10, 10, 575, 822)),
                            saveFileDialog.FileName);
                        break;
                }
                

                _canvasExport.StartWork(wfa);
            }
        }

        private void GereralDatatoExport(CanvasDrawing drawing,CanvasDrawing canvasExport)
        {
            
            foreach (var drawingLineType in drawing.LineTypes)
            {
                if (!canvasExport.LineTypes.Contains(drawingLineType))
                {
                    canvasExport.LineTypes.Add(drawingLineType);
                }
                
            }

            foreach (var drawingLayer in drawing.Layers)
            {
                if (!canvasExport.Layers.Contains(drawingLayer))
                {
                    canvasExport.Layers.Add(drawingLayer);
                }
                
            }

            foreach (var drawingBlock in drawing.Blocks)
            {
                if (!canvasExport.Blocks.Contains(drawingBlock))
                {
                    canvasExport.Blocks.Add(drawingBlock);
                }
            }
            
            var entities = new List<Entity>(drawing.Entities) ;
            foreach (var modelEntity in entities)
            {
                if (modelEntity is IRectangleSolid beam)
                {
                    //var outerList = new List<Point2D>()
                    //    {beam.OuterStartPoint, beam.OuterEndPoint, beam.InnerEndPoint, beam.InnerStartPoint,beam.OuterStartPoint};
                    //var beamQuad = Mesh.CreatePlanar(Plane.XY, outerList, Mesh.natureType.Plain);
                    var beamPolyLine = new LinearPath(beam.StartCenterLinePoint, beam.EndCenterLinePoint);
                    beamPolyLine.Color = beam.Color;
                    beamPolyLine.ColorMethod = colorMethodType.byEntity;
                    beamPolyLine.GlobalWidth = beam.Thickness;
                    foreach (var point3D in beamPolyLine.Vertices)
                    {
                        point3D.Z = 0.0;
                    }
                    canvasExport.Entities.Add(beamPolyLine);
                    continue;
                }

                else if (modelEntity is Hanger2D hanger)
                {
                    var hangerText = new Text(Plane.XY, hanger.InsertionPoint, hanger.TextString, hanger.Height,
                        hanger.Alignment);
                    hangerText.Color = hanger.Color;
                    hangerText.ColorMethod = colorMethodType.byEntity;
                    var hangerCirCle = new Circle(Plane.XY, hanger.InsertionPoint, hangerText.Height);
                    hangerCirCle.Color = hanger.Color;
                    hangerCirCle.ColorMethod = colorMethodType.byEntity;
                    canvasExport.Entities.Add(hangerText);
                    canvasExport.Entities.Add(hangerCirCle);
                    continue;
                }

                else if (modelEntity is JoistArrowEntity joistArrow)
                {
                    var points = new List<Point3D>()
                        {joistArrow.StartArrow, joistArrow.StartPoint, joistArrow.EndPoint, joistArrow.EndArrow};
                    var joistArrowPath = new LinearPath(points);
                    joistArrowPath.Color = joistArrow.Color;
                    joistArrowPath.ColorMethod = colorMethodType.byEntity;
                    canvasExport.Entities.Add(joistArrowPath);
                    continue;
                }
                else
                {
                    canvasExport.Entities.Add(modelEntity);
                }
               
            }
            canvasExport.Entities.Regen();
        }

        private void OnImPortPDF()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF drawings (*.PDF)|*.PDF";
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                
                //ReadFile readFile = new ReadFile(openFileDialog.FileName, new EzequoteFileSerializer());
                //_drawingModel.StartWork(readFile);
            }
        }

        private void OnSaveDrawing()
        {
            /*
            
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter =
                "CAD drawings (*.eye)|*.eye";

            saveFileDialog.Title = "Saving File";
            saveFileDialog.AddExtension = true;
            saveFileDialog.CheckPathExists = true;
            var result = saveFileDialog.ShowDialog();

            if (result == true)
            {

                WriteFile writeFile = new WriteFile(
                    new WriteFileParams(_drawingModel)
                    , saveFileDialog.FileName,
                    new EzequoteFileSerializer()
                );
                _drawingModel.StartWork(writeFile);
            }*/
            if (!_isSaving)
            {
                _isSaving = true;
            
                if (this.JobModel != null )
                {
                    _isSaving = true;
                    var fileName = JobModel.Info.JobLocation + "\\" + JobModel.Info.JobNumber + ".eye";
                    WriteFile writeFile = new WriteFile(
                        new WriteFileParams(_drawingModel)
                        , fileName,
                        new EzequoteFileSerializer()
                    );

                    _drawingModel.StartWork(writeFile);
                    
                }
            }
        }

        private void OnOpenDrawing()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CAD drawings (*.eye)|*.eye";
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                _drawingModel.Clear();
                ReadFile readFile = new ReadFile(openFileDialog.FileName,new EzequoteFileSerializer());
                _drawingModel.StartWork(readFile);
            }

        }
        private void OnImportDWG()
        {
            //OpenFileDialog<ImportFileAddOn> importFileDialog = new OpenFileDialog<ImportFileAddOn>();
            var openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                _drawingModel.Clear();
                ReadAutodesk ra = new ReadAutodesk(openFileDialog.FileName);
                _drawingModel.StartWork(ra);
                _drawingModel.SetView(viewType.Top, true, true);

            }

            //importFileDialog.Filter =
            //    "CAD drawings (*.dwg)|*.dwg|Drawing Exchange Format (*.dxf)|*.dxf|All compatible file types (*.*)|*.dwg;*.dxf";
            //importFileDialog.Title = "Import";
            //importFileDialog.Multiselect = false;
            //importFileDialog.AddExtension = true;
            //importFileDialog.CheckFileExists = true;
            //importFileDialog.CheckPathExists = true;
            //importFileDialog.FileDlgStartLocation = AddonWindowLocation.Right;
            //importFileDialog.FileDlgDefaultViewMode = NativeMethods.FolderViewMode.Tiles;

            //var result = importFileDialog.ShowDialog();

            //if (result == true)
            //{
            //    _drawingModel.Clear();
            //    _yAxisUp = importFileDialog.ChildWnd.YAxisUp;

            //    ReadAutodesk ra = new ReadAutodesk(importFileDialog.FileName);
            //    _drawingModel.StartWork(ra);
            //}
        }

        private void AddNewLayer(LayerItem newLayer)
        {
            //if (this.Layers == null)
            //{
            //    this.Layers = new ObservableCollection<LayerItem>();
            //}

            //if (Layers.Contains(newLayer))
            //{
            //    return;
            //}
            //else
            //{
            //    Layers.Add(newLayer);
            //    if (this._drawingModel == null )
            //    {
            //        return;
            //    }
            //    var newCanvasLayer = new Layer(newLayer.Name,newLayer.Color,newLayer.LineTypeName,newLayer.LineWeight,newLayer.Visible,newLayer.Locked);
            //    this._drawingModel.Layers.Add(newCanvasLayer);
            //}
        }

        private void RemoveLayer(LayerItem removeLayer)
        {
            //if (this.Layers == null)
            //{
            //    return;

            //}

            //if (this.Layers.Contains(removeLayer))
            //{
            //    this.Layers.Remove(removeLayer);
            //    foreach (var drawingModelLayer in _drawingModel.Layers)
            //    {
            //        if (drawingModelLayer.Name == removeLayer.Name)
            //        {
            //            _drawingModel.Layers.Remove(drawingModelLayer);
            //            return;
            //        }
            //    }
            //}

            
        }
        //private void InitsLayers()
        //{
            
        //    if (this._drawingModel == null )
        //    {
        //        return;
        //    }
        //    this.LayerManager.SetLayer(_drawingModel.Layers);
        //}

        //private void InitEntitiesManager()
        //{
        //    if (this._drawingModel == null)
        //    {
        //        return;
        //    }

        //    this._entitiesManager.SetEntitiesList(this._drawingModel.Entities);

        //}
        private CanvasDrawing _canvasExport;
        private void WindowLoaded(DrawingWindowView window)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));
            this._window = window ?? throw new ArgumentNullException(nameof(window));
            this._drawingModel = window.CanvasDrawing.CanvasDrawing;
            this._drawingModel.WorkCompleted += _drawingModel_WorkCompleted;
            _canvasExport = window.CanvasDrawing.CanvasExportDrawing;
            //InitsLayers();
            //InitEntitiesManager();
            this.SetRegionManager();
            if (this._drawingModel != null && this.LayerManager!= null)
            {
                //this.LayerManager.PropertyChanged += LayerManager_PropertyChanged;
            }
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

        //private void LayerManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    var layerManager = sender as ILayerManager;
        //    if (e.PropertyName == "SelectedLayer")
        //    {
        //        this._drawingModel.ActiveLayerName = layerManager.SelectedLayer.Name;
        //    }
        //}

        private void SetRegionManager()
        {
            this.RegionManager = this.RegionManager.CreateRegionManager();
            var drawingWindowRegionManager = this.RegionManager.CreateRegionManager();
            Prism.Regions.RegionManager.SetRegionManager(this._window, drawingWindowRegionManager);
            this.RegionManager = drawingWindowRegionManager;
            this.LoadLayerManger();
            this.LoadEntityPropertiesManager();
        }

        private void LoadEntityPropertiesManager()
        {
            this.RegionManager.RequestNavigate("RightContentRegiom", nameof(SelectedEntityPropertiesView));
        }

        private void LoadLayerManger()
        {
            //var parameters = new NavigationParameters { { "Layers", Layers } };
            //if (this.JobDefaultInfo.Info != null)
            //{
                this.RegionManager.RequestNavigate("LayerManagerRegion", nameof(LayerManagerView));
            //}

        }

        //public bool ProcessMessages(ref MSG msg, ref bool handled)
        //{
        //    return false;
        //}


    #endregion

    }
}
