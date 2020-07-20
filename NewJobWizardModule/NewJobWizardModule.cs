// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewJobWizardModule.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The window control module.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using DataType.Class;

namespace NewJobWizardModule
{
    using System;

    using ApplicationCore.BaseModule;

    using JobInfoModule.Views;

    using Prism.Events;
    using Prism.Ioc;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// The window control module.
    /// </summary>
    public class NewJobWizard : BaseModule
    {
        #region Private Member

        
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="NewJobWizard"/> class. 
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
        public NewJobWizard(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(unityContainer, regionManager, eventAggregator)
        {
        }

        #region Public Event

        /// <summary>
        /// The clientPoco changed.
        /// </summary>
        public event EventHandler ClientChanged;

        #endregion

        /// <summary>
        /// The on initialized.
        /// </summary>
        /// <param name="containerProvider">
        /// The container provider.
        /// </param>
        public override void OnInitialized(IContainerProvider containerProvider)
        {
        }

        /// <summary>
        /// The register view.
        /// </summary>
        private void RegisterView()
        {
            this.RegionManager.RegisterViewWithRegion("NewProJectRegion", typeof(ChoosingCustomerView));
            this.RegionManager.RegisterViewWithRegion("BasicInfoRegion", typeof(JobInfoView));
            this.RegionManager.RegisterViewWithRegion("WindInfoRegion", typeof(WindCategoryView));
            this.RegionManager.RegisterViewWithRegion("RoofInfoRegion", typeof(RoofInfoView));
            this.RegionManager.RegisterViewWithRegion("DesignRegion", typeof(DesignInfoView));
            this.RegionManager.RegisterViewWithRegion("FloorNumberChooseRegion", typeof(LevelInfoView));
            this.RegionManager.RegisterViewWithRegion(NewJobWizardRegions.AddStickFrameInfoRegion, typeof(AdditionInforForStickFrameView));
        }

    }
}
