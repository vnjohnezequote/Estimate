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
using AppModels.PocoDataModel;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The level wall default info.
    /// </summary>
    public class LevelGlobalWallInfo : BindableBase,IGlobalWallInfo
    {
        #region Private Field
        //private IGlobalWallInfo __globalWallInfo;
        private int _wallHeight;
        private int _externalDoorHeight;
        private int _internalDoorHeight;
        private int _externalWallSpacing;
        private int _internalWallSpacing;
        private int _extternalWallThickness;
        private int _internalWallThickness;
        
        #endregion
        #region public Property

        public JobInfo GlobalInfo { get; set; }
        public int WallHeight
        {
            get => _wallHeight == 0 ? GlobalWallInformation.WallHeight : _wallHeight;
            set
            {
                if (value == GlobalWallInformation.WallHeight)
                {
                    value = 0;
                }
                this.SetProperty(ref this._wallHeight, value);
            }
        }
        public int InternalWallHeight => WallHeight - 35;
        public int ExternalDoorHeight
        {
            get => _externalDoorHeight != 0 ? _externalDoorHeight : GlobalWallInformation.ExternalDoorHeight;
            set
            {
                if (value == GlobalWallInformation.ExternalDoorHeight)
                {
                    value = 0;
                }
                SetProperty(ref _externalDoorHeight, value);
            }
        }
        public int InternalDoorHeight
        {
            get => _internalDoorHeight !=0 ? _internalDoorHeight : GlobalWallInformation.InternalDoorHeight;
            set
            {
                if (value == GlobalWallInformation.InternalDoorHeight)
                {
                    value = 0;
                }
                SetProperty(ref _internalDoorHeight, value);
            }
        }
        public IGlobalWallInfo GlobalWallInformation { get; }
        public int ExternalWallSpacing
        {
            get => _externalWallSpacing != 0 ? _externalWallSpacing : GlobalWallInformation.ExternalWallSpacing;
            set 
            {
                if (value == GlobalWallInformation.ExternalWallSpacing)
                {
                    value = 0;
                }
                SetProperty(ref _externalWallSpacing, value);
            } 
        }
        public int InternalWallSpacing
        {
            get => _internalWallSpacing!=0 ? _internalWallSpacing : GlobalWallInformation.InternalWallSpacing;
            set
            {
                if (value== GlobalWallInformation.InternalWallSpacing)
                {
                    value = 0;
                }
                SetProperty(ref _internalWallSpacing, value);
            } 
        }
        public int ExternalWallThickness
        {
            get => _extternalWallThickness!= 0 ? _extternalWallThickness : GlobalWallInformation.ExternalWallThickness;
            set
            {
                if (value == GlobalWallInformation.ExternalWallThickness)
                {
                    value = 0;
                }
                SetProperty(ref _extternalWallThickness, value);
            } 
        }
        public int InternalWallThickness
        {
            get => _internalWallThickness!=0 ? _internalWallThickness : GlobalWallInformation.InternalWallThickness;
            set
            {
                if (value == GlobalWallInformation.InternalWallThickness)
                {
                    value = 0;
                }
                SetProperty(ref _internalWallThickness, value);
            }
        }
        public int ExternalWallTimberDepth => GlobalWallInformation.ExternalWallTimberDepth;
        public int InternalWallTimberDepth => GlobalWallInformation.InternalWallTimberDepth;
        public string ExternalWallTimberGrade => GlobalWallInformation.ExternalWallTimberGrade;
        public string InternalWallTimberGrade => GlobalWallInformation.InternalWallTimberGrade;
        public IBasicWallInfo GlobalExternalWallInfo { get;private set; }
        public IBasicWallInfo GlobalInternalWallInfo { get;private set; }
        public IGlobalWallDetail GlobalExtWallDetailInfo { get;private set; }
        public IGlobalWallDetail GlobalIntWallDetailInfo { get;private set; }
        public IWallMemberInfo GlobalNoggingInfo { get;private set; }
        public IWallMemberInfo GlobalDoorJambInfo { get;private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelGlobalWallInfo"/> class.
        /// </summary>
        /// <param name="globalWallInfo">
        /// The default info.
        /// </param>
        public LevelGlobalWallInfo(IGlobalWallInfo globalWallInfo)
        {
            this.GlobalWallInformation = globalWallInfo;
            GlobalExternalWallInfo = new BasicWallInfor(this, new WallTypePoco(){IsLoadBearingWall = true});
            GlobalInternalWallInfo = new BasicWallInfor(this, new WallTypePoco(){IsLoadBearingWall = false});
            if (GlobalWallInformation!=null)
            {
                GlobalNoggingInfo = GlobalWallInformation.GlobalNoggingInfo;
                GlobalExtWallDetailInfo = new LevelWallDetailInfo(GlobalExternalWallInfo, GlobalWallInformation.GlobalExtWallDetailInfo);
                GlobalIntWallDetailInfo = new LevelWallDetailInfo(GlobalInternalWallInfo, GlobalWallInformation.GlobalIntWallDetailInfo);
                GlobalDoorJambInfo = new GlobalWallMemberInfo(GlobalExternalWallInfo, WallMemberType.DoorJamb, GlobalWallInformation.GlobalDoorJambInfo);
                this.InitializeLevelDefault();
            }
            
            
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
                case "WallHeight":
                    RaisePropertyChanged(nameof(InternalWallHeight));
                    break;
                case "ExternalDoorHeight":
                    RaisePropertyChanged(nameof(ExternalDoorHeight));
                    RaisePropertyChanged(nameof(InternalDoorHeight));
                    break;
                case "InternalDoorHeight":
                    RaisePropertyChanged(nameof(InternalDoorHeight));
                    break;
                case "ExternalWallSpacing":
                    RaisePropertyChanged(nameof(ExternalWallSpacing));
                    RaisePropertyChanged(nameof(InternalWallSpacing));
                    break;
                case "InternalWallSpacing":
                    RaisePropertyChanged(nameof(InternalWallSpacing));
                    break;
                case "ExternalWallThickness":
                    RaisePropertyChanged(nameof(ExternalWallThickness));
                    RaisePropertyChanged(nameof(InternalWallThickness));
                    break;
                case "InternalWallThickness":
                    RaisePropertyChanged(nameof(InternalWallThickness));
                    break;
                case "StepDown":
                    RaisePropertyChanged(nameof(StepDown));
                    break;
                case "RoofPitch":
                case "CeilingPitch":
                    RaisePropertyChanged(nameof(CeilingPitch));
                    break;
                case nameof(RaisedCeilingHeight):
                    RaisePropertyChanged(nameof(RaisedCeilingHeight));
                    break;
                case nameof(ExternalWallTimberDepth):
                    RaisePropertyChanged(nameof(ExternalWallTimberDepth));
                    RaisePropertyChanged(nameof(InternalWallTimberDepth));
                    break;
                case nameof(InternalWallTimberDepth):
                    RaisePropertyChanged(nameof(InternalWallTimberDepth));
                    break;
                case nameof(ExternalWallTimberGrade):
                    RaisePropertyChanged(nameof(ExternalWallTimberGrade));
                    RaisePropertyChanged(nameof(InternalWallTimberGrade));
                    break;
                case nameof(InternalWallTimberGrade):
                    RaisePropertyChanged(nameof(InternalWallTimberGrade));
                    break;
            }
        }




        #endregion

    }
}
