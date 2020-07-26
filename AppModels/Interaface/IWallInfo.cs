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
        IGlobalWallDetail GlobalWallDetailInfo { get; }
        int Id { get; }
        int WallThickness { get; }
        LayerItem WallColorLayer { get; }
        int WallPitchingHeight { get; }
        int WallEndHeight { get; }
        int WallHeight { get; }
        bool IsRakedWall { get; }
        bool IsWallUnderFlatCeiling { get; }
        bool IsShorWall { get; }
        int RunLength { get; }
        int CeilingPitch { get; }
        int HPitching { get; }
        bool IsStepdown { get; }
        int StepDown { get; }
        bool IsRaisedCeiling { get; }
        int RaisedCeiling { get; }
        bool IsParallelWithRoofFrame { get;}
        double WallLength { get; }
    }
}