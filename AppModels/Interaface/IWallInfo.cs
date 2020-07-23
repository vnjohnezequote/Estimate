using System.ComponentModel;
using AppModels.AppData;
using AppModels.Enums;

namespace AppModels.Interaface
{
    public interface IWallInfo:INotifyPropertyChanged
    {
        WallType WallType { get; }
        NoggingMethodType NoggingMethod { get; }
        IGlobalWallInfo GlobalWallInfo { get; }
        int Id { get; }
        string WallThickness { get; }
        LayerItem WallColorLayer { get; }
        string WallPitchingHeight { get; }
        int WallEndHeight { get; }
        int WallHeight { get; }
        bool IsRakedWall { get; }
        bool IsWallUnderFlatCeiling { get; }
        bool IsShorWall { get; }
        int RunLength { get; }
        string CeilingPitch { get; }
        int HPitching { get; }
        bool IsStepdown { get; }
        string StepDown { get; }
        bool IsRaisedCeiling { get; }
        string RaisedCeiling { get; }
        bool IsParallelWithRoofFrame { get;}
    }
}