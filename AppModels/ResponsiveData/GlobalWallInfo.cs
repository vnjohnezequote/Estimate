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
        private int _externalDoorHeight;
        private int _internalDoorHeight;
        private int _externalWallSpacing;
        private int _internalWallSpacing;
        private int _externalWallThickness;
        private int _internalWallThickness;
        private int _externalWallTimberDepth;
        private int _internalWallTimberDepth;
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
            get => _internalDoorHeight==0 ? _externalDoorHeight : _internalDoorHeight;
            set => SetProperty(ref _internalDoorHeight, value);
        }
        public int ExternalWallSpacing
        {
            get=>_externalWallSpacing;
            set=>SetProperty(ref _externalWallSpacing,value);
        }
        public int InternalWallSpacing
        {
            get => _internalWallSpacing==0 ? _externalWallSpacing : _internalWallSpacing;
            set
            {
                if (value == _externalWallSpacing)
                {
                    value = 0;
                }
                SetProperty(ref _internalWallSpacing, value);
            } 
        }
        public int ExternalWallThickness
        {
            get=>_externalWallThickness;
            set=>SetProperty(ref _externalWallThickness,value);
        }
        public int InternalWallThickness
        {
            get => _internalWallThickness==0 ? _externalWallThickness : _internalWallThickness;
            set => SetProperty(ref _internalWallThickness, value);
        }
        public int ExternalWallTimberDepth 
        { 
            get=>_externalWallTimberDepth;
            set => SetProperty(ref _externalWallTimberDepth, value);
        }
        public int InternalWallTimberDepth
        {
            get => _internalWallTimberDepth==0 ? _externalWallTimberDepth : _internalWallTimberDepth;

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
            get => _raisedCeilingHeight;
            set => SetProperty(ref _raisedCeilingHeight, value);
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
        public IBasicWallInfo GlobalExternalWallInfo { get;private set; }
        public IBasicWallInfo GlobalInternalWallInfo { get; private set; }
        public IGlobalWallDetail GlobalExtWallDetailInfo { get;private set; }
        public IGlobalWallDetail GlobalIntWallDetailInfo { get;private set; }
        public IWallMemberInfo GlobalNoggingInfo { get;private set; }
        public IWallMemberInfo GlobalDoorJambInfo { get;private set; }
        #endregion

        #region Constructor

        public GlobalWallInfo()
        {
            WallHeight = 2440;
            ExternalDoorHeight = 2100;
            ExternalWallThickness = 90;
            ExternalWallTimberDepth = 35;
            ExternalWallTimberGrade = "MGP10";
            ExternalWallSpacing = 450;
            TrussSpacing = 600;
            RoofFrameType = RoofFrameType.Truss;
            NoggingMethod = NoggingMethodType.AsWall;
            GlobalExternalWallInfo = new BasicWallInfor(this,WallType.LBW);
            GlobalInternalWallInfo= new BasicWallInfor(this, WallType.NonLBW);
            GlobalNoggingInfo = new GlobalWallMemberInfo(GlobalInternalWallInfo,WallMemberType.Nogging);
            GlobalExtWallDetailInfo = new GlobalWallDetailInfo(GlobalExternalWallInfo,GlobalNoggingInfo);
            GlobalIntWallDetailInfo = new GlobalWallDetailInfo(GlobalInternalWallInfo, GlobalNoggingInfo);
            GlobalDoorJambInfo = new GlobalWallMemberInfo(GlobalExternalWallInfo,WallMemberType.DoorJamb,GlobalExtWallDetailInfo.Stud);
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
