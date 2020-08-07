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
using AppModels.ResponsiveData.EngineerMember;
using devDept.Geometry;
using Prism.Mvvm;
using ProtoBuf;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The job model.
    /// </summary>
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
        #endregion
        /// <summary>
        /// Initializes a new instance of the <see cref="JobModel"/> class.
        /// </summary>
        public JobModel()
        {
            this.Info = new JobInfo();
            GlobalWallInfo = new GlobalWallInfo(Info);
            //EngineerMemberList = new ObservableCollection<EngineerMemberInfo>();
            this.Levels = new ObservableCollection<LevelWall>();
        }

        public void LoadJob(JobModelPoco jobLoaded)
        {
            Info.LoadJobInfo(jobLoaded.Info);
            GlobalWallInfo.LoadWallGlobalInfo(jobLoaded.GlobalWallInfo);
            LoadLevel(jobLoaded.Levels);
        }

        public void LoadLevel(List<LevelWallPoco> levels )
        {
            foreach (var levelWallPoco in levels)
            {
                var level = new LevelWall(GlobalWallInfo);
                level.LoadLevelInfo(levelWallPoco);
                Levels.Add(level);
            }

        }

    }
}
