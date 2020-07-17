using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using ProtoBuf;

namespace AppModels.PocoDataModel
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
    }
}
