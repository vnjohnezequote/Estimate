﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.BaseModule;
using ApplicationInterfaceCore;
using AppModels;
using AppModels.Interaface;
using AppModels.ResponsiveData;
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
           IEventAggregator eventAggregator,ILayerManager layerManager,IJob jobModel)
           : base(unityContainer, regionManager, eventAggregator,layerManager,jobModel)
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
