using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel.Framings.Beams;
using AppModels.PocoDataModel.Framings.FloorAndRafter;
using AppModels.PocoDataModel.Openings;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.ResponsiveData.Openings;

namespace AppModels.ResponsiveData.Framings.FloorAndRafters.Beam
{
    public class FBeam: Openings.Beam,IContaintHanger,IContaintOutTrigger
    {
        #region Properties
        public Hanger HangerA { get; set; }
        public Hanger HangerB { get; set; }
       

        public OutTrigger OutTriggerA { get; set; }
        public OutTrigger OutTriggerB { get; set; }
       

        #endregion
        public FBeam(FramingTypes beamType, LevelWall level) : base(beamType, level)
        {
        }

        public FBeam(FramingSheet framingSheet) : base(framingSheet)
        {
        }

        public FBeam(FBeamPoco beamPoco,FramingSheet framingSheet, List<TimberBase> timberList, List<EngineerMemberInfo> engineerMemberInfos) : base(beamPoco,framingSheet, timberList, engineerMemberInfos)
        {
        }

        public FBeam(Openings.Beam another) : base(another)
        {
        }
        public void LoadHangers(List<Hanger> hangers, FramingBasePoco joistPoco)
        {
            foreach (var hanger in hangers)
            {
                if (hanger.Id == ((FBeamPoco)joistPoco).HangerAId)
                {
                    HangerA = hanger;
                }

                if (hanger.Id == ((FBeamPoco)joistPoco).HangerBId)
                {
                    HangerB = hanger;
                }
            }
        }

        public void LoadOutTriggers(List<IFraming> outTriggers, FramingBasePoco joistPoco)
        {
            foreach (var outTrigger in outTriggers)
            {
                if (outTrigger.Id == ((FBeamPoco)joistPoco).OutTriggerAId)
                {
                    OutTriggerA = (OutTrigger)outTrigger;
                }

                if (outTrigger.Id == ((FBeamPoco)joistPoco).OutTriggerBId)
                {
                    OutTriggerB = (OutTrigger)outTrigger;
                }
            }
        }
        public override object Clone()
        {
            return new FBeam(this);
        }
    }
}
