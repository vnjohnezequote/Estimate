using System;
using System.Collections.Generic;
using System.Linq;
using AppModels.Interaface;
using AppModels.PocoDataModel.Framings;
using AppModels.PocoDataModel.Openings;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Framings;
using AppModels.ResponsiveData.Openings;
using ProtoBuf;

namespace AppModels.PocoDataModel
{
    [ProtoContract]
    public class LevelWallPoco
    {
        public Guid Id { get; set; }
        public string LevelName
        {
            get; set;
        }
        public double TotalWallLength
        {
            get; set;
        }
        public double LintelLm
        {
            get; set;
        }
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
        public List<FramingSheetPoco> FramingSheets { get; set; }
        public int CostDelivery
        {
            get; set;
        }
        public List<BeamPoco> RoofBeams
        {
            get; set;
        }
        public List<OpeningPoco> Openings
        {
            get; set;
        }
        public GlobalWallInfoPoco LevelInfo
        {
            get; set;
        }
        public LevelWallPoco()
        {

        }
        public LevelWallPoco(LevelWall level)
        {
            Id = level.Id;
            LevelName = level.LevelName;
            TotalWallLength = level.TotalWallLength;
            LintelLm = level.LintelLm;
            CostDelivery = level.CostDelivery;
            LevelInfo = new GlobalWallInfoPoco(level.LevelInfo);
            this.InitializerWallLayers(level.WallLayers.ToList());
            this.TimberWallBracings = level.TimberWallBracings.ToList();
            this.GeneralBracings = level.GeneralBracings.ToList();
            this.InitializerRoofBeams(level.RoofBeams.ToList());
            this.InitializerOpenings(level.Openings.ToList());
            this.InitializerFramings(level.FramingSheets.ToList());
            
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
        public void InitializerRoofBeams(List<IFraming> roofBeams)
        {
            RoofBeams = new List<BeamPoco>();
            foreach (var roofBeam in roofBeams)
            {
                var rBeam = new BeamPoco((Beam)roofBeam);
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

        public void InitializerFramings(List<FramingSheet> framingSheets)
        {
            FramingSheets = new List<FramingSheetPoco>();
            foreach (var framingSheet in framingSheets)
            {
                var framingSheetPoco = new FramingSheetPoco(framingSheet);
                FramingSheets.Add(framingSheetPoco);
            }
        }
    }
}
