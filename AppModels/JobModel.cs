// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the JobModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AppModels
{
    using System.Collections.ObjectModel;

    using Prism.Mvvm;

    /// <summary>
    /// The job model.
    /// </summary>
    public class JobModel : BindableBase
    {
        /// <summary>
        /// The inFor.
        /// </summary>
        private JobInfo inFor;

        /// <summary>
        /// The levels.
        /// </summary>
        private ObservableCollection<LevelWall> levels;

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
        public JobInfo Info
        {
            get => this.inFor;
            set => this.SetProperty(ref this.inFor, value);
        }

        /// <summary>
        /// Gets or sets the levels.
        /// </summary>
        public ObservableCollection<LevelWall> Levels { get => this.levels; set => this.SetProperty(ref this.levels, value); }

    }
}
