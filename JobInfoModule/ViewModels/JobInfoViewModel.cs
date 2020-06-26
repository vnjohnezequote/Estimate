// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobInfoViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the JobInfoViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using ApplicationInterfaceCore;

namespace JobInfoModule.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Controls;

    using AppDataBase.DataBase;

    using ApplicationCore.BaseModule;

    using ApplicationService;

    using AppModels;

    using Prism.Commands;
    using Prism.Events;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// The job info view model.
    /// </summary>
    public class JobInfoViewModel : BaseViewModelAware
    {
        #region Private Member

        private Client _client;
        private ObservableCollection<string> _builders;


        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="JobInfoViewModel"/> class.
        /// </summary>
        public JobInfoViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobInfoViewModel"/> class.
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
        public JobInfoViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager)
            : base(unityContainer, regionManager, eventAggregator, layerManager)
        {
            this.BuilderNameLostFocusCommand = new DelegateCommand<ComboBox>(this.OnAddBuilderNamesChanged, this.CanAddBuiderNameCanExcute);
            this.EventAggre.GetEvent<CustomerService>().Subscribe(this.SelectedClientReceive);
        }

        #endregion

        #region Command		
        public DelegateCommand<ComboBox> BuilderNameLostFocusCommand { get; private set; }

        #endregion

        #region Public Member

        public bool IsBuilderEnable
        {
            get
            {
                if (this.SelectClient == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public Client SelectClient
        {
            get => this._client;
            set
            {
                this.SetProperty(ref this._client, value);
                this.RaisePropertyChanged(nameof(IsBuilderEnable));
            }
        }

        public ObservableCollection<string> Builders
        {
            get => this._builders;
            set => this.SetProperty(ref this._builders, value);
        }

        #endregion

        #region Private Funtc

        private void SelectedClientReceive(Client selectClient)
        {
            if (selectClient == null)
            {
                return;
            }
            this.SelectClient = selectClient;
            BuilderNamesReseive(selectClient.Builders);
        }

        private void BuilderNamesReseive(List<string> builderNames)
        {
            this.Builders = new ObservableCollection<string>(builderNames);
        }

        private bool CanAddBuiderNameCanExcute(ComboBox parameter)
        {
            var canExcute = false;
            if (parameter is ComboBox inforView)
            {
                if (string.IsNullOrEmpty(inforView.Text) || SelectClient is null)
                {
                    canExcute = false;
                }
                else
                {
                    canExcute = true;
                }
            }

            return canExcute;
        }

        private void OnAddBuilderNamesChanged(ComboBox parameter)
        {
            if (!(parameter is ComboBox inforView)) return;
            if (this.SelectClient.Builders.Contains(inforView.Text) || string.IsNullOrEmpty(inforView.Text))
            {
                return;
            }
            this.Builders.Add(inforView.Text);

            this.SelectClient.Builders = this.Builders.ToList();

            var dataBase = this.UnityContainer.Resolve<ClientDataBase>();
            dataBase.UpdateClient(this.SelectClient);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            var db = this.UnityContainer.Resolve<ClientDataBase>();
            if (string.IsNullOrEmpty(this.Job.ClientName))
            {
                return;
            }
            this.SelectClient = db.GetClient(this.Job.ClientName);
            this.BuilderNamesReseive(this.SelectClient.Builders);
        }

        #endregion

    }
}
