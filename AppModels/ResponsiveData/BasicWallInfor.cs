using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    public class BasicWallInfor: BindableBase,IBasicWallInfo
    {
        public NoggingMethodType NoggingMethod => GlobalWallInfo?.NoggingMethod ?? NoggingMethodType.AsWall;

        public IGlobalWallInfo GlobalWallInfo { get; set; }
        public WallTypePoco WallType { get; }

        public int WallSpacing
        {
            get
            {
                if (GlobalWallInfo==null)
                {
                    return 0;
                }
                return WallType.IsLoadBearingWall
                    ? GlobalWallInfo.ExternalWallSpacing
                    : GlobalWallInfo.InternalWallSpacing;
            }

        }

        public int WallThickness
        {
            get
            {
                if (GlobalWallInfo==null)
                {
                    return 0;
                }

                return WallType.IsLoadBearingWall
                    ? GlobalWallInfo.ExternalWallThickness
                    : GlobalWallInfo.InternalWallThickness;
            }
        }
        public int Depth {
            get
            {
                if (GlobalWallInfo==null)
                {
                    return 0;
                }

                return WallType.IsLoadBearingWall
                    ? GlobalWallInfo.ExternalWallTimberDepth
                    : GlobalWallInfo.InternalWallTimberDepth;
            }
            }

        public string TimberGrade
        {
            get
            {
                if (GlobalWallInfo == null)
                {
                    return null;
                }

                return WallType.IsLoadBearingWall
                    ? GlobalWallInfo.ExternalWallTimberGrade
                    : GlobalWallInfo.InternalWallTimberGrade;
            }
        }

        public BasicWallInfor(IGlobalWallInfo globalWallInfo, WallTypePoco wallType)
        {
            WallType = wallType;
            GlobalWallInfo = globalWallInfo;
            if (GlobalWallInfo!=null)
            {
                GlobalWallInfo.PropertyChanged += GlobalWallInfo_PropertyChanged;
            }
            
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