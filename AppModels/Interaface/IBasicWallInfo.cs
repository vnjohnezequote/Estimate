using System.ComponentModel;
using AppModels.Enums;
using AppModels.PocoDataModel;

namespace AppModels.Interaface
{
    public interface IBasicWallInfo: INotifyPropertyChanged
    {
        NoggingMethodType NoggingMethod { get; }
        IGlobalWallInfo GlobalWallInfo { get; }
        WallTypePoco WallType { get; }

        int WallThickness { get; }
        int Depth { get; }
        string TimberGrade { get; }
        int WallSpacing { get; }
    }
}