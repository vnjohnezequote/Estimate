// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the JobModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using AppModels.Interaface;
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
        /// <summary>
        /// Gets or sets the levels.
        /// </summary>
        public ObservableCollection<LevelWall> Levels { get => this._levels; set => this.SetProperty(ref this._levels, value); }
        #endregion
        /// <summary>
        /// Initializes a new instance of the <see cref="JobModel"/> class.
        /// </summary>
        public JobModel()
        {
            this.Info = new JobInfo();
            this.Levels = new ObservableCollection<LevelWall>();
        }

    }
}
