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
using AppModels.AppData;
using devDept.Eyeshot;
using DrawingModule.ViewModels;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.Windows.Tools.Controls;

namespace DrawingModule.Views
{
    /// <summary>
    /// Interaction logic for LayerManagerView.xaml
    /// </summary>
    public partial class LayerManagerView : UserControl
    {
        private LayerManagerViewModel _viewModel;
        public LayerManagerView()
        {
            InitializeComponent();
            _viewModel = DataContext as LayerManagerViewModel;
            this.LayerManagerGrid.AddNewRowInitiating+=LayerManagerGridOnAddNewRowInitiating;
        }

        private void LayerManagerGridOnAddNewRowInitiating(object sender, AddNewRowInitiatingEventArgs e)
        {
            if (_viewModel.LayerManager.Layers == null || _viewModel.LayerManager.Layers.Count > 0)
            {
                var newData = e.NewObject as LayerItem;

                var checkName = _viewModel.LayerManager.Layers.Any(layer => layer.Name == newData.Name);
                if (checkName)
                {
                    newData.Name = newData.Name + _viewModel.LayerManager.Layers.Count.ToString();
                }
            }

            //Layer layer;
        }

       
    }
}
