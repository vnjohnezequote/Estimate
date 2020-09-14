﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using AppModels.CustomEntity;
using AppModels.ResponsiveData;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Graphics;
using DrawingModule.CustomControl.PaperSpaceControl;
using DrawingModule.ViewModels;
using DynamicData;
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
            Drawings drawings = CanvasDrawing.PaperSpace;
            foreach (var canvasDrawingLineType in CanvasDrawing.CanvasDrawing.LineTypes)
            {
                if (!drawings.LineTypes.Contains(canvasDrawingLineType))
                {
                    drawings.LineTypes.Add(canvasDrawingLineType);
                }
            }
            foreach (var canvasDrawingLayer in CanvasDrawing.CanvasDrawing.Layers)
            {
                if (!drawings.Layers.Contains(canvasDrawingLayer))
                {
                    drawings.Layers.Add(canvasDrawingLayer);
                }
            }

            if (this.CanvasDrawing.TabPaperSpace.IsSelected && drawings.Sheets.Count > 0)
            {
                //drawingsUserControl1.EnableUIElements(false);

                // rebuilds all the "dirty" views for all sheets.
                drawings.Rebuild(this.CanvasDrawing.CanvasDrawing, true, true);
                //var sheet = CanvasDrawing.PaperSpace.GetActiveSheet() as CustomSheet;
                //if (sheet != null)
                //{
                //   UpDateLayout();
                //}
                drawings.Entities.Regen();
                drawings.Invalidate();
            }
        }


        private void CanvasDrawing_WorkCompleted(object sender, WorkCompletedEventArgs e)
        {
            //EnableImportExportButtons(true);
            var model1 = this.CanvasDrawing.CanvasDrawing;

            if (!(e.WorkUnit is ReadFileAsyncWithDrawings)) return;
           
            var rfa = (ReadFileAsyncWithDrawings)e.WorkUnit;
           
            rfa.AddToScene(model1);
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
        }

        private void DrawingWindowView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.CanvasDrawing.CanvasDrawing.Renderer = rendererType.Direct3D;
            this.CanvasDrawing.CanvasDrawing.Turbo.MaxComplexity = 100;
            this.CanvasDrawing.CanvasDrawing.HideSmall = true;
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
            CanvasDrawing.CanvasDrawing.Renderer = rendererType.Native;
            this.CanvasDrawing.PaperSpace.Rebuild(this.CanvasDrawing.CanvasDrawing, true, true);
        }

        protected override void OnContentRendered(System.EventArgs e)
        {

            //ReadFile rf = new ReadFile("Crankcase.eye");
            //rf.DoWork();
            // sets the units of the model.
            //this.CanvasDrawing.CanvasDrawing.Units = rf.Units;
            //this.CanvasDrawing.CanvasDrawing.Entities.Add(rf.Entities[0]);
            if (this.CanvasDrawing.PaperSpace.Sheets.Count != 0) return;
            foreach (var levelWall in _drawingWindowViewModel.Levels)
            {

                CanvasDrawing.AddSheet(levelWall.LevelName, linearUnitsType.Millimeters, formatType.A4_LANDSCAPE_ISO, _drawingWindowViewModel.JobModel);
            }

           // BuildTestBeamBlock();

            //CanvasDrawing.TestPaperSpace.LineTypes.ReplaceItem(new LinePattern(CanvasDrawing.HiddenSegmentsLineTypeName, new float[] { 0.8f, -0.4f }));
        }

        private void BuildTestBeamBlock()
        {
            Block beam = new Block("B1");
            Line beamLine = new Line(new Point3D(0,0,0),new Point3D(1000,0,0));
            beamLine.ColorMethod = colorMethodType.byEntity;
            beamLine.Color = Color.Blue;
            beam.Entities.Add(beamLine);
            Leader beamLeader = new Leader(Plane.XY,new Point3D(500,0,0),new Point3D(500,2000,0));
            beamLeader.ArrowheadSize = 500;
            beam.Entities.Add(beamLeader);
            var beamText = new Attribute(new Point3D(250,2050,0),"Name","B1",500);
            beam.Entities.Add(beamText);
            CanvasDrawing.CanvasDrawing.Blocks.Add(beam);
            BlockReference reference = new BlockReference(new Point3D(0,0,0),"B1",0);
            reference.Attributes.Add("Name","B1");
            CanvasDrawing.CanvasDrawing.Entities.Add(reference);


        }

        protected override void OnSourceInitialized(System.EventArgs e)
        {
            base.OnSourceInitialized(e);
            //IntPtr handle = new WindowInteropHelper(this).Handle;
            //int num = Win32.GetWindowLong(handle, -16).ToInt32();
            //num = ((num | int.MinValue) & -524289);
            //Win32.SetWindowLong(handle, -16, new IntPtr(num));
            //HwndSource hwndSource = HwndSource.FromHwnd(handle);
            //hwndSource.AddHook(new HwndSourceHook(DrawingWindowView.WndProc));
        }

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
            Sheet activeSheet = CanvasDrawing.PaperSpace.GetActiveSheet();

            //devDept.Eyeshot.Entities.View view = null;

            //view = new VectorView(100, 30, viewType.Top, 0.01, CanvasDrawing.GetViewName((CustomSheet)activeSheet, viewType.Front, false));
            foreach (var activeSheetEntity in activeSheet.Entities)
            {
                if (activeSheetEntity is VectorView vectorViews)
                {
                    vectorViews.Rebuild(CanvasDrawing.CanvasDrawing, activeSheet, CanvasDrawing.PaperSpace, true);
                }
            }

            //if (CanvasDrawing.PaperSpace.Blocks.Contains(view.BlockName))
            //    CanvasDrawing.PaperSpace.Blocks.Remove(view.BlockName); // it removes also related block references.

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
