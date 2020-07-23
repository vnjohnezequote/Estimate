// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelGlobalWallInfo.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the LevelWallDefaulInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using AppModels.Enums;
using AppModels.Interaface;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The level wall default info.
    /// </summary>
    public class LevelGlobalWallInfo : BindableBase,IGlobalWallInfo
    {
        #region Private Field
        private IGlobalWallInfo __globalWallInfo;
        private int _wallHeight;
        private string _externalDoorHeight;
        private string _internalDoorHeight;

        #endregion
        #region public Property

        public int WallHeight
        {
            get => this._wallHeight;
            set => this.SetProperty(ref this._wallHeight, value);
        }
        public string ExternalDoorHeight
        {
            get;
        }
        public string InternalDoorHeight
        {
            get;
        }
        public int StepDown => GlobalWallInformation.StepDown;
        public int RaisedCeilingHeight { get; }
        public string ExternalWallTimberDepth { get; }
        public string InternalWallTimberDepth { get; }
        public string ExternalWallTimberGrade { get; }
        public string InternalWallTimberGrade { get; }

        public IGlobalWallInfo GlobalWallInformation { get; }
        public NoggingMethodType NoggingMethod { get; }
        public double CeilingPitch => GlobalWallInformation.CeilingPitch;
        public string ExternalWallSpacing { get; }
        public string InternalWallSpacing { get; }
        public string ExternalWallThickness { get; }
        public string InternalWallThickness { get; }
        public GlobalWallDetailInfo GlobalExtWallDetailInfo { get; }
        public GlobalWallDetailInfo GlobalIntWallDetaiInfo { get; }
        public WallMemberInfo RibbonPlate { get; }
        public WallMemberInfo TopPlate { get; }
        public WallMemberInfo Stud { get; }
        public WallMemberInfo Nogging { get; }
        public WallMemberInfo Trimmer { get; }
        public WallMemberInfo BottomPlate { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelGlobalWallInfo"/> class.
        /// </summary>
        /// <param name="defaultDefaultInfo">
        /// The default info.
        /// </param>
        public LevelGlobalWallInfo(IGlobalWallInfo defaultDefaultInfo)
        {
            this.GlobalWallInformation = defaultDefaultInfo;
            this.InitializeLevelDefault();
        }
        

        #endregion
        

        #region private Method

        private void InitializeLevelDefault()
        {
            GlobalWallInformation.PropertyChanged += GlobalWallInfo_PropertyChanged;
        }

        private void GlobalWallInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ExternalDoorHeight":
                    RaisePropertyChanged(nameof(ExternalDoorHeight));
                    RaisePropertyChanged(nameof(InternalDoorHeight));
                    break;
                case "InternalDoorHeight":
                    RaisePropertyChanged(nameof(InternalDoorHeight));
                    break;
                case "StepDown":
                    RaisePropertyChanged(nameof(StepDown));
                    break;
                case "RoofPitch":
                case "CeilingPitch":
                    RaisePropertyChanged(nameof(CeilingPitch));
                    break;
            }
        }




        #endregion

    }
}
