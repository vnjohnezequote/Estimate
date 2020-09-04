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
using System.Windows.Documents;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.PocoDataModel.Openings;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Openings;
using devDept.Geometry;
using Prism.Mvvm;
using ProtoBuf;

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

        /// <summary>
        /// The levels.
        /// </summary>
        private ObservableCollection<LevelWall> _levels;


        #endregion

        #region Property
        /// <summary>
        /// Gets or sets the info.
        /// </summary>
        public JobInfo Info
        {
            get => this._inFor;
            set => this.SetProperty(ref this._inFor, value);
        }
        public GlobalWallInfo GlobalWallInfo { get; set; }
        /// <summary>
        /// Gets or sets the levels.
        /// </summary>
        public ObservableCollection<LevelWall> Levels { get => this._levels; set => this.SetProperty(ref this._levels, value); }
        //public ObservableCollection<EngineerMemberInfo> EngineerMemberList { get; }
        public MyObservableCollection<EngineerMemberInfo> EngineerMemberList { get; } = new MyObservableCollection<EngineerMemberInfo>();
        public ObservableCollection<OpeningInfo> DoorSchedules { get; } = new MyObservableCollection<OpeningInfo>();
        #endregion
        /// <summary>
        /// Initializes a new instance of the <see cref="JobModel"/> class.
        /// </summary>
        public JobModel()
        {
            this.Info = new JobInfo(this);
            GlobalWallInfo = new GlobalWallInfo(Info);
            this.Levels = new ObservableCollection<LevelWall>();
        }

        public void LoadJob(JobModelPoco jobLoaded,List<ClientPoco> clients)
        {
            Info.LoadJobInfo(jobLoaded.Info,clients);
            GlobalWallInfo.LoadWallGlobalInfo(jobLoaded.GlobalWallInfo);
            LoadEngineerList(jobLoaded.EngineerMemberList,Info.Client.Beams);
            LoadDoorSchedules(jobLoaded.DoorSchedules);
            LoadLevel(jobLoaded.Levels);
        }

        private void LoadLevel(List<LevelWallPoco> levels )
        {
            foreach (var levelWallPoco in levels)
            {
                var level = new LevelWall(GlobalWallInfo);
                level.LoadLevelInfo(levelWallPoco);
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
