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
        TrussBeam,
        [ProtoEnum]
        FloorBeam,
        [ProtoEnum]
        RafterBeam

    }
}