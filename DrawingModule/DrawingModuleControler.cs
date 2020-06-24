  // --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawingModuleControler.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the DrawingModuleControler type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DrawingModule.Views;

namespace DrawingModule
{
    using ApplicationCore.BaseModule;

    using Prism.Events;
    using Prism.Ioc;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// The drawing module controler.
    /// </summary>
    public class DrawingModuleControler : BaseModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingModuleControler"/> class.
        /// </summary>
        /// <param name="unityContainer">
        /// The unity container.
        /// </param>
        /// <param name="regionManager">
        /// The region manager.
        /// </param>
        /// <param name="eventAggregator">
        /// The event aggregator.
        /// </param>
        public DrawingModuleControler(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(unityContainer, regionManager, eventAggregator)
        {
        }

        /// <summary>
        /// The register types.
        /// </summary>
        /// <param name="containerRegistry">
        /// The container registry.
        /// </param>
        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);
            
            //containerRegistry.RegisterForNavigation<CommandLineView>();

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
            //this.RegionManager.RegisterViewWithRegion()
        }
    }
}
