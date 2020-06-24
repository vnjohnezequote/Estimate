// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WinCategoryViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the WinCategoryViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AppModels;

namespace JobInfoModule.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Input;

    using ApplicationCore.BaseModule;

    using ApplicationService;

    using CustomControls.Controls;
    using Views;

    using Prism.Commands;
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
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private FlatWindow _window;
        /// <summary>
        /// The general wind.
        /// </summary>
        //private GeneralWindCategoryView _generalWind;

        private string _clientName;
        /// <summary>
        /// The prenai wind.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        //private PrenailWindCategoryView _prenaiWind;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="WindCategoryViewModel"/> class.
        /// </summary>
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
        public WindCategoryViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(unityContainer, regionManager, eventAggregator)
        {
            this.RegionManager = this.RegionManager.CreateRegionManager();
            //this.EventAggre.GetEvent<CustomerService>().Subscribe(this.SetClient);
        }

        #region Command

        #endregion
		
		#region Public Member

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
        /// The set client.
        /// </summary>
        /// <param name="clientName">
        /// The client name.
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
        /// The client name.
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
                    break;
                default:
                    break;
            }
        }
		
		private void TranfersJob(string regionName, string viewPath)
		{
			var parameters = new NavigationParameters();
                parameters.Add("Job", Job);
                if (Job!=null)
                {
                    this.RegionManager.RequestNavigate(regionName,viewPath,parameters);   
                }
		}

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            this.SetClient(this.Job.ClientName);
        }

        #endregion
    }
}

