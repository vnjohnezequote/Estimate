﻿using System.ComponentModel;
using AppModels.Enums;
using AppModels.ResponsiveData;

namespace AppModels.Interaface
{
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

        IBasicWallInfo GlobalExternalWallInfo { get; }
        IBasicWallInfo GlobalInternalWallInfo { get; }
        IGlobalWallDetail GlobalExtWallDetailInfo { get;  }
        IGlobalWallDetail GlobalIntWallDetailInfo { get;  }
        IWallMemberInfo GlobalNoggingInfo { get;  }
        IWallMemberInfo GlobalDoorJambInfo { get;  }


    }
}