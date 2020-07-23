using AppModels.Enums;
using AppModels.Interaface;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    public class BasicWallInfor: BindableBase,IBasicWallInfo
    {
        public NoggingMethodType NoggingMethod => GlobalWallInfo.NoggingMethod;
        public IGlobalWallInfo GlobalWallInfo { get; private set; }
        public WallType WallType { get; }
        public int WallSpacing => WallType == WallType.LBW
            ? GlobalWallInfo.ExternalWallSpacing
            : GlobalWallInfo.InternalWallSpacing;

        public int WallThickness => WallType == WallType.LBW
            ? GlobalWallInfo.ExternalWallThickness
            : GlobalWallInfo.InternalWallThickness;
        public int Depth => WallType == WallType.LBW
            ? GlobalWallInfo.ExternalWallTimberDepth
            : GlobalWallInfo.InternalWallTimberDepth;
        public string TimberGrade=>WallType == WallType.LBW
            ? GlobalWallInfo.ExternalWallTimberGrade
            : GlobalWallInfo.InternalWallTimberGrade;

        public BasicWallInfor(IGlobalWallInfo globalWallInfo, WallType wallType)
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