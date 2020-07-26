// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Layer.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the Layer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The wall layer.
    /// </summary>
    public class WallLayer : WallBase, IComparable<WallLayer>
    {

        #region Private field
        private WallTypePoco _wallType;
        ///// <summary>
        ///// The wall heights.
        ///// </summary>
        //private List<IntegerDimension> _wallHeights;

        ///// <summary>
        ///// The wall type.
        ///// </summary>
        //private WallTypePoco _timberWallTypePoco;

        ///// <summary>
        ///// The wall thickness.
        ///// </summary>
        //private IntegerDimension _wallThickness;

        ///// <summary>
        ///// The default info.
        ///// </summary>
        //private LevelGlobalWallInfo _defaultInfo;

        ///// <summary>
        ///// The stud spacing.
        ///// </summary>
        //private IntegerDimension _studSpacing;

        ///// <summary>
        ///// The is step down.
        ///// </summary>
        //private int _isStepDown;

        ///// <summary>
        ///// The run length.
        ///// </summary>
        //private int _runLength;

        ///// <summary>
        ///// The is raised ceiling.
        ///// </summary>
        //private int _isRaisedCeiling;

        ///// <summary>
        ///// The extra length.
        ///// </summary>
        //private double _extraLength;

        ///// <summary>
        ///// The temp length.
        ///// </summary>
        //private double _tempLength;

        ///// <summary>
        ///// The ceiling raised.
        ///// </summary>
        //private int _ceilingRaised;

        ///// <summary>
        ///// The wall height.
        ///// </summary>
        //private IntegerDimension _lastWallHeight;


        #endregion

        #region Property

        public override WallTypePoco WallType
        {
            get=>_wallType;
            set=>SetProperty(ref _wallType,value);
        }
        //{
        //    get => _wallType;
        //    set => SetProperty(ref _wallType, value);
        //    //ChangeWallType();
        //}


        #endregion

        //#region Constructor

        ///// <summary>
        ///// Initializes a new instance of the <see cref="WallLayer"/> class.
        ///// </summary>
        ///// <param name="wallId">
        ///// The wall Id.
        ///// </param>
        ///// <param name="wallTypePoco">
        ///// The wall type.
        ///// </param>
        ///// <param name="defaultInfo">
        ///// The default info.
        ///// </param>
        public WallLayer(int wallId, IGlobalWallInfo globalWallInfo,WallTypePoco wallType) : base(wallId,globalWallInfo,wallType)
        {
        //    this.Id = wallId;
        //    //this.GlobalWallInfo = globalWallInfo;
        //    //this.TimberWallTypePoco = wallTypePoco;

        //    // this.InitializeWallLayer();
        //    this.InitializeWallMember();
        //    this.GlobalWallInfo.PropertyChanged += this.DefaultInfoPropertyChanged;

        //    this.StudSpacing.PropertyChanged += this.StudSpacingPropertyChanged;
        //    this.WallThickness.PropertyChanged += this.WallThicknessPropertyChanged;
        //    //this.StepDown.PropertyChanged += this.StepDownPropertyChanged;
        //    //this.PitchingHeight.PropertyChanged += this.PitchingHeightPropertyChanged;
        //    //this.RoofPitch.PropertyChanged += this.RoofPitchPropertyChanged;
        //    this.LastWallHeight.PropertyChanged += this.LastWallHeightPropertyChanged;

        }




        //#endregion

        //#region Property


        //public List<IntegerDimension> WallHeights
        //{
        //    get => this._wallHeights;
        //    set => this.SetProperty(ref this._wallHeights, value);
        //}

        ///// <summary>
        ///// Gets the first wall height.
        ///// </summary>
        //public int FirstWallHeight => this.LastWallHeight.Size - this.HPitching;

        ///// <summary>
        ///// Gets or sets the wall type.
        ///// </summary>
        //public WallTypePoco TimberWallTypePoco
        //{
        //    get => this._timberWallTypePoco;
        //    set
        //    {
        //        this.SetProperty(ref this._timberWallTypePoco, value);
        //        this.OnWallTypeChanged();
        //        this.RaisePropertyChanged(nameof(this.IsRakedArea));
        //        //this.RaisePropertyChanged(nameof(this.SubStud));
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the ceiling raised.
        ///// </summary>
        //public new int CeilingRaised
        //{
        //    get => this._ceilingRaised;
        //    set
        //    {
        //        this.SetProperty(ref this._ceilingRaised, value);
        //        this.RaisePropertyChanged(nameof(this.LastWallHeight));
        //        this.RaisePropertyChanged(nameof(this.WallRaiseHeight));
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the wall thickness.
        ///// </summary>
        //public IntegerDimension WallThickness
        //{
        //    get => this._wallThickness;
        //    set => this.SetProperty(ref this._wallThickness, value);
        //}

        ///// <summary>
        ///// Gets or sets the default info.
        ///// </summary>
        //public LevelGlobalWallInfo GlobalWallInfo
        //{
        //    get => this._defaultInfo;
        //    set => this.SetProperty(ref this._defaultInfo, value);
        //}

        ///// <summary>
        ///// Gets or sets the stud spacing.
        ///// </summary>
        //public IntegerDimension StudSpacing
        //{
        //    get => this._studSpacing;
        //    set => this.SetProperty(ref this._studSpacing, value);
        //}

        ///// <summary>
        ///// Gets or sets the run length.
        ///// </summary>
        //public new int RunLength
        //{
        //    get => this._runLength;
        //    set
        //    {
        //        this._runLength = value;
        //        this.RaisePropertyChanged(nameof(this.IsRakedArea));
        //        //this.RaisePropertyChanged(nameof(this.SubStud));
        //        this.RaisePropertyChanged(nameof(this.HPitching));
        //        this.RaisePropertyChanged(nameof(this.LastWallHeight));
        //        this.RaisePropertyChanged(nameof(this.FirstWallHeight));
        //        this.RaisePropertyChanged(nameof(this.WallRaiseHeight));
        //    }
        //}

        ///// <summary>
        ///// Gets the h pitching.
        ///// </summary>
        //public new int HPitching { get; }
        //    //(int)Math.Ceiling((this.RunLength * Math.Tan(Convert.ToDouble((this.CeilingPitch) * Math.PI / 180))) / 5) * 5;


        ///// <summary>
        ///// Gets or sets the is step down.
        ///// </summary>
        //public int IsStepDown
        //{
        //    get => this._isStepDown;
        //    set
        //    {
        //        this.SetProperty(ref this._isStepDown, value);
        //        this.RaisePropertyChanged(nameof(this.LastWallHeight));
        //        this.RaisePropertyChanged(nameof(this.WallRaiseHeight));
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the is raised ceiling.
        ///// </summary>
        //public int IsRaisedCeiling
        //{
        //    get => this._isRaisedCeiling;
        //    set
        //    {
        //        this.SetProperty(ref this._isRaisedCeiling, value);
        //        this.RaisePropertyChanged(nameof(this.LastWallHeight));
        //        this.RaisePropertyChanged(nameof(this.WallRaiseHeight));
        //    }
        //}

        ///// <summary>
        ///// Gets the sub stud.
        ///// </summary>
        ////public int SubStud
        ////{
        ////    get
        ////    {
        ////        if (this.TimberWallTypePoco.IsLoadBearingWall || this.TimberWallTypePoco.IsRaked || this.IsRakedArea || this.WallPitchingHeight < this.GlobalWallInfo.InternalWallHeight)
        ////        {
        ////            return 0;
        ////        }
        ////        else
        ////        {
        ////            return -35;
        ////        }
        ////    }
        ////}

        ///// <summary>
        ///// Gets a value indicating whether is raked area.
        ///// </summary>
        //public bool IsRakedArea => this.RunLength != 0 || this.TimberWallTypePoco.IsRaked;

        ///// <summary>
        ///// Gets or sets the wall height.
        ///// </summary>
        //[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1407:ArithmeticExpressionsMustDeclarePrecedence", Justification = "Reviewed. Suppression is OK here.")]
        //public IntegerDimension LastWallHeight
        //{
        //    get => this._lastWallHeight;
        //    set => this.SetProperty(ref this._lastWallHeight, value);
        //}

        ///// <summary>
        ///// Gets the wall raise height.
        ///// </summary>
        ////public int WallRaiseHeight =>
        ////    this.HPitching + this.StepDown.Size * this.IsStepDown + this.CeilingRaised * this.IsRaisedCeiling
        ////    + this.SubStud;

        ///// <summary>
        ///// Gets or sets the wall length.
        ///// </summary>
        //public double WallLength
        //{
        //    get => Math.Round(this.TempLength, 0) + this.ExtraLength;
        //}

        ///// <summary>
        ///// Gets or sets the extra length.
        ///// </summary>
        //public double ExtraLength
        //{
        //    get => this._extraLength;
        //    set
        //    {
        //        this.SetProperty(ref this._extraLength, value);
        //        this.RaisePropertyChanged(nameof(this.WallLength));
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the temp length.
        ///// </summary>
        //public double TempLength
        //{
        //    get => this._tempLength;
        //    set
        //    {
        //        this.SetProperty(ref this._tempLength, value);
        //        this.RaisePropertyChanged(nameof(this.WallLength));
        //    }
        //}
        //#endregion
        //#region public method

        ///// <summary>
        ///// The change wall thickness.
        ///// </summary>
        //public void ChangeWallThickness()
        //{
        //    if (this.WallThickness.IsDefaultValue)
        //    {
        //        this.SetWallThickness();
        //    }
        //}

        ///// <summary>
        ///// The compare to.
        ///// </summary>
        ///// <param name="other">
        ///// The other.
        ///// </param>
        ///// <returns>
        ///// The <see cref="int"/>.
        ///// </returns>
        public int CompareTo(WallLayer other)
        {
            //return this.TimberWallTypePoco.CompareTo(other.TimberWallTypePoco);
            return 0;
        }

        //#endregion

        //#region Private Method

        ///// <summary>
        ///// The initialize wall member.
        ///// </summary>
        //private void InitializeWallMember()
        //{
        //    this.RibbonPlate = new WallMember();
        //    this.TopPlate = new WallMember();
        //    this.Stud = new WallStud();
        //    this.BottomPlate = new WallMember();
        //}

        ///// <summary>
        ///// The last wall height property changed.
        ///// </summary>
        ///// <param name="sender">
        ///// The sender.
        ///// </param>
        ///// <param name="e">
        ///// The e.
        ///// </param>
        //private void LastWallHeightPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "Size")
        //    {
        //        this.RaisePropertyChanged(nameof(this.FirstWallHeight));
        //    }
        //}

        ///// <summary>
        ///// The wall layer initialize.
        ///// </summary>
        //private void InitializeWallLayer()
        //{
        //    if (this.TimberWallTypePoco.IsLoadBearingWall)
        //    {
        //        this.StudSpacing = new IntegerDimension(this.GlobalWallInfo.ExternalWallSpacing.Size);
        //        this.WallThickness = new IntegerDimension(this.GlobalWallInfo.ExternalWallThickness);
        //        this.PitchingHeight = new IntegerDimension(this.GlobalWallInfo.WallHeight);

        //    }
        //    else
        //    {
        //        this.StudSpacing = new IntegerDimension(this.GlobalWallInfo.InternalWallSpacing.Size);
        //        this.WallThickness = new IntegerDimension(this.GlobalWallInfo.InternalWallThickness);
        //        this.PitchingHeight = new IntegerDimension(this.GlobalWallInfo.InternalWallHeight);
        //    }
        //    this.WallHeights = new List<IntegerDimension>();
        //    this.LastWallHeight = new IntegerDimension();
        //    this.WallHeights.Add(this.PitchingHeight);
        //    this.WallHeights.Add(this.LastWallHeight);

        //    this.RoofPitch = new DoubleDimension(this.GlobalWallInfo.RoofPitch.Size);
        //    this.StepDown = new IntegerDimension(this.GlobalWallInfo.StepDown.Size);
        //}

        ///// <summary>
        ///// The stud spacing property changed.
        ///// </summary>
        ///// <param name="sender">
        ///// The sender.
        ///// </param>
        ///// <param name="e">
        ///// The e.
        ///// </param>
        //private void StudSpacingPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName != "Size")
        //    {
        //        return;
        //    }

        //    if (this.TimberWallTypePoco.IsLoadBearingWall)
        //    {
        //        this.StudSpacing.IsDefaultValue = this.StudSpacing.Size == this.GlobalWallInfo.ExternalWallSpacing.Size;

        //    }
        //    else
        //    {
        //        this.StudSpacing.IsDefaultValue = this.StudSpacing.Size == this.GlobalWallInfo.InternalWallSpacing.Size;
        //    }
        //}

        ///// <summary>
        ///// The wall thickness property changed.
        ///// </summary>
        ///// <param name="sender">
        ///// The sender.
        ///// </param>
        ///// <param name="e">
        ///// The e.
        ///// </param>
        //private void WallThicknessPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName != "Size")
        //    {
        //        return;
        //    }

        //    if (this.TimberWallTypePoco.IsLoadBearingWall)
        //    {

        //        this.WallThickness.IsDefaultValue = this.WallThickness.Size == this.GlobalWallInfo.ExternalWallThickness;

        //    }
        //    else
        //    {
        //        this.WallThickness.IsDefaultValue = this.WallThickness.Size == this.GlobalWallInfo.InternalWallThickness;
        //    }
        //}

        ///// <summary>
        ///// The step down property changed.
        ///// </summary>
        ///// <param name="sender">
        ///// The sender.
        ///// </param>
        ///// <param name="e">
        ///// The e.
        ///// </param>
        //private void StepDownPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName != "Size")
        //    {
        //        return;
        //    }
        //    this.StepDown.IsDefaultValue = this.StepDown.Size == this.GlobalWallInfo.StepDown.Size;
        //    this.RaisePropertyChanged(nameof(this.WallHeight));
        //    this.RaisePropertyChanged(nameof(this.WallRaiseHeight));
        //}

        ///// <summary>
        ///// The pitching height property changed.
        ///// </summary>
        ///// <param name="sender">
        ///// The sender.
        ///// </param>
        ///// <param name="e">
        ///// The e.
        ///// </param>
        //private void PitchingHeightPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName != "Size")
        //    {
        //        return;
        //    }
        //    if (this.TimberWallTypePoco.IsLoadBearingWall)
        //    {
        //        this.PitchingHeight.IsDefaultValue = this.PitchingHeight.Size == this.GlobalWallInfo.WallHeight;
        //    }
        //    else
        //    {
        //        this.PitchingHeight.IsDefaultValue = this.PitchingHeight.Size == this.GlobalWallInfo.InternalWallHeight;
        //    }
        //    this.RaisePropertyChanged(nameof(this.WallHeight));
        //    this.RaisePropertyChanged(nameof(this.SubStud));
        //    this.RaisePropertyChanged(nameof(this.WallRaiseHeight));
        //}

        ///// <summary>
        ///// The roof pitch property changed.
        ///// </summary>
        ///// <param name="sender">
        ///// The sender.
        ///// </param>
        ///// <param name="e">
        ///// The e.
        ///// </param>
        //private void RoofPitchPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName != "Size")
        //    {
        //        return;
        //    }

        //    this.RoofPitch.IsDefaultValue = this.RoofPitch.Size.Equals(this.GlobalWallInfo.RoofPitch.Size);
        //    this.RaisePropertyChanged(nameof(this.HPitching));
        //}

        ///// <summary>
        ///// The default info property changed.
        ///// </summary>
        ///// <param name="sender">
        ///// The sender.
        ///// </param>
        ///// <param name="e">
        ///// The e.
        ///// </param>
        //private void DefaultInfoPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{

        //    switch (e.PropertyName)
        //    {
        //        case "ExternalWallThickness":
        //        case "InternalWallThickness":
        //            {
        //                if (this.WallThickness.IsDefaultValue)
        //                {
        //                    // this.SetWallThickness();
        //                }

        //                break;
        //            }

        //        case "WallHeight":
        //        case "InternalWallHeight":
        //            {
        //                if (this.PitchingHeight.IsDefaultValue)
        //                {
        //                    this.SetWallPitchingHeight();
        //                }
        //                this.RaisePropertyChanged(nameof(this.SubStud));
        //                this.RaisePropertyChanged(nameof(this.WallRaiseHeight));
        //                break;
        //            }

        //        case "StepDown":
        //            {
        //                if (this.StepDown.IsDefaultValue)
        //                {
        //                    this.SetStepDown();
        //                }

        //                break;
        //            }

        //        case "RoofPitch":
        //            {
        //                if (this.RoofPitch.IsDefaultValue)
        //                {
        //                    this.SetRoofPitch();
        //                }

        //                break;
        //            }

        //        case "ExternalWallSpacing":
        //        case "InternalWallSpacing":
        //            {
        //                if (this.StudSpacing.IsDefaultValue)
        //                {
        //                    this.SetWallSpacing();
        //                }

        //                break;
        //            }

        //        default:
        //            break;
        //    }
        //}



        ///// <summary>
        ///// The on wall type changed.
        ///// </summary>
        //private void OnWallTypeChanged()
        //{
        //    if (this.CheckSpacingThicknessStepDownNull())
        //    {
        //        this.InitializeWallLayer();
        //    }
        //    else
        //    {
        //        if (this.StudSpacing.IsDefaultValue)
        //        {
        //            this.SetWallSpacing();
        //        }

        //        if (this.WallThickness.IsDefaultValue)
        //        {
        //            this.SetWallThickness();
        //        }

        //        if (this.PitchingHeight.IsDefaultValue)
        //        {
        //            this.SetWallPitchingHeight();
        //        }

        //        if (this.StepDown.IsDefaultValue)
        //        {
        //            this.SetStepDown();
        //        }

        //        if (this.RoofPitch.IsDefaultValue)
        //        {
        //            this.SetRoofPitch();
        //        }

        //        this.RaisePropertyChanged(nameof(this.LastWallHeight));
        //        this.RaisePropertyChanged(nameof(this.WallRaiseHeight));
        //    }
        //}

        ///// <summary>
        ///// The check null.
        ///// </summary>
        ///// <returns>
        ///// The <see cref="bool"/>.
        ///// </returns>
        //private bool CheckSpacingThicknessStepDownNull()
        //{
        //    return this.StudSpacing == null || this.WallThickness == null || this.StepDown == null
        //           || this.RoofPitch == null;
        //}

        ///// <summary>
        ///// The set wall spacing.
        ///// </summary>
        //private void SetWallSpacing()
        //{
        //    this.StudSpacing.Size = this.TimberWallTypePoco.IsLoadBearingWall
        //                                       ? this.GlobalWallInfo.ExternalWallSpacing.Size
        //                                       : this.GlobalWallInfo.InternalWallSpacing.Size;
        //}

        ///// <summary>
        ///// The set wall pitching height.
        ///// </summary>
        //private void SetWallPitchingHeight()
        //{
        //    if (this.TimberWallTypePoco.IsLoadBearingWall)
        //    {
        //        if (this.GlobalWallInfo.WallHeight != null)
        //        {
        //            this.PitchingHeight.Size = (int)this.GlobalWallInfo.WallHeight;
        //        }
        //    }
        //    else
        //    {
        //        if (this.GlobalWallInfo.InternalWallHeight != null)
        //        {
        //            this.PitchingHeight.Size = (int)this.GlobalWallInfo.InternalWallHeight;
        //        }
        //    }
        //}

        ///// <summary>
        ///// The set wall thickness.
        ///// </summary>
        //private void SetWallThickness()
        //{
        //    if (this.TimberWallTypePoco.IsLoadBearingWall)
        //    {
        //        if (this.GlobalWallInfo.ExternalWallThickness != null)
        //        {
        //            this.WallThickness.Size = (int)this.GlobalWallInfo.ExternalWallThickness;
        //        }
        //    }
        //    else
        //    {
        //        if (this.GlobalWallInfo.InternalWallThickness != null)
        //        {
        //            this.WallThickness.Size = (int)this.GlobalWallInfo.InternalWallThickness;
        //        }
        //    }
        //}

        ///// <summary>
        ///// The set step down.
        ///// </summary>
        //private void SetStepDown()
        //{
        //    this.StepDown.Size = this.GlobalWallInfo.StepDown.Size;
        //}

        ///// <summary>
        ///// The set roof pitch.
        ///// </summary>
        //private void SetRoofPitch()
        //{
        //    this.RoofPitch.Size = this.GlobalWallInfo.RoofPitch.Size;

        //}

        //#endregion


    }
}
