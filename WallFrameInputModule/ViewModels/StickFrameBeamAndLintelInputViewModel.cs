using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
using Syncfusion.UI.Xaml.Grid;
using Unity;

namespace WallFrameInputModule.ViewModels
{
    public class StickFrameBeamAndLintelInputViewModel : BaseFloorViewModelAware
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
            set
            {
                SetProperty(ref _levelInfo, value);
                if (LevelInfo!=null)
                {
                    if (LevelInfo.Openings!=null)
                    {
                        LevelInfo.Openings.CollectionChanged += Openings_CollectionChanged;
                    }
                }
            } 
        }

        //public ObservableCollection<Beam> Beams { get; } = new ObservableCollection<Beam>();
        //public ObservableCollection<OpeningInfo> DoorSchedules { get; } = new MyObservableCollection<OpeningInfo>();
        public ObservableCollection<EngineerMemberInfo> EngineerList { get; }
        //public ObservableCollection<Opening> DoorAndWindowList { get; }
        public ObservableCollection<string> TimberGradeList { get; } = new ObservableCollection<string>() { "LVL", "MGP10", "MGP12", "17C", "F17", "F27","Steel" };

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
        public ICommand DeleteDoorShedulesRowCommand { get; private set; }
        public ICommand DeleteOpeningRowCommand { get; private set; }
        public bool EngineerReferenceVisibility => JobModel.EngineerMemberList == null || JobModel.EngineerMemberList.Count == 0;

        #endregion
        public StickFrameBeamAndLintelInputViewModel(){}

        public StickFrameBeamAndLintelInputViewModel(IUnityContainer unityContainer, IRegionManager regionManager,
            IEventAggregator eventAggregator, ILayerManager layerManager, IJob jobModel) : base(unityContainer,
            regionManager, eventAggregator, layerManager, jobModel)
        {
            EngineerList = new ObservableCollection<EngineerMemberInfo>();
            //DoorAndWindowList = new ObservableCollection<Opening>();
            CreateNewBeamCommand = new DelegateCommand(OnCreateBeamCommand);
            CreateNewOpeningCommand = new DelegateCommand(OnCreateNewDoorWindow);
            AutoMathBeamWithEngineerBeamList = new DelegateCommand(OnAutoMathBeamList);
            AddSupportToBeam = new DelegateCommand(OnAddSupportToBeam);
            CreateNewDoorScheduleCommand = new DelegateCommand(OnCreateNewDoorSchedule);
            DeleteDoorShedulesRowCommand = new DelegateCommand<SfDataGrid>(OnDeletedDoorSchedulesItem);
            DeleteOpeningRowCommand = new DelegateCommand<SfDataGrid>(OnDeletedOpening);
            JobModel.EngineerMemberList.CollectionChanged += EngineerMemberList_CollectionChanged;
            
        }

        private void Openings_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var newItem in e.NewItems)
                {
                    if (newItem is Opening newDoor)
                    {
                        
                        
                    }
                }
            }
        }

        private void OnDeletedOpening(SfDataGrid openingGrid)
        {
            var recordId = openingGrid.SelectedIndex;
            if (recordId < 0)
            {
                return;
            }

            if (LevelInfo.Lintels.Contains(LevelInfo.Openings[recordId].Lintel))
            {
                LevelInfo.Lintels.Remove(LevelInfo.Openings[recordId].Lintel);
            }
            LevelInfo.Openings.RemoveAt(recordId);
        }
        private void OnDeletedDoorSchedulesItem(SfDataGrid doorSchedulesGrid)
        {
            var recordId = doorSchedulesGrid.SelectedIndex;
            if (recordId < 0)
            {
                return;
            }
            this.JobModel.DoorSchedules.RemoveAt(recordId);
        }
        private void OnCreateNewDoorWindow()
        {
            this._startDoorId = LevelInfo.Openings.Count + 1;
            var door = new Opening(this.LevelInfo.LevelInfo) { Id = _startDoorId };
            LevelInfo.AddOpening(door);
            //LevelInfo.LintelCollectionChangedEvent += LevelInfo_LintelCollectionChangedEvent;
            //door.PropertyChanged += Door_PropertyChanged;
            //LevelInfo.Openings.Add(door);
        }

        //private void LevelInfo_LintelCollectionChangedEvent(object sender, EventArgs e)
        //{
        //    if (!(sender is Opening door)) return;
        //    if (door.IsDoorUnderLbw && !Lintels.Contains(door.Lintel))
        //    {
        //        Lintels.Add(door.Lintel);
        //        door.Lintel.Index = Lintels.IndexOf(door.Lintel) + 1;
        //    }
        //    else if (!door.IsDoorUnderLbw && Lintels.Contains(door.Lintel))
        //    {
        //        Lintels.Remove(door.Lintel);
        //        var i = 1;
        //        foreach (var lintelBeam in Lintels)
        //        {
        //            door.Lintel.Index = i;
        //            i++;
        //        }

        //    }
        //}

        private void OnCreateNewDoorSchedule()
        {
            var door = new OpeningInfo(this.LevelInfo.LevelInfo);
            door.Id = JobModel.DoorSchedules.Count;
            this.JobModel.DoorSchedules.Add(door);
        }

        //private void Door_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (!(sender is Opening door)) return;
        //    if (e.PropertyName != "IsDoorUnderLbw") return;
        //    if (door.IsDoorUnderLbw && !Lintels.Contains(door.Lintel))
        //    {
        //        Lintels.Add(door.Lintel);
        //        door.Lintel.Index = Lintels.IndexOf(door.Lintel)+1;
        //    }
        //    else if(!door.IsDoorUnderLbw && Lintels.Contains(door.Lintel))
        //    {
        //        Lintels.Remove(door.Lintel);
        //        var i = 1;
        //        foreach (var lintelBeam in Lintels)
        //        {
        //            door.Lintel.Index = i;
        //            i++;
        //        }
                        
        //    }

        //}

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
            
            foreach (var beam in LevelInfo.RoofBeams)
            {
                ((Beam)beam).NotifyPropertyChanged();
            }
        }

        private void OnCreateBeamCommand()
        {
            this._startBeamId = LevelInfo.RoofBeams.Count + 1;
            //var beam = new Beam(FramingTypes.TrussBeam,this.LevelInfo.LevelInfo){Index = _startBeamId};
            //LevelInfo.RoofBeams.Add(beam);
        }

        private void OnAddSupportToBeam()
        {
            SelectedBeam.AddSupport();
        }

        private void OnAutoMathBeamList()
        {
            foreach (var beam in LevelInfo.RoofBeams)
            {
                foreach (var engineerBeam in EngineerList)
                {
                    if (beam.FramingSpan<=0)
                    {
                        continue;
                    }

                    if (beam.FramingSpan>engineerBeam.MinSpan && beam.FramingSpan<=engineerBeam.MaxSpan)
                    {
                        beam.EngineerMember = engineerBeam;
                    }
                }
            }
        }
    }
}
