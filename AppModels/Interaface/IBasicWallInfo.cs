using System.ComponentModel;
using AppModels.Enums;
using AppModels.PocoDataModel;
using Newtonsoft.Json;

namespace AppModels.Interaface
{
    [JsonObject(IsReference = true)]
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