using System.ComponentModel;
using AppModels.Enums;
using AppModels.PocoDataModel;

namespace AppModels.Interaface
{
    public interface IGlobalWallDetail: INotifyPropertyChanged
    {
        WallTypePoco WallType { get; }
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