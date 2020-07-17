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
    public class BracingPoco
    {
        [ProtoMember(1)]
        public BracingBase BracingInfo { get; set; }
        [ProtoMember(2)]
        public int Quantity { get; set; }
    }
}
