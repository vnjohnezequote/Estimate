// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HorizontalJobInfoViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the HorizontalJobInfoViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;
using AppDataBase.DataBase;
using ApplicationInterfaceCore;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.ResponsiveData;

namespace JobInfoModule.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using AppModels;
    using System.Windows.Controls;

    using ApplicationCore.BaseModule;

    using ApplicationService;

    using Prism.Commands;
    using Prism.Events;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// The job info view model.
    /// </summary>
    public class HorizontalJobInfoViewModel : BaseViewModelAware
    {
        #region Private Member

        private ClientPoco _clientPoco;
        private ObservableCollection<string> _builders;


        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalJobInfoViewModel"/> class.
        /// </summary>
        public HorizontalJobInfoViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalJobInfoViewModel"/> class.
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
        public HorizontalJobInfoViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager,IJob jobModel)
            : base(unityContainer, regionManager, eventAggregator, layerManager,jobModel)
        {
            this.BuilderNameLostFocusCommand = new DelegateCommand<ComboBox>(this.OnAddBuilderNamesChanged, this.CanAddBuiderNameCanExcute);
            this.EventAggregator.GetEvent<CustomerService>().Subscribe(this.SelectedClientReceive);
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

        public ClientPoco SelectClient
        {
            get => this._clientPoco;
            set
            {
                this.SetProperty(ref this._clientPoco, value);
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

        private void SelectedClientReceive(ClientPoco selectClient)
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
            if (string.IsNullOrEmpty(this.JobInfo.ClientName))
            {
                return;
            }
            this.SelectClient = db.GetClient(this.JobInfo.ClientName);
            this.BuilderNamesReseive(this.SelectClient.Builders);
        }

        #endregion

    }
}
