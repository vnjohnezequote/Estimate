using ProtoBuf;

namespace AppModels.Enums
{
    [ProtoContract]
    public enum WallType
    {
        [ProtoEnum]
        LBW,
        [ProtoEnum]
        NonLBW,
    }
}