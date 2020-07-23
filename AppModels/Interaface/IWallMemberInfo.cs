using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;

namespace AppModels.Interaface
{
    public interface IWallMemberInfo:INotifyPropertyChanged
    {
        //IWallInfo WallInfo { get; }
        WallType WallType { get; }
        IWallMemberInfo BaseMaterialInfo { get; }
        string NoItem { get; }
        string Thickness { get; }
        string Depth { get; }
        string TimberGrade { get; }
        WallMemberType MemberType { get; }
    }
}
