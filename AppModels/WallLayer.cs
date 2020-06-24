// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Layer.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the Layer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AppModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The wall layer.
    /// </summary>
    public class WallLayer : WallBase, IComparable<WallLayer>
    {

        #region Private field

        /// <summary>
        /// The wall heights.
        /// </summary>
        private List<IntegerDimension> wallHeights;

        /// <summary>
        /// The wall type.
        /// </summary>
        private WallType timberWallType;

        /// <summary>
        /// The wall thickness.
        /// </summary>
        private IntegerDimension wallThickness;

        /// <summary>
        /// The default info.
        /// </summary>
        private LevelWallDefaultInfo defaultInfo;

        /// <summary>
        /// The stud spacing.
        /// </summary>
        private IntegerDimension studSpacing;

        /// <summary>
        /// The is step down.
        /// </summary>
        private int isStepDown;

        /// <summary>
        /// The run length.
        /// </summary>
        private int runLength;

        /// <summary>
        /// The is raised ceiling.
        /// </summary>
        private int isRaisedCeiling;

        /// <summary>
        /// The extra length.
        /// </summary>
        private double extraLength;

        /// <summary>
        /// The temp length.
        /// </summary>
        private double tempLength;

        /// <summary>
        /// The ceiling raised.
        /// </summary>
        private int ceilingRaised;

        /// <summary>
        /// The wall height.
        /// </summary>
        private IntegerDimension lastWallHeight;


        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="WallLayer"/> class.
        /// </summary>
        /// <param name="wallId">
        /// The wall Id.
        /// </param>
        /// <param name="wallType">
        /// The wall type.
        /// </param>
        /// <param name="defaultInfo">
        /// The default info.
        /// </param>
        public WallLayer(int wallId, WallType wallType, LevelWallDefaultInfo defaultInfo)
        {
            this.Id = wallId;
            this.DefaultInfo = defaultInfo;
            this.TimberWallType = wallType;

            // this.InitializeWallLayer();
            this.InitializeWallMember();
            this.DefaultInfo.PropertyChanged += this.DefaultInfoPropertyChanged;

            this.StudSpacing.PropertyChanged += this.StudSpacingPropertyChanged;
            this.WallThickness.PropertyChanged += this.WallThicknessPropertyChanged;
            this.StepDown.PropertyChanged += this.StepDownPropertyChanged;
            this.PitchingHeight.PropertyChanged += this.PitchingHeightPropertyChanged;
            this.RoofPitch.PropertyChanged += this.RoofPitchPropertyChanged;
            this.LastWallHeight.PropertyChanged += this.LastWallHeightPropertyChanged;

        }




        #endregion

        #region Property
        /// <summary>
        /// Gets or sets the wall heights.
        /// </summary>
        public List<IntegerDimension> WallHeights
        {
            get => this.wallHeights;
            set => this.SetProperty(ref this.wallHeights, value);
        }

        /// <summary>
        /// Gets the first wall height.
        /// </summary>
        public int FirstWallHeight => this.LastWallHeight.Size - this.HPitching;

        /// <summary>
        /// Gets or sets the wall type.
        /// </summary>
        public WallType TimberWallType
        {
            get => this.timberWallType;
            set
            {
                this.SetProperty(ref this.timberWallType, value);
                this.OnWallTypeChanged();
                this.RaisePropertyChanged(nameof(this.IsRakedArea));
                this.RaisePropertyChanged(nameof(this.SubStud));
            }
        }

        /// <summary>
        /// Gets or sets the ceiling raised.
        /// </summary>
        public new int CeilingRaised
        {
            get => this.ceilingRaised;
            set
            {
                this.SetProperty(ref this.ceilingRaised, value);
                this.RaisePropertyChanged(nameof(this.LastWallHeight));
                this.RaisePropertyChanged(nameof(this.WallRaiseHeight));
            }
        }

        /// <summary>
        /// Gets or sets the wall thickness.
        /// </summary>
        public IntegerDimension WallThickness
        {
            get => this.wallThickness;
            set => this.SetProperty(ref this.wallThickness, value);
        }

        /// <summary>
        /// Gets or sets the default info.
        /// </summary>
        public LevelWallDefaultInfo DefaultInfo
        {
            get => this.defaultInfo;
            set => this.SetProperty(ref this.defaultInfo, value);
        }

        /// <summary>
        /// Gets or sets the stud spacing.
        /// </summary>
        public IntegerDimension StudSpacing
        {
            get => this.studSpacing;
            set => this.SetProperty(ref this.studSpacing, value);
        }

        /// <summary>
        /// Gets or sets the run length.
        /// </summary>
        public new int RunLength
        {
            get => this.runLength;
            set
            {
                this.runLength = value;
                this.RaisePropertyChanged(nameof(this.IsRakedArea));
                this.RaisePropertyChanged(nameof(this.SubStud));
                this.RaisePropertyChanged(nameof(this.HPitching));
                this.RaisePropertyChanged(nameof(this.LastWallHeight));
                this.RaisePropertyChanged(nameof(this.FirstWallHeight));
                this.RaisePropertyChanged(nameof(this.WallRaiseHeight));
            }
        }

        /// <summary>
        /// Gets the h pitching.
        /// </summary>
        public new int HPitching =>
            (int)Math.Ceiling((this.RunLength * Math.Tan(this.RoofPitch.Size * Math.PI / 180)) / 5) * 5;


        /// <summary>
        /// Gets or sets the is step down.
        /// </summary>
        public int IsStepDown
        {
            get => this.isStepDown;
            set
            {
                this.SetProperty(ref this.isStepDown, value);
                this.RaisePropertyChanged(nameof(this.LastWallHeight));
                this.RaisePropertyChanged(nameof(this.WallRaiseHeight));
            }
        }

        /// <summary>
        /// Gets or sets the is raised ceiling.
        /// </summary>
        public int IsRaisedCeiling
        {
            get => this.isRaisedCeiling;
            set
            {
                this.SetProperty(ref this.isRaisedCeiling, value);
                this.RaisePropertyChanged(nameof(this.LastWallHeight));
                this.RaisePropertyChanged(nameof(this.WallRaiseHeight));
            }
        }

        /// <summary>
        /// Gets the sub stud.
        /// </summary>
        public int SubStud
        {
            get
            {
                if (this.TimberWallType.IsLBW || this.TimberWallType.IsRaked || this.IsRakedArea || this.PitchingHeight.Size < this.DefaultInfo.InternalWallHeight)
                {
                    return 0;
                }
                else
                {
                    return -35;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether is raked area.
        /// </summary>
        public bool IsRakedArea => this.RunLength != 0 || this.TimberWallType.IsRaked;

        /// <summary>
        /// Gets or sets the wall height.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1407:ArithmeticExpressionsMustDeclarePrecedence", Justification = "Reviewed. Suppression is OK here.")]
        public IntegerDimension LastWallHeight
        {
            get => this.lastWallHeight;
            set => this.SetProperty(ref this.lastWallHeight, value);
        }

        /// <summary>
        /// Gets the wall raise height.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1407:ArithmeticExpressionsMustDeclarePrecedence", Justification = "Reviewed. Suppression is OK here.")]
        public int WallRaiseHeight =>
            this.HPitching + this.StepDown.Size * this.IsStepDown + this.CeilingRaised * this.IsRaisedCeiling
            + this.SubStud;

        /// <summary>
        /// Gets or sets the wall length.
        /// </summary>
        public double WallLength
        {
            get => Math.Round(this.TempLength, 0) + this.ExtraLength;
        }

        /// <summary>
        /// Gets or sets the extra length.
        /// </summary>
        public double ExtraLength
        {
            get => this.extraLength;
            set
            {
                this.SetProperty(ref this.extraLength, value);
                this.RaisePropertyChanged(nameof(this.WallLength));
            }
        }

        /// <summary>
        /// Gets or sets the temp length.
        /// </summary>
        public double TempLength
        {
            get => this.tempLength;
            set
            {
                this.SetProperty(ref this.tempLength, value);
                this.RaisePropertyChanged(nameof(this.WallLength));
            }
        }
        #endregion
        #region public method

        /// <summary>
        /// The change wall thickness.
        /// </summary>
        public void ChangeWallThickness()
        {
            if (this.WallThickness.IsDefaultValue)
            {
                this.SetWallThickness();
            }
        }

        /// <summary>
        /// The compare to.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int CompareTo(WallLayer other)
        {
            return this.TimberWallType.CompareTo(other.TimberWallType);
        }

        #endregion

        #region Private Method

        /// <summary>
        /// The initialize wall member.
        /// </summary>
        private void InitializeWallMember()
        {
            this.RibbonPlate = new WallMemberBase();
            this.TopPlate = new WallMemberBase();
            this.Stud = new WallStud();
            this.BottomPlate = new WallMemberBase();
        }

        /// <summary>
        /// The last wall height property changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void LastWallHeightPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Size")
            {
                this.RaisePropertyChanged(nameof(this.FirstWallHeight));
            }
        }

        /// <summary>
        /// The wall layer initialize.
        /// </summary>
        private void InitializeWallLayer()
        {
            if (this.TimberWallType.IsLBW)
            {
                this.StudSpacing = new IntegerDimension(this.DefaultInfo.ExternalWallSpacing.Size);
                this.WallThickness = new IntegerDimension(this.DefaultInfo.ExternalWallThickness);
                this.PitchingHeight = new IntegerDimension(this.DefaultInfo.ExternalWallHeight);

            }
            else
            {
                this.StudSpacing = new IntegerDimension(this.DefaultInfo.InternalWallSpacing.Size);
                this.WallThickness = new IntegerDimension(this.DefaultInfo.InternalWallThickness);
                this.PitchingHeight = new IntegerDimension(this.DefaultInfo.InternalWallHeight);
            }
            this.WallHeights = new List<IntegerDimension>();
            this.LastWallHeight = new IntegerDimension();
            this.WallHeights.Add(this.PitchingHeight);
            this.WallHeights.Add(this.LastWallHeight);

            this.RoofPitch = new DoubleDimension(this.DefaultInfo.RoofPitch.Size);
            this.StepDown = new IntegerDimension(this.DefaultInfo.StepDown.Size);
        }

        /// <summary>
        /// The stud spacing property changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void StudSpacingPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Size")
            {
                return;
            }

            if (this.TimberWallType.IsLBW)
            {
                this.StudSpacing.IsDefaultValue = this.StudSpacing.Size == this.DefaultInfo.ExternalWallSpacing.Size;

            }
            else
            {
                this.StudSpacing.IsDefaultValue = this.StudSpacing.Size == this.DefaultInfo.InternalWallSpacing.Size;
            }
        }

        /// <summary>
        /// The wall thickness property changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WallThicknessPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Size")
            {
                return;
            }

            if (this.TimberWallType.IsLBW)
            {

                this.WallThickness.IsDefaultValue = this.WallThickness.Size == this.DefaultInfo.ExternalWallThickness;

            }
            else
            {
                this.WallThickness.IsDefaultValue = this.WallThickness.Size == this.DefaultInfo.InternalWallThickness;
            }
        }

        /// <summary>
        /// The step down property changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void StepDownPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Size")
            {
                return;
            }
            this.StepDown.IsDefaultValue = this.StepDown.Size == this.DefaultInfo.StepDown.Size;
            this.RaisePropertyChanged(nameof(this.WallHeight));
            this.RaisePropertyChanged(nameof(this.WallRaiseHeight));
        }

        /// <summary>
        /// The pitching height property changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void PitchingHeightPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Size")
            {
                return;
            }
            if (this.TimberWallType.IsLBW)
            {
                this.PitchingHeight.IsDefaultValue = this.PitchingHeight.Size == this.DefaultInfo.ExternalWallHeight;
            }
            else
            {
                this.PitchingHeight.IsDefaultValue = this.PitchingHeight.Size == this.DefaultInfo.InternalWallHeight;
            }
            this.RaisePropertyChanged(nameof(this.WallHeight));
            this.RaisePropertyChanged(nameof(this.SubStud));
            this.RaisePropertyChanged(nameof(this.WallRaiseHeight));
        }

        /// <summary>
        /// The roof pitch property changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void RoofPitchPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Size")
            {
                return;
            }

            this.RoofPitch.IsDefaultValue = this.RoofPitch.Size.Equals(this.DefaultInfo.RoofPitch.Size);
            this.RaisePropertyChanged(nameof(this.HPitching));
        }

        /// <summary>
        /// The default info property changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void DefaultInfoPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            switch (e.PropertyName)
            {
                case "ExternalWallThickness":
                case "InternalWallThickness":
                    {
                        if (this.WallThickness.IsDefaultValue)
                        {
                            // this.SetWallThickness();
                        }

                        break;
                    }

                case "ExternalWallHeight":
                case "InternalWallHeight":
                    {
                        if (this.PitchingHeight.IsDefaultValue)
                        {
                            this.SetWallPitchingHeight();
                        }
                        this.RaisePropertyChanged(nameof(this.SubStud));
                        this.RaisePropertyChanged(nameof(this.WallRaiseHeight));
                        break;
                    }

                case "StepDown":
                    {
                        if (this.StepDown.IsDefaultValue)
                        {
                            this.SetStepDown();
                        }

                        break;
                    }

                case "RoofPitch":
                    {
                        if (this.RoofPitch.IsDefaultValue)
                        {
                            this.SetRoofPitch();
                        }

                        break;
                    }

                case "ExternalWallSpacing":
                case "InternalWallSpacing":
                    {
                        if (this.StudSpacing.IsDefaultValue)
                        {
                            this.SetWallSpacing();
                        }

                        break;
                    }

                default:
                    break;
            }
        }



        /// <summary>
        /// The on wall type changed.
        /// </summary>
        private void OnWallTypeChanged()
        {
            if (this.CheckSpacingThicknessStepDownNull())
            {
                this.InitializeWallLayer();
            }
            else
            {
                if (this.StudSpacing.IsDefaultValue)
                {
                    this.SetWallSpacing();
                }

                if (this.WallThickness.IsDefaultValue)
                {
                    this.SetWallThickness();
                }

                if (this.PitchingHeight.IsDefaultValue)
                {
                    this.SetWallPitchingHeight();
                }

                if (this.StepDown.IsDefaultValue)
                {
                    this.SetStepDown();
                }

                if (this.RoofPitch.IsDefaultValue)
                {
                    this.SetRoofPitch();
                }

                this.RaisePropertyChanged(nameof(this.LastWallHeight));
                this.RaisePropertyChanged(nameof(this.WallRaiseHeight));
            }
        }

        /// <summary>
        /// The check null.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CheckSpacingThicknessStepDownNull()
        {
            return this.StudSpacing == null || this.WallThickness == null || this.StepDown == null
                   || this.RoofPitch == null;
        }

        /// <summary>
        /// The set wall spacing.
        /// </summary>
        private void SetWallSpacing()
        {
            this.StudSpacing.Size = this.TimberWallType.IsLBW
                                               ? this.DefaultInfo.ExternalWallSpacing.Size
                                               : this.DefaultInfo.InternalWallSpacing.Size;
        }

        /// <summary>
        /// The set wall pitching height.
        /// </summary>
        private void SetWallPitchingHeight()
        {
            if (this.TimberWallType.IsLBW)
            {
                if (this.DefaultInfo.ExternalWallHeight != null)
                {
                    this.PitchingHeight.Size = (int)this.DefaultInfo.ExternalWallHeight;
                }
            }
            else
            {
                if (this.DefaultInfo.InternalWallHeight != null)
                {
                    this.PitchingHeight.Size = (int)this.DefaultInfo.InternalWallHeight;
                }
            }
        }

        /// <summary>
        /// The set wall thickness.
        /// </summary>
        private void SetWallThickness()
        {
            if (this.TimberWallType.IsLBW)
            {
                if (this.DefaultInfo.ExternalWallThickness != null)
                {
                    this.WallThickness.Size = (int)this.DefaultInfo.ExternalWallThickness;
                }
            }
            else
            {
                if (this.DefaultInfo.InternalWallThickness != null)
                {
                    this.WallThickness.Size = (int)this.DefaultInfo.InternalWallThickness;
                }
            }
        }

        /// <summary>
        /// The set step down.
        /// </summary>
        private void SetStepDown()
        {
            this.StepDown.Size = this.DefaultInfo.StepDown.Size;
        }

        /// <summary>
        /// The set roof pitch.
        /// </summary>
        private void SetRoofPitch()
        {
            this.RoofPitch.Size = this.DefaultInfo.RoofPitch.Size;

        }

        #endregion


    }
}
