// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the JobModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AppModels.Interaface;
using ProtoBuf;

namespace AppModels
{
    using System.Collections.ObjectModel;

    using Prism.Mvvm;

    /// <summary>
    /// The job model.
    /// </summary>
    [ProtoContract]
    public class JobModel : BindableBase,IJob
    {
        /// <summary>
        /// The inFor.
        /// </summary>
        private JobInfo _inFor;

        /// <summary>
        /// The levels.
        /// </summary>
        private ObservableCollection<LevelWall> _levels;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobModel"/> class.
        /// </summary>
        public JobModel()
        {
            this.Info = new JobInfo();
            this.Levels = new ObservableCollection<LevelWall>();
        }

        /// <summary>
        /// Gets or sets the info.
        /// </summary>
        [ProtoMember(1)]
        public JobInfo Info
        {
            get => this._inFor;
            set => this.SetProperty(ref this._inFor, value);
        }

        /// <summary>
        /// Gets or sets the levels.
        /// </summary>
        [ProtoMember(1)]
        public ObservableCollection<LevelWall> Levels { get => this._levels; set => this.SetProperty(ref this._levels, value); }

    }
}
