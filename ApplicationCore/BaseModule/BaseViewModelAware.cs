// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The base view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AppModels;

namespace ApplicationCore.BaseModule
{

    using Prism.Events;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// The base view model.
    /// </summary>
    public abstract class BaseViewModelAware : BaseViewModel, INavigationAware
    {
        #region private Member

        /// <summary>
        /// The _job.
        /// </summary>
        private JobInfo _job;

        /// <summary>
        /// The _selected client.
        /// </summary>
        private Client _selectedClient;
        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModelAware"/> class. 
        /// </summary>
        protected BaseViewModelAware()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModelAware"/> class. 
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
        protected BaseViewModelAware(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator)
        : base(unityContainer, regionManager, eventAggregator)
        {
            // this.RegionManager = regionManager.CreateRegionManager();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the job.
        /// </summary>
        public JobInfo Job
        {
            get => this._job;
            set => this.SetProperty(ref this._job, value);
        }

        public Client SelectedClient
        {
            get => this._selectedClient;
            set => this.SetProperty(ref this._selectedClient, value);
        }
        #endregion Properties

        #region private funtion

        /// <summary>
        /// The on navigated to.
        /// </summary>
        /// <param name="navigationContext">
        /// The navigation context.
        /// </param>
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters["Job"] is JobInfo job)
            {
                this.Job = job;
            }

            if (navigationContext.Parameters["SelectedClient"] is Client selectedClient)
            {
                this.SelectedClient = selectedClient;
            }
        }

        /// <summary>
        /// The is navigation target.
        /// </summary>
        /// <param name="navigationContext">
        /// The navigation context.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <summary>
        /// The on navigated from.
        /// </summary>
        /// <param name="navigationContext">
        /// The navigation context.
        /// </param>
        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        #endregion
    }
}
