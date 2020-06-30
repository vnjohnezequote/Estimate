using System;
using System.Collections.Generic;
using System.Windows.Input;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Graphics;

namespace DrawingModule.Views
{
    using CustomControls.Controls;
    public partial class DrawingWindowView : FlatWindow
    {
        #region Field

        #endregion

        #region Properties

        

        

        #endregion
        public DrawingWindowView()
        {
            this.InitializeComponent();
            this.Loaded += DrawingWindowView_Loaded;
            this.CanvasDrawing.CanvasDrawing.WorkCompleted += CanvasDrawing_WorkCompleted;
            this.CanvasDrawing.TabControlDrawing.SelectionChanged += TabControl1_SelectionChanged;
        }

        private void TabControl1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Drawings drawings = CanvasDrawing.PaperSpace;

            if (this.CanvasDrawing.TabPaperSpace.IsSelected && drawings.Sheets.Count > 0)
            {
                //drawingsUserControl1.EnableUIElements(false);

                // rebuilds all the "dirty" views for all sheets.
                drawings.Rebuild(this.CanvasDrawing.CanvasDrawing, true, true);
            }
        }

        private void CanvasDrawing_WorkCompleted(object sender, WorkCompletedEventArgs e)
        {
            //EnableImportExportButtons(true);
            var model1 = this.CanvasDrawing.CanvasDrawing;

            if (!(e.WorkUnit is ReadFileAsyncWithDrawings)) return;
            var rfa = (ReadFileAsyncWithDrawings)e.WorkUnit;
            rfa.AddToScene(model1);
            model1.SetView(viewType.Trimetric, true, false);
            var drawings = this.CanvasDrawing.PaperSpace;

            model1.Units = rfa.Units;
            rfa.AddToDrawings(drawings);

            // If there are no sheets adds a default one to have a ready-to-use paper space.
            //if (drawings.Sheets.Count == 0)
            //    this.CanvasDrawing.AddDefaultSheet();
        }

        private void DrawingWindowView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //this.CanvasDrawing.CanvasDrawing.Renderer = rendererType.Direct3D;
            //this.CanvasDrawing.CanvasDrawing.Turbo.MaxComplexity = 100;
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
            this.CanvasDrawing.PaperSpace.Rebuild(this.CanvasDrawing.CanvasDrawing, true, true);
        }

        protected override void OnContentRendered(System.EventArgs e)
        {
            
            ReadFile rf = new ReadFile("Crankcase.eye");
            rf.DoWork();
            // sets the units of the model.
                //this.CanvasDrawing.CanvasDrawing.Units = rf.Units;
                this.CanvasDrawing.CanvasDrawing.Entities.Add(rf.Entities[0]);
                if (this.CanvasDrawing.PaperSpace.Sheets.Count == 0)
                {
                    CanvasDrawing.AddSheet("Sheet1", linearUnitsType.Millimeters, formatType.A4_LANDSCAPE_ISO);
                    //CanvasDrawing.TestPaperSpace.LineTypes.ReplaceItem(new LinePattern(CanvasDrawing.HiddenSegmentsLineTypeName, new float[] { 0.8f, -0.4f }));
            }
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
                var handler = this.CanvasDrawing.CanvasDrawing.PreProcessKeyDownForCanvasView(e);
                    //Dispatcher.Invoke((Func<bool>)(() => this.CanvasDrawing.CanvasDrawing.PreProcessKeyDownForCanvasView(e)));

                if (!handler)
                {
                    handler =
                        //Dispatcher.Invoke((Func<bool>)(() => this.CanvasDrawing.DynamicInput.PreProcessKeyboardInput(e))); 
                   this.CanvasDrawing.DynamicInput.PreProcessKeyboardInput(e);
                    if (!handler)
                    {
                        //Dispatcher.Invoke((Action)(() => this.CanvasDrawing.FocusDynamicCommandLine()));
                        this.CanvasDrawing.FocusDynamicCommandLine();
                    }

                }
            }
            e.Handled = false;
            base.OnPreviewKeyDown(e);
        }
        
    }
}
