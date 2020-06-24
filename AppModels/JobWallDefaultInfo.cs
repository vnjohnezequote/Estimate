// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobWallDefaultInfo.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the WallDefaultInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AppModels
{
    using Prism.Mvvm;

    /// <summary>
    /// The wall default info.
    /// </summary>
    public class JobWallDefaultInfo : BindableBase
    {
        #region private Field

        /// <summary>
        /// The external wall spacing.
        /// </summary>
        private int? externalWallSpacing;

        /// <summary>
        /// The internal wall spacing.
        /// </summary>
        private int? internalWallSpacing;

        /// <summary>
        /// The step down.
        /// </summary>
        private int stepDown;

        /// <summary>
        /// The roof pitch.
        /// </summary>
        private double? roofPitch;
        
        #endregion

        #region public Property

        /// <summary>
        /// Gets or sets the external wall spacing.
        /// </summary>
        public int? ExternalWallSpacing
        {
            get => this.externalWallSpacing;
            set => this.SetProperty(ref this.externalWallSpacing, value);
        }

        /// <summary>
        /// Gets or sets the internal wall spacing.
        /// </summary>
        public int? InternalWallSpacing
        {
            get => this.internalWallSpacing;
            set => this.SetProperty(ref this.internalWallSpacing, value);
        }

        /// <summary>
        /// Gets or sets the step down.
        /// </summary>
        public int StepDown
        {
            get => this.stepDown;
            set => this.SetProperty(ref this.stepDown, value);
        }

        /// <summary>
        /// Gets or sets the roof pitch.
        /// </summary>
        public double? RoofPitch
        {
            get => this.roofPitch;
            set => this.SetProperty(ref this.roofPitch, value);
        }

        #endregion


    }
}
