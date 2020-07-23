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
        private string _ceilingPitch;
        private string _pitchingHeight;
        private string _stepDown;
        private LayerItem _wallColorLayer;
        private string _wallThickness;
        private string _wallSpacing;
        private string _wallPitchingHeight;
        private string _raisedCeiling;

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
        public string WallThickness
        {
            get
            {
                if (!string.IsNullOrEmpty(_wallThickness))
                {
                    return _wallThickness;
                }

                return WallType == WallType.LBW ? GlobalWallInfo.ExternalWallThickness : GlobalWallInfo.InternalWallThickness;
            }
            set => SetProperty(ref _wallThickness, value);

        }
        public string WallSpacing
        {
            get
            {
                if (!string.IsNullOrEmpty(_wallSpacing))
                {
                    return _wallThickness;
                }

                return WallType == WallType.LBW ? GlobalWallInfo.ExternalWallSpacing : GlobalWallInfo.InternalWallSpacing;
            }
            set => SetProperty(ref _wallThickness, value);
        }
        public string WallPitchingHeight
        {
            get
            {
                if (!string.IsNullOrEmpty(_wallPitchingHeight))
                {
                    return _wallPitchingHeight;
                }

                return GlobalWallInfo.WallHeight.ToString();
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
        public string CeilingPitch
        {
            get => this._ceilingPitch;
            set => this.SetProperty(ref this._ceilingPitch, value);
        }
        public int IsHPitching { get; set; }
        public int HPitching { get; set; }
        
        
        public string StepDown
        {
            get
            {
                if (!IsStepdown)
                {
                    return "0";
                }

                return !string.IsNullOrEmpty(_stepDown) ? _stepDown : GlobalWallInfo.StepDown.ToString();
            }
            set => this.SetProperty(ref this._stepDown, value);
        }
        public string RaisedCeiling
        {
            get
            {
                if (!IsRaisedCeiling)
                {
                    return "0";
                }

                return !string.IsNullOrEmpty(_raisedCeiling) ? _raisedCeiling : GlobalWallInfo.RaisedCeilingHeight.ToString();
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
