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
        string Thickness { get; }
        LayerItem WallColorLayer { get; }
        string Start_WallHeight { get; }
        int End_WallHeight { get; }
        int Show_WallHeight { get; }
        bool IsRakedWall { get; }
        bool IsWallUnderFlatCeiling { get; }
        bool IsShorWall { get; }
        int RunLength { get; }
        string CeilingPitch { get; }
        int HPitching { get; }
        bool IsStepdown { get; }
        string StepDown { get; }
        bool IsRaisedCeiling { get; }
        int RaisedCeiling { get; }
        bool IsParallelWithTruss { get;}
    }
}