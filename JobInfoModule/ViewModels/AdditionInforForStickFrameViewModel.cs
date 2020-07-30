using System;
using System.Collections.Generic;
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
        public List<string> TieDowns { get; } = new List<string>(){"900","1200","1500","1800","Direct"};

        public AdditionInforForStickFrameViewModel() : base()
        {

        }
        public AdditionInforForStickFrameViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager, IJob jobModel) : base(unityContainer, regionManager, eventAggregator, layerManager, jobModel)
        {
        }

    }
}
