using Newtonsoft.Json;

namespace AppModels.Interaface
{
    [JsonObject(IsReference = true)]
    public interface IWallMember
    {
        IWallMemberInfo TimberMaterialInfo { get; }
        IWallInfo WallInfo { get; }
        int Thickness { get; }
    }
}