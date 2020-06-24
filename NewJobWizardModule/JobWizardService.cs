// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobWizardService.cs" company="John Nguyen">
// John Nguyen  
// </copyright>
// <summary>
//   The job wizard service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Windows;
using ApplicationCore.BaseModule;
using CommonServiceLocator;

namespace NewJobWizardModule

{
   using ApplicationInterfaceCore;

   using NewJobWizardModule.Views;

   using Prism.Regions;

    using Unity;

    /// <summary>
    /// The job wizard service.
    /// </summary>
    public class JobWizardService : BaseWindowService
    {
        /// <summary>
        /// The _unity container.
        /// </summary>
       //private IUnityContainer _unityContainer;

        /// <summary>
        /// The _region manager.
        /// </summary>
        //private IRegionManager _regionManager;

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
            :base(unityContainer, regionManager)
        {
           
        }

        /// <summary>
        /// The show shell.
        /// </summary>
        public override void ShowShell<T>()
        {
            var shell = this.UnityContainer.Resolve<T>();
            //var shell = ServiceLocator.Current.GetInstance<T>();
            var scopedRegion = this.RegionManagerment.CreateRegionManager();
            RegionManager.SetRegionManager(shell, scopedRegion);

            // RegionManager.UpdateRegions();
            shell.ShowDialog();
        }
    }
}
