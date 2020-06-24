// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelWallDefaultInfo.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the LevelWallDefaulInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



namespace AppModels
{
    using System.Runtime.CompilerServices;

    using Prism.Mvvm;

    /// <summary>
    /// The level wall default info.
    /// </summary>
    public class LevelWallDefaultInfo : BindableBase
    {
        #region Private Field

        /// <summary>
        /// The external wall height.
        /// </summary>
        private int? externalWallHeight;

        /// <summary>
        /// The internal wall height.
        /// </summary>
        private int? internalWallHeight;

        /// <summary>
        /// The external wall thickness.
        /// </summary>
        private int? externalWallThickness;

        /// <summary>
        /// The internal wall thickness.
        /// </summary>
        private int? internalWallThickness;

        /// <summary>
        /// The step down.
        /// </summary>
        private IntegerDimension stepDown;

        /// <summary>
        /// The roof pitch.
        /// </summary>
        private DoubleDimension roofPitch;

        /// <summary>
        /// The job info.
        /// </summary>
        private JobWallDefaultInfo jobInfo;

        /// <summary>
        /// The external wall spacing.
        /// </summary>
        private IntegerDimension externalWallSpacing;

        /// <summary>
        /// The internal wall spacing.
        /// </summary>
        private IntegerDimension internalWallSpacing;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelWallDefaultInfo"/> class.
        /// </summary>
        /// <param name="defaultInfo">
        /// The default info.
        /// </param>
        public LevelWallDefaultInfo(JobWallDefaultInfo defaultInfo)
        {
            this.JobInfo = defaultInfo;
            this.InitializeLevelDefault();
        }
        

        #endregion
        #region public Property

        /// <summary>
        /// Gets or sets the external wall height.
        /// </summary>
        public int? ExternalWallHeight
        {
            get => this.externalWallHeight;
            set => this.SetProperty(ref this.externalWallHeight, value);
        }

        /// <summary>
        /// Gets or sets the internal wall height.
        /// </summary>
        public int? InternalWallHeight
        {
            get => this.internalWallHeight;
            set => this.SetProperty(ref this.internalWallHeight, value);
        }

        /// <summary>
        /// Gets or sets the external wall thickness.
        /// </summary>
        public int? ExternalWallThickness
        {
            get => this.externalWallThickness;
            set => this.SetProperty(ref this.externalWallThickness, value);
        }

        /// <summary>
        /// Gets or sets the internal wall thickness.
        /// </summary>
        public int? InternalWallThickness
        {
            get => this.internalWallThickness;
            set => this.SetProperty(ref this.internalWallThickness, value);
        }

        /// <summary>
        /// Gets or sets the stepdown.
        /// </summary>
        public IntegerDimension StepDown
        {
            get => this.stepDown;
            set => this.SetProperty(ref this.stepDown, value);
        }

        /// <summary>
        /// Gets or sets the roof pitch.
        /// </summary>
        public DoubleDimension RoofPitch
        {
            get => this.roofPitch;
            set => this.SetProperty(ref this.roofPitch, value);
        }

        /// <summary>
        /// Gets or sets the job info.
        /// </summary>
        public JobWallDefaultInfo JobInfo
        {
            get => this.jobInfo;
            set => this.SetProperty(ref this.jobInfo, value);
        }

        /// <summary>
        /// Gets or sets the external wall spacing.
        /// </summary>
        public IntegerDimension ExternalWallSpacing
        {
            get => this.externalWallSpacing;
            set => this.SetProperty(ref this.externalWallSpacing, value);
        }

        /// <summary>
        /// Gets or sets the internal wall spacing.
        /// </summary>
        public IntegerDimension InternalWallSpacing
        {
            get => this.internalWallSpacing;
            set => this.SetProperty(ref this.internalWallSpacing, value);
        }

        #endregion

        #region private Method

        /// <summary>
        /// The initialize level default.
        /// </summary>
        private void InitializeLevelDefault()
        {
            this.StepDown = new IntegerDimension(this.JobInfo.StepDown);
            this.RoofPitch = this.JobInfo.RoofPitch == null ? new DoubleDimension() : new DoubleDimension(this.JobInfo.RoofPitch);

            this.ExternalWallSpacing = this.JobInfo.ExternalWallSpacing == null
                                           ? new IntegerDimension()
                                           : new IntegerDimension(this.JobInfo.ExternalWallSpacing);
            this.InternalWallSpacing = this.JobInfo.InternalWallSpacing == null 
                                           ? new IntegerDimension() 
                                           : new IntegerDimension(this.JobInfo.InternalWallSpacing);

            this.JobInfo.PropertyChanged += this.JobInfoPropertyChanged;
            this.StepDown.PropertyChanged += this.StepDownPropertyChanged;
            this.RoofPitch.PropertyChanged += this.RoofPitchPropertyChanged;
            this.ExternalWallSpacing.PropertyChanged += this.ExternalWallSpacingPropertyChanged;
            this.InternalWallSpacing.PropertyChanged += this.InternalWallSpacingPropertyChanged;
        }

        /// <summary>
        /// The internal wall spacing property changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void InternalWallSpacingPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Size")
            {
                return;
            }

            this.InternalWallSpacing.IsDefaultValue = this.InternalWallSpacing.Size == this.JobInfo.InternalWallSpacing;
            this.RaisePropertyChanged(nameof(this.InternalWallSpacing));

        }

        /// <summary>
        /// The external wall spacing property changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ExternalWallSpacingPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Size")
            {
                return;
            }

            this.ExternalWallSpacing.IsDefaultValue = this.ExternalWallSpacing.Size == this.JobInfo.ExternalWallSpacing;
            this.RaisePropertyChanged(nameof(this.ExternalWallSpacing));
        }

        /// <summary>
        /// The roof pitch property changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void RoofPitchPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Size")
            {
                return;
            }
            this.RoofPitch.IsDefaultValue = this.RoofPitch.Size.Equals(this.JobInfo.RoofPitch);
            this.RaisePropertyChanged(nameof(this.RoofPitch));
        }

        /// <summary>
        /// The step down property changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void StepDownPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Size")
            {
                return;
            }
            this.StepDown.IsDefaultValue = this.StepDown.Size == this.JobInfo.StepDown;
            this.RaisePropertyChanged(nameof(this.StepDown));
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
            switch (e.PropertyName)
            {
                case nameof(this.JobInfo.StepDown):
                    if (this.StepDown.IsDefaultValue)
                    {
                        this.StepDown.Size = this.JobInfo.StepDown;
                    }
                    break;
                case nameof(this.JobInfo.RoofPitch):
                    
                    if (this.RoofPitch.IsDefaultValue)
                    {
                        var jobInfoRoofPitch = this.JobInfo.RoofPitch;
                        if (jobInfoRoofPitch != null)
                        {
                            this.RoofPitch.Size = (double)jobInfoRoofPitch;
                        }
                    }
                    

                    break;
                case nameof(this.JobInfo.ExternalWallSpacing):
                    if (this.ExternalWallSpacing.IsDefaultValue)
                    {
                        var jobInfoExternalWallSpacing = this.JobInfo.ExternalWallSpacing;
                        if (jobInfoExternalWallSpacing != null)
                        {
                            this.ExternalWallSpacing.Size = (int)jobInfoExternalWallSpacing;
                        }
                    }

                    break;
                case nameof(this.JobInfo.InternalWallSpacing):
                    if (this.InternalWallSpacing.IsDefaultValue)
                    {
                        var jobInfoInternalWallSpacing = this.JobInfo.InternalWallSpacing;
                        if (jobInfoInternalWallSpacing != null)
                        {
                            this.InternalWallSpacing.Size = (int)jobInfoInternalWallSpacing;
                        }
                    }

                    break;
                default:
                    break;
                
            }
        }



        #endregion

    }
}
