using System.ComponentModel;
using AppModels.Enums;

namespace AppModels.Interaface
{
    public interface IBasicWallInfo: INotifyPropertyChanged
    {
        NoggingMethodType NoggingMethod { get; }
        IGlobalWallInfo GlobalWallInfo { get; }
        WallType WallType { get; }

        int WallThickness { get; }
        int Depth { get; }
        string TimberGrade { get; }
        int WallSpacing { get; }
    }
}