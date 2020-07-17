using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace AppModels.PocoDataModel
{
    [ProtoContract]
    public class GenericBracingBasePoco
    {
        [ProtoMember(1)]
        public string Name { get; set; }
        [ProtoMember(1)]
        public double UnitPrice { get; set; }
    }
}
