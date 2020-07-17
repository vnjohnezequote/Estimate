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
    public class JobModelPoco
    {
        [ProtoMember(1)]
        public JobInfoPoco Info { get; set; }
        [ProtoMember(2)]
        public LevelWallPoco[] Levels { get; set; }
    }
}
