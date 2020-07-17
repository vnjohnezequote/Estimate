using ProtoBuf;
using System.Runtime.Serialization;

namespace AppModels.Enums
{
    
    [ProtoContract]
    public enum BeamType
    {
        [ProtoEnum]
        Lintel,
        [ProtoEnum]
        RoofBeam,
        [ProtoEnum]
        FloorBeam
    }
}