using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace AppModels.PocoDataModel
{
    [ProtoContract]
    public class JobWallDefaultInfoPoco
    {
        [ProtoMember(1)]
        public int ExternalWallSpacing { get; set; }
        [ProtoMember(2)]
        public int? InternalWallSpacing { get; set; }
        [ProtoMember(3)]
        public int StepDown { get; set; }
        [ProtoMember(4)]
        public double? RoofPitch { get; set; }
        [ProtoMember(5)]
        public TimberBasePoco ExternalWallMaterial { get; set; }
        [ProtoMember(6)]
        public TimberBasePoco InternalWallMaterial { get; set; }
        [ProtoMember(6)]
        public double ExternalWallHeight { get; set; }
        [ProtoMember(6)]
        public double InternalWallHeight { get; set; }
        public TimberBasePoco GlobalNoggingMaterial { get; set; }
    }
}
