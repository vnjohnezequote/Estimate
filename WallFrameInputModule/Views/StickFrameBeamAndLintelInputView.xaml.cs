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
using Syncfusion.UI.Xaml.Grid;
using WallFrameInputModule.ViewModels;

namespace WallFrameInputModule.Views
{
    /// <summary>
    /// Interaction logic for StickFrameBeamAndLintelInputView.xaml
    /// </summary>
    public partial class StickFrameBeamAndLintelInputView : UserControl
    {
        private StickFrameBeamAndLintelInputViewModel _viewModel;
        public StickFrameBeamAndLintelInputView()
        {
            InitializeComponent();
            if (DataContext !=null && DataContext is StickFrameBeamAndLintelInputViewModel vm)
            {
                _viewModel = vm;
                (this.BeamSupportDataGrid.Columns["EngineerMemberInfo"] as GridComboBoxColumn).UseBindingValue = true;

                (this.BeamSupportDataGrid.Columns["EngineerMemberInfo"] as GridComboBoxColumn).ItemsSource =
                    _viewModel.EngineerList;

                (this.LintelSupportDataGrid.Columns["EngineerMemberInfo"] as GridComboBoxColumn).UseBindingValue = true;
                (this.LintelSupportDataGrid.Columns["EngineerMemberInfo"] as GridComboBoxColumn).ItemsSource =
                    _viewModel.EngineerList;
                

            }
            Loaded += StickFrameBeamAndLintelInputView_Loaded;
        }

        private void StickFrameBeamAndLintelInputView_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel?.InitEngineerList();
        }
    }
}
