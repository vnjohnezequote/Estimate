using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.BaseModule;
using ApplicationInterfaceCore;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.EngineerMember;
using JobInfoModule.Views;
using Prism.Events;
using Prism.Regions;
using Unity;

namespace JobInfoModule.ViewModels
{
    public class EngineerInfoViewModel: BaseJobInForViewModel
    {
        public ObservableCollection<string> LevelTypes { get; } = new ObservableCollection<string>() { "Global" };
        public ObservableCollection<string> TimberGrade { get; } = new ObservableCollection<string>(){"LVL","MGP10","MGP12","GL17C","F17","F27"};

        public ObservableCollection<EngineerMemberInfo> EngineerMemberList { get; } = new ObservableCollection<EngineerMemberInfo>();
        public EngineerInfoViewModel(IUnityContainer unityContainer, IRegionManager regionManager,
            IEventAggregator eventAggregator, ILayerManager layerManager, IJob jobModel) : base(unityContainer,
            regionManager, eventAggregator, layerManager, jobModel)
        {
            JobModel.Levels.CollectionChanged += Levels_CollectionChanged;
            JobModel.Info.PropertyChanged += Info_PropertyChanged;
        }

        private void Info_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName =="Supplier")
            {
                RenameForLVLBeam(JobInfo.Supplier);
            }
        }

        private void Levels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var jobModelLevel in JobModel.Levels)
            {
                if (!LevelTypes.Contains(jobModelLevel.LevelName))
                {
                    LevelTypes.Add(jobModelLevel.LevelName);
                }
            }
        }

        public void RenameForLVLBeam(Suppliers suplier)
        {
           
            //foreach (var engineerMemberInfo in EngineerMemberList)
            //{
            //    if (engineerMemberInfo.TimberGrade == "LVL")
            //    {
            //        if (suplier == Suppliers.TILLINGS)
            //        {
            //            engineerMemberInfo.TimberGrade = "LVL15";
            //        }

            //    }
            //}
        }
    }
}
