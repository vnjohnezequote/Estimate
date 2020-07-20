// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelWallDefaultInfo.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the LevelWallDefaulInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The level wall default info.
    /// </summary>
    public class LevelWallDefaultInfo : BindableBase
    {
        #region Private Field

        /// <summary>
        /// The external wall height.
        /// </summary>
        private int? _externalWallHeight;

        /// <summary>
        /// The internal wall height.
        /// </summary>
        private int? _internalWallHeight;

        /// <summary>
        /// The external wall thickness.
        /// </summary>
        private int? _externalWallThickness;

        /// <summary>
        /// The internal wall thickness.
        /// </summary>
        private int? _internalWallThickness;

        /// <summary>
        /// The step down.
        /// </summary>
        private IntegerDimension _stepDown;

        /// <summary>
        /// The roof pitch.
        /// </summary>
        private DoubleDimension _roofPitch;

        /// <summary>
        /// The job info.
        /// </summary>
        private JobWallDefaultInfo _jobDefaultInfo;

        /// <summary>
        /// The external wall spacing.
        /// </summary>
        private IntegerDimension _externalWallSpacing;

        /// <summary>
        /// The internal wall spacing.
        /// </summary>
        private IntegerDimension _internalWallSpacing;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelWallDefaultInfo"/> class.
        /// </summary>
        /// <param name="defaultDefaultInfo">
        /// The default info.
        /// </param>
        public LevelWallDefaultInfo(JobWallDefaultInfo defaultDefaultInfo)
        {
            this.JobDefaultInfo = defaultDefaultInfo;
            this.InitializeLevelDefault();
        }
        

        #endregion
        #region public Property

        /// <summary>
        /// Gets or sets the external wall height.
        /// </summary>
        public int? ExternalWallHeight
        {
            get => this._externalWallHeight;
            set => this.SetProperty(ref this._externalWallHeight, value);
        }

        /// <summary>
        /// Gets or sets the internal wall height.
        /// </summary>
        public int? InternalWallHeight
        {
            get => this._internalWallHeight;
            set => this.SetProperty(ref this._internalWallHeight, value);
        }

        /// <summary>
        /// Gets or sets the external wall thickness.
        /// </summary>
        public int? ExternalWallThickness
        {
            get => this._externalWallThickness;
            set => this.SetProperty(ref this._externalWallThickness, value);
        }

        /// <summary>
        /// Gets or sets the internal wall thickness.
        /// </summary>
        public int? InternalWallThickness
        {
            get => this._internalWallThickness;
            set => this.SetProperty(ref this._internalWallThickness, value);
        }

        /// <summary>
        /// Gets or sets the stepdown.
        /// </summary>
        public IntegerDimension StepDown
        {
            get => this._stepDown;
            set => this.SetProperty(ref this._stepDown, value);
        }

        /// <summary>
        /// Gets or sets the roof pitch.
        /// </summary>
        public DoubleDimension RoofPitch
        {
            get => this._roofPitch;
            set => this.SetProperty(ref this._roofPitch, value);
        }

        /// <summary>
        /// Gets or sets the job info.
        /// </summary>
        public JobWallDefaultInfo JobDefaultInfo
        {
            get => this._jobDefaultInfo;
            set => this.SetProperty(ref this._jobDefaultInfo, value);
        }

        /// <summary>
        /// Gets or sets the external wall spacing.
        /// </summary>
        public IntegerDimension ExternalWallSpacing
        {
            get => this._externalWallSpacing;
            set => this.SetProperty(ref this._externalWallSpacing, value);
        }

        /// <summary>
        /// Gets or sets the internal wall spacing.
        /// </summary>
        public IntegerDimension InternalWallSpacing
        {
            get => this._internalWallSpacing;
            set => this.SetProperty(ref this._internalWallSpacing, value);
        }

        #endregion

        #region private Method

        /// <summary>
        /// The initialize level default.
        /// </summary>
        private void InitializeLevelDefault()
        {
            this.StepDown = new IntegerDimension(this.JobDefaultInfo.StepDown);
            this.RoofPitch = this.JobDefaultInfo.RoofPitch == null ? new DoubleDimension() : new DoubleDimension(this.JobDefaultInfo.RoofPitch);

            //this.ExternalWallSpacing = this.JobDefaultInfo.ExternalWallSpacing == null
            //                               ? new IntegerDimension()
            //                               : new IntegerDimension(this.JobDefaultInfo.ExternalWallSpacing);
            //this.InternalWallSpacing = this.JobDefaultInfo.InternalWallSpacing == null 
            //                               ? new IntegerDimension() 
            //                               : new IntegerDimension(this.JobDefaultInfo.InternalWallSpacing);

            this.JobDefaultInfo.PropertyChanged += this.JobInfoPropertyChanged;
            this.StepDown.PropertyChanged += this.StepDownPropertyChanged;
            this.RoofPitch.PropertyChanged += this.RoofPitchPropertyChanged;
            //this.ExternalWallSpacing.PropertyChanged += this.ExternalWallSpacingPropertyChanged;
            //this.InternalWallSpacing.PropertyChanged += this.InternalWallSpacingPropertyChanged;
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
        //private void InternalWallSpacingPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName != "Size")
        //    {
        //        return;
        //    }

        //    this.InternalWallSpacing.IsDefaultValue = this.InternalWallSpacing.Size == this.JobDefaultInfo.InternalWallSpacing;
        //    this.RaisePropertyChanged(nameof(this.InternalWallSpacing));

        //}

        /// <summary>
        /// The external wall spacing property changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        //private void ExternalWallSpacingPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName != "Size")
        //    {
        //        return;
        //    }

        //    this.ExternalWallSpacing.IsDefaultValue = this.ExternalWallSpacing.Size == this.JobDefaultInfo.ExternalWallSpacing;
        //    this.RaisePropertyChanged(nameof(this.ExternalWallSpacing));
        //}

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
            this.RoofPitch.IsDefaultValue = this.RoofPitch.Size.Equals(this.JobDefaultInfo.RoofPitch);
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
            this.StepDown.IsDefaultValue = this.StepDown.Size == this.JobDefaultInfo.StepDown;
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
            //switch (e.PropertyName)
            //{
            //    case nameof(this.JobDefaultInfo.StepDown):
            //        if (this.StepDown.IsDefaultValue)
            //        {
            //            this.StepDown.Size = this.JobDefaultInfo.StepDown;
            //        }
            //        break;
            //    case nameof(this.JobDefaultInfo.RoofPitch):
                    
            //        if (this.RoofPitch.IsDefaultValue)
            //        {
            //            var jobInfoRoofPitch = this.JobDefaultInfo.RoofPitch;
            //            if (jobInfoRoofPitch != null)
            //            {
            //                this.RoofPitch.Size = (double)jobInfoRoofPitch;
            //            }
            //        }
                    

            //        break;
            //    case nameof(this.JobDefaultInfo.ExternalWallSpacing):
            //        if (this.ExternalWallSpacing.IsDefaultValue)
            //        {
            //            var jobInfoExternalWallSpacing = this.JobDefaultInfo.ExternalWallSpacing;
            //            if (jobInfoExternalWallSpacing != null)
            //            {
            //                this.ExternalWallSpacing.Size = (int)jobInfoExternalWallSpacing;
            //            }
            //        }

            //        break;
            //    case nameof(this.JobDefaultInfo.InternalWallSpacing):
            //        if (this.InternalWallSpacing.IsDefaultValue)
            //        {
            //            var jobInfoInternalWallSpacing = this.JobDefaultInfo.InternalWallSpacing;
            //            if (jobInfoInternalWallSpacing != null)
            //            {
            //                this.InternalWallSpacing.Size = (int)jobInfoInternalWallSpacing;
            //            }
            //        }

            //        break;
            //    default:
            //        break;
                
            //}
        }



        #endregion

    }
}
