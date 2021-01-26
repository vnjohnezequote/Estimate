using System.Windows;
using System.Windows.Controls;
using AppModels.ResponsiveData;
using Syncfusion.UI.Xaml.Grid;
using WallFrameInputModule.ViewModels;


namespace WallFrameInputModule.Views
{
    /// <summary>
    /// Interaction logic for PrenailFloorInputView.xaml
    /// </summary>
    public partial class PrenailFloorInputView : UserControl
    {
        private readonly PrenailFloorInputViewModel _viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrenailFloorInputView"/> class.
        /// </summary>
        public PrenailFloorInputView()
        {

            this.InitializeComponent();

            if (this.DataContext != null)
            {
                _viewModel = this.DataContext as PrenailFloorInputViewModel;
            }

            this.Loaded += PrenailFloorInputView_OnLoaded;
        }


        private void WallInput_OnCurrentCellDropDownSelectionChanged(object sender,
            CurrentCellDropDownSelectionChangedEventArgs e)
        {
            _viewModel?.CalculatorWallLength();
        }

        private void WallInput_OnCurrentCellValidated(object sender, CurrentCellValidatedEventArgs e)
        {
            if (_viewModel == null)
            {
                return;
            }

            var column = e.Column;
            if (column.MappingName != "Stud.SizeGrade")
            {
                return;
            }

            var dataGrid = sender as SfDataGrid;
            var currendRow = e.RowData as PrenailWallLayer;
            var oldData = e.OldValue as string;
            if (currendRow.RibbonPlate.SizeGrade == oldData)
            {
                currendRow.RibbonPlate.SizeGrade = e.NewValue as string;
            }

            if (currendRow.TopPlate.SizeGrade == oldData)
            {
                currendRow.TopPlate.SizeGrade = e.NewValue as string;
            }

            if (currendRow.BottomPlate.SizeGrade == oldData)
            {
                currendRow.BottomPlate.SizeGrade = e.NewValue as string;
            }

        }


        private void PrenailFloorInputView_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel?.InitEngineerList();
            _viewModel?.CalculatorWallLength();
            //if (this.BeamInput.DataContext is StickFrameBeamAndLintelInputViewModel stickViewModel)
            //{
            //    stickViewModel.LevelInfo = _viewModel.Level;
            //}
        }
    }
}
