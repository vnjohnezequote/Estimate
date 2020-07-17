using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.ResponsiveData;
using ProtoBuf;

namespace AppModels.PocoDataModel
{
    [ProtoContract]
    public class OpeningPoco
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public OpeningType OpeningType { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public string FullName { get; set; }
        [ProtoMember(5)]
        public string ShortName { get; set; }
        [ProtoMember(6)]
        public int Height { get; set; }
        [ProtoMember(7)]
        public int Width { get; set; }
        [ProtoMember(8)]
        public int WallThickness { get; set; }
        [ProtoMember(9)]
        public BeamPoco Lintel { get; set; }

    }
}
