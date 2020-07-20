// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobWallDefaultInfo.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the WallDefaultInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AppModels.Enums;
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
        /// The truss spacing.
        /// </summary>
        private int _trussSpacing;

        /// <summary>
        /// The rafter spacing.
        /// </summary>
        private int _rafterSpacing;

        /// <summary>
        /// The step down.
        /// </summary>
        private int _stepDown;

        private int _roofOverHang;

        /// <summary>
        /// The roof pitch.
        /// </summary>
        private double _roofPitch;

        private double _ceilingPitch;

        public RoofFrameType _roofFrameType;

        #endregion

        #region public Property

        public int RoofOverHang
        {
            get => _roofOverHang;
            set => SetProperty(ref _roofOverHang, value);
        }

        public RoofFrameType RoofFrameType
        {
            get=>_roofFrameType;
            set
            {
                SetProperty(ref _roofFrameType, value);
                RaisePropertyChanged(nameof(CeilingPitch));
            }
        }

        /// <summary>
        /// Gets or sets the truss spacing.
        /// </summary>
        public int TrussSpacing
        {
            get => this._trussSpacing;
            set
            {
                this.SetProperty(ref this._trussSpacing, value);
                RaisePropertyChanged(nameof(RafterSpacing));
            } 
        }

        /// <summary>
        /// Gets or sets the rafter spacing.
        /// </summary>
        public int RafterSpacing
        {
            get => this._rafterSpacing == 0 ? _trussSpacing : this._rafterSpacing;
            set => this.SetProperty(ref this._rafterSpacing, value);
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
       
        public double RoofPitch
        {
            get => this._roofPitch;
            set
            {
                this.SetProperty(ref this._roofPitch, value);
                this.RaisePropertyChanged(nameof(CeilingPitch));
            } 
        }

        public double CeilingPitch
        {
            get => this._roofFrameType == RoofFrameType.Truss ? _ceilingPitch : _roofPitch;
            set => SetProperty(ref _ceilingPitch, value);
        }

        public WallDefaultInfo ExternalWallDefaultInfo { get; set; }
        public WallDefaultInfo InternalWallDefaultInfo { get; set; }

        #endregion

        #region Constructor

        public JobWallDefaultInfo()
        {
            RoofFrameType = RoofFrameType.Truss;
            ExternalWallDefaultInfo = new WallDefaultInfo();
            InternalWallDefaultInfo= new WallDefaultInfo();
        }

        #endregion


    }
}
