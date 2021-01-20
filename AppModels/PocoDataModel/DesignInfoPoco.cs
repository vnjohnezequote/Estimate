using LiteDB;
using ProtoBuf;

namespace AppModels.PocoDataModel
{
    public class DesignInfoPoco
    {
        [BsonId]
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string InfoType { get; set; }
        [ProtoMember(3)]
        public string Content { get; set; }
        [ProtoMember(4)]
        public string Header { get; set; }
    }
}
