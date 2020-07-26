using System.ComponentModel;
using AppModels.Enums;

namespace AppModels.Interaface
{
    public interface IGlobalWallDetail: INotifyPropertyChanged
    {
        WallType WallType { get; }
        int WallSpacing { get; }
        NoggingMethodType NoggingMethod { get; }
        IBasicWallInfo GlobalWallInfo { get; }
        IWallMemberInfo RibbonPlate { get;  }
        IWallMemberInfo TopPlate { get;  }
        IWallMemberInfo Stud { get;  }
        IWallMemberInfo BottomPlate { get; }
        IWallMemberInfo Nogging { get;}
        IWallMemberInfo Trimmer { get; }
    }
}