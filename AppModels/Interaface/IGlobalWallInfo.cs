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
        string ExternalDoorHeight { get; }
        string InternalDoorHeight { get; }
        string ExternalWallSpacing { get; }
        string InternalWallSpacing { get; }
        string ExternalWallThickness { get; }
        string InternalWallThickness { get; }
        int WallHeight { get; }
        int StepDown { get; }
        int RaisedCeilingHeight { get; }
        string ExternalWallTimberDepth { get; }
        string InternalWallTimberDepth { get; }
        string ExternalWallTimberGrade { get; }
        string InternalWallTimberGrade { get; }
    }
}