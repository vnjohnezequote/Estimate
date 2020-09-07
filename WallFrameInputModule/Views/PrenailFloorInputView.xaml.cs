using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AppModels.ResponsiveData;
using Syncfusion.UI.Xaml.Grid;
using WallFrameInputModule.ViewModels;

//using Syncfusion.UI.Xaml;



namespace WallFrameInputModule.Views
{
    using System.Collections.ObjectModel;

    using AppModels;

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
            //if (this.BeamInput.DataContext is StickFrameBeamAndLintelInputViewModel stickViewModel)
            //{
            //    stickViewModel.LevelInfo = _viewModel.Level;
            //}
        }
    }
}
