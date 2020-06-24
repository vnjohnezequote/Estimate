// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The base view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.InteropServices;
using ApplicationService;
using AppModels;

namespace ApplicationCore.BaseModule
{

    using Prism.Events;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// The base view model.
    /// </summary>
    public abstract class BaseFloorViewModelAware : BaseViewModel, INavigationAware
    {
        #region private Member
        private LevelWall _level;
        private Client _selectedClient;
        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
        /// </summary>
        public BaseFloorViewModelAware()
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
        protected BaseFloorViewModelAware(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator)
        : base(unityContainer, regionManager, eventAggregator)
        {
            this.EventAggre.GetEvent<CustomerService>().Subscribe(OnSetCustomer);
        }

        #endregion

        #region Properties
		public LevelWall Level { get => this._level; set => this.SetProperty(ref this._level, value); }
        public Client SelectedClient { get=>this._selectedClient;
            set => this.SetProperty(ref _selectedClient, value);
        }
        #endregion Properties

        #region private funtion

        private void OnSetCustomer(Client selectedClient)
        {
            if (selectedClient != null)
                this.SelectedClient = selectedClient;
        }
    public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (!(navigationContext.Parameters["Level"] is LevelWall level)) return;
            this.Level = level;
            if (!(navigationContext.Parameters["SelectedClient"] is Client client)) return;
            {
                this.SelectedClient = client;
            }

        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var level = navigationContext.Parameters["Level"] as LevelWall;

            if (level != null)
            {
                return this.Level != null && this.Level.LevelName == level.LevelName;
            }
            else
            {
                return false;
            }

        }
		
        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        #endregion

        
    }
}
