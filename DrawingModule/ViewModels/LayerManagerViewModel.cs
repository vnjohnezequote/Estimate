using System.Windows.Input;
using ApplicationCore.BaseModule;
using ApplicationInterfaceCore;
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
        #endregion

        #region Constructor

        public LayerManagerViewModel()
        {

        }
        public LayerManagerViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager, IEntitiesManager entitiesManager)
            : base(unityContainer, regionManager, eventAggregator,layerManager)
        {
            _entitiesManger = entitiesManager;
            DeleteLayerCommand = new DelegateCommand<SfDataGrid>(OnDeleteLayerRow);
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
