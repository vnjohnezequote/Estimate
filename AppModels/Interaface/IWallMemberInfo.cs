using System.ComponentModel;
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
