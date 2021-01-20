using ProtoBuf;

namespace AppModels.PocoDataModel.Bracing
{
    [ProtoContract]
    public class BracingPoco
    {
        [ProtoMember(1)]
        public BracingBasePoco BracingInfo { get; set; }
        [ProtoMember(2)]
        public int Quantity { get; set; }
    }
}
