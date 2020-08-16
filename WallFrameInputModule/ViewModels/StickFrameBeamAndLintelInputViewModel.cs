using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ApplicationCore.BaseModule;
using ApplicationInterfaceCore;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Openings;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Unity;

namespace WallFrameInputModule.ViewModels
{
    public class StickFrameBeamAndLintelInputViewModel : BaseViewModel
    {
        #region Field

        private int _startBeamId;
        private int _startDoorId;
        private Beam _selectedBeam;
        private LevelWall _levelInfo;
        private bool _engineerReferenceVisibility;
        private Opening _selectedOpenting;
        #endregion

        #region Properties
        public LevelWall LevelInfo { get=>_levelInfo;
            set => SetProperty(ref _levelInfo, value);
        }
        public ObservableCollection<Beam> Beams { get; } = new ObservableCollection<Beam>();
        public ObservableCollection<OpeningInfo> DoorSchedules { get; } = new MyObservableCollection<OpeningInfo>();
        public ObservableCollection<EngineerMemberInfo> EngineerList { get; }
        public ObservableCollection<Opening> DoorAndWindowList { get; }
        public ObservableCollection<string> TimberGradeList { get; } = new ObservableCollection<string>() { "LVL", "MGP10", "MGP12", "17C", "F17", "F27","Steel" };
        public ObservableCollection<LintelBeam> Lintels { get; }

        public Beam SelectedBeam
        {
            get=>_selectedBeam; 
            set=>SetProperty(ref _selectedBeam,value);
        }

        public Opening SelectedOpening
        {
            get=>_selectedOpenting; 
            set=>SetProperty(ref _selectedOpenting,value);
        }
        #endregion
        #region Command
        public ICommand CreateNewBeamCommand { get;private set; }
        public ICommand CreateNewOpeningCommand { get; private set; }
        public ICommand AutoMathBeamWithEngineerBeamList { get; private set; }
        public ICommand CreateNewDoorScheduleCommand { get; private set; }
        public ICommand AddSupportToBeam { get; private set; }
        public bool EngineerReferenceVisibility => JobModel.EngineerMemberList == null || JobModel.EngineerMemberList.Count == 0;

        #endregion
        public StickFrameBeamAndLintelInputViewModel(){}

        public StickFrameBeamAndLintelInputViewModel(IUnityContainer unityContainer, IRegionManager regionManager,
            IEventAggregator eventAggregator, ILayerManager layerManager, IJob jobModel) : base(unityContainer,
            regionManager, eventAggregator, layerManager, jobModel)
        {
            EngineerList = new ObservableCollection<EngineerMemberInfo>();
            DoorAndWindowList = new ObservableCollection<Opening>();
            Lintels = new ObservableCollection<LintelBeam>();
            CreateNewBeamCommand = new DelegateCommand(OnCreateBeamCommand);
            CreateNewOpeningCommand = new DelegateCommand(OnCreateNewDoorWindow);
            AutoMathBeamWithEngineerBeamList = new DelegateCommand(OnAutoMathBeamList);
            AddSupportToBeam = new DelegateCommand(OnAddSupportToBeam);
            CreateNewDoorScheduleCommand = new DelegateCommand(OnCreateNewDoorSchedule);
            JobModel.EngineerMemberList.CollectionChanged += EngineerMemberList_CollectionChanged;
        }

        private void OnCreateNewDoorWindow()
        {
            this._startDoorId = DoorAndWindowList.Count + 1;
            var door = new Opening(this.LevelInfo.LevelInfo) { Id = _startDoorId };
            door.PropertyChanged += Door_PropertyChanged;
            DoorAndWindowList.Add(door);
        }

        private void OnCreateNewDoorSchedule()
        {
            var door = new OpeningInfo();
            this.DoorSchedules.Add(door);
        }

        private void Door_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is Opening door)
            {
                if (e.PropertyName == "IsDoorUnderLbw")
                {
                    if (door.IsDoorUnderLbw && !Lintels.Contains(door.Lintel))
                    {
                        Lintels.Add(door.Lintel);
                        door.Lintel.Id = Lintels.IndexOf(door.Lintel)+1;
                    }
                    else if(!door.IsDoorUnderLbw && Lintels.Contains(door.Lintel))
                    {
                        Lintels.Remove(door.Lintel);
                        var i = 1;
                        foreach (var lintelBeam in Lintels)
                        {
                            door.Lintel.Id = i;
                            i++;
                        }
                        
                    }
                }
            }
            
        }

        public void InitEngineerList()
        {
            if (LevelInfo == null) return;
            foreach (var engineerMemberInfo in JobModel.EngineerMemberList)
            {

                if (EngineerList.Contains(engineerMemberInfo))
                {
                    continue;
                }
                if ((engineerMemberInfo.LevelType == this.LevelInfo.LevelName) || engineerMemberInfo.LevelType == "Global")
                {
                    EngineerList.Add(engineerMemberInfo);
                }
            }
        }

        private void EngineerMemberList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(EngineerReferenceVisibility));
            EngineerList.Clear();
           
            InitEngineerList();
            
            foreach (var beam in Beams)
            {
                beam.NotifyPropertyChanged();
            }
        }

        private void OnCreateBeamCommand()
        {
            this._startBeamId = Beams.Count + 1;
            var beam = new Beam(BeamType.RoofBeam,this.LevelInfo.LevelInfo){Id = _startBeamId};
            Beams.Add(beam);
        }

        private void OnAddSupportToBeam()
        {
            SelectedBeam.AddSupport();
        }

        private void OnAutoMathBeamList()
        {
            foreach (var beam in Beams)
            {
                foreach (var engineerBeam in EngineerList)
                {
                    if (beam.SpanLength<=0)
                    {
                        continue;
                    }

                    if (beam.SpanLength>engineerBeam.MinSpan && beam.SpanLength<=engineerBeam.MaxSpan)
                    {
                        beam.EngineerMemberInfo = engineerBeam;
                    }
                }
            }
        }
    }
}
