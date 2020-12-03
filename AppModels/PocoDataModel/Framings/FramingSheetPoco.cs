using System;
using System.Collections.Generic;
using System.Linq;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel.Framings.Beams;
using AppModels.PocoDataModel.Framings.Blocking;
using AppModels.PocoDataModel.Framings.FloorAndRafter;
using AppModels.PocoDataModel.Openings;
using AppModels.ResponsiveData.Framings;
using AppModels.ResponsiveData.Framings.Blocking;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Beam;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.ResponsiveData.Openings;

namespace AppModels.PocoDataModel.Framings
{
    public class FramingSheetPoco
    {
        public Guid Id { get; set; }
        public Guid LevelId { get; set; }
        public int Index { get; set; }
        public int FramingSpacing { get; set; }
        public bool ShowSheetId { get; set; }
        public string FloorName { get; set; }
        public FramingSheetTypes FramingSheetType { get; set; }
        public List<JoistPoco> Joists { get; set; } 
        public List<FBeamPoco> Beams { get; set; } 
        public List<OutTriggerPoco> OutTriggers { get; set; }
        public List<HangerPoco> Hangers { get; set; } 
        public List<BlockingPoco> Blockings { get; set; }
        public List<TieDownPoco> TieDowns { get; set; }

        public FramingSheetPoco()
        {

        }
        public FramingSheetPoco(FramingSheet framingSheet)
        {
            Id = framingSheet.Id;
            LevelId = framingSheet.LevelId;
            Index = framingSheet.Index;
            FramingSpacing = framingSheet.FramingSpacing;
            ShowSheetId = framingSheet.ShowSheetId;
            FloorName = framingSheet.FloorName;
            FramingSheetType = framingSheet.FramingSheetType;
            this.LoadJoists(framingSheet.Joists.ToList());
            this.LoadBeams(framingSheet.Beams.ToList());
            this.LoadOutTriggers(framingSheet.OutTriggers.ToList());
            this.LoadHangers(framingSheet.Hangers.ToList());
            this.LoadBlockings(framingSheet.Blockings.ToList());
            this.LoadTieDowns(framingSheet.TieDowns.ToList());
        }

        private void LoadJoists(List<IFraming> joists)
        {
            Joists = new List<JoistPoco>();
            foreach (var joist in joists)
            {
                var joistPoco = new JoistPoco((Joist)joist);
                Joists.Add(joistPoco);
            }
        }

        private void LoadBeams(List<IFraming> beams)
        {
            Beams = new List<FBeamPoco>();
            foreach (var beam in beams)
            {
                var beamPoco = new FBeamPoco((FBeam)beam);
                Beams.Add(beamPoco);
            }
        }

        private void LoadOutTriggers(List<IFraming> outTriggers)
        {
            OutTriggers = new List<OutTriggerPoco>();
            foreach (var outTrigger in outTriggers)
            {
                var outTriggerPoco = new OutTriggerPoco((OutTrigger)outTrigger);
                OutTriggers.Add(outTriggerPoco);
            }
        }

        private void LoadHangers(List<Hanger> hangers)
        {
            Hangers = new List<HangerPoco>();
            foreach (var hanger in hangers)
            {
                var hangerPoco = new HangerPoco(hanger);
                Hangers.Add(hangerPoco);
            }
        }

        private void LoadBlockings(List<ResponsiveData.Framings.Blocking.Blocking> blockings)
        {
            Blockings = new List<BlockingPoco>();
            foreach (var blocking in blockings)
            {
                var blockingPoco = new BlockingPoco(blocking);
                Blockings.Add(blockingPoco);
            }
        }

        private void LoadTieDowns(List<TieDown> tieDowns)
        {
            TieDowns = new List<TieDownPoco>();
            foreach (var tieDown in tieDowns)
            {
                var tiewDownPoco = new TieDownPoco(tieDown);
                TieDowns.Add(tiewDownPoco);
            }
        }
    }
}
