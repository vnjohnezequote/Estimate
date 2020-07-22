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


        WallMemberInfo RibbonPlate { get;  }
        WallMemberInfo TopPlate { get; }
        WallMemberInfo Stud { get; }
        WallMemberInfo Nogging { get; }
        WallMemberInfo Trimmer { get; }
        

    }
}