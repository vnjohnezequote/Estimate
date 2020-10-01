using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using AppModels.CustomEntity;
using AppModels.Enums;
using AppModels.ResponsiveData;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Graphics;
using DrawingModule.CustomControl.CanvasControl;
using DrawingModule.CustomControl.PaperSpaceControl;
using DrawingModule.ViewModels;
using DynamicData;
using ReactiveUI;
using Attribute = devDept.Eyeshot.Entities.Attribute;
using Size = System.Drawing.Size;

namespace DrawingModule.Views
{
    using CustomControls.Controls;
    public partial class DrawingWindowView : FlatWindow
    {
        #region Field

        private DrawingWindowViewModel _drawingWindowViewModel;
        #endregion

        #region Properties
        #endregion
        public DrawingWindowView()
        {
            this.InitializeComponent();
            if (this.DataContext != null)
            {
                _drawingWindowViewModel = this.DataContext as DrawingWindowViewModel;
                _drawingWindowViewModel.PropertyChanged += _drawingWindowViewModel_PropertyChanged;
            }
            this.Loaded += DrawingWindowView_Loaded;
            this.CanvasDrawing.CanvasDrawing.WorkCompleted += CanvasDrawing_WorkCompleted;
            this.CanvasDrawing.TabControlDrawing.SelectionChanged += TabControl1_SelectionChanged;
            
        }

        private void _drawingWindowViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsOrthorMode")
            {
                this.CanvasDrawing.CanvasDrawing.IsOrthoModeEnable = _drawingWindowViewModel.IsOrthorMode;
            }
        }

        private void TabControl1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!CanvasDrawing.CanvasDrawingControl.PaperSpace.IsImported && CanvasDrawing.CanvasDrawingControl.PaperSpace.IsModified)
            {
                ReloadDrawings();
            }
            else if(CanvasDrawing.CanvasDrawingControl.TabControlDrawing.SelectedIndex == 1 && CanvasDrawing.CanvasDrawingControl.PaperSpace.Sheets.Count>0 && CanvasDrawing.CanvasDrawingControl.PaperSpace.IsToReload)
            {
                //CanvasDrawing.CanvasDrawingControl.PaperSpace.IsToReload = false;
                CanvasDrawing.CanvasDrawingControl.StartViewBuilder();
            }
        }

        private void ReloadDrawings()
        {
            if (CanvasDrawing.TabControlDrawing.SelectedIndex == 1 && CanvasDrawing.PaperSpace.IsModified)
            {
                InitializeDrawing();
                CanvasDrawing.PaperSpace.IsModified = false;
            }
            
            if (CanvasDrawing.CanvasDrawingControl.PaperSpace.Sheets.Count>0)
            {
                CanvasDrawing.CanvasDrawingControl.StartViewBuilder();
            }

            CanvasDrawing.PaperSpace.IsToReload = true;
        }

        private void InitializeDrawing()
        {
            CanvasDrawing.CanvasDrawingControl.CLear();
            foreach (var levelWall in _drawingWindowViewModel.Levels)
            {
                CanvasDrawing.AddSheet(levelWall.LevelName, linearUnitsType.Millimeters, formatType.A4_LANDSCAPE_ISO, _drawingWindowViewModel.JobModel);
            }

        }

        private void CanvasDrawing_WorkCompleted(object sender, WorkCompletedEventArgs e)
        {
            //EnableImportExportButtons(true);
            var model1 = this.CanvasDrawing.CanvasDrawing;

            if (!(e.WorkUnit is ReadFileAsyncWithDrawings)) return;
           
            var rfa = (ReadFileAsyncWithDrawings)e.WorkUnit;
           
            rfa.AddToScene(model1);
            //if (_drawingWindowViewModel != null && _drawingWindowViewModel.EntitiesManager != null)
            //{
            //    _drawingWindowViewModel.EntitiesManager.SetEntitiesList(model1.Entities);
            //}

            var listPicEntitiesToRemove = new List<Entity>();
            var listPicEntitiesToAdd = new List<Entity>();
            foreach (var rfaEntity in model1.Entities)
            {
                if (rfaEntity is Picture picEntity)
                {
                    var myPic = new PictureEntity(picEntity);
                    listPicEntitiesToAdd.Add(myPic);
                    listPicEntitiesToRemove.Add(rfaEntity);
                }
            }

            foreach (var model1Entity in listPicEntitiesToRemove)
            {
                model1.Entities.Remove(model1Entity);
            }
            model1.Entities.AddRange(listPicEntitiesToAdd);

            model1.Entities.Regen();

            var drawings = this.CanvasDrawing.PaperSpace;

            model1.Units = rfa.Units;
            
            rfa.AddToDrawings(drawings);

            //// If there are no sheets adds a default one to have a ready-to-use paper space.
            if (drawings.Sheets.Count == 0)
                this.CanvasDrawing.AddDefaultSheet();
            model1.LayersManager.SetLayerList(model1.Layers);
            if (_drawingWindowViewModel != null && _drawingWindowViewModel.EntitiesManager != null)
            {
                //_drawingWindowViewModel.EntitiesManager.Blocks = model1.Blocks;
                _drawingWindowViewModel.EntitiesManager.Entities = model1.Entities;
                var lisEntitiesRemove = new List<BeamEntity>();
                var listBlock = new List<Block>();
                foreach (var model1Entity in model1.Entities)
                {
                    if (model1Entity is BeamEntity beam)
                    {
                        foreach (var entitiesManagerBlock in _drawingWindowViewModel.EntitiesManager.Blocks)
                        {
                            if (entitiesManagerBlock.Name == beam.BlockName)
                            {
                                listBlock.Add(entitiesManagerBlock);
                            }
                        }
                        lisEntitiesRemove.Add(beam);
                    }
                }

                foreach (var block in listBlock)
                {
                    _drawingWindowViewModel.EntitiesManager.Blocks.Remove(block);
                }

                foreach (var entity in lisEntitiesRemove)
                {
                    _drawingWindowViewModel.EntitiesManager.Entities.Remove(entity);
                }

                foreach (var entity in lisEntitiesRemove)
                {
                    _drawingWindowViewModel.EntitiesManager.Blocks.Add(entity.BeamBlock);
                    _drawingWindowViewModel.EntitiesManager.Entities.Add(entity);
                }
                ReloadBeamReferenceForBeamLayout();
                //_drawingWindowViewModel.EntitiesManager.Blocks.Clear();
                //_drawingWindowViewModel.EntitiesManager.Blocks = model1.Blocks;
                //model1.Blocks.Clear();
                //foreach (var model1Entity in model1.Entities)
                //{
                //    if (model1Entity is BeamEntity beam)
                //    {
                //        model1.Blocks.Add(beam.BeamBlock);
                //    }
                //}
                //model1.Entities.Regen();
                //model1.Invalidate();

            }
        }
        private void ReloadBeamReferenceForBeamLayout()
        {
            var entitiesManager = _drawingWindowViewModel.EntitiesManager;
            var jobModel = _drawingWindowViewModel.JobModel;
            if (entitiesManager != null && entitiesManager.Entities != null &&
                entitiesManager.Entities.Count != 0 && jobModel != null && jobModel.Levels != null &&
                jobModel.Levels.Count != 0)
            {
                foreach (var entitiesManagerEntity in entitiesManager.Entities)
                {
                    LevelWall level = null;
                    if (entitiesManagerEntity is BeamEntity beam)
                    {
                        foreach (var jobModelLevel in jobModel.Levels)
                        {
                            if (beam.LevelName == jobModelLevel.LevelName)
                            {
                                level = jobModelLevel;
                            }
                        }

                        if (level != null)
                        {
                            foreach (var roofBeam in level.RoofBeams)
                            {
                                if (roofBeam.Id == beam.BeamReferenceId)
                                {
                                    beam.BeamReference = roofBeam;
                                }
                            }
                        }

                    }
                }
            }

        }

        private void DrawingWindowView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.CanvasDrawing.CanvasDrawing.Renderer = rendererType.Native;
            this.CanvasDrawing.CanvasDrawing.Turbo.MaxComplexity = 100;
            this.CanvasDrawing.CanvasDrawing.Wireframe.SilhouettesDrawingMode = silhouettesDrawingType.Never;
            this.CanvasDrawing.CanvasDrawing.Shaded.SilhouettesDrawingMode = silhouettesDrawingType.Never;
            this.CanvasDrawing.CanvasDrawing.Rendered.SilhouettesDrawingMode = silhouettesDrawingType.Never;
            this.CanvasDrawing.CanvasDrawing.Flat.SilhouettesDrawingMode = silhouettesDrawingType.Never;
            this.CanvasDrawing.CanvasDrawing.HiddenLines.SilhouettesDrawingMode = silhouettesDrawingType.Never;
            this.CanvasDrawing.CanvasDrawing.Rendered.PlanarReflections = false;
            this.CanvasDrawing.CanvasDrawing.Rendered.ShadowMode = shadowType.None;
            this.CanvasDrawing.CanvasDrawing.Rendered.RealisticShadowQuality = realisticShadowQualityType.Low;
            this.CanvasDrawing.CanvasDrawing.Shaded.ShowEdges = false;
            this.CanvasDrawing.CanvasDrawing.Shaded.ShowInternalWires = false;
            this.CanvasDrawing.CanvasDrawing.Rendered.ShowEdges = false;
            CanvasDrawing.CanvasDrawing.Camera.ProjectionMode = projectionType.Orthographic;
            CanvasDrawing.CanvasDrawing.CurrentBlock.Units = linearUnitsType.Millimeters;

            //this.CanvasDrawing.PaperSpace.Rebuild(this.CanvasDrawing.CanvasDrawing, true, true);
        }

        protected override void OnContentRendered(System.EventArgs e)
        {
        //    //if (this.CanvasDrawing.PaperSpace.Sheets.Count != 0) return;
        //    //foreach (var levelWall in _drawingWindowViewModel.Levels)
        //    //{

        //    //    CanvasDrawing.AddSheet(levelWall.WallLevelName, linearUnitsType.Millimeters, formatType.A4_LANDSCAPE_ISO, _drawingWindowViewModel.JobModel);
        //    //}
        //    base.OnContentRendered(e);

            //BuildTestBeamBlock();
            base.OnContentRendered(e);

        //    //CanvasDrawing.TestPaperSpace.LineTypes.ReplaceItem(new LinePattern(CanvasDrawing.HiddenSegmentsLineTypeName, new float[] { 0.8f, -0.4f }));
        }

        private void BuildTestBeamBlock()
        {
            var line = new Line(new Point3D(200, 3000, 0), new Point3D(3200, 3000, 0));
            CanvasDrawing.CanvasDrawing.Entities.Add(line);
            var beamEntity = new BeamEntity(new Point3D(0, 0, 0), "B1", new Point3D(200, 3000, 0), new Point3D(3200,3000, 0), "Prenail", "GroundFloor", 0, BeamMarkedLocation.bottom);
            CanvasDrawing.CanvasDrawing.Blocks.Add(beamEntity.BeamBlock);
            beamEntity.Attributes.Add("Name", "195x65 GL17C @ 1/3.3");
            beamEntity.ColorMethod = colorMethodType.byEntity;
            beamEntity.LineTypeMethod = colorMethodType.byEntity;
            beamEntity.LineTypeScale = 0.5f;
            beamEntity.LineWeightMethod = colorMethodType.byEntity;
            CanvasDrawing.CanvasDrawing.Entities.Add(beamEntity);
        }

        //protected override void OnSourceInitialized(System.EventArgs e)
        //{
        //    base.OnSourceInitialized(e);
        //    //IntPtr handle = new WindowInteropHelper(this).Handle;
        //    //int num = Win32.GetWindowLong(handle, -16).ToInt32();
        //    //num = ((num | int.MinValue) & -524289);
        //    //Win32.SetWindowLong(handle, -16, new IntPtr(num));
        //    //HwndSource hwndSource = HwndSource.FromHwnd(handle);
        //    //hwndSource.AddHook(new HwndSourceHook(DrawingWindowView.WndProc));
        //}

        //private static IntPtr WndProc(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
        //{
        //    ContainerMessageDispatcher.RaiseWin32Message(hWnd, message, wParam, lParam, ref handled);
        //    return IntPtr.Zero;
        //}
        

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (CanvasDrawing.TabDrawing.IsSelected)
            {
                if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    if (_drawingWindowViewModel != null)
                        _drawingWindowViewModel.SaveJob();
                    return;
                }
                if (e.Key == Key.F8)
                {
                    this.CanvasDrawing.CanvasDrawing.IsOrthoModeEnable =
                        !this.CanvasDrawing.CanvasDrawing.IsOrthoModeEnable;
                    if (this._drawingWindowViewModel != null)
                    {
                        this._drawingWindowViewModel.IsOrthorMode = this.CanvasDrawing.CanvasDrawing.IsOrthoModeEnable;
                    }
                    return;
                }
                var handler = this.CanvasDrawing.CanvasDrawing.PreProcessKeyDownForCanvasView(e);
                //Dispatcher.Invoke((Func<bool>)(() => this.CanvasDrawing.CanvasDrawing.PreProcessKeyDownForCanvasView(e)));

                if (!handler)
                {
                    if (CanvasDrawing.CanvasDrawing.IsMouseOver)
                    {
                        handler = //Dispatcher.Invoke((Func<bool>)(() => this.CanvasDrawing.DynamicInput.PreProcessKeyboardInput(e))); 
                            this.CanvasDrawing.DynamicInput.PreProcessKeyboardInput(e);
                    }

                    if (!handler&& CanvasDrawing.CanvasDrawing.IsMouseOver)
                    {
                        //Dispatcher.Invoke((Action)(() => this.CanvasDrawing.FocusDynamicCommandLine()));
                        this.CanvasDrawing.FocusDynamicCommandLine();
                    }

                }
            }
            //e.Handled = false;
            base.OnPreviewKeyDown(e);
        }

        private void UpdateLayoutClick(object sender, RoutedEventArgs e)
        {
            UpDateLayout();
        }

        private void UpDateLayout()
        {
            //Sheet activeSheet = CanvasDrawing.PaperSpace.ActiveSheet;

            //devDept.Eyeshot.Entities.View view = null;
            //view = new VectorView(100, 30, viewType.Top, 0.01, CanvasDrawing.GetViewName((CustomSheet)activeSheet, viewType.Front, false));

            //foreach (var activeSheetEntity in activeSheet.Entities)
            //{
            //    if (activeSheetEntity is VectorView vectorViews)
            //    {
            //        vectorViews.Rebuild(CanvasDrawing.CanvasDrawing, activeSheet, CanvasDrawing.PaperSpace, true);
            //    }
            //}
            CanvasDrawing.CanvasDrawingControl.StartViewBuilder(null,false);

            //if (CanvasDrawing.PaperSpace.Blocks.Contains(view.BlockName))
            //   CanvasDrawing.PaperSpace.Blocks.Remove(view.BlockName); // it removes also related block references.

            //view.Rebuild(CanvasDrawing.CanvasDrawing, activeSheet, CanvasDrawing.PaperSpace, true);

        }

        private void PrintLayoutClick(object sender, RoutedEventArgs e)
        {
            if (CanvasDrawing.PaperSpace.PageSetup(true,true,0)==false) return;
            CanvasDrawing.PaperSpace.Print();
            //var hdSetting = new HiddenLinesViewSettings(CanvasDrawing.PaperSpace);
            //hdSetting.KeepEntityColor = true;
            //hdSetting.KeepEntityLineWeight = true;
            //var hlv = new HiddenLinesViewOnPaper(hdSetting);
            //CanvasDrawing.PaperSpace.StartWork(hlv);
            //CanvasDrawing.PaperSpace.PrintPreview(new System.Drawing.Size(800, 600));
            //var hdSetting = new HiddenLinesViewSettings(CanvasDrawing.PaperSpace);
            //if (CanvasDrawing.CanvasDrawing.PageSetup(true, true, 0) == false) return;
            
            //if (CanvasDrawing.CanvasDrawing.PageSetup(true,true,0)==false) return;
            //CanvasDrawing.CanvasDrawing.Camera.ProjectionMode = projectionType.Orthographic;
            //var hdSetting = new HiddenLinesViewSettings(CanvasDrawing.CanvasDrawing);
            //hdSetting.KeepEntityColor = true;
            //hdSetting.KeepEntityLineWeight = true;
            //hdSetting.TreatWhiteAsBlack = true;
            //var hlv = new HiddenLinesViewOnPaper(hdSetting);
            //CanvasDrawing.CanvasDrawing.StartWork(hlv);

            //CanvasDrawing.CanvasDrawing.DoWork(hlv);
            
            //if (CanvasDrawing.CanvasDrawing.PageSetup(true,true,0)==false) return;
            //CanvasDrawing.CanvasDrawing.Print();
            //CanvasDrawing.CanvasDrawing.


        }
    }
}
