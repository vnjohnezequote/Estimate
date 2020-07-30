using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDataBase.DataBase;
using ApplicationInterfaceCore;
using ApplicationService;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.ResponsiveData;
using Prism.Events;
using Prism.Regions;
using Unity;

namespace ApplicationCore.BaseModule
{
    public abstract class BaseJobInForViewModel: BaseViewModel
    {
        private JobInfo _jobInfo;
        public JobInfo JobInfo { get => _jobInfo; set => SetProperty(ref _jobInfo, value); }
        private ClientPoco _selectedClient;
        public ClientPoco SelectedClient { get => _selectedClient; set => SetProperty(ref _selectedClient, value); }

        protected BaseJobInForViewModel(): base()
        {

        }
        protected BaseJobInForViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager, IJob jobModel)
            : base(unityContainer, regionManager, eventAggregator, layerManager, jobModel)
        {
            this.JobInfo = jobModel.Info;
            this.Initilazied();
            this.EventAggregator.GetEvent<CustomerService>().Subscribe(this.SelectedClientReceive);
        }

        protected virtual void Initilazied()
        {
            var db = this.UnityContainer.Resolve<ClientDataBase>();
            if (string.IsNullOrEmpty(this.JobInfo.ClientName))
            {
                return;
            }
            this.SelectedClient = db.GetClient(this.JobInfo.ClientName);
        }
        protected virtual void SelectedClientReceive(ClientPoco selectClient)
        {
            if (selectClient == null)
            {
                return;
            }
            this.SelectedClient = selectClient;
        }
    }
}
