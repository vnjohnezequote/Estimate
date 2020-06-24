using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.Control;
using DrawingModule.CustomControl;
using DrawingModule.CustomControl.PaperSpaceControl;
using DrawingModule.Helper;
using DrawingModule.ViewModels;
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
        #region Constructor
        public CanvasDrawingView()
        {
            InitializeComponent();
            this.PaperSpace.WorkCompleted += TestPaperSpace_WorkCompleted;
            this.TabControlDrawing.SelectionChanged += TabControlDrawing_SelectionChanged;
            Loaded += CanvasDrawingView_Loaded;
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

        private BlockReference GetFormatBlockReference(Sheet sheet)
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
            return blockReference is devDept.Eyeshot.Entities.View == false && blockReference.Attributes.ContainsKey("Format");
        }
        private void TestPaperSpace_WorkCompleted(object sender, WorkCompletedEventArgs e)
        {
            var drawings1 = this.PaperSpace;
            ViewBuilder vb = e.WorkUnit as ViewBuilder;
            if (vb != null && PaperSpace.Sheets.Count != 0)
            {
                vb.AddToDrawings(drawings1);
                this.PaperSpace.SetActiveSheet("Sheet1");
                CustomSheet activeSheet = (CustomSheet)PaperSpace.GetActiveSheet();
                var formatBlockReference = GetFormatBlockReference(activeSheet);
                if (formatBlockReference != null)
                {
                    formatBlockReference.Attributes["Format"].Value = formatBlockReference.BlockName.Replace("Sheet1", String.Empty).Trim();
                    var sheetNumber = 1;
                    var sheetsCount = 1;
                    formatBlockReference.Attributes["Sheet"].Value = string.Format("SHEET {0} OF {1}", sheetNumber, sheetsCount);

                    //scaleComboBox.SelectedItem = formatBlockReference.Attributes["Scale"].ToString().Replace("SCALE: ", "");

                    // updates FastZPR representation
                    //drawings.Entities.UpdateBoundingBox();
                }
                drawings1.ZoomFit();

                //if (drawings1.GetActiveSheet() == null)
                //{
                //    drawingsPanel1.ActivateSheet(drawings1.Sheets[0].Name);

                //    if (_treeIsDirty)
                //    {
                //        drawingsPanel1.SyncTree();
                //        _treeIsDirty = false;
                //    }

                //    drawings1.ZoomFit();
                //}

                drawings1.Invalidate();
            }
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


        public void AddSheet(string name, linearUnitsType units, formatType formatType, bool addDefaultView = true)
        {
            Tuple<double, double> size = DrawingHelper.GetFormatSize(units, formatType);
            CustomSheet sheet = new CustomSheet(units, size.Item1, size.Item2, name);

            Block block;
            //BlockReference br = CreateFormatBlock(formatType, sheet, "My DrawingLine", out block);
            //BlockReference br = sheet.BuildA4ISO(out block);
            BlockReference br = BuildFormatBlock(formatType, sheet, out block);
            //BlockReference br = CreateFormatBlock(formatType, sheet,"Test", out block);

            br.Attributes["WindRate"] = new AttributeReference("N2");
            br.Attributes["RoofMaterial"] = new AttributeReference("Sheets");
            br.Attributes["TieDown"] = new AttributeReference("1200 TIE-DOWN");
            //br.Attributes["Format"] = new AttributeReference("Test 2");
            //br.Attributes["DwgNo"] = new AttributeReference("Test 3");
            //br.Attributes["Scale"] = new AttributeReference("Test 4");
            //br.Attributes["Sheet"] = new AttributeReference("Test 5");

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
        private TreeNode _rootTreeNode;
        private BlockReference CreateFormatBlock(formatType formatType, CustomSheet sheet, string titleAttribute,
            out Block block)
        {
            BlockReference br = null;
            block = null;

            if (_formatBlockNames.ContainsKey(sheet.Name))
            {
                PaperSpace.Blocks.Remove(_formatBlockNames[sheet.Name]); // removes both block and related block reference.
                _formatBlockNames.Remove(sheet.Name);
            }

            switch (formatType)
            {
                case formatType.A0_ISO:
                    br = sheet.BuildA0ISO(out block);
                    break;
                case formatType.A1_ISO:
                    br = sheet.BuildA1ISO(out block);
                    break;
                case formatType.A2_ISO:
                    br = sheet.BuildA2ISO(out block);
                    break;
                case formatType.A3_ISO:
                    br = sheet.BuildA3ISO(out block);
                    break;
                case formatType.A4_ISO:
                    br = sheet.BuildA4ISO(out block);
                    break;
                case formatType.A4_LANDSCAPE_ISO:
                    br = sheet.BuildA4LANDSCAPEISO(out block);
                    break;
                case formatType.A_ANSI:
                    br = sheet.BuildAANSI(out block);
                    break;
                case formatType.A_LANDSCAPE_ANSI:
                    br = sheet.BuildALANDSCAPEANSI(out block);
                    break;
                case formatType.B_ANSI:
                    br = sheet.BuildBANSI(out block);
                    break;
                case formatType.C_ANSI:
                    br = sheet.BuildCANSI(out block);
                    break;
                case formatType.D_ANSI:
                    br = sheet.BuildDANSI(out block);
                    break;
                case formatType.E_ANSI:
                    br = sheet.BuildEANSI(out block);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _formatBlockNames.Add(sheet.Name, br.BlockName); // adds format block name to the dictionary

            // Initializes attributes
            br.Attributes["Title"] = new AttributeReference(titleAttribute);
            br.Attributes["Format"] = new AttributeReference(formatType.GetDisplayName());
            br.Attributes["DwgNo"] = new AttributeReference("Root");
            br.Attributes["Scale"] = new AttributeReference("1:2");
            var sheetNumber = PaperSpace.Sheets.IndexOf(PaperSpace.GetActiveSheet()) + 1;
            var sheetsCount = PaperSpace.Sheets.Count;
            if (sheetsCount == 0)
            {
                sheetNumber++;
                sheetsCount++;
            }

            br.Attributes["Sheet"] = new AttributeReference(string.Format("SHEET {0} OF {1}", sheetNumber, sheetsCount));

            return br;
        }
        private void AddDefaultViews(CustomSheet sheet)
        {
            double unitsConversionFactor = Utility.GetLinearUnitsConversionFactor(linearUnitsType.Millimeters, sheet.Units);

            // adds Front vector view
            sheet.Entities.Add(new VectorView(70 * unitsConversionFactor, 230 * unitsConversionFactor, viewType.Front, 0.5, GetViewName(sheet, viewType.Front)));
            // adds Trimetric raster view            
            //sheet.Entities.Add(new RasterView(150 * unitsConversionFactor, 230 * unitsConversionFactor, viewType.Trimetric, sheet.Scale, GetViewName(sheet, viewType.Trimetric, true)));
            // adds Top vector view
            //sheet.Entities.Add(new VectorView(70 * unitsConversionFactor, 130 * unitsConversionFactor, viewType.Top, sheet.Scale, GetViewName(sheet, viewType.Top)));
        }
        private string GetViewName(CustomSheet sheet, viewType viewType, bool isRaster = false)
        {
            return String.Format("{0} {1} {2}", sheet.Name, viewType.ToString(), isRaster ? "Raster View" : "Vector View");
        }


        #endregion

        #region Private Method

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            System.Windows.Point position = e.GetPosition(this);
            this.DynamicInput.Margin = new Thickness(position.X+15, position.Y+15, 0.0, 0.0);
        }
        
        

        #endregion

        
    }
}
