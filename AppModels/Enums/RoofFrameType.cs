using ProtoBuf;

namespace AppModels.Enums
{
    [ProtoContract]
    public enum RoofFrameType
    {
        [ProtoEnum]
        Truss,
        [ProtoEnum]
        Rafter,
        [ProtoEnum]
        TrussAndRafter
    }
}