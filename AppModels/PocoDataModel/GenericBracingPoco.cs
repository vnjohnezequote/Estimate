using ProtoBuf;

namespace AppModels.PocoDataModel
{
    [ProtoContract]
    public class GenericBracingPoco
    {
        [ProtoMember(1)] public GenericBracingBasePoco BracingInfo { get; set; }
        [ProtoMember(1)] public int Quantity { get; set; }
    }
}
