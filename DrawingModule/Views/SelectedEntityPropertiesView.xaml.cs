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
using ApplicationInterfaceCore;
using AppModels.Interaface;

namespace DrawingModule.Views
{
    /// <summary>
    /// Interaction logic for SelectedEntityProperty.xaml
    /// </summary>
    public partial class SelectedEntityPropertiesView : UserControl
    {
        private IEntitiesManager _entitiesManger;

        public SelectedEntityPropertiesView()
        {
            InitializeComponent();
        }
    }
}
