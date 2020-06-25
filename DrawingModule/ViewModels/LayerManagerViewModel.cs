using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ApplicationCore.BaseModule;
using AppModels.AppData;
using devDept.Eyeshot;
using DynamicData.Annotations;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Unity;

namespace DrawingModule.ViewModels
{
    public class LayerManagerViewModel : BaseViewModel, INavigationAware
    {
        #region Field
        private ObservableCollection<LayerItem> _layers;
        private ObservableCollection<LayerItem> _seletedLayers;
        private LayerItem _seletedLayer;
        private string _activeLayer;



        #endregion

        #region Properties

        public ObservableCollection<LayerItem> Layers
        {
            get => _layers;
            private set => SetProperty(ref _layers, value);
        }

        public ObservableCollection<LayerItem> SelectedLayers
        {
            get => _seletedLayers;
            set => SetProperty(ref _seletedLayers, value);
        }

        public LayerItem SelectedLayer
        {
            get => _seletedLayer;
            set => SetProperty(ref _seletedLayer, value);
        }

        public string ActiveLayer
        {
            get => _activeLayer;
            set => SetProperty(ref _activeLayer, value);
        }
        public ICommand LayerChangedColorCommand { get; private set; }
        #endregion

        #region Constructor

        public LayerManagerViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(unityContainer, regionManager, eventAggregator)
        {
            Layers = new ObservableCollection<LayerItem>()
            {
                new LayerItem("1") {Color = Color.Red},
                new LayerItem("2") {Color = Color.Green},
                new LayerItem("3") {Color = Color.Blue},
                new LayerItem("4") {Color = Color.Aqua},
                new LayerItem("5") {Color = Color.BlueViolet},
                new LayerItem("6") {Color = Color.Chartreuse},
            };
            SelectedLayers = new ObservableCollection<LayerItem>();
            LayerChangedColorCommand = new DelegateCommand(ChangeSelectedColor);
            //var layer = new Layer("TEst");
        }

        #endregion
        private void ChangeSelectedColor ()
        {
            if (this.SelectedLayer!=null)
            {
                SelectedLayer.Color = Color.DeepPink;
            }
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (!(navigationContext.Parameters["Layers"] is LayerKeyedCollection layers)) return;
            foreach (var layer in layers)
            {
                var layerItem =
                    new LayerItem(layer)
                    {
                        IsSelected = false,
                        PrintAble = true,
                    };
                    this.Layers.Add(layerItem);
                    
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
    }
}
