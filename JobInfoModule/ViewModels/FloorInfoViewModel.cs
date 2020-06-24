using System.Windows.Input;
using ApplicationCore.BaseModule;
using AppModels;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Unity;

namespace JobInfoModule.ViewModels
{
    public class FloorInfoViewModel : BaseFloorViewModelAware
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FloorInfoViewModel"/> class.
        /// </summary>
        public FloorInfoViewModel()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloorInfoViewModel"/> class.
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
        public FloorInfoViewModel(
           IUnityContainer unityContainer,
           IRegionManager regionManager,
           IEventAggregator eventAggregator)
           : base(unityContainer, regionManager, eventAggregator)
        {
            

        }


        #endregion
    }
}
