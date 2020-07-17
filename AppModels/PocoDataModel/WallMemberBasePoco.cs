using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace AppModels.PocoDataModel
{
    [ProtoContract]
    [ProtoInclude(1, typeof(BeamPoco))]
    public class WallMemberBasePoco
    {
        [ProtoMember(1)]
        public ushort Id { get; set; }
        [ProtoMember(2)]
        public string TimberGrade { get; set; }
        [ProtoMember(3)]
        public ushort Quantity { get; set; }
        [ProtoMember(4)]
        public TimberBasePoco TimberInfo { get; set; }
        [ProtoMember(5)]
        public double Length { get; set; }
    }
}
