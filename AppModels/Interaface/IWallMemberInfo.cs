using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.PocoDataModel;
using AppModels.PocoDataModel.WallMemberData;
using Newtonsoft.Json;

namespace AppModels.Interaface
{
    [JsonObject(IsReference = true)]
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

        void LoadMemberInfo(WallMemberBasePoco wallMember);
    }
}
