// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseModule.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the BaseModule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ApplicationCore.BaseModule
{
    using System.Diagnostics.CodeAnalysis;

    using Prism.Events;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// The base module.
    /// </summary>
    public abstract class BaseModule : IModule
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseModule"/> class.
        /// </summary>
        protected BaseModule()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseModule"/> class. 
        /// Constructor
        /// </summary>
        /// <param name="unityContainer">
        /// The Unity container.
        /// </param>
        /// <param name="regionManager">
        /// The region manager.
        /// </param>
        protected BaseModule(IUnityContainer unityContainer, IRegionManager regionManager,IEventAggregator eventAggregator)
        {
            this.UnityContainer = unityContainer;
            this.RegionManager = regionManager;
            this.EventAggre = eventAggregator;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets the unity container.
        /// </summary>
        protected IUnityContainer UnityContainer { get; private set; }

        /// <summary>
        /// Gets the region manager.
        /// </summary>
        protected IRegionManager RegionManager { get; set; }

        /// <summary>
        /// Gets the event aggre.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        protected IEventAggregator EventAggre { get; private set; }
        #endregion Properties

        #region Private Member

        #endregion

        #region Interface IModule
        /// <summary>
        /// Register Type For Imodule
        /// </summary>
        /// <param name="containerRegistry">Register Type For Container</param>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public virtual void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        /// <summary>
        /// Initialize module
        /// </summary>
        /// <param name="containerProvider">
        /// The container Provider.
        /// </param>
        public virtual void OnInitialized(IContainerProvider containerProvider)
        {
        }
        #endregion Interface IModule
    }
}
