using System.ComponentModel;
using AppModels.AppData;
using AppModels.Enums;
using AppModels.PocoDataModel;
using AppModels.ResponsiveData.WallMemberData;
using Newtonsoft.Json;

namespace AppModels.Interaface
{
    public interface IWallInfo:INotifyPropertyChanged
    {
        
        int Id { get; }
        WallTypePoco WallType { get; }
        NoggingMethodType NoggingMethod { get; }
        IGlobalWallInfo GlobalWallInfo { get; }
        IGlobalWallDetail GlobalWallDetailInfo { get; }
        LayerItem WallColorLayer { get; }
        int WallThickness { get; }
        int WallSpacing { get; }
        int WallPitchingHeight { get; }
        int WallEndHeight { get; }
        int WallHeight { get; }
        int FinalWallHeight { get; }
        int StudHeight { get; }
        bool IsWallUnderRakedArea { get; }
        bool ForcedWallUnderRakedArea { get; }
        double WallLength { get; }
        bool IsStepdown { get; }
        bool IsRaisedCeiling { get; }
        bool IsShortWall { get; }
        bool IsNeedTobeDesign { get; }
        int TypeId { get; }
        bool IsDesigned { get; }
        int RunLength { get; }
        double CeilingPitch { get; }
        int HPitching { get; }
        int StepDown { get; }
        int RaisedCeiling { get; }
        string WallName { get; }
        WallMemberBase RibbonPlate { get; }
        WallMemberBase TopPlate { get; }
        WallMemberBase BottomPlate { get; }
        WallStud Stud { get; }
        WallMemberBase Nogging { get; }
    }
}