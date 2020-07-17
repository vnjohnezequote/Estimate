using ProtoBuf;

namespace AppModels.Enums
{
    [ProtoContract]
    public enum LoadBearingWallType
    {
        [ProtoEnum]
        LoadBearingWall,
        [ProtoEnum]
        NonLoadBearingWall
    }
}