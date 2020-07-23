using System.ComponentModel;
using AppModels.Enums;

namespace AppModels.Interaface
{
    public interface IBasicWallInfo: INotifyPropertyChanged
    {
        NoggingMethodType NoggingMethod { get; }
        IGlobalWallInfo GlobalWallInfo { get; }
        WallType WallType { get; }

        string WallThickness { get; }
        string Depth { get; }
        string TimberGrade { get; }
        string WallSpacing { get; }
    }
}