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

            if (this.DataContext!=null)
            {
                _viewModel = this.DataContext as PrenailFloorInputViewModel;
            }
        }


        private void WallInput_OnCurrentCellDropDownSelectionChanged(object sender, CurrentCellDropDownSelectionChangedEventArgs e)
        {
            _viewModel?.CalculatorWallLength();
        }
    }
}
