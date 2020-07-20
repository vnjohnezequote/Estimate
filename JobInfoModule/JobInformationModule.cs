// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobInformationModule.cs" company="John Nguyen">
// John Nguyen
// </copyright>
// <summary>
//   Defines the JobInformationModule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace JobInfoModule
{
    using ApplicationCore.BaseModule;

    using JobInfoModule.Views;

    using Prism.Events;
    using Prism.Ioc;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// The job information module.
    /// </summary>
    public class JobInformationModule : BaseModule
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="JobInformationModule"/> class.
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
        public JobInformationModule(IUnityContainer unityContainer, IRegionManager regionManager,IEventAggregator eventAggregator)
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
            containerRegistry.RegisterForNavigation<JobInfomationView>();
            containerRegistry.RegisterForNavigation<ChoosingCustomerView>();
            containerRegistry.RegisterForNavigation<JobInfoView>();
            containerRegistry.RegisterForNavigation<HorizontalJobInfoView>();
            containerRegistry.RegisterForNavigation<WindCategoryView>();
            containerRegistry.RegisterForNavigation<RoofInfoView>();
            containerRegistry.RegisterForNavigation<DesignInfoView>();
            containerRegistry.RegisterForNavigation<LevelInfoView>();
            containerRegistry.RegisterForNavigation<FloorInfoView>();
            containerRegistry.RegisterForNavigation<CostDeliveryView>();
            containerRegistry.RegisterForNavigation<GeneralWindCategoryView>();
            containerRegistry.RegisterForNavigation<PrenailWindCategoryView>();
            containerRegistry.RegisterForNavigation<AdditionInforForStickFrameView>();

        }

        #region Public Property

        #endregion

        #region Private funtion
       

        #endregion
    }
}
