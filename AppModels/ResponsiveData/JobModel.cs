// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the JobModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.PocoDataModel.EngineerMember;
using AppModels.PocoDataModel.Openings;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Framings;
using AppModels.ResponsiveData.Openings;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The job model.
    /// </summary>
    /// 
    public class JobModel : BindableBase,IJob
    {
        #region Field
        /// <summary>
        /// The inFor.
        /// </summary>

        private JobInfo _inFor;

        private bool _currentIsLoadBearingWall;
        private bool _cCMode;
        private int _selectedWallThickness;
        private int _defaultJoistSpacing;
        /// <summary>
        /// The levels.
        /// </summary>
        private ObservableCollection<LevelWall> _levels;
        private FramingSheet _activeFloorSheet;
        private TimberBase _selectedJoistMaterial;
        #region Property

        #region MyRegion
        public FramingSheet ActiveFloorSheet
        {
            get => _activeFloorSheet;
            set
            {
                SetProperty(ref _activeFloorSheet, value);
            }
        }
        public bool CurrentIsLoadBearingWall { get => _currentIsLoadBearingWall; set => SetProperty(ref _currentIsLoadBearingWall, value); }
        public int SelectedWallThickness { get => _selectedWallThickness; set => SetProperty(ref _selectedWallThickness, value); }
        #endregion
        public bool CCMode { get => _cCMode; set => SetProperty(ref _cCMode, value); }
        public int DefaultJoistSpacing
        {
            get => _defaultJoistSpacing == 0 ? 450 : _defaultJoistSpacing;
            set=>SetProperty(ref _defaultJoistSpacing,value);
        }

        #endregion
        
        #region Save Properties

        public JobInfo Info
        {
            get => this._inFor;
            set => this.SetProperty(ref this._inFor, value);
        }
        public GlobalWallInfo GlobalWallInfo { get; set; }
        public ObservableCollection<LevelWall> Levels { get => this._levels; set => this.SetProperty(ref this._levels, value); }
        public MyObservableCollection<EngineerMemberInfo> EngineerMemberList { get; } = new MyObservableCollection<EngineerMemberInfo>();
        public ObservableCollection<OpeningInfo> DoorSchedules { get; } = new MyObservableCollection<OpeningInfo>();
        #endregion
        /// <summary>
        /// Gets or sets the info.
        /// </summary>

        public TimberBase SelectedJoitsMaterial
        {
            get => this._selectedJoistMaterial;
            set => SetProperty(ref _selectedJoistMaterial, value);
        }
        
        #endregion
        /// <summary>
        /// Initializes a new instance of the <see cref="JobModel"/> class.
        /// </summary>
        public JobModel()
        {
            this.Info = new JobInfo(this);
            GlobalWallInfo = new GlobalWallInfo(Info);
            this.Levels = new ObservableCollection<LevelWall>();
            ActiveFloorSheet = null;
        }

        public void LoadJob(JobModelPoco jobLoaded,List<ClientPoco> clients)
        {
            Info.LoadJobInfo(jobLoaded.Info,clients);
            GlobalWallInfo.LoadWallGlobalInfo(jobLoaded.GlobalWallInfo);
            LoadEngineerList(jobLoaded.EngineerMemberList,Info.Client.Beams);
            LoadDoorSchedules(jobLoaded.DoorSchedules);
            if (Info.Client !=null)
            {
                LoadLevel(jobLoaded.Levels,Info.Client);    
            }
            
        }
        private void LoadLevel(List<LevelWallPoco> levels,ClientPoco client )
        {
            foreach (var levelWallPoco in levels)
            {
                var level = new LevelWall(GlobalWallInfo);
                level.LoadLevelInfo(levelWallPoco,client);
                Levels.Add(level);
            }
        }
        private void LoadDoorSchedules(List<OpeningInfoPoco> doorSchedulePocos)
        {
            foreach (var doorSchedulePoco in doorSchedulePocos)
            {
                var doorSchedule = new OpeningInfo(GlobalWallInfo);
                doorSchedule.LoadOpeningInfo(doorSchedulePoco);
                DoorSchedules.Add(doorSchedule);
            }

        }
        private void LoadEngineerList(List<EngineerMemberInfoPoco> engineerList,Dictionary<string,List<TimberBase>> timberInfos)
        {
            foreach (var memberInfo  in engineerList)
            {
                var member = new EngineerMemberInfo(Info);
                member.LoadMemberInfo(memberInfo,timberInfos);
                EngineerMemberList.Add(member);
            }
        }

    }
}
