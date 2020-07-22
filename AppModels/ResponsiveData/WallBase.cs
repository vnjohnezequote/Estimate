// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WallBase.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the WallInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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

        private WallType _wallType;
        private int _id;
        private string _ceilingPitch;
        private string _pitchingHeight;
        private string _stepDown;
        private LayerItem _wallColorLayer;

        #endregion
        #region Constructor




        #endregion
        #region Property

        public WallType WallType { get=>_wallType; set=>SetProperty(ref _wallType,value); }
        public NoggingMethodType NoggingMethod => GlobalWallInfo.NoggingMethod;
        public IGlobalWallInfo GlobalWallInfo { get; set; }
        public string Thickness { get; }

        public LayerItem WallColorLayer
        {
            get => this._wallColorLayer;
            set => this.SetProperty(ref this._wallColorLayer, value);
        }

        public string Start_WallHeight { get; }
        public int End_WallHeight { get; }
        public int Show_WallHeight { get; }

        public int Id
        {
            get => this._id;
            set => this.SetProperty(ref this._id, value);
        }
        public string PitchingHeight
        {
            get => this._pitchingHeight;
            set => this.SetProperty(ref this._pitchingHeight, value);
        }
        public int WallHeight { get; set; }
        public string Treatment { get; set; }
        public bool IsRakedWall { get; set; }
        public bool IsWallUnderFlatCeiling { get; }
        public bool IsShorWall { get; }
        public int RunLength { get; set; }
        public string CeilingPitch
        {
            get => this._ceilingPitch;
            set => this.SetProperty(ref this._ceilingPitch, value);
        }
        public int IsHPitching { get; set; }
        public int HPitching { get; set; }
        public bool IsStepdown { get; set; }
        public string StepDown
        {
            get => this._stepDown;
            set => this.SetProperty(ref this._stepDown, value);
        }

        public bool IsRaisedCeiling { get; }
        public int RaisedCeiling { get; }

        public bool IsCeilingRaised { get; set; }
        public int CeilingRaised { get; set; }
        public bool IsParallelWithTruss { get; set; }
        public int IsNonLbWall { get; set; }
        public WallMember RibbonPlate { get; set; }
        public WallMember TopPlate { get; set; }
        public WallMember BottomPlate { get; set; }
        public WallStud Stud { get; set; }

        #endregion

        #region Private Method

        #endregion
    }
}
