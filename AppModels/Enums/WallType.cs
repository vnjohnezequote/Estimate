using ProtoBuf;

namespace AppModels.Enums
{
    [ProtoContract]
    public enum WallType
    {
        [ProtoEnum]
        External_LBW,
        [ProtoEnum]
        External_NonLBW,
        [ProtoEnum]
        Internal_LBW,
        [ProtoEnum]
        Internal_NonLBW
    }
}