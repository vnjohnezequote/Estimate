
using System.Windows;
using ApplicationCore.BaseModule;
using ApplicationService;
using AppModels;
using DataType.Class;
using JobInfoModule.Views;
using Prism.Events;
using Prism.Regions;
using Unity;

namespace JobInfoModule.ViewModels
{
    public class JobInfomationViewModel : BaseViewModel, INavigationAware
    {
		#region private member
        private Visibility _isDesignVisibility;
		private JobModel _job;
        private Client _selectedClient;
		
		#endregion
		
		#region Constructor
		public JobInfomationViewModel()
		{}
		
		 public JobInfomationViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(unityContainer, regionManager, eventAggregator)
        {
			this.EventAggre.GetEvent<CustomerService>().Subscribe(this.ChangeClient);
		}
		#endregion
		
		#region Public Member
		/// <summary>
        /// Gets or sets the is design visibility.
        /// </summary>
        public Visibility IsDesignVisibility
        {
            get => this._isDesignVisibility;
            set => this.SetProperty(ref this._isDesignVisibility, value);
            
        }
		
		public JobModel Job
        {
            get => this._job;
            set => this.SetProperty(ref this._job, value);
        }

        public Client SelectedClient
        {
            get => this._selectedClient;
            set => this.SetProperty(ref _selectedClient, value);
        }
		#endregion
		
		#region private function

        /// <summary>
        /// The Change Client
        /// </summary>
        /// <param name="selectClient"></param>
        private void ChangeClient(Client selectClient)
        {
            if (selectClient ==null)
            {
                this.IsDesignVisibility = Visibility.Collapsed;
                return;
            }
            this.IsDesignVisibility = selectClient.Name == "Prenail" ? Visibility.Visible : Visibility.Collapsed;
			this.LoadInformation();
        }
		
		public void OnNavigatedTo(NavigationContext navigationContext)
        {
             if (navigationContext.Parameters["Job"] is JobModel job)
             {
                 this.Job = job;
             }
			
             this.LoadInformation();
        }
		
		public void LoadInformation()
		{
			this.TranfersJob(JobInformationRegions.HJobInformation,nameof(HorizontalJobInfoView));
			this.TranfersJob(JobInformationRegions.MainWindInforRegion,nameof(WindCategoryView));
			this.TranfersJob(JobInformationRegions.MainRoofInfoRegion,nameof(RoofInfoView));
			if (this.Job.Info.ClientName == "Prenail")
			{
				this.TranfersJob(JobInformationRegions.MainDesignRegion,nameof(DesignInfoView));
			}
			
			this.TranferFloorInfo(JobInformationRegions.MainFloorChooseRegion,nameof(LevelInfoView));

		}
		private void TranfersJob(string regionName, string uriPath)
		{
            var parameters = new NavigationParameters {{"Job", Job.Info}};
            if (Job!=null)
            {
                this.RegionManager.RequestNavigate(regionName,uriPath,parameters);   
            }
		}
		
		private void TranferFloorInfo(string regionName, string uriPath)
		{
			var parameters = new NavigationParameters {{"Job", Job.Info}};
			parameters.Add("Levels", Job.Levels);
            if (Job.Info!=null && Job.Levels != null)
            {
                this.RegionManager.RequestNavigate(regionName,uriPath,parameters);   
            }
		}
		
		public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
		
		#endregion
    }
}
