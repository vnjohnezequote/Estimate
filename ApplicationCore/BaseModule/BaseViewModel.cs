// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The base view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using ApplicationInterfaceCore;
using AppModels.Interaface;

namespace ApplicationCore.BaseModule
{
    using System.Diagnostics.CodeAnalysis;

    using Prism.Events;
    using Prism.Mvvm;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// The base view model.
    /// </summary>
    public abstract class BaseViewModel : BindableBase
    {
        #region Field
        private ILayerManager _layerManager;


        #endregion

        #region Constructor


        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
        /// </summary>
        protected BaseViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
        /// </summary>
        /// <param name="unityContainer">
        /// The unity Container.
        /// </param>
        /// <param name="regionManager">
        /// The region Manager.
        /// </param>
        /// <param name="eventAggregator">
        /// The event Aggregator.
        /// </param>
        protected BaseViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager,IJob jobModel)
        {
            this.JobModel = jobModel;
            this.UnityContainer = unityContainer;
            this.RegionManager = regionManager;
            this.EventAggregator = eventAggregator;
            _layerManager = layerManager;
            this.RaisePropertyChanged(nameof(LayerManager));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unity container.
        /// </summary>
        protected IUnityContainer UnityContainer { get; private set; }
        public ILayerManager LayerManager => _layerManager;
        public IRegionManager RegionManager { get; set; }
        protected IEventAggregator EventAggregator { get; private set; }
        public IJob JobModel { get; private set; }
        #endregion Properties
    }
}
