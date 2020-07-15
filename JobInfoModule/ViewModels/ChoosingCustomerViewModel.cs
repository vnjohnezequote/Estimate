// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChoosingCustomerViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The choosing customer view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using ApplicationInterfaceCore;

namespace JobInfoModule.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using AppDataBase.DataBase;

    using ApplicationCore.BaseModule;

    using ApplicationService;

    using AppModels;

    using JetBrains.Annotations;

    using MaterialDesignExtensions.Controls;

    using Prism.Commands;
    using Prism.Events;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// The choosing customer view model.
    /// </summary>
    public class ChoosingCustomerViewModel : BaseViewModelAware
    {
        #region Private Member

        /// <summary>
        /// The db base.
        /// </summary>
        private ClientDataBase _dbBase;

        /// <summary>
        /// The clients.
        /// </summary>
        private ObservableCollection<Client> _clients;

        
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChoosingCustomerViewModel"/> class.
        /// </summary>
        public ChoosingCustomerViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChoosingCustomerViewModel"/> class.
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
        public ChoosingCustomerViewModel(
            [NotNull] IUnityContainer unityContainer,
            [NotNull] IRegionManager regionManager,
            [NotNull] IEventAggregator eventAggregator,
            ILayerManager layermanager)
            : base(unityContainer, regionManager, eventAggregator,layermanager)
        {
            this._dbBase = this.UnityContainer.Resolve<ClientDataBase>();
            this.Clients = new ObservableCollection<Client>(this._dbBase.Clients);
            this.CustomerMenuLoadedCommand = new DelegateCommand(this.OnCustomerMenuSelect);
            this.CustomerMenuSelectCommand = new DelegateCommand(this.OnCustomerMenuSelect);
            this.JobLocationCommand = new DelegateCommand(this.OnJobLocationSelect);
        }
        #endregion

        #region Command

        /// <summary>
        /// Gets the customer menu select command.
        /// </summary>
        public ICommand CustomerMenuSelectCommand { get; private set; }

        /// <summary>
        /// Gets the customer menu loaded command.
        /// </summary>
        public ICommand CustomerMenuLoadedCommand { get; private set; }

        /// <summary>
        /// Gets the job location command.
        /// </summary>
        public ICommand JobLocationCommand { get; private set; }
        #endregion
        #region Public Property

        /// <summary>
        /// Gets or sets the clients.
        /// </summary>
        public ObservableCollection<Client> Clients
        {
            get => this._clients;
            set => this.SetProperty(ref this._clients, value);
        }

        #endregion
        #region Private Funtion

        /// <summary>
        /// The on customer menu select.
        /// </summary>
        /// <param name="customerMenu">
        /// The customer Menu.
        /// </param>
        private void OnCustomerMenuSelect()
        {
            if (this.SelectedClient is null)
            {
                return;
            }
            this.Job.ClientName = this.SelectedClient.Name;
            this.EventAggre.GetEvent<CustomerService>().Publish(this.SelectedClient);
        }

        private async void OnJobLocationSelect()
        {
            OpenDirectoryDialogArguments dialogArgs = new OpenDirectoryDialogArguments()
            {
                Width = 600,
                Height = 400,
                CreateNewDirectoryEnabled = true
            };
            OpenDirectoryDialogResult result = await OpenDirectoryDialog.ShowDialogAsync("dialogHost", dialogArgs);
            if (!result.Canceled)
            {
                this.Job.JobLocation = result.DirectoryInfo.FullName;
            }
        }


        #endregion
    }
}
