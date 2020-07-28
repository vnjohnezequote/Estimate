using System.ComponentModel;
using AppModels.AppData;
using AppModels.Enums;
using AppModels.PocoDataModel;

namespace AppModels.Interaface
{
    public interface IWallInfo:INotifyPropertyChanged
    {
        WallTypePoco WallType { get; }
        NoggingMethodType NoggingMethod { get; }
        IGlobalWallInfo GlobalWallInfo { get; }
        IGlobalWallDetail GlobalWallDetailInfo { get; }
        int Id { get; }
        int WallThickness { get; }
        LayerItem WallColorLayer { get; }
        int WallPitchingHeight { get; }
        int WallEndHeight { get; }
        int WallHeight { get; }
        int PrenailWallHeight { get; }
        int StudHeight { get; }
        bool IsWallUnderRakedArea { get; }
        bool ForcedWallUnderRakedArea { get; }
        bool IsShortWall { get; }
        int RunLength { get; }
        double CeilingPitch { get; }
        int HPitching { get; }
        bool IsStepdown { get; }
        int StepDown { get; }
        bool IsRaisedCeiling { get; }
        int RaisedCeiling { get; }
        double WallLength { get; }
        string WallName { get; }
    }
}