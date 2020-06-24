using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.BaseModule;
using AppModels;
using Prism.Events;
using Prism.Regions;
using Unity;

namespace JobInfoModule.ViewModels
{
    public class CostDeliveryViewModel : BaseViewModel, INavigationAware, IRegionMemberLifetime
    {
        private LevelWall _level;
        public CostDeliveryViewModel()
        {
            
        }

        public CostDeliveryViewModel(
           IUnityContainer unityContainer,
           IRegionManager regionManager,
           IEventAggregator eventAggregator)
           : base(unityContainer, regionManager, eventAggregator)
        {


        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (!(navigationContext.Parameters["Level"] is LevelWall level)) return;
            this.Level = level;
        }
        public LevelWall Level { get => this._level; set => this.SetProperty(ref this._level, value); }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var level = navigationContext.Parameters["Level"] as LevelWall;

            if (level != null)
            {
                return this.Level != null && this.Level.LevelName == level.LevelName;
            }
            else
            {
                return true;
            }

        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public bool KeepAlive => false;
    }
}
