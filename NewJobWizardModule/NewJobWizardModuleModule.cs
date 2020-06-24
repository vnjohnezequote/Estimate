
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewJobWizardModule.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the NewJobWizardModule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NewJobWizardModule
{
    using ApplicationCore.BaseModule;

    using global::NewJobWizardModule.Views;

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
        /// Initializes a new instance of the <see cref="NewJobWizardModule"/> class.
        /// </summary>
        /// <param name="unityContainer">
        /// The unity container.
        /// </param>
        /// <param name="regionManager">
        /// The region manager.
        /// </param>
        public NewJobWizard(IUnityContainer unityContainer, IRegionManager regionManager)
            : base(unityContainer, regionManager)
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
            
        }
    }
}
