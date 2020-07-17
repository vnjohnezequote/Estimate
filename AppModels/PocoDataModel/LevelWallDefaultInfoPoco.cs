using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.ResponsiveData;
using ProtoBuf;

namespace AppModels.PocoDataModel
{
    [ProtoContract]
    public class LevelWallDefaultInfoPoco
    {
        [ProtoMember(1)]
        public int? ExternalWallHeight { get; set; }
        [ProtoMember(2)]
        public int? InternalWallHeight { get; set; }
        [ProtoMember(3)]
        public int? ExternalWallThickness { get; set; }
        [ProtoMember(4)]
        public int? InternalWallThickness { get; set; }
        [ProtoMember(5)]
        public IntegerDimension StepDown { get; set; }
        [ProtoMember(6)]
        public DoubleDimension RoofPitch { get; set; }
        [ProtoMember(7)]
        public JobWallDefaultInfoPoco JobInfo { get; set; }
        [ProtoMember(8)]
        public IntegerDimension ExternalWallSpacing { get; set; }
        [ProtoMember(9)]
        public IntegerDimension InternalWallSpacing { get; set; }
       
    }
}
