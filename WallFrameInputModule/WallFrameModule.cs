using ApplicationCore.BaseModule;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using Unity;
using WallFrameInputModule.Views;

namespace WallFrameInputModule
{
    public class WallFrameModule : BaseModule
    {

        public WallFrameModule(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(unityContainer, regionManager, eventAggregator)
        {
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);
            containerRegistry.RegisterForNavigation<WallFrameInputView>();
            containerRegistry.RegisterForNavigation<PrenailFloorInputView>();
           

        }

        /// <summary>
        /// The on initialized.
        /// </summary>
        /// <param name="containerProvider">
        /// The container provider.
        /// </param>
        public override void OnInitialized(IContainerProvider containerProvider)
        {
            base.OnInitialized(containerProvider);

        }
    }
}
