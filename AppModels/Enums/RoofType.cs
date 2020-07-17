using ProtoBuf;

namespace AppModels.Enums
{
    [ProtoContract]
    public enum RoofType
    {
        [ProtoEnum]
        Sheet,
        [ProtoEnum]
        Tile
    }
}