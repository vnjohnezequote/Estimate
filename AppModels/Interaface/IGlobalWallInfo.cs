using System.ComponentModel;
using AppModels.PocoDataModel;
using AppModels.ResponsiveData;
using Newtonsoft.Json;

namespace AppModels.Interaface
{
    [JsonObject(IsReference = true)]
    public interface IGlobalWallInfo : INotifyPropertyChanged
    {
        JobInfo GlobalInfo { get; }
        IGlobalWallInfo GlobalWallInformation { get; }
        //string LevelName{ get;}
        int ExternalDoorHeight { get; }
        int InternalDoorHeight { get; }
        int ExternalWallSpacing { get; }
        int InternalWallSpacing { get; }
        int ExternalWallThickness { get; }
        int InternalWallThickness { get; }
        int WallHeight { get; }
        int ExternalWallTimberDepth { get; }
        int InternalWallTimberDepth { get; }
        string ExternalWallTimberGrade { get; }
        string InternalWallTimberGrade { get; }
        int JoistSpacing { get; }
        IBasicWallInfo GlobalExternalWallInfo { get; }
        IBasicWallInfo GlobalInternalWallInfo { get; }
        IGlobalWallDetail GlobalExtWallDetailInfo { get;  }
        IGlobalWallDetail GlobalIntWallDetailInfo { get;  }
        IWallMemberInfo GlobalNoggingInfo { get;  }
        IWallMemberInfo GlobalDoorJambInfo { get;  }

        void LoadWallGlobalInfo(GlobalWallInfoPoco globalInfo);


    }
}