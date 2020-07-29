using AppModels.Enums;
using AppModels.Interaface;
using ProtoBuf;

namespace AppModels.PocoDataModel.WallMemberData
{
    [ProtoContract]
    public class BeamPoco: WallMemberBasePoco
    {
        [ProtoMember(5)]
        public string Location { get; set; }
        [ProtoMember(6)]
        public string NamePrefix { get; set; }
        [ProtoMember(7)]
        public BeamType Type { get; set; }

        public BeamPoco(): base()
        {

        }
        public BeamPoco(IWallMemberInfo memberInfo) : base(memberInfo)
        {
        }
    }
}
