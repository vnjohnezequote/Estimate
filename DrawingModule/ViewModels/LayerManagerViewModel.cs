using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
//using System.Windows.Input;
using ApplicationCore.BaseModule;
using ApplicationInterfaceCore;
using AppModels.AppData;
using AppModels.Interaface;
using devDept.Eyeshot;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Syncfusion.UI.Xaml.Grid;
using Unity;

namespace DrawingModule.ViewModels
{
    public class LayerManagerViewModel : BaseViewModel
    {
        #region Field

        private IEntitiesManager _entitiesManger;
        #endregion

        #region Properties
        public ICommand DeleteLayerCommand { get; private set; }
        public ICommand AddNewLayerCommand { get; private set; }
        public ObservableCollection<LineType> LineTypes { get; set; }
        #endregion

        #region Constructor

        public LayerManagerViewModel()
        {

        }
        public LayerManagerViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager, IEntitiesManager entitiesManager,IJob jobModel)
            : base(unityContainer, regionManager, eventAggregator,layerManager,jobModel)
        {
            _entitiesManger = entitiesManager;
            LineTypes = new ObservableCollection<LineType>();
            PrepareLineTypes();
            PrepareLayers();
            DeleteLayerCommand = new DelegateCommand<SfDataGrid>(OnDeleteLayerRow);
            AddNewLayerCommand = new DelegateCommand(OnAddNewLayer);
        }

        private void OnAddNewLayer()
        {
            if (LayerManager.Layers.Count <= 0) return;
            var newLayer = new LayerItem();
            var newName = LayerManager.SelectedLayer.Name;
            newLayer.Name = newName;
            var checkName = LayerManager.Layers.Any(layer => layer.Name == newLayer.Name);
            if (checkName)
            {
                newLayer.Name = newLayer.Name + LayerManager.Layers.Count.ToString();
            }
            LayerManager.Layers.Add(newLayer);
            LayerManager.SelectedLayer = newLayer;
        }

        private void PrepareLineTypes()
        {
            //LineTypes.Add(new LinePattern("Continues",null,"Continues __________________"));
            var continues = new LineType("Continues", null, "Continues __________________");
            if (!LineTypes.Contains(continues))
            {
                LineTypes.Add(continues);
            }
            var dashDot =  new LineType("Dash Dot", new[] { 5f, -1f, 1f, -1f }, "Dash dot _ . _ . _ . _ .");
            if (!LineTypes.Contains(continues))
            {
                LineTypes.Add(dashDot);
            }
            var dashSpace = new LineType("Dash Space", new[] { 5f, -5f }, "Dash space __ __ __ __ __");

            if (!LineTypes.Contains(dashSpace))
            {
                LineTypes.Add(dashSpace);
            }
            
            
        }

        private void PrepareLayers()
        {
            
        }
        private void OnDeleteLayerRow(SfDataGrid layerGrid)
        {
            var recordId = layerGrid.SelectedIndex;

            if (recordId < 0)
            {
                return;
            }
            this.LayerManager.RemoveAt(recordId);
            //this.Layers.RemoveAt(recordId);
        }

        #endregion
    }
}
