// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobWallDefaultInfo.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the GlobalWallInfo type.
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
        private int _trussSpacing;
        private int _rafterSpacing;
        private int _stepDown;
        private double _roofPitch;
        private double _ceilingPitch;
        public RoofFrameType _roofFrameType;
        private int _externalDoorHeight;
        private int _internalDoorHeight;
        private NoggingMethodType _globalNoggingMethodType;

        #endregion

        #region public Property

        public NoggingMethodType GlobalNoggingMethodType
        {
            get => _globalNoggingMethodType;
            set
            {
                SetProperty(ref _globalNoggingMethodType, value);
            } 
        }

        public int ExternalDoorHeight
        {
            get => _externalDoorHeight;
            set
            {
                SetProperty(ref _externalDoorHeight, value);
                RaisePropertyChanged(nameof(InternalDoorHeight));
            }
        }
        public int InternalDoorHeight
        {
            get => _internalDoorHeight == 0 ? _externalDoorHeight : _internalDoorHeight;
            set => SetProperty(ref _internalDoorHeight, value);
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
        public int TrussSpacing
        {
            get => this._trussSpacing;
            set
            {
                this.SetProperty(ref this._trussSpacing, value);
                RaisePropertyChanged(nameof(RafterSpacing));
            } 
        }
        public int RafterSpacing
        {
            get => this._rafterSpacing == 0 ? _trussSpacing : this._rafterSpacing;
            set => this.SetProperty(ref this._rafterSpacing, value);
        }
        public int StepDown
        {
            get => this._stepDown;
            set => this.SetProperty(ref this._stepDown, value);
        }
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
        public GlobalWallInfo GlobalExternalWallInfo { get; set; }
        public GlobalWallInfo GlobalInternalWallInfo { get; set; }
        public GlobalWallDetailInfo GlobalExtWallDetailInfo { get; set; }
        public GlobalWallDetailInfo GlobalIntWallDetaiInfo { get; set; }
        public TimberWallMemberBase GlobalNoggingInfo { get; set; }
        public TimberWallMemberBase GlobalDoorJambInfo { get; set; }
        #endregion

        #region Constructor

        public JobWallDefaultInfo()
        {
            ExternalDoorHeight = 2100;
            RoofFrameType = RoofFrameType.Truss;
            GlobalExternalWallInfo = new GlobalWallInfo(WallType.External_LBW,this);
            GlobalInternalWallInfo= new GlobalWallInfo(WallType.Internal_NonLBW,this);
            GlobalNoggingInfo = new TimberWallMemberBase(GlobalExternalWallInfo);
            GlobalDoorJambInfo = new TimberWallMemberBase(GlobalExternalWallInfo);
            GlobalExtWallDetailInfo = new GlobalWallDetailInfo(GlobalExternalWallInfo,GlobalNoggingInfo,GlobalDoorJambInfo);
            GlobalIntWallDetaiInfo = new GlobalWallDetailInfo(GlobalInternalWallInfo, GlobalNoggingInfo, GlobalDoorJambInfo);
            TrussSpacing = 600;
        }

        #endregion


    }
}
