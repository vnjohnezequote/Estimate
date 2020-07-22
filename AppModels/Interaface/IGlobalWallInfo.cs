using System.ComponentModel;
using AppModels.Enums;
using AppModels.ResponsiveData;

namespace AppModels.Interaface
{
    public interface IGlobalWallInfo : INotifyPropertyChanged
    {
        NoggingMethodType NoggingMethod { get; }
        int Thickness { get;  }
        int Depth { get; }
        string TimberGrade { get; }
        double CeilingPitch { get; }


        WallMember RibbonPlate { get;  }
        WallMember TopPlate { get; }
        WallMember Stud { get; }
        WallMember Nogging { get; }
        WallMember Trimmer { get; }
        

    }
}