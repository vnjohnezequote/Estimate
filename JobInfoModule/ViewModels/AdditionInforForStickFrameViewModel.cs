using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.BaseModule;
using ApplicationInterfaceCore;
using AppModels.Interaface;
using Prism.Events;
using Prism.Regions;
using Unity;

namespace JobInfoModule.ViewModels
{
    public class AdditionInforForStickFrameViewModel : BaseJobInForViewModel
    {

        public AdditionInforForStickFrameViewModel() : base()
        {

        }
        public AdditionInforForStickFrameViewModel(IUnityContainer unityContainer, IRegionManager regionManager,
            IEventAggregator eventAggregator, ILayerManager layerManager, IJob jobModel) : base(unityContainer,
            regionManager, eventAggregator, layerManager, jobModel)
        {
        }

    }
}
