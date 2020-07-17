using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace AppModels.PocoDataModel
{
    [ProtoContract]
    public class LevelWallPoco
    {
        [ProtoMember(1)]
        public string LevelName { get; set; }
        [ProtoMember(2)]
        public int TotalWallLength { get; set; }
        [ProtoMember(3)]
        public WallLayerPoco[] WallLayers { get; set; }
        [ProtoMember(4)]
        public BracingPoco[] TimberWallBracings { get; set; }
        [ProtoMember(5)]
        public GenericBracingPoco[] GeneralBracings { get; set; }
        [ProtoMember(6)]
        public int CostDelivery { get; set; }
        [ProtoMember(7)]
        public BeamPoco[] RoofBeams { get; set; }
        [ProtoMember(8)]
        public OpeningPoco[] Openings { get; set; }
        [ProtoMember(9)]
        public LevelWallDefaultInfoPoco[] LevelInfo { get; set; }
    }
}
