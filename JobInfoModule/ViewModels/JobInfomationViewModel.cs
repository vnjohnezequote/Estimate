
using System.Windows;
using ApplicationCore.BaseModule;
using ApplicationInterfaceCore;
using ApplicationService;
using AppModels;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.ResponsiveData;
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
        private ClientPoco _selectedClient;
		
		#endregion
		
		#region Constructor
		public JobInfomationViewModel()
		{}
		
		 public JobInfomationViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager,IJob jobModel)
            : base(unityContainer, regionManager, eventAggregator, layerManager,jobModel)
        {
			this.EventAggregator.GetEvent<CustomerService>().Subscribe(this.ChangeClient);
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

        public ClientPoco SelectedClient
        {
            get => this._selectedClient;
            set => this.SetProperty(ref _selectedClient, value);
        }
		#endregion
		
		#region private function

        /// <summary>
        /// The Change ClientPoco
        /// </summary>
        /// <param name="selectClient"></param>
        private void ChangeClient(ClientPoco selectClient)
        {
            if (selectClient ==null)
            {
                this.IsDesignVisibility = Visibility.Collapsed;
                return;
            }
            this.IsDesignVisibility = selectClient.Name == "Prenail" ? Visibility.Visible : Visibility.Collapsed;
			//this.LoadInformation();
        }
		
		public void OnNavigatedTo(NavigationContext navigationContext)
        {
             //if (navigationContext.Parameters["JobInfo"] is JobModel job)
             //{
             //    this.JobInfo = job;
             //}
			
             //this.LoadInformation();
        }
		
		public void LoadInformation()
		{
			//this.TranfersJob(JobInformationRegions.HJobInformation,nameof(HorizontalJobInfoView));
			//this.TranfersJob(JobInformationRegions.MainWindInforRegion,nameof(WindCategoryView));
			//this.TranfersJob(JobInformationRegions.MainRoofInfoRegion,nameof(RoofInfoView));
			//if (this.JobInfo.Info.ClientName == "Prenail")
			//{
			//	this.TranfersJob(JobInformationRegions.MainDesignRegion,nameof(DesignInfoView));
			//}
			
			//this.TranferFloorInfo(JobInformationRegions.MainFloorChooseRegion,nameof(LevelInfoView));

		}
		private void TranfersJob(string regionName, string uriPath)
		{
            var parameters = new NavigationParameters {{"JobInfo", JobModel.Info}};
            if (Job!=null)
            {
                this.RegionManager.RequestNavigate(regionName,uriPath,parameters);   
            }
		}
		
		private void TranferFloorInfo(string regionName, string uriPath)
		{
			var parameters = new NavigationParameters {{"JobInfo", JobModel.Info}};
			parameters.Add("Levels", JobModel.Levels);
            if (Job.Info!=null && JobModel.Levels != null)
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
