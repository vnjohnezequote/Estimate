using System.ComponentModel;
using AppModels.Enums;
using AppModels.ResponsiveData;
using Newtonsoft.Json;

namespace AppModels.Interaface
{
    [JsonObject(IsReference = true)]
    public interface IGlobalWallInfo : INotifyPropertyChanged
    {
        IGlobalWallInfo GlobalWallInformation { get; }
        NoggingMethodType NoggingMethod { get; }
        double CeilingPitch { get; set; }
        int ExternalDoorHeight { get; }
        int InternalDoorHeight { get; }
        int ExternalWallSpacing { get; }
        int InternalWallSpacing { get; }
        int ExternalWallThickness { get; }
        int InternalWallThickness { get; }
        int WallHeight { get; }
        int StepDown { get; set; }
        int RaisedCeilingHeight { get; set; }
        int ExternalWallTimberDepth { get; }
        int InternalWallTimberDepth { get; }
        string ExternalWallTimberGrade { get; }
        string InternalWallTimberGrade { get; }
        Suppliers Supplier { get; }

        IBasicWallInfo GlobalExternalWallInfo { get; }
        IBasicWallInfo GlobalInternalWallInfo { get; }
        IGlobalWallDetail GlobalExtWallDetailInfo { get;  }
        IGlobalWallDetail GlobalIntWallDetailInfo { get;  }
        IWallMemberInfo GlobalNoggingInfo { get;  }
        IWallMemberInfo GlobalDoorJambInfo { get;  }


    }
}