// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WallBase.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the WallInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Dynamic;
using System.Windows.Media.Effects;
using AppModels.AppData;
using AppModels.Enums;
using AppModels.Factories;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.PocoDataModel.WallMemberData;
using AppModels.ResponsiveData.WallMemberData;
using devDept.Geometry;
using Newtonsoft.Json;
using OpenGL.Delegates;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    public abstract class WallBase : BindableBase, IWallInfo, IComparable<WallBase>
    {
        #region private field

        private WallTypePoco _wallType;
        private int _id;
        private double _ceilingPitch;
        private int _stepDown;
        private LayerItem _wallColorLayer;
        private int _wallThickness;
        private int _wallSpacing;
        private int _wallPitchingHeight;
        private int _raisedCeiling;
        private int _runLength;
        private bool _isStepDown;
        private bool _isRaisedCeiling;
        private double _wallLength;
        private bool _forcedWallUnderRakedArea;
        private bool _isDesigned;
        private int _typeId;
        private double _tempLength;
        private double _extraLength;
        #endregion

        #region Property

        public int Id
        {
            get => this._id;
            set => this.SetProperty(ref this._id, value);
        }
        public WallTypePoco WallType 
        {
            get => _wallType;
            set => SetProperty(ref _wallType, value);
        }
        public NoggingMethodType NoggingMethod => GlobalWallInfo.GlobalInfo.NoggingMethod;
        public IGlobalWallInfo GlobalWallInfo { get; set; }
        public IGlobalWallDetail GlobalWallDetailInfo => WallType.IsLoadBearingWall ? GlobalWallInfo.GlobalExtWallDetailInfo : GlobalWallInfo.GlobalIntWallDetailInfo;
        public LayerItem WallColorLayer
        {
            get => this._wallColorLayer;
            set => this.SetProperty(ref this._wallColorLayer, value);
        }
        public int WallThickness
        {
            get
            {
                if (_wallThickness!=0)
                {
                    return _wallThickness;
                }

                return WallType.IsLoadBearingWall ? GlobalWallInfo.ExternalWallThickness : GlobalWallInfo.InternalWallThickness;
            }
            set
            {
                if (value == GlobalWallDetailInfo.GlobalWallInfo.WallThickness)
                {
                    value = 0;
                }
                
                SetProperty(ref _wallThickness, value);
            } 

        }
        public int WallSpacing
        {
            get
            {
                if (_wallSpacing!=0)
                {
                    return _wallSpacing;
                }

                return WallType.IsLoadBearingWall ? GlobalWallInfo.ExternalWallSpacing : GlobalWallInfo.InternalWallSpacing;
            }
            set
            {
                if (value == GlobalWallDetailInfo.GlobalWallInfo.WallSpacing)
                {
                    value = 0;
                }
                SetProperty(ref _wallSpacing, value);
            } 
        }
        public int WallPitchingHeight
        {
            get => _wallPitchingHeight!=0 ? _wallPitchingHeight : GlobalWallInfo.WallHeight;
            set
            {
                if (value == GlobalWallInfo.WallHeight)
                {
                    value = 0;
                }
                this.SetProperty(ref this._wallPitchingHeight, value);
            } 
        }
        public int WallEndHeight => WallPitchingHeight + HPitching;
        public int WallHeight
        {
            get=>WallEndHeight + StepDown + RaisedCeiling;
            set
            {
                var pitchingHeight = value - StepDown - RaisedCeiling - HPitching;
                WallPitchingHeight = pitchingHeight;
            }
        }
        public abstract int FinalWallHeight { get; }
        public int StudHeight
        {
            get
            {
                var tbtPlate = RibbonPlate.NoItem * RibbonPlate.Depth + TopPlate.NoItem * TopPlate.Depth +
                               BottomPlate.NoItem * BottomPlate.Depth;
                if (!WallType.IsLoadBearingWall && !IsShortWall)
                {
                    tbtPlate += 35;
                }
                var studHeight = WallHeight - tbtPlate;
                if (WallType.IsRaked|| ForcedWallUnderRakedArea|| IsWallUnderRakedArea)
                {
                    studHeight = studHeight.RoundUpTo300();
                }
                return studHeight;
            }
        }
        public bool IsWallUnderRakedArea => WallType.IsRaked || RunLength != 0;
        public bool ForcedWallUnderRakedArea
        {
            get=>_forcedWallUnderRakedArea;
            set
            {
                SetProperty(ref _forcedWallUnderRakedArea, value);
                RaisePropertyChanged(nameof(StudHeight));
            }
        }
        public bool IsStepdown
        {
            get => _isStepDown;
            set
            {
                SetProperty(ref _isStepDown, value);
                RaisePropertyChanged(nameof(StepDown));
            }
        }
        public bool IsRaisedCeiling
        {
            get => _isRaisedCeiling;
            set
            {
                SetProperty(ref _isRaisedCeiling, value);
                RaisePropertyChanged(nameof(RaisedCeiling));
            }
        }
        public bool IsShortWall
        {
            get
            {
                if (WallType.IsLoadBearingWall)
                {
                    return false;
                }
                if (WallType.IsRaked )
                {
                    return false;
                }

                return WallHeight < GlobalWallInfo.WallHeight;
            }
        }
        public bool IsNeedTobeDesign
        {
            get
            {
                if (WallType.IsLoadBearingWall)
                {
                    return WallHeight >= 2750 && !IsDesigned;
                }

                return false;
            }
        }
        public int TypeId { get=>_typeId; set=>SetProperty(ref _typeId,value); }
        public bool IsDesigned { get=>_isDesigned; set=>SetProperty(ref _isDesigned,value); }
        public int RunLength 
        { 
            get=>_runLength;
            set => SetProperty(ref _runLength, value);
        }
        public double CeilingPitch
        {
            get => Math.Abs(_ceilingPitch) > 0.001 ? _ceilingPitch : GlobalWallInfo.GlobalInfo.CeilingPitch;
            set
            {
                if (Math.Abs(value - GlobalWallInfo.GlobalInfo.CeilingPitch) < 0.0001)
                {
                    value = 0;
                }
                SetProperty(ref _ceilingPitch, value);
            } 
        }
        public int HPitching =>
            RunLength <= 0 ? 0 : (RunLength * Math.Tan(Utility.DegToRad(CeilingPitch))).RoundUpto5();
        //(int)(Math.Ceiling((RunLength * Math.Tan(Utility.DegToRad(CeilingPitch))) / 5) * 5)
        public int StepDown
        {
            get
            {
                if (!IsStepdown)
                {
                    return 0;
                }

                return _stepDown!=0 ?  _stepDown: GlobalWallInfo.GlobalInfo.StepDown;
            }
            set
            {
                if (value == GlobalWallInfo.GlobalInfo.StepDown)
                {
                    value = 0;
                }
                this.SetProperty(ref this._stepDown, value);
            }
        }
        public int RaisedCeiling
        {
            get
            {
                if (!IsRaisedCeiling)
                {
                    return 0;
                }

                return _raisedCeiling != 0 ? _raisedCeiling : GlobalWallInfo.GlobalInfo.RaisedCeilingHeight;
            }
            set
            {
                if (value == GlobalWallInfo.GlobalInfo.RaisedCeilingHeight)
                {
                    value = 0;
                }
                SetProperty(ref _raisedCeiling, value);
            }
        }
        public string WallName => WallType.AliasName.Replace('_',' ') +" "+ FinalWallHeight + "mm";
        public WallMemberBase RibbonPlate { get; private set; }
        public WallMemberBase TopPlate { get; private set; }
        public WallMemberBase BottomPlate { get; private set; }
        public WallStud Stud { get; private set; }
        public WallMemberBase Nogging { get; private set; }

        public double TempLength
        {
            get=>_tempLength; 
            set=>SetProperty(ref _tempLength,value);
        }
        public double ExtraLength { 
            get => _extraLength; 
            set => SetProperty(ref _extraLength, value);
        }
        public double WallLength => Math.Ceiling(TempLength + ExtraLength);

        #endregion
        #region Constructor
        public WallBase(int id ,IGlobalWallInfo globalWallInfo,WallTypePoco wallType,int typeId = 1)
        {
            WallType = wallType;
            this.Id = id;
            TypeId = typeId;
            this.GlobalWallInfo = globalWallInfo;
            GlobalWallInfo.GlobalInfo.PropertyChanged += GlobalWallInfo_PropertyChanged;
            RibbonPlate = new WallRibbonPlate(this);
            TopPlate = new WallTopPlate(this);
            BottomPlate = new WallBottomPlate(this);
            Stud = new WallStud(this);
            Nogging = new WallNogging(this);
            PropertyChanged += WallBase_PropertyChanged;
            RibbonPlate.PropertyChanged += WallPlate_PropertyChanged;
            TopPlate.PropertyChanged += WallPlate_PropertyChanged;
            BottomPlate.PropertyChanged += WallPlate_PropertyChanged;
        }
        private void WallPlate_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "NoItem" || e.PropertyName=="Depth")
            {
                RaisePropertyChanged(nameof(StudHeight));
            }
        }
        #endregion
        #region Private Method
        public int CompareTo(WallBase other)
        {
            return this.WallType.CompareTo(other.WallType);
        }
        private void GlobalWallInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CeilingPitch):
                    RaisePropertyChanged(nameof(CeilingPitch));
                    RaisePropertyChanged(nameof(HPitching));
                    RaisePropertyChanged(nameof(WallEndHeight));
                    break;
                case nameof(StepDown):
                    RaisePropertyChanged(nameof(StepDown));
                    break;
                case "RaisedCeilingHeight":
                    RaisePropertyChanged(nameof(RaisedCeiling));
                    break;
            }

            RaisePropertyChanged(nameof(WallPitchingHeight));
            RaisePropertyChanged(nameof(WallThickness));
            RaisePropertyChanged(nameof(WallSpacing));
            RaisePropertyChanged(nameof(WallEndHeight));
        }
        protected virtual void WallBase_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "WallType":
                    //RaisePropertyChanged(nameof(IsShortWall));
                    RaisePropertyChanged(nameof(IsWallUnderRakedArea));
                    RaisePropertyChanged(nameof(WallPitchingHeight));
                    RaisePropertyChanged(nameof(WallThickness));
                    RaisePropertyChanged(nameof(WallSpacing));
                    RaisePropertyChanged(nameof(WallEndHeight));
                    RaisePropertyChanged(nameof(WallHeight));
                    RaisePropertyChanged(nameof(IsNeedTobeDesign));
                    RaisePropertyChanged(nameof(WallName));
                    break;
                case nameof(RunLength):
                    RaisePropertyChanged(nameof(IsWallUnderRakedArea));
                    RaisePropertyChanged(nameof(HPitching));
                    RaisePropertyChanged(nameof(WallEndHeight));
                    RaisePropertyChanged(nameof(WallHeight));
                    break;
                case nameof(StepDown):
                    RaisePropertyChanged(nameof(WallHeight));
                    break;
                case nameof(RaisedCeiling):
                    RaisePropertyChanged(nameof(WallHeight));
                    break;
                case nameof(WallHeight):
                    //RaisePropertyChanged(nameof(IsShortWall));
                    RaisePropertyChanged(nameof(FinalWallHeight));
                    RaisePropertyChanged(nameof(StudHeight));
                    RaisePropertyChanged(nameof(IsNeedTobeDesign));
                    RaisePropertyChanged(nameof(WallName));
                    break;
                case nameof(WallPitchingHeight):
                    RaisePropertyChanged(nameof(WallEndHeight));
                    RaisePropertyChanged(nameof(WallHeight));
                    RaisePropertyChanged(nameof(FinalWallHeight));
                    RaisePropertyChanged(nameof(StudHeight));
                    RaisePropertyChanged(nameof(IsNeedTobeDesign));
                    RaisePropertyChanged(nameof(WallName));
                    break;
                case nameof(ForcedWallUnderRakedArea):
                    RaisePropertyChanged(nameof(StudHeight));
                    RaisePropertyChanged(nameof(FinalWallHeight));
                    RaisePropertyChanged(nameof(WallName));
                    break;
                case nameof(IsDesigned):
                    RaisePropertyChanged(nameof(IsNeedTobeDesign));
                    break;
                case nameof(TempLength):
                case nameof(ExtraLength):
                    RaisePropertyChanged(nameof(WallLength));
                    break;
                    
            }
        }

        public void LoadWallInfo(WallLayerPoco wall)
        {
            //Id = wall.Id;
            TypeId = wall.TypeId;
            //WallType = wall.WallType;
            WallColorLayer = wall.WallColorLayer;
            WallThickness = wall.WallThickness;
            WallSpacing = wall.WallSpacing;
            WallPitchingHeight = wall.WallPitchingHeight;
            ForcedWallUnderRakedArea = wall.ForcedWallUnderRakedArea;
            //WallLength = wall.WallLength;
            IsStepdown = wall.IsStepdown;
            IsRaisedCeiling = wall.IsRaisedCeiling;
            IsDesigned = wall.IsDesigned;
            RunLength = wall.RunLength;
            CeilingPitch = wall.CeilingPitch;
            StepDown = wall.StepDown;
            RaisedCeiling = wall.RaisedCeiling;
            RibbonPlate.LoadMemberInfo(wall.RibbonPlate);
            TopPlate.LoadMemberInfo(wall.TopPlate);
            Stud.LoadMemberInfo(wall.Stud);
            BottomPlate.LoadMemberInfo(wall.BottomPlate);
            Nogging.LoadMemberInfo(wall.Nogging);
        }
        #endregion
    }
}
