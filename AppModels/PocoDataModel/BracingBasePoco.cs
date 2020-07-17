using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace AppModels.PocoDataModel
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
