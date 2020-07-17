using ProtoBuf;

namespace AppModels.Enums
{
    [ProtoContract]
    public enum OpeningType
    {
        [ProtoEnum]
        Door,
        Window
    }
}