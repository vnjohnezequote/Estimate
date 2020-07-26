using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    public class BasicWallInfor: BindableBase,IBasicWallInfo
    {
        public NoggingMethodType NoggingMethod => GlobalWallInfo.NoggingMethod;
        public IGlobalWallInfo GlobalWallInfo { get; private set; }
        public WallTypePoco WallType { get; }
        public int WallSpacing => WallType.IsLoadBearingWall
            ? GlobalWallInfo.ExternalWallSpacing
            : GlobalWallInfo.InternalWallSpacing;

        public int WallThickness => WallType.IsLoadBearingWall
            ? GlobalWallInfo.ExternalWallThickness
            : GlobalWallInfo.InternalWallThickness;
        public int Depth => WallType.IsLoadBearingWall
            ? GlobalWallInfo.ExternalWallTimberDepth
            : GlobalWallInfo.InternalWallTimberDepth;
        public string TimberGrade=>WallType.IsLoadBearingWall
            ? GlobalWallInfo.ExternalWallTimberGrade
            : GlobalWallInfo.InternalWallTimberGrade;

        public BasicWallInfor(IGlobalWallInfo globalWallInfo, WallTypePoco wallType)
        {
            WallType = wallType;
            GlobalWallInfo = globalWallInfo;
            GlobalWallInfo.PropertyChanged += GlobalWallInfo_PropertyChanged;
        }

        private void GlobalWallInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ExternalWallThickness":
                case "InternalWallThickness":
                    RaisePropertyChanged(nameof(WallThickness));
                    break;
                case "ExternalWallSpacing":
                case "InternalWallSpacing":
                    RaisePropertyChanged(nameof(WallSpacing));
                    break;
                case "ExternalWallTimberDepth":
                case "InternalWallTimberDepth":
                    RaisePropertyChanged(nameof(Depth));
                    break;
                case "ExternalWallTimberGrade":
                case "InternalWallTimberGrade":
                    RaisePropertyChanged(nameof(TimberGrade));
                    break;
                case "NoggingMethod":
                    RaisePropertyChanged(nameof(NoggingMethod));
                    RaisePropertyChanged(nameof(WallThickness));
                    RaisePropertyChanged(nameof(Depth));
                    RaisePropertyChanged(nameof(TimberGrade));
                    break;
            }
        }
    }
}