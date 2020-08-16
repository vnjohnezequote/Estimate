using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ApplicationCore.BaseModule;
using ApplicationInterfaceCore;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.EngineerMember;
using JobInfoModule.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Unity;

namespace JobInfoModule.ViewModels
{
    public class EngineerInfoViewModel: BaseJobInForViewModel
    {
        #region Field



        #endregion

        #region Properties



        #endregion

        #region ICommand
        public ICommand CreateNewEngineerMemberCommand{ get; private set; }
        

        #endregion
        public ObservableCollection<string> LevelTypes { get; } = new ObservableCollection<string>() { "Global" };
        public ObservableCollection<string> TimberGradeList { get; } = new ObservableCollection<string>(){"LVL","MGP10","MGP12","17C","F17","F27","Steel"};
        

        public EngineerInfoViewModel(IUnityContainer unityContainer, IRegionManager regionManager,
            IEventAggregator eventAggregator, ILayerManager layerManager, IJob jobModel) : base(unityContainer,
            regionManager, eventAggregator, layerManager, jobModel)
        {
            JobModel.Levels.CollectionChanged += Levels_CollectionChanged;
            CreateNewEngineerMemberCommand = new DelegateCommand(OnCreateNewEngineerMemberCommand);
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

        private void OnCreateNewEngineerMemberCommand()
        {
            var member = new EngineerMemberInfo(JobModel.Info)
            {
                LevelType =  "Global",
                MemberType = WallMemberType.Lintel,
                MaterialType = MaterialTypes.Timber
            };
            JobModel.EngineerMemberList.Add(member);
        }
        
    }
}
