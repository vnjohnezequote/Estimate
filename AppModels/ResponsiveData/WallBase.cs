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
using AppModels.Interaface;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The wall base.
    /// </summary>
    public abstract class WallBase : BindableBase, IWallInfo
    {
        #region private field

        private WallType _wallType= WallType.LBW;
        private int _id;
        private int _ceilingPitch;
        private int _pitchingHeight;
        private int _stepDown;
        private LayerItem _wallColorLayer;
        private int _wallThickness;
        private int _wallSpacing;
        private int _wallPitchingHeight;
        private int _raisedCeiling;

        #endregion

        #region Property
        public int Id
        {
            get => this._id;
            set => this.SetProperty(ref this._id, value);
        }
        public WallType WallType
        {
            get => _wallType;
            set
            {
                SetProperty(ref _wallType, value);
                ChangeWallType();
            }
        }
        public NoggingMethodType NoggingMethod => GlobalWallInfo.NoggingMethod;
        public IGlobalWallInfo GlobalWallInfo { get; set; }
        public LayerItem WallColorLayer
        {
            get => this._wallColorLayer;
            set => this.SetProperty(ref this._wallColorLayer, value);
        }
        public int WallThickness
        {
            get
            {
                if (_wallThickness==0)
                {
                    return _wallThickness;
                }

                return WallType == WallType.LBW ? GlobalWallInfo.ExternalWallThickness : GlobalWallInfo.InternalWallThickness;
            }
            set => SetProperty(ref _wallThickness, value);

        }
        public int WallSpacing
        {
            get
            {
                if (_wallSpacing==0)
                {
                    return _wallThickness;
                }

                return WallType == WallType.LBW ? GlobalWallInfo.ExternalWallSpacing : GlobalWallInfo.InternalWallSpacing;
            }
            set => SetProperty(ref _wallThickness, value);
        }
        public int WallPitchingHeight
        {
            get
            {
                if (_wallPitchingHeight==0)
                {
                    return _wallPitchingHeight;
                }

                return GlobalWallInfo.WallHeight;
            }
            set => this.SetProperty(ref this._wallPitchingHeight, value);
        }
        public int WallEndHeight { get; }
        public int WallHeight { get; }
        public bool IsRakedWall { get; set; }
        public bool IsWallUnderFlatCeiling { get; set; }
        public bool IsParallelWithRoofFrame { get; set; }
        public bool IsStepdown { get; set; }
        public bool IsRaisedCeiling { get; set; }
        public bool IsShorWall { get; set; }
        public int RunLength { get; set; }
        public int CeilingPitch
        {
            get => this._ceilingPitch;
            set => this.SetProperty(ref this._ceilingPitch, value);
        }
        public int IsHPitching { get; set; }
        public int HPitching { get; set; }
        
        
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
            set => this.SetProperty(ref this._stepDown, value);
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

        }

        
        public int IsNonLbWall { get; set; }
        public WallMember RibbonPlate { get; set; }
        public WallMember TopPlate { get; set; }
        public WallMember BottomPlate { get; set; }
        public WallStud Stud { get; set; }

        #endregion
        #region Constructor

        public WallBase(int id ,IGlobalWallInfo globalWallInfo)
        {
            
        }


        #endregion
        

        #region Private Method

        private void ChangeWallType()
        {
            RaisePropertyChanged(nameof(WallSpacing));
            RaisePropertyChanged(nameof(WallThickness));
        }
        #endregion
    }
}
