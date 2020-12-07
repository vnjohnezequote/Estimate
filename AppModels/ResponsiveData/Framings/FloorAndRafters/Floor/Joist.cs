using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AppModels.CustomEntity;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.PocoDataModel.Framings.FloorAndRafter;
using AppModels.PocoDataModel.Openings;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Openings;
using devDept.Geometry;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Framings.FloorAndRafters.Floor
{
    public class Joist: FramingBase,IContaintHanger,IContaintOutTrigger
    {
        public override double QuoteLength
        {
            get
            {
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
                    HangerA = hanger;
                }

                if (hanger.Id == ((JoistPoco)joistPoco).HangerBId)
                {
                    HangerB = hanger;
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
