// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WinCategoryViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the WinCategoryViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using ApplicationInterfaceCore;
using AppModels.Interaface;

namespace JobInfoModule.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using ApplicationCore.BaseModule;
    using CustomControls.Controls;
    using Views;
    using Prism.Events;
    using Prism.Regions;

    using Unity;

    /// <summary>
    /// The win category view model.
    /// </summary>
    public class WindCategoryViewModel : BaseViewModelAware
    {
        #region Private Region
        /// <summary>
        /// The _window.
        /// </summary>
        private FlatWindow _window;
        /// <summary>
        /// The general wind.
        /// </summary>
        //private GeneralWindCategoryView _generalWind;

        private string _clientName;
        /// <summary>
        /// The prenai wind.
        /// </summary>
        //private PrenailWindCategoryView _prenaiWind;
        private ObservableCollection<string> _windRates;
        private ObservableCollection<string> _treatment;
        #endregion

        public WindCategoryViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindCategoryViewModel"/> class.
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
        public WindCategoryViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator,ILayerManager layerManager,IJob jobModel)
            : base(unityContainer, regionManager, eventAggregator,layerManager,jobModel)
        {
            this.RegionManager = this.RegionManager.CreateRegionManager();
            WindRates = new ObservableCollection<string>();
            Treatment = new ObservableCollection<string>();
            //this.EventAggregator.GetEvent<CustomerService>().Subscribe(this.SetClient);
        }

        #region Command

        #endregion

        #region Public Member
        
        public ObservableCollection<string> WindRates
        {
            get => _windRates;
            set => SetProperty(ref _windRates, value);
        }

       
        public ObservableCollection<string> Treatment
        {
            get => _treatment;
            set => SetProperty(ref _treatment, value);
        }

        public string ClientName
        {
            get => this._clientName;
            set
            {
                this.SetProperty(ref this._clientName, value);
                this.LoadWindCategory(value);
            } 
        }

        #endregion
        #region Private Funtion

        /// <summary>
        /// The set clientPoco.
        /// </summary>
        /// <param name="clientName">
        /// The clientPoco name.
        /// </param>
        private void SetClient(string selectClientName)
        {
            if (string.IsNullOrEmpty(selectClientName))
            {
                return;
            }
            this.ClientName = selectClientName;
        }

        /// <summary>
        /// The load wind category.
        /// </summary>
        /// <param name="clientName">
        /// The clientPoco name.
        /// </param>
        private void LoadWindCategory(string clientName)
        {
            var windRegion = "WindRegion";
            switch (clientName)
            {
                case "Warnervale":
					this.TranfersJob(windRegion,nameof(GeneralWindCategoryView));
                    break;
                case "Prenail":
                    this.TranfersJob(windRegion, nameof(PrenailWindCategoryView));
                    break;
                case "Rivo":
                    this.TranfersJob(windRegion, nameof(GeneralWindCategoryView));
                    break;
                case "StickFrame":
                    this.TranfersJob(windRegion,nameof(PrenailWindCategoryView));
                    break;
                default:
                    break;
            }
        }
		
		private void TranfersJob(string regionName, string viewPath)
		{
			var parameters = new NavigationParameters();
                parameters.Add("JobInfo", JobInfo);
                if (JobInfo!=null)
                {
                    this.RegionManager.RequestNavigate(regionName,viewPath,parameters);   
                }
		}

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            this.SetClient(this.JobInfo.ClientName);
        }

        #endregion
    }
}

