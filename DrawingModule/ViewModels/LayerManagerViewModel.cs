using System.Collections.ObjectModel;
using System.Windows.Input;
using ApplicationCore.BaseModule;
using ApplicationInterfaceCore;
using devDept.Eyeshot;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.XPS;
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
        public ObservableCollection<LinePattern> LineTypes { get; set; }
        #endregion

        #region Constructor

        public LayerManagerViewModel()
        {

        }
        public LayerManagerViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager, IEntitiesManager entitiesManager)
            : base(unityContainer, regionManager, eventAggregator,layerManager)
        {
            _entitiesManger = entitiesManager;
            LineTypes = new ObservableCollection<LinePattern>();
            PrepareLineTypes();
            DeleteLayerCommand = new DelegateCommand<SfDataGrid>(OnDeleteLayerRow);
        }

        private void PrepareLineTypes()
        {
            //var lineType = new LinePattern("Dash dot", new[] { 5f, -1f, 1f, -1f }, "Dash dot _._._._.");
            //lineType.Name;
            //lineType.Description
            LineTypes.Add(new LinePattern("Continues",null,"Continues __________________"));
            LineTypes.Add(new LinePattern("Dash Dot",new []{ 5f, -1f, 1f, -1f },"Dash dot _ . _ . _ . _ ."));
            LineTypes.Add(new LinePattern("Dash Space", new []{5f,-5f}, "Dash space __ __ __ __ __"));
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
