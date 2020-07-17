// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobWallDefaultInfo.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the WallDefaultInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Prism.Mvvm;
using ProtoBuf;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The wall default info.
    /// </summary>
    public class JobWallDefaultInfo : BindableBase
    {
        #region private Field

        /// <summary>
        /// The external wall spacing.
        /// </summary>
        private int? _externalWallSpacing;

        /// <summary>
        /// The internal wall spacing.
        /// </summary>
        private int? _internalWallSpacing;

        /// <summary>
        /// The step down.
        /// </summary>
        private int _stepDown;

        /// <summary>
        /// The roof pitch.
        /// </summary>
        private double? _roofPitch;

        #endregion

        #region public Property

        /// <summary>
        /// Gets or sets the external wall spacing.
        /// </summary>
        public int? ExternalWallSpacing
        {
            get => this._externalWallSpacing;
            set => this.SetProperty(ref this._externalWallSpacing, value);
        }

        /// <summary>
        /// Gets or sets the internal wall spacing.
        /// </summary>
  
        public int? InternalWallSpacing
        {
            get => this._internalWallSpacing;
            set => this.SetProperty(ref this._internalWallSpacing, value);
        }

        /// <summary>
        /// Gets or sets the step down.
        /// </summary>
     
        public int StepDown
        {
            get => this._stepDown;
            set => this.SetProperty(ref this._stepDown, value);
        }

        /// <summary>
        /// Gets or sets the roof pitch.
        /// </summary>
       
        public double? RoofPitch
        {
            get => this._roofPitch;
            set => this.SetProperty(ref this._roofPitch, value);
        }

        #endregion


    }
}
