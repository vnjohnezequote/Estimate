// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesignInfoViewModel.cs" company="John nguyen">
//   John nguyen
// </copyright>
// <summary>
//   The design info view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using ApplicationInterfaceCore;
using AppModels.Interaface;
using AppModels.ResponsiveData;

namespace JobInfoModule.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using ApplicationCore.BaseModule;

    using AppModels;

    using JetBrains.Annotations;

    using LiteDB;
    using Prism.Events;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// The design info view model.
    /// </summary>
    public class DesignInfoViewModel : BaseViewModelAware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DesignInfoViewModel"/> class.
        /// </summary>
        public DesignInfoViewModel()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DesignInfoViewModel"/> class.
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
        public DesignInfoViewModel(
            [NotNull] IUnityContainer unityContainer,
            [NotNull] IRegionManager regionManager,
            [NotNull] IEventAggregator eventAggregator,ILayerManager layerManager,IJob jobModel)
            : base(unityContainer, regionManager, eventAggregator,layerManager,jobModel)
        {
        }


       

    }
}