using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDataBase.DataBase;
using ApplicationCore.BaseModule;
using ApplicationInterfaceCore;
using AppModels.CustomEntity;
using AppModels.ViewModelEntity;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Unity;

namespace DrawingModule.ViewModels
{
    public class WallPropertiesManagerViewModel : BaseViewModel
    {
        private Wall2DVm _wall;

        public Wall2DVm Wall
        {
            get => _wall;
            set => SetProperty(ref _wall, value);
        }
        public WallPropertiesManagerViewModel()
        {

        }
        public WallPropertiesManagerViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager)
            : base(unityContainer, regionManager, eventAggregator, layerManager)
        {
            
        }
    }
}
