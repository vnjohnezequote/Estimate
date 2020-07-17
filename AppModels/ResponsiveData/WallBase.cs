// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WallBase.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the WallInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AppModels.AppData;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The wall base.
    /// </summary>
    public abstract class WallBase : BindableBase
    {
        #region private field

        /// <summary>
        /// The id.
        /// </summary>
        private int _id;
        
        /// <summary>
        /// The roof pitch.
        /// </summary>
        private DoubleDimension _roofPitch;

        /// <summary>
        /// The pitching height.
        /// </summary>
        private IntegerDimension _pitchingHeight;

        /// <summary>
        /// The step down.
        /// </summary>
        private IntegerDimension _stepDown;

        /// <summary>
        /// The is sub stud.
        /// </summary>
        private int _isSubStud;

        /// <summary>
        /// The wall color layer.
        /// </summary>
        private LayerItem _wallColorLayer;

        #endregion

        #region Constructor




        #endregion
        #region Property

        /// <summary>
        /// Gets or sets the wall color layer.
        /// </summary>
        public LayerItem WallColorLayer
        {
            get => this._wallColorLayer;
            set => this.SetProperty(ref this._wallColorLayer, value);
        }

        /// <summary>
        /// Gets or sets the wall id.
        /// </summary>
        public int Id
        {
            get => this._id;
            set => this.SetProperty(ref this._id, value);
        }

        /// <summary>
        /// Gets or sets the pitching height.
        /// </summary>
        public IntegerDimension PitchingHeight
        {
            get => this._pitchingHeight;
            set => this.SetProperty(ref this._pitchingHeight, value);
        }

        /// <summary>
        /// Gets or sets the wall height.
        /// </summary>
        public int WallHeight { get; set; }
        

        /// <summary>
        /// Gets or sets the treatment.
        /// </summary>
        public string Treatment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is raked wall.
        /// </summary>
        public bool IsRakedWall { get; set; }


        /// <summary>
        /// Gets or sets the run length.
        /// </summary>
        public int RunLength { get; set; }

        /// <summary>
        /// Gets or sets the roof pitch.
        /// </summary>
        public DoubleDimension RoofPitch
        {
            get => this._roofPitch;
            set => this.SetProperty(ref this._roofPitch, value);
        }

        /// <summary>
        /// Gets or sets the is h pitching.
        /// </summary>
        public int IsHPitching { get; set; }

        /// <summary>
        /// Gets or sets the h pitching.
        /// = Run length * tan (roof Pitching)
        /// </summary>
        public int HPitching { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is step down.
        /// </summary>
        public bool IsStepdown { get; set; }

        /// <summary>
        /// Gets or sets the stepdown.
        /// </summary>
        public IntegerDimension StepDown
        {
            get => this._stepDown;
            set => this.SetProperty(ref this._stepDown, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether is ceiling raised.
        /// </summary>
        public bool IsCeilingRaised { get; set; }

        /// <summary>
        /// Gets or sets the ceiling raised.
        /// </summary>
        public int CeilingRaised { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is sub stud.
        /// </summary>
        public int IsSubStud
        {
            get => this._isSubStud;
            set => this.SetProperty(ref this._isSubStud, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether is parallel with truss.
        /// </summary>
        public bool IsParallelWithTruss { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is hight wall.
        /// </summary>
        public bool IsHightWall { get; set; }

        /// <summary>
        /// Gets or sets the sud stud.
        /// </summary>
        public int SudStud { get; set; }

        /// <summary>
        /// Gets or sets the is non load bearing wall.
        /// </summary>
        public int IsNonLbWall { get; set; }
        
        /// <summary>
        /// Gets or sets the ribbon plate.
        /// </summary>
        public WallMemberBase RibbonPlate { get; set; }

        /// <summary>
        /// Gets or sets the top plate.
        /// </summary>
        public WallMemberBase TopPlate { get; set; }

        /// <summary>
        /// Gets or sets the bottom plate.
        /// </summary>
        public WallMemberBase BottomPlate { get; set; }

        /// <summary>
        /// Gets or sets the stud.
        /// </summary>
        public WallStud Stud { get; set; }

        #endregion

        #region Private Method

        #endregion
    }
}
