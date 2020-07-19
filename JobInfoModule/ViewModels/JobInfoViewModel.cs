// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobInfoViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the JobInfoViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using ApplicationInterfaceCore;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.ResponsiveData;
using Prism.Mvvm;

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
    public class JobInfoViewModel : BaseJobInForViewModel
    {
        #region Private Member

        //private ClientPoco _clientPoco;
        private ObservableCollection<string> _builders;
        #endregion

        #region Property

        public bool IsBuilderEnable => SelectedClient != null;

        public ObservableCollection<string> Builders
        {
            get => this._builders;
            set => this.SetProperty(ref this._builders, value);
        }
        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="JobInfoViewModel"/> class.
        /// </summary>
        //public JobInfoViewModel()
        //{
        //}

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
        public JobInfoViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager,IJob jobModel)
            : base(unityContainer, regionManager, eventAggregator, layerManager,jobModel)
        {
            this.BuilderNameLostFocusCommand = new DelegateCommand<ComboBox>(this.OnAddBuilderNamesChanged, this.CanAddBuiderNameCanExcute);
            //this.EventAggregator.GetEvent<CustomerService>().Subscribe(this.SelectedClientReceive);
        }

        #endregion

        #region Command		
        public DelegateCommand<ComboBox> BuilderNameLostFocusCommand { get; private set; }

        #endregion

       

        #region Private Funtc

        protected override void SelectedClientReceive(ClientPoco selectClient)
        {
           base.SelectedClientReceive(selectClient);
            this.RaisePropertyChanged(nameof(IsBuilderEnable));
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
                if (string.IsNullOrEmpty(inforView.Text) || SelectedClient is null)
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
            if (this.SelectedClient.Builders.Contains(inforView.Text) || string.IsNullOrEmpty(inforView.Text))
            {
                return;
            }
            this.Builders.Add(inforView.Text);

            this.SelectedClient.Builders = this.Builders.ToList();

            var dataBase = this.UnityContainer.Resolve<ClientDataBase>();
            dataBase.UpdateClient(this.SelectedClient);
        }

        protected override void Initilazied()
        {
            base.Initilazied();
            this.BuilderNamesReseive(this.SelectedClient.Builders);
        }

        #endregion

    }
}
