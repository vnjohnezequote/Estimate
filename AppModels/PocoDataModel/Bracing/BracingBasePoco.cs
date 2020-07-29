using ProtoBuf;

namespace AppModels.PocoDataModel.Bracing
{
    [ProtoContract]
    public class BracingBasePoco
    {
        [ProtoMember(1)]
        public int Width { get; set; }
        [ProtoMember(2)]
        public int Height { get; set; }
        [ProtoMember(3)]
        public int Thickness { get; set; }
        [ProtoMember(4)]
        public double UnitPrice { get; set; }
    }
}
