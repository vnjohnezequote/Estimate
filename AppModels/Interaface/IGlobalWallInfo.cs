using System.ComponentModel;
using AppModels.Enums;
using AppModels.ResponsiveData;
using Newtonsoft.Json;

namespace AppModels.Interaface
{
    [JsonObject(IsReference = true)]
    public interface IGlobalWallInfo : INotifyPropertyChanged
    {
        JobInfo GlobalInfo { get; }
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
        string WindRate { get; }
        string Customer { get; }
        int TrussSpacing { get; }
        int RafterSpacing { get; }
        string BuilderName { get; }
        int RoofOverHang { get; }
        bool QuoteCeilingBattent { get ; }
        CeilingBattensType CeilingBattensType { get;  } 
        string Treatment { get; }
        string RoofMaterial { get; }
        string TieDown { get; }
        int QuoteTolengthSize { get; }
        bool JambBeamSupport { get; }
        bool NoggingsAndSillInLM { get; }
        bool UpToLength { get;  }
        RoofFrameType RoofFrameType { get; }
        Suppliers Supplier { get; }

        IBasicWallInfo GlobalExternalWallInfo { get; }
        IBasicWallInfo GlobalInternalWallInfo { get; }
        IGlobalWallDetail GlobalExtWallDetailInfo { get;  }
        IGlobalWallDetail GlobalIntWallDetailInfo { get;  }
        IWallMemberInfo GlobalNoggingInfo { get;  }
        IWallMemberInfo GlobalDoorJambInfo { get;  }


    }
}