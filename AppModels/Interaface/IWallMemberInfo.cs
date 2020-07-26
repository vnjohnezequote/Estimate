using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.PocoDataModel;

namespace AppModels.Interaface
{
    public interface IWallMemberInfo:INotifyPropertyChanged
    {
        WallTypePoco WallType { get; }
        IWallMemberInfo BaseMaterialInfo { get; }
        int NoItem { get; }
        int Thickness { get; }
        int Depth { get; }
        string TimberGrade { get; }
        WallMemberType MemberType { get; }
        string Size { get; }
        string SizeGrade { get; }
    }
}
