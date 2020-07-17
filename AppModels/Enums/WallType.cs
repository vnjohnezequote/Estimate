using ProtoBuf;

namespace AppModels.Enums
{
    [ProtoContract]
    public enum WallType
    {
        [ProtoEnum]
        External,
        [ProtoEnum]
        Internal
    }
}