// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobWallDefaultInfo.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the JobGlobalWallInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using AppModels.Enums;
using AppModels.Interaface;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The wall default info.
    /// </summary>
    public class GlobalWallInfo : BindableBase,IGlobalWallInfo
    {
        #region private Field
        private int _trussSpacing;
        private int _rafterSpacing;
        private int _stepDown;
        private double _roofPitch;
        private double _ceilingPitch;
        public RoofFrameType _roofFrameType;
        private NoggingMethodType _noggingMethod;
        private string _externalDoorHeight;
        private string _internalDoorHeight;
        private string _externalWallSpacing;
        private string _internalWallSpacing;
        private string _externalWallThickness;
        private string _internalWallThickness;
        private string _externalWallTimberDepth;
        private string _internalWallTimberDepth;
        private string _externalWallTimberGrade;
        private string _internalWallTimberGrade;
        private int _wallHeight;
        private int _raisedCeilingHeight;

        #endregion

        #region public Property

        public NoggingMethodType NoggingMethod
        {
            get => _noggingMethod;
            set => SetProperty(ref _noggingMethod, value);
        }
        public int WallHeight { 
            get=>_wallHeight;
            set => SetProperty(ref _wallHeight, value);
        }
        public string ExternalDoorHeight
        {
            get => _externalDoorHeight;
            set
            {
                SetProperty(ref _externalDoorHeight, value);
                RaisePropertyChanged(nameof(InternalDoorHeight));
            }
        }
        public string InternalDoorHeight
        {
            get => (string.IsNullOrEmpty(_internalDoorHeight)) ? _externalDoorHeight : _internalDoorHeight;
            set => SetProperty(ref _internalDoorHeight, value);
        }
        public string ExternalWallSpacing
        {
            get=>_externalWallSpacing;
            set=>SetProperty(ref _externalWallSpacing,value);
        }
        public string InternalWallSpacing
        {
            get => string.IsNullOrEmpty(_internalWallSpacing) ? _externalWallSpacing : _internalWallSpacing;
            set
            {
                if (value == _externalWallSpacing )
                {
                    value = null;
                }
                SetProperty(ref _internalWallSpacing, value);
            } 
        }
        public string ExternalWallThickness
        {
            get=>_externalWallThickness;
            set=>SetProperty(ref _externalWallThickness,value);
        }
        public string InternalWallThickness
        {
            get => string.IsNullOrEmpty(_internalWallThickness) ? _externalWallThickness : _internalWallThickness;
            set => SetProperty(ref _internalWallThickness, value);
        }
        public string ExternalWallTimberDepth 
        { 
            get=>_externalWallTimberDepth;
            set => SetProperty(ref _externalWallTimberDepth, value);
        }
        public string InternalWallTimberDepth
        {
            get => string.IsNullOrEmpty(_internalWallTimberDepth) ? _externalWallTimberDepth : _internalWallTimberDepth;

            set => SetProperty(ref _internalWallTimberDepth, value);
        }
        public string ExternalWallTimberGrade
        {
            get => _externalWallTimberGrade;
            set => SetProperty(ref _externalWallTimberGrade, value);
        }
        public string InternalWallTimberGrade 
        {
            get => string.IsNullOrEmpty(_internalWallTimberGrade) ? _externalWallTimberGrade : _internalWallTimberGrade;

            set => SetProperty(ref _internalWallTimberGrade, value);
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
        public int RaisedCeilingHeight
        {
            get=>_raisedCeilingHeight;
            set=>SetProperty(ref _raisedCeilingHeight,value);
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
        public IGlobalWallInfo GlobalWallInformation { get; private set; } =null;
        public double CeilingPitch
        {
            get => this._roofFrameType == RoofFrameType.Truss ? _ceilingPitch : _roofPitch;
            set => SetProperty(ref _ceilingPitch, value);
        }
        public IBasicWallInfo GlobalExternalWallInfo { get; set; }
        public IBasicWallInfo GlobalInternalWallInfo { get; set; }
        public GlobalWallDetailInfo GlobalExtWallDetailInfo { get; set; }
        public GlobalWallDetailInfo GlobalIntWallDetaiInfo { get; set; }
        public IWallMemberInfo GlobalNoggingInfo { get; set; }
        public IWallMemberInfo GlobalDoorJambInfo { get; set; }
        #endregion

        #region Constructor

        public GlobalWallInfo()
        {
            ExternalDoorHeight = "2100";
            ExternalWallThickness = "90";
            ExternalWallTimberDepth = "35";
            ExternalWallTimberGrade = "MGP10";
            ExternalWallSpacing = "450";
            RoofFrameType = RoofFrameType.Truss;
            NoggingMethod = NoggingMethodType.AsWall;
            GlobalExternalWallInfo = new BasicWallInfor(this,WallType.LBW);
            GlobalInternalWallInfo= new BasicWallInfor(this, WallType.NonLBW);
            GlobalNoggingInfo = new GlobalWallMemberInfo(GlobalInternalWallInfo,WallMemberType.Nogging);
            GlobalExtWallDetailInfo = new GlobalWallDetailInfo(GlobalExternalWallInfo,GlobalNoggingInfo);
            GlobalIntWallDetaiInfo = new GlobalWallDetailInfo(GlobalInternalWallInfo, GlobalNoggingInfo);
            GlobalDoorJambInfo = new GlobalWallMemberInfo(GlobalExternalWallInfo,WallMemberType.DoorJamb,GlobalExtWallDetailInfo.Stud);
            TrussSpacing = 600;
            PropertyChanged += GlobalWallInfo_PropertyChanged;
        }

        private void GlobalWallInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ExternalWallThickness):
                    RaisePropertyChanged(nameof(InternalWallThickness));
                    break;
                case nameof(ExternalWallSpacing):
                    RaisePropertyChanged(nameof(InternalWallSpacing));
                    break;
                case nameof(ExternalWallTimberDepth):
                    RaisePropertyChanged(nameof(InternalWallTimberDepth));
                    break;
                case nameof(ExternalWallTimberGrade):
                    RaisePropertyChanged(nameof(InternalWallTimberGrade));
                    break;

            }


        }

        #endregion


    }
}
