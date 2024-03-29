﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDataBase.DataBase;
using ApplicationCore.BaseModule;
using ApplicationInterfaceCore;
using AppModels.CustomEntity;
using AppModels.Interaface;
using AppModels.ViewModelEntity;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Unity;

namespace DrawingModule.ViewModels
{
    public class WallPropertiesManagerViewModel : BaseViewModel
    {
        private WallLine2DVm _wallLine;

        public WallLine2DVm WallLine
        {
            get => _wallLine;
            set => SetProperty(ref _wallLine, value);
        }
        public WallPropertiesManagerViewModel()
        {

        }
        public WallPropertiesManagerViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager,IJob jobModel)
            : base(unityContainer, regionManager, eventAggregator, layerManager,jobModel)
        {
            
        }
    }
}
