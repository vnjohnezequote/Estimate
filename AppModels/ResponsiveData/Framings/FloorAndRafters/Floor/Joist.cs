using System;
using System.Collections.Generic;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel.Framings.FloorAndRafter;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Openings;
using devDept.Geometry;

namespace AppModels.ResponsiveData.Framings.FloorAndRafters.Floor
{
    public class Joist: FramingBase,IContaintHanger,IContaintOutTrigger
    {
        
        protected override List<FramingTypes> FramingTypeAccepted { get; } = new List<FramingTypes>() {FramingTypes.RafterJoist,FramingTypes.FloorJoist,FramingTypes.BoundaryJoist,FramingTypes.CeilingJoist,FramingTypes.Purlin,FramingTypes.Fascia,FramingTypes.Trimmer,FramingTypes.Rimboard,FramingTypes.DeckJoist};
        public override double QuoteLength
        {
            get
            {
                //if (FramingType == FramingTypes.RafterJoist)
                //{
                    
                //}
                var fullLength = FullLength / Math.Cos(Utility.DegToRad(Pitch));
                var quoteLengthinMM = ((int)fullLength).RoundUpTo300();
                var quoteLength = (double) quoteLengthinMM / 1000 + ExtraLength;
                return quoteLength;
            }
       
        }
        
        public override object Clone()
        {
            return new Joist(this);
        }
        public Hanger HangerA { get; set; }
        public Hanger HangerB { get; set; }
        public OutTrigger OutTriggerA { get; set; }
        public OutTrigger OutTriggerB { get; set; }
        public Joist()
        { }

        public Joist(FramingSheet framingSheet) : base(framingSheet)
        {
            if (framingSheet.FramingSheetType == FramingSheetTypes.FloorFraming)
            {
                FramingType = FramingTypes.FloorJoist;
            }
            else if(framingSheet.FramingSheetType == FramingSheetTypes.RafterFraming)
            {
                FramingType = FramingTypes.RafterJoist;
            }
            else if (framingSheet.FramingSheetType == FramingSheetTypes.CeilingJoistFraming)
            {
                FramingType = FramingTypes.CeilingJoist;
            }
            else
            {
                FramingType = FramingTypes.Purlin;
            }
        }
        public Joist(JoistPoco joistPoco,FramingSheet framingSheet, List<TimberBase> timberList,List<EngineerMemberInfo> engineerMemberInfos) : base(joistPoco, framingSheet,timberList,engineerMemberInfos)
        {

        }

        public Joist(Joist another):base(another)
        {

        }

        public void LoadHangers(List<Hanger> hangers,FramingBasePoco joistPoco )
        {
            foreach (var hanger in hangers)
            {
                if (hanger.Id == ((JoistPoco)joistPoco).HangerAId)
                {
                    HangerA = (Hanger)hanger;
                }

                if (hanger.Id == ((JoistPoco)joistPoco).HangerBId)
                {
                    HangerB = (Hanger)hanger;
                }
            }
        }

        public void LoadOutTriggers(List<IFraming> outTriggers, FramingBasePoco joistPoco)
        {
            foreach (var outTrigger in outTriggers)
            {
                if (outTrigger.Id == ((JoistPoco)joistPoco).OutTriggerAId)
                {
                    OutTriggerA = (OutTrigger)outTrigger;
                }

                if (outTrigger.Id == ((JoistPoco)joistPoco).OutTriggerBId)
                {
                    OutTriggerB = (OutTrigger)outTrigger;
                }
            }
        }


    }
}
