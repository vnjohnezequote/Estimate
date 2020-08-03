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
    public interface IWallMemberInfo:ITimberInfo,INotifyPropertyChanged
    {
        WallTypePoco WallType { get; }
        IWallMemberInfo BaseMaterialInfo { get; }
        WallMemberType MemberType { get; }
        void LoadMemberInfo(WallMemberBasePoco wallMember);
    }
}
