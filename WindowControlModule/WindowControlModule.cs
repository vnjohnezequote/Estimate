
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowControlModule.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the WindowControlModule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WindowControlModule
{
    using ApplicationCore.BaseModule;

    using global::WindowControlModule.Views;

    using Prism.Events;
    using Prism.Ioc;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// The window control module.
    /// </summary>
    public class WindowControlModule : BaseModule
    {
        #region Private Member

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowControlModule"/> class.
        /// </summary>
        /// <param name="unityContainer">
        /// The unity container.
        /// </param>
        /// <param name="regionManager">
        /// The region manager.
        /// </param>
        /// <param name="eventAggregator">
        /// The event Aggregator.
        /// </param>
        public WindowControlModule(IUnityContainer unityContainer, IRegionManager regionManager,IEventAggregator eventAggregator)
            : base(unityContainer, regionManager, eventAggregator)
        {
        }

        /// <summary>
        /// The on initialized.
        /// </summary>
        /// <param name="containerProvider">
        /// The container provider.
        /// </param>
        public override void OnInitialized(IContainerProvider containerProvider)
        {
            this.RegionManager.RegisterViewWithRegion("WindowControlRegion", typeof(WindowControlBarView));
            //this.RegionManager.Regions["WindowControlRegion"].Add(UnityContainer.Resolve<WindowControlBarView>());
        }
    }
}
