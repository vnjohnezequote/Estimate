// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobWizardService.cs" company="John Nguyen">
// John Nguyen  
// </copyright>
// <summary>
//   The job wizard service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NewJobWizardModule

{
   using ApplicationInterfaceCore;

   using NewJobWizardModule.Views;

   using Prism.Regions;

    using Unity;

    /// <summary>
    /// The job wizard service.
    /// </summary>
    public class JobWizardService : IShellService
    {
        /// <summary>
        /// The _unity container.
        /// </summary>
       private IUnityContainer _unityContainer;

        /// <summary>
        /// The _region manager.
        /// </summary>
        private IRegionManager _regionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobWizardService"/> class. 
        /// </summary>
        /// <param name="unityContainer">
        /// The unity container.
        /// </param>
        /// <param name="regionManager">
        /// The region manager.
        /// </param>
        public JobWizardService(IUnityContainer unityContainer, IRegionManager regionManager)
        {
            this._unityContainer = unityContainer;
            this._regionManager = regionManager;

        }

        /// <summary>
        /// The show shell.
        /// </summary>
        public void ShowShell()
        {
            var shell = this._unityContainer.Resolve<NewJobWizardView>();
            var scopedRegion = this._regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(shell, scopedRegion);

            // RegionManager.UpdateRegions();
            shell.ShowDialog();
        }
    }
}
