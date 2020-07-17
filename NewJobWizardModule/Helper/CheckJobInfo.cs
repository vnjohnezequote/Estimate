// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckJobInfo.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the CheckJobInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AppModels.ResponsiveData;

namespace NewJobWizardModule.Helper
{
    using Prism.Mvvm;

    /// <summary>
    /// The check job info.
    /// </summary>
    public class CheckJobInfo : BindableBase
    {
        /// <summary>
        /// The _can create job.
        /// </summary>
        private bool _canCreateJob;

        /// <summary>
        /// The inFor.
        /// </summary>
        private JobInfo _inFor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckJobInfo"/> class.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        public CheckJobInfo(JobInfo info)
        {
            if (info != null)
            {
                this.Info = info;
            }


            this.Info.PropertyChanged += this.JobInfoPropertyChanged;
        }

        /// <summary>
        /// Gets or sets a value indicating whether can create job.
        /// </summary>
        public bool CanCreateJob
        {
            get
            {
                if (this.Info == null)
                {
                    return false;
                }

                return !(string.IsNullOrEmpty(this.Info.JobLocation) || string.IsNullOrEmpty(this.Info.JobNumber) || string.IsNullOrEmpty(this.Info.ClientName));
            }

            set => this.SetProperty(ref this._canCreateJob, value);
        }

        /// <summary>
        /// Gets or sets the info.
        /// </summary>
        public JobInfo Info
        {
            get => this._inFor;
            set
            {
                this.SetProperty(ref this._inFor, value);
                this.RaisePropertyChanged(nameof(this.CanCreateJob));
            }
        }

        /// <summary>
        /// The job info property changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void JobInfoPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.Info.JobLocation) || e.PropertyName == nameof(this.Info.JobNumber)
                                                                || e.PropertyName == nameof(this.Info.ClientName))
            {
                this.RaisePropertyChanged(nameof(this.CanCreateJob));
            }
                
        }
    }
}
