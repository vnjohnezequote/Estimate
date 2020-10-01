using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.PocoDataModel.Bracing;
using AppModels.PocoDataModel.Openings;
using AppModels.PocoDataModel.WallMemberData;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Openings;
using ProtoBuf;

namespace AppModels.PocoDataModel
{
    [ProtoContract]
    public class LevelWallPoco
    {
        public string LevelName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the total wall length.
        /// </summary>
        public double TotalWallLength
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the lintel lm.
        /// </summary>
        public double LintelLm
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the layers.
        /// </summary>
        public List<WallLayerPoco> WallLayers
        {
            get; set;
        }

        public List<ResponsiveData.Bracing> TimberWallBracings
        {
            get; set;
        }
        public List<GenericBracing> GeneralBracings
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the cost delivery.
        /// </summary>
        public int CostDelivery
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the roof beams.
        /// </summary>
        public List<BeamPoco> RoofBeams
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the opening.
        /// </summary>
        public List<OpeningPoco> Openings
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the level info.
        /// </summary>
        public GlobalWallInfoPoco LevelInfo
        {
            get; set;
        }

        public LevelWallPoco()
        {

        }
        public LevelWallPoco(LevelWall level)
        {
            LevelName = level.LevelName;
            TotalWallLength = level.TotalWallLength;
            LintelLm = level.LintelLm;
            CostDelivery = level.CostDelivery;
            LevelInfo = new GlobalWallInfoPoco(level.LevelInfo);
            this.InitializerWallLayers(level.WallLayers.ToList());
            //this.InitializerTimberBracings(level.TimberWallBracings.ToList());
            this.TimberWallBracings = level.TimberWallBracings.ToList();
            this.GeneralBracings = level.GeneralBracings.ToList();
            //this.InitializerGeneralBracings(level.GeneralBracings.ToList());
            this.InitializerRoofBeams(level.RoofBeams.ToList());
            this.InitializerOpenings(level.Openings.ToList());
            
        }

        public void InitializerWallLayers(List<WallBase> wallLayers)
        {
            WallLayers = new List<WallLayerPoco>();
            foreach (var wallLayer in wallLayers)
            {
                var wallLayerPoco = new WallLayerPoco(wallLayer);
                WallLayers.Add(wallLayerPoco);
            }
        }

        public void InitializerTimberBracings(List<ResponsiveData.Bracing> timberBracings)
        {
            //TimberWallBracings = new List<BracingPoco>();
        }

        public void InitializerGeneralBracings(List<GenericBracing> genericBracings)
        {
            //GeneralBracings = new List<GenericBracingPoco>();
        }

        public void InitializerRoofBeams(List<Beam> roofBeams)
        {
            RoofBeams = new List<BeamPoco>();
            foreach (var roofBeam in roofBeams)
            {
                var rBeam = new BeamPoco(roofBeam);
                RoofBeams.Add(rBeam);
            }
        }

        public void InitializerOpenings(List<Opening> openings)
        {
            Openings = new List<OpeningPoco>();
            foreach (var opening in openings)
            {
                var openingPoco = new OpeningPoco(opening);
                Openings.Add(openingPoco);
            }
        }
    }
}
