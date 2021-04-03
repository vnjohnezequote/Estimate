using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AppModels;
using AppModels.CustomEntity;
using AppModels.EventArg;
using AppModels.Factories;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CustomControl.PaperSpaceControl;
using DrawingModule.Helper;
using DrawingModule.ViewModels;
using MathExtension;
using Block = devDept.Eyeshot.Block;

namespace DrawingModule.Views
{
    
    public enum formatType
    {
        A0_ISO, A1_ISO, A2_ISO, A3_ISO, A4_ISO, A4_LANDSCAPE_ISO, A_ANSI, A_LANDSCAPE_ANSI, B_ANSI, C_ANSI, D_ANSI, E_ANSI
    }
    /// <summary>
    /// Interaction logic for CanvasDrawingView.xaml
    /// </summary>
    public partial class CanvasDrawingView : UserControl
    {
        //private MyViewBuilder _viewBuilder;
        //private WallNameBuilder _wallNameBuilder;
        private CanvasDrawingViewModel _viewModel;
        public static readonly DependencyProperty ActiveLayerNameProperty =
            DependencyProperty.Register("ActiveLayerName", typeof(string), typeof(CanvasDrawingView),
                new PropertyMetadata("Default"));

        public string ActiveLayerName
        {
            get => (string)GetValue(ActiveLayerNameProperty);
            set => SetValue(ActiveLayerNameProperty, value);
        }
        #region Constructor
        public CanvasDrawingView()
        {
            InitializeComponent();
            try
            {
                this.CanvasDrawing.Unlock("PF21-12NG0-L7K3F-M0CS-FE82");
                this.PaperSpace.Unlock("PF21-12NG0-L7K3F-M0CS-FE82");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }

            //this.PaperSpace.Unlock("PF20 - 21MV5 - 0NHNJ - WYUX - F6CQ");
            if (this.DataContext != null)
            {
                _viewModel = this.DataContext as CanvasDrawingViewModel;
                if (_viewModel != null && _viewModel.JobModel!=null && _viewModel.JobModel.Info!=null)
                {
                    _viewModel.JobModel.Info.PropertyChanged += Info_PropertyChanged;
                }
            }
            this.PaperSpace.WorkCompleted += PaperSpace_WorkCompleted;
            this.TabControlDrawing.SelectionChanged += TabControlDrawing_SelectionChanged;
            Loaded += CanvasDrawingView_Loaded;
            //this.CanvasDrawing.Wireframe
        }

        private void Info_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            foreach (var paperSpaceSheet in this.PaperSpace.Sheets)
            {
                var br = GetFormatBlockReference(paperSpaceSheet);
                if (br != null)
                {
                    switch (e.PropertyName)
                    {
                        case nameof(_viewModel.JobModel.Info.WindRate):
                            br.Attributes["WindRate"].Value = _viewModel.JobModel.Info.WindRate;
                            break;
                        case nameof(_viewModel.JobModel.Info.RoofMaterial):
                            br.Attributes["RoofMaterial"].Value = _viewModel.JobModel.Info.RoofMaterial;
                            break;
                        case "SetTieDown":
                            br.Attributes["TieDown"].Value = _viewModel.JobModel.Info.TieDown;
                            break;
                        case nameof(_viewModel.JobModel.Info.JobAddress):
                        case nameof(_viewModel.JobModel.Info.SubAddress):
                            br.Attributes["Address"].Value = _viewModel.JobModel.Info.JobAddress;
                            br.Attributes["City"].Value = _viewModel.JobModel.Info.SubAddress;
                            break;
                        case nameof(_viewModel.JobModel.Info.JobNumber):
                            br.Attributes["JobNo"].Value = _viewModel.JobModel.Info.JobNumber;
                            break;
                        case nameof(_viewModel.JobModel.Info.Customer):
                            if (_viewModel.JobModel.Info.Client.Name == "StickFrame" || _viewModel.JobModel.Info.QuoteFloorFrame)
                            {
                                if (string.IsNullOrEmpty(_viewModel.JobModel.Info.Customer))
                                    br.Attributes["Client"].Value = "";
                                else
                                br.Attributes["Client"].Value = _viewModel.JobModel.Info.Customer;
                            }
                            break;
                    }
                }
            }
            //var activeSheet = this.PaperSpace.GetActiveSheet();
            PaperSpace.Entities.UpdateBoundingBox();
            PaperSpace.Invalidate();
        }

        private void CanvasDrawingView_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.TabDrawing.IsSelected)
            {
                if (DynamicInput.DataContext != null && DynamicInput.DataContext is DynamicInputViewModel dynamicInputViewModel)
                {
                    dynamicInputViewModel.SetCurrentCanvas(CanvasDrawing);
                }
            }
        }

        private void TabControlDrawing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.TabDrawing.IsSelected)
            {
                if (DynamicInput.DataContext!= null && DynamicInput.DataContext is DynamicInputViewModel dynamicInputViewModel)
                {
                    dynamicInputViewModel.SetCurrentCanvas(CanvasDrawing);
                }
            }
            else
            {
                if (DynamicInput.DataContext != null && DynamicInput.DataContext is DynamicInputViewModel dynamicInputViewModel)
                {
                    dynamicInputViewModel.SetCurrentCanvas(null);
                }
            }
            
        }

        public BlockReference GetFormatBlockReference(Sheet sheet)
        {
            if (!_formatBlockNames.ContainsKey(sheet.Name)) return null;

            foreach (var entity in sheet.Entities)
            {
                if (entity is BlockReference && IsFormatBlockReference((BlockReference)entity))
                {
                    BlockReference br = (BlockReference)entity;
                    if (br.BlockName.Equals(_formatBlockNames[sheet.Name]))
                        return br;
                }
            }

            return null;
        }
        private bool IsFormatBlockReference(BlockReference blockReference)
        {
            return blockReference is View == false ;
                //&& blockReference.Attributes.ContainsKey("Format")
        }

        public WallName GetFloorNameRef(Sheet sheet)
        {
            foreach (var sheetEntity in sheet.Entities)
            {
                if (sheetEntity is WallName wallName)
                    return wallName;
            }
            return null;
        }
        public FloorQuantity GetFloorQuantityRef(Sheet sheet)
        {
            foreach (var sheetEntity in sheet.Entities)
            {
                if (sheetEntity is FloorQuantity floorQuantity)
                    return floorQuantity;
            }
            return null;
        }
        public VectorView GetVecterViewRef(Sheet sheet)
        {
            foreach (var entity in sheet.Entities)
            {
                if (entity is VectorView vectorView)
                {
                    return vectorView;
                }
            }

            return null;
        }
        private void PaperSpace_WorkCompleted(object sender, WorkCompletedEventArgs e)
        {
            //if (_wallNameBuilder == null)
            //{
            //    _wallNameBuilder = new WallNameBuilder(PaperSpace, _viewModel.JobModel);
            //    PaperSpace.StartWork(_wallNameBuilder);
            //}
            //else
            //{
            //    //if (_wallNameBuilder != null)
            //    //{
            //    //    _wallNameBuilder.AddToDrawings();
            //    //}
            //    //if (_viewBuilder != null)
            //    //{
            //    //    _viewBuilder.AddToDrawings(PaperSpace);

            //    //    if (PaperSpace.ActiveSheet == null)
            //    //    {
            //    //        var sheet = PaperSpace.Sheets[0];
            //    //        PaperSpace.ActiveSheet = sheet;
            //    //        var blockRefSheet = GetFormatBlockReference(PaperSpace.ActiveSheet);
            //    //        var vectorView = GetVecterViewRef(PaperSpace.ActiveSheet);
            //    //        if (blockRefSheet!=null && vectorView!=null)
            //    //        {
            //    //            if (blockRefSheet.Attributes.ContainsKey("Scale"))
            //    //            {
            //    //                var f = (Rational)vectorView.Scale;
            //    //                var scale =  f.ToString().Replace('/', ':');
            //    //                blockRefSheet.Attributes["Scale"].Value = scale;
            //    //            }
            //    //        }
            //    //        PaperSpace.ZoomFit();
            //    //    }
            //    //    PaperSpace.Entities.Regen();
            //    //    PaperSpace.Invalidate();

            //    //    if (!_viewBuilder.QueueCompleted)
            //    //    {
            //    //        PaperSpace.StartWork(_viewBuilder);
            //    //    }
            //    //    else
            //    //    {
            //    //        _viewBuilder.Dispose();
            //    //        _viewBuilder = null;
            //    //        _wallNameBuilder.Dispose();
            //    //        _wallNameBuilder = null;
            //    //    }

            //    //    if (PaperSpace.IsToReload && !PaperSpace.IsImported && PaperSpace.IsModified)
            //    //    {
            //    //        PaperSpace.IsToReload = false;
            //    //    }
            //    //}
            //}


        }


        #endregion

        #region Public Method

        public void FocusDynamicCommandLine()
        {
            this.DynamicInput.FocusCommandTextbox();
        }
        public void AddDefaultSheet()
        {
            //AddSheet("Sheet1", linearUnitsType.Millimeters, formatType.A4_ISO);
        }

        public void AddSheet(FramingSheet framingSheet, linearUnitsType units, formatType formatType, IJob job,
            bool addDefaultView = true)
        {
            Tuple<double, double> size = DrawingHelper.GetFormatSize(units, formatType);
            CustomSheet sheet = null;
            if (this._viewModel != null && this._viewModel.JobModel != null)
            {
                sheet = new CustomSheet(units, size.Item1, size.Item2, framingSheet, _viewModel.JobModel);
            }
            else
            {
                sheet = new CustomSheet(units, size.Item1, size.Item2, framingSheet);
            }
            //Sheet sheet = new Sheet(units,size.Item1,size.Item2,name);

            Block block;
            BlockReference br = CreateFormatBlock(formatType, sheet, out block, job);

            this.PaperSpace.Blocks.Add(block);

            sheet.Entities.Add(br);  // not possible adding the entity to Drawings because the control handle is not created yet. it will be added when this sheet will be set as the active one.
            this.PaperSpace.Sheets.Add(sheet);

            // adds a set of default views.
            AddDefaultViews(sheet);
        }

        public void AddSheet(string name, linearUnitsType units, formatType formatType, IJob job, bool addDefaultView = true)
        {
            Tuple<double, double> size = DrawingHelper.GetFormatSize(units, formatType);
            CustomSheet sheet = null;
            if (this._viewModel != null && this._viewModel.JobModel != null)
            {
                sheet = new CustomSheet(units, size.Item1, size.Item2, name, _viewModel.JobModel);
            }
            else
            {
                sheet = new CustomSheet(units, size.Item1, size.Item2, name);
            }
            //Sheet sheet = new Sheet(units,size.Item1,size.Item2,name);
            
            Block block;
            BlockReference br = CreateFormatBlock(formatType, sheet, out block,job);

            this.PaperSpace.Blocks.Add(block);

            sheet.Entities.Add(br);  // not possible adding the entity to Drawings because the control handle is not created yet. it will be added when this sheet will be set as the active one.
            this.PaperSpace.Sheets.Add(sheet);

            // adds a set of default views.
            AddDefaultViews(sheet);
        }
        public static string GetFormattedScaleAttributeValue(string value)
        {
            return String.Format("SCALE: {0}", value);
        }

        public static BlockReference BuildFormatBlock(formatType formatType, CustomSheet sheet, out Block block)
        {
            BlockReference br = null;
            block = null;

            br = sheet.BuildPaper(formatType, out block);
            //br = sheet.BuildA4ISO(out block);

            return br;
        }

        public static Tuple<double, double> GetFormatSize(linearUnitsType units, formatType formatType)
        {
            // values on this method are millimeters so it uses this factor to get converted values according to the current units.
            var conversionFactor = Utility.GetLinearUnitsConversionFactor(linearUnitsType.Millimeters, units);

            switch (formatType)
            {
                case formatType.A0_ISO:
                    return new Tuple<double, double>(1189 * conversionFactor, 841 * conversionFactor);
                case formatType.A1_ISO:
                    return new Tuple<double, double>(841 * conversionFactor, 594 * conversionFactor);
                case formatType.A2_ISO:
                    return new Tuple<double, double>(594 * conversionFactor, 420 * conversionFactor);
                case formatType.A3_ISO:
                    return new Tuple<double, double>(420 * conversionFactor, 297 * conversionFactor);
                case formatType.A4_ISO:
                    return new Tuple<double, double>(210 * conversionFactor, 297 * conversionFactor);
                case formatType.A4_LANDSCAPE_ISO:
                    return new Tuple<double, double>(297 * conversionFactor, 210 * conversionFactor);
                case formatType.A_ANSI:
                    return new Tuple<double, double>(215.9 * conversionFactor, 279.4 * conversionFactor);
                case formatType.A_LANDSCAPE_ANSI:
                    return new Tuple<double, double>(279.4 * conversionFactor, 215.9 * conversionFactor);
                case formatType.B_ANSI:
                    return new Tuple<double, double>(431.8 * conversionFactor, 279.4 * conversionFactor);
                case formatType.C_ANSI:
                    return new Tuple<double, double>(558.8 * conversionFactor, 431.8 * conversionFactor);
                case formatType.D_ANSI:
                    return new Tuple<double, double>(863.6 * conversionFactor, 558.8 * conversionFactor);
                case formatType.E_ANSI:
                    return new Tuple<double, double>(1117.6 * conversionFactor, 863.6 * conversionFactor);
                default:
                    return new Tuple<double, double>(210 * conversionFactor, 297 * conversionFactor);
            }
        }

        private readonly Dictionary<string, string> _formatBlockNames = new Dictionary<string, string>();
        //private TreeNode _rootTreeNode;
        private BlockReference CreateFormatBlock(formatType formatType, CustomSheet sheet, out Block block,IJob job)
        {
            if (_formatBlockNames.ContainsKey(sheet.Name))
            {
                PaperSpace.Blocks.Remove(_formatBlockNames[sheet.Name]); // removes both block and related block reference.
                _formatBlockNames.Remove(sheet.Name);
            }

            BlockReference br = BuildFormatBlock(formatType,sheet,out block);
            _formatBlockNames.Add(sheet.Name,br.BlockName);

            br.Attributes["WindRate"] = new AttributeReference(job.Info.WindRate);
            br.Attributes["RoofMaterial"] = new AttributeReference(job.Info.RoofMaterial);
            br.Attributes["TieDown"] = new AttributeReference(job.Info.TieDown + " TIE - DOWN");
            if (job.Info.Client.Name == "StickFrame")
            {
                br.Attributes["Client"] = new AttributeReference(job.Info.Customer);
            }
            else
            {
                br.Attributes["Client"] = new AttributeReference("");
            }
            br.Attributes["Address"] = new AttributeReference(job.Info.JobAddress);
            br.Attributes["City"] = new AttributeReference(job.Info.SubAddress);
            br.Attributes["Title"] = new AttributeReference(sheet.Name);
            DateTime today = DateTime.Today;
            var nowDate = today.ToString("MM/dd/yyyy");
            br.Attributes["Date"] = new AttributeReference(nowDate);
            br.Attributes["JobNo"] = new AttributeReference(job.Info.JobNumber);
            br.Attributes["Scale"] = new AttributeReference("ScaleFactor");
            return br;
        }
        public void AddDefaultViews(CustomSheet sheet)
        {
            double unitsConversionFactor = Utility.GetLinearUnitsConversionFactor(linearUnitsType.Millimeters, sheet.Units);
            double extensionAmount = Math.Min(sheet.Width, sheet.Height) / 200;
            // adds Front vector view
            //var view = new CustomVectorView(-100 * unitsConversionFactor, -300 , viewType.Top, 0.01, GetViewName(sheet, viewType.Front,false)){CenterlinesExtensionAmount = extensionAmount};
            var view = new CustomVectorView(127.5, 105, viewType.Top, 0.02, GetViewName(sheet, viewType.Front, false)) { CenterlinesExtensionAmount = extensionAmount };
            var wallName = new WallName(new Point3D(0,0,0),sheet.Name+"Wall2D" );
            wallName.Attributes["Title"] = new AttributeReference(sheet.Name);

            var floorQty = new FloorQuantity(new Point3D(0, 0, 0), sheet.Name + "FloorQty");
            floorQty.Attributes["Title"] = new AttributeReference(sheet.Name);
            
            ////{ CenterlinesExtensionAmount = extensionAmount}
            view.KeepEntityColor = true;
            view.KeepEntityLineType = true;
            view.KeepEntityLineWeight = true;
            view.FillRegions = true;
            //view.HiddenSegments = false;
            //view.ColorMethod = colorMethodType.byEntity;
            //view.LineTypeMethod = colorMethodType.byEntity;
            sheet.AddWallPlaceHolder(wallName, PaperSpace, "Wall2D Name Place");
            sheet.AddFloorQtyPaceHolder(floorQty,PaperSpace, "Floor Qty Place");
            sheet.AddViewPlaceHolder(view,CanvasDrawing,PaperSpace,view.BlockName.Replace(sheet.Name,String.Empty));
            
            //var view = new RasterView(7 * unitsConversionFactor, 100 * unitsConversionFactor, viewType.Top, 0.5, GetViewName(sheet, viewType.Top, true));
            //view.ColorMethod = colorMethodType.byLayer;
            //view.LineTypeMethod = colorMethodType.byLayer;
            //sheet.Entities.Add(view);
            // adds Trimetric raster view            
            //sheet.Entities.Add(new RasterView(150 * unitsConversionFactor, 230 * unitsConversionFactor, viewType.Trimetric, sheet.Scale, GetViewName(sheet, viewType.Trimetric, true)));
            // adds Top vector view
            //sheet.Entities.Add(new VectorView(70 * unitsConversionFactor, 130 * unitsConversionFactor, viewType.Top, sheet.Scale, GetViewName(sheet, viewType.Top)));
        }
        public string GetViewName(CustomSheet sheet, viewType viewType, bool isRaster = false)
        {
            return String.Format("{0} {1} {2}", sheet.Name, viewType.ToString(), isRaster ? "Raster View" : "Vector View");
        }

        private WorkManager<MyViewBuilder> _workManager;
        private void CreateWorkManager()
        {
            if (_workManager !=null) return;
            _workManager = new WorkManager<MyViewBuilder>();
            _workManager.WorkUnitCompleted += WorkManagerWorkUnitCompleted;
            _workManager.QueueCompleted += WorkManagerQueueCompleted;
            _workManager.QueueCancelled += WorkManagerQueueCancelled;
        }

        

        //private void CreateWallNameBuilderManager()
        //{
        //    if (_wallNameBuilderManager!=null) return;
        //    _wallNameBuilderManager = new WorkManager<WallNameBuilder>();
        //    _wallNameBuilderManager.WorkUnitCompleted += WallNameBuilderManagerOnWorkUnitCompleted;
        //    _wallNameBuilderManager.QueueCompleted+= WallNameBuilderManagerOnQueueCompleted;
        //    _wallNameBuilderManager.QueueCancelled += WallNameBuilderManagerOnQueueCancelled;
        //}

        //private void WallNameBuilderManagerOnQueueCompleted(object sender, EventArgs e)
        //{
        //    DestroyWallBuilderManager();
        //}

        //private void WallNameBuilderManagerOnQueueCancelled(object sender, WorkUnitEventArgs e)
        //{
        //    DestroyWallBuilderManager();
        //}

        //private void WallNameBuilderManagerOnWorkUnitCompleted(object sender, WorkUnitEventArgs e)
        //{
        //    var wallNameBuilder = (WallNameBuilder) e.WorkUnit;
        //    wallNameBuilder.AddToDrawings();
        //    _wallNameBuilderManager.RemoveFromQueue(wallNameBuilder);
        //}

        //private void DestroyWallBuilderManager()
        //{
        //    if(_wallNameBuilderManager== null) return;
        //    _wallNameBuilderManager.WorkUnitCompleted -= WallNameBuilderManagerOnWorkUnitCompleted;
        //    _wallNameBuilderManager.QueueCompleted -= WallNameBuilderManagerOnQueueCompleted;
        //    _wallNameBuilderManager.QueueCancelled -= WallNameBuilderManagerOnQueueCancelled;
        //    _wallNameBuilderManager.Dispose();
        //    _wallNameBuilderManager = null;
        //}
        

        private void DestroyWorkManager()
        {
            if (_workManager == null) return;
            _workManager.WorkUnitCompleted -= WorkManagerWorkUnitCompleted;
            _workManager.QueueCompleted -= WorkManagerQueueCompleted;
            _workManager.QueueCancelled -= WorkManagerQueueCancelled;
            _workManager.Dispose();
            _workManager = null;
        }

        private void WorkManagerWorkUnitCompleted(object sender, WorkUnitEventArgs e)
        {
            MyViewBuilder vb = (MyViewBuilder) e.WorkUnit;
            vb.AddToDrawings(PaperSpace);
            _workManager.RemoveFromQueue(vb);
            RefreshDrawings();
        }

        private void RefreshDrawings()
        {
            if (PaperSpace.ActiveSheet == null)
            {
                if (PaperSpace.Sheets.Count == 0)
                {
                    return;
                }
                this.ActivateSheet(PaperSpace.Sheets[0]);
                PaperSpace.ZoomFit();
            }

            if (PaperSpace.IsToReload && !PaperSpace.IsImported && !PaperSpace.IsModified)
            {
                //PaperSpace.IsToReload = false;
            }
            PaperSpace.Invalidate();
        }

        private void ActivateSheet(Sheet sheet)
        {
            if (PaperSpace.ActiveSheet == null || sheet != null || PaperSpace.ActiveSheet.Name.Equals(sheet.Name))
            {
                PaperSpace.ActiveSheet = sheet;
                var blockRefSheet = GetFormatBlockReference(PaperSpace.ActiveSheet);
                var vectorView = GetVecterViewRef(PaperSpace.ActiveSheet);
                if (blockRefSheet != null && vectorView != null)
                {
                    if (blockRefSheet.Attributes.ContainsKey("Scale"))
                    {
                        var f = (Rational)vectorView.Scale;
                        var scale = f.ToString().Replace('/', ':');
                        blockRefSheet.Attributes["Scale"].Value = scale;
                    }
                    PaperSpace.UpdateBoundingBox();
                }
                PaperSpace.Invalidate();
            }
            
        }
        private void WorkManagerQueueCancelled(object sender, WorkUnitEventArgs e)
        {
            DestroyWorkManager();
        }

        private void WorkManagerQueueCompleted(object sender, EventArgs e)
        {
            DestroyWorkManager();
        }
        public void StartViewBuilder(View singleView = null, bool changedOnly = true)
        {
            //CreateWallNameBuilderManager();
            CreateWorkManager();
            if (singleView!=null)
            {
                PaperSpace.ActiveSheet.AddViewPlaceHolder(singleView,CanvasDrawing,PaperSpace);
                _workManager.AppendToQueue(new MyViewBuilder(CanvasDrawing,PaperSpace,singleView,PaperSpace.ActiveSheet,_viewModel.JobModel));
            }
            else
            {
                foreach (var paperSpaceSheet in PaperSpace.Sheets)
                {
                    for (int i = 0; i < paperSpaceSheet.Entities.Count; i++)
                    {
                        var view  = paperSpaceSheet.Entities[i] as CustomVectorView;
                        if (view!=null )
                        {
                            if (_workManager.GetQueueItems().Count(x => x.Contains(view)) > 0)
                                continue;
                            paperSpaceSheet.AddViewPlaceHolder(view,CanvasDrawing,PaperSpace);
                            var wallName = new WallName(new Point3D(0,0,0), paperSpaceSheet.Name+"Wall2D" );
                            wallName.Attributes["Title"] = new AttributeReference(paperSpaceSheet.Name);
                            ((CustomSheet)paperSpaceSheet).AddWallPlaceHolder(wallName,PaperSpace, "Wall2D Name Place");
                            var floorQty = new FloorQuantity(new Point3D(0, 0, 0), paperSpaceSheet.Name + "FloorQty");
                            floorQty.Attributes["Title"] = new AttributeReference(paperSpaceSheet.Name);
                            ((CustomSheet)paperSpaceSheet).AddFloorQtyPaceHolder(floorQty, PaperSpace, "Floor Qty Place");
                            _workManager.AppendToQueue(new MyViewBuilder(CanvasDrawing,PaperSpace,view, paperSpaceSheet,_viewModel.JobModel));
                            //_wallNameBuilderManager.AppendToQueue(new WallNameBuilder(PaperSpace,_viewModel.JobModel));
                            
                        }
                    }
                }
            }
            if (_workManager.QueueCount > 0)
            {
                //_wallNameBuilderManager.RunAll(PaperSpace);
                _workManager.RunAll(PaperSpace);
            }
            else
            {
               RefreshDrawings();
               DestroyWorkManager();
               //DestroyWallBuilderManager();
                
            }

            
            
            //if (!PaperSpace.IsBusy)
            //{
            //    PaperSpace.StartWork(_viewBuilder);
            //}

        }

        #endregion

        #region Private Method

        public void CLear()
        {
            PaperSpace.Clear();
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            System.Windows.Point position = e.GetPosition(this);
            this.DynamicInput.Margin = new Thickness(position.X+15, position.Y+15, 0.0, 0.0);
        }
        
        #endregion

        private void PaperSpace_OnEntitiesSelectedChanged(object sender, EntityEventArgs e)
        {
            
                    if (_viewModel != null)
                    {
                        if (e == null)
                        {
                            _viewModel.SelectedEntity = null;
                            return;
                        }
                        var entityFactory = new EntitiyVmFactory();
                        _viewModel.SelectedEntity = entityFactory.creatEntityVm(e.Item,null);
                    }
        }
    }
}
