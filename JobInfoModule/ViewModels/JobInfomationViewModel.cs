
using System;
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
    public class JobInfomationViewModel : BaseViewModel,INavigationAware
    {
		#region private member
        private Visibility _isDesignVisibility;
		//private JobModel _job;
        private ClientPoco _selectedClient;
		
		#endregion
		
		#region Constructor
		public JobInfomationViewModel()
		{}
		
		 public JobInfomationViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager,IJob jobModel)
            : base(unityContainer, regionManager, eventAggregator, layerManager,jobModel)
        {
            JobModel.Info.PropertyChanged += Info_PropertyChanged;
            //this.LoadInformation();
			//this.EventAggregator.GetEvent<CustomerService>().Subscribe(this.ChangeClient);
        }

        private void Info_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ClientName")
            {
                RaisePropertyChanged(nameof(IsDesignVisibility));
            }
        }
        #endregion

        #region Public Member
        /// <summary>
        /// Gets or sets the is design visibility.
        /// </summary>
        public Visibility IsDesignVisibility
        {
            get
            {
                if (string.IsNullOrEmpty(JobModel.Info.ClientName) || JobModel.Info.ClientName != "Prenail")
                    return Visibility.Collapsed;
                return Visibility.Visible;
            }

            
        }

        //public JobModel Job
        //{
        //    get => this._job;
        //    set => this.SetProperty(ref this._job, value);
        //}

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
            //this.LoadInformation();
        }
		
		public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //if (navigationContext.Parameters["JobModel"] is JobModel job)
            //{
            //    this.Job = job;
            //}

            this.LoadInformation();
        }
		
		public void LoadInformation()
		{
			this.TranfersJob(JobInformationRegions.HJobInformation,nameof(JobInfoView));
			this.TranfersJob(JobInformationRegions.MainWindInforRegion,nameof(WindCategoryView));
			this.TranfersJob(JobInformationRegions.MainRoofInfoRegion,nameof(RoofInfoView));
			//if (this.JobModel.Info.ClientName == "Prenail")
			//{
			this.TranfersJob(JobInformationRegions.MainDesignRegion,nameof(DesignInfoView));
			//}
			
			this.TranfersFloorInfo(JobInformationRegions.MainFloorChooseRegion,nameof(LevelInfoView));

		}
		private void TranfersJob(string regionName, string uriPath)
		{
            //var parameters = new NavigationParameters {{"JobDefaultInfo", JobModel.Info}};
            //if (Job!=null)
            //{
            //    this.RegionManager.RequestNavigate(regionName,uriPath,parameters);   
            //}
            this.RegionManager.RequestNavigate(regionName, uriPath);
        }
		
		private void TranfersFloorInfo(string regionName, string uriPath)
		{
            //var parameters = new NavigationParameters {{"JobDefaultInfo", JobModel.Info}};
            //parameters.Add("Levels", JobModel.Levels);
            //         if (Job.Info!=null && JobModel.Levels != null)
            //         {
            //             this.RegionManager.RequestNavigate(regionName,uriPath,parameters);   
            //         }
            var parameters = new NavigationParameters
            {
                { "JobDefaultInfo", this.JobModel.Info },
                { "Levels", this.JobModel.Levels }
            };
            if (this.JobModel.Info != null && this.JobModel.Levels != null)
            {
                this.RegionManager.RequestNavigate(regionName, uriPath, parameters);
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
