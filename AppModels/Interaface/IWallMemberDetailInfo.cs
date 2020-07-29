using AppModels.Enums;
using Newtonsoft.Json;

namespace AppModels.Interaface
{
    [JsonObject(IsReference = true)]
    public interface IWallMemberDetailInfo:IWallMemberInfo
    {
       IWallInfo WallInfo { get; }
    }
}