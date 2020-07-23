using System.ComponentModel;
using AppModels.Enums;
using AppModels.ResponsiveData;

namespace AppModels.Interaface
{
    public interface IGlobalWallInfo : INotifyPropertyChanged
    {
        IGlobalWallInfo GlobalWallInformation { get; }
        NoggingMethodType NoggingMethod { get; }
        double CeilingPitch { get; }
        int ExternalDoorHeight { get; }
        int InternalDoorHeight { get; }
        int ExternalWallSpacing { get; }
        int InternalWallSpacing { get; }
        int ExternalWallThickness { get; }
        int InternalWallThickness { get; }
        int WallHeight { get; }
        int StepDown { get; }
        int RaisedCeilingHeight { get; }
        int ExternalWallTimberDepth { get; }
        int InternalWallTimberDepth { get; }
        string ExternalWallTimberGrade { get; }
        string InternalWallTimberGrade { get; }
    }
}