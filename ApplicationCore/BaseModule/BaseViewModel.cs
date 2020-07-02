﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The base view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using ApplicationInterfaceCore;

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
        protected BaseViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager)
        {
            this.UnityContainer = unityContainer;
            this.RegionManager = regionManager;
            this.EventAggre = eventAggregator;
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

        /// <summary>
        /// Gets or sets the region manager.
        /// </summary>
        public IRegionManager RegionManager { get; set; }

        /// <summary>
        /// Gets the event aggre.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        protected IEventAggregator EventAggre { get; private set; }
        #endregion Properties
    }
}
