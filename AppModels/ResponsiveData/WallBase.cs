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
using AppModels.AppData;
using AppModels.Enums;
using AppModels.Factories;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.ResponsiveData.WallMemberData;
using devDept.Geometry;
using OpenGL.Delegates;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The wall base.
    /// </summary>
    public abstract class WallBase : BindableBase, IWallInfo
    {
        #region private field

        private WallTypePoco _wallType;
        private int _id;
        private double _ceilingPitch;
        private int _pitchingHeight;
        private int _stepDown;
        private LayerItem _wallColorLayer;
        private int _wallThickness;
        private int _wallSpacing;
        private int _wallPitchingHeight;
        private int _raisedCeiling;
        private int _runLength;
        private bool _isStepDown;
        private bool _isRaisedCeiling;
        #endregion

        #region Property
        public int Id
        {
            get => this._id;
            set => this.SetProperty(ref this._id, value);
        }
        public virtual WallTypePoco WallType /*{ get; set; }*/
        {
            get => _wallType;
            set => SetProperty(ref _wallType, value);
        //ChangeWallType();
        }
    public NoggingMethodType NoggingMethod => GlobalWallInfo.NoggingMethod;
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
            set => this.SetProperty(ref this._wallPitchingHeight, value);
        }
        public int WallEndHeight => WallPitchingHeight + HPitching;
        public int WallHeight { get; }
        public bool IsRakedWall { get; set; }
        public bool IsWallUnderFlatCeiling { get; set; }
        public bool IsParallelWithRoofFrame { get; set; }
        public double WallLength { get; }

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

        public bool IsShorWall { get; set; }
        public int RunLength 
        { 
            get=>_runLength;
            set
            {
                SetProperty(ref _runLength, value);
                RaisePropertyChanged(nameof(HPitching));
            }
        }
        public double CeilingPitch
        {
            get => Math.Abs(_ceilingPitch) > 0.001 ? _ceilingPitch : GlobalWallInfo.CeilingPitch;
            set => this.SetProperty(ref this._ceilingPitch, value);
        }
        public int IsHPitching { get; set; }

        public int HPitching
        {
            get
            {
                if (RunLength > 0)
                {
                    return (int)(Math.Ceiling((RunLength * Math.Tan(Utility.DegToRad(CeilingPitch)))/5)*5);
                    
                }

                return 0;
            }

        }
        public int StepDown
        {
            get
            {
                if (!IsStepdown)
                {
                    return 0;
                }

                return _stepDown!=0 ?  _stepDown: GlobalWallInfo.StepDown;
            }
            set
            {
                if (value == GlobalWallInfo.StepDown)
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

                return _raisedCeiling != 0 ? _raisedCeiling : GlobalWallInfo.RaisedCeilingHeight;
            }
            set
            {
                if (value == GlobalWallInfo.RaisedCeilingHeight)
                {
                    value = 0;
                }
                SetProperty(ref _raisedCeiling, value);
            }
        }

        public WallMemberBase RibbonPlate { get; private set; }
        public WallMemberBase TopPlate { get; private set; }
        public WallMemberBase BottomPlate { get; private set; }
        public WallStud Stud { get; private set; }
        public WallMemberBase Nogging { get; private set; }
        #endregion
        #region Constructor

        public WallBase(int id ,IGlobalWallInfo globalWallInfo,WallTypePoco wallType)
        {
            WallType = wallType;
            this.Id = id;
            this.GlobalWallInfo = globalWallInfo;
            GlobalWallInfo.PropertyChanged += GlobalWallInfo_PropertyChanged;
            RibbonPlate = new WallRibbonPlate(this);
            TopPlate = new WallTopPlate(this);
            BottomPlate = new WallBottomPlate(this);
            Stud = new WallStud(this);
            Nogging = new WallNogging(this);
            PropertyChanged += WallBase_PropertyChanged;
            
        }

        private void GlobalWallInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CeilingPitch")
            {
                RaisePropertyChanged(nameof(CeilingPitch));
                RaisePropertyChanged(nameof(HPitching));
                RaisePropertyChanged(nameof(WallEndHeight));
            }

            if (e.PropertyName == "StepDown")
            {
                RaisePropertyChanged(nameof(StepDown));
            }

            if (e.PropertyName == "RaisedCeilingHeight")
            {
                RaisePropertyChanged(nameof(RaisedCeiling));
            }

            RaisePropertyChanged(nameof(WallPitchingHeight));
            RaisePropertyChanged(nameof(WallThickness));
            RaisePropertyChanged(nameof(WallSpacing));
            RaisePropertyChanged(nameof(WallEndHeight));
        }

        protected virtual void WallBase_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "WallType")
            {
                RaisePropertyChanged(nameof(WallPitchingHeight));
                RaisePropertyChanged(nameof(WallThickness));
                RaisePropertyChanged(nameof(WallSpacing));
                RaisePropertyChanged(nameof(WallHeight));
                RaisePropertyChanged(nameof(WallEndHeight));
            }
        }


        #endregion


        #region Private Method

        #endregion
    }
}
