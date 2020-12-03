using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.PocoDataModel.Openings;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Beam;

namespace AppModels.PocoDataModel.Framings.Beams
{
    public class FBeamPoco: BeamPoco
    {
        public Guid? HangerAId { get; set; }
        public Guid? HangerBId { get; set; }
        public Guid? OutTriggerAId { get; set; }
        public Guid? OutTriggerBId { get; set; }
        public FBeamPoco()
        {

        }

        public FBeamPoco(FBeam beam):base(beam)
        {
            if (beam.HangerA != null)
            {
                HangerAId = beam.HangerA.Id;
            }
            if (beam.HangerB != null)
            {
                HangerBId = beam.HangerB.Id;
            }

            if (beam.OutTriggerA != null)
            {
                OutTriggerAId = beam.OutTriggerA.Id;
            }

            if (beam.OutTriggerB != null)
            {
                OutTriggerBId = beam.OutTriggerB.Id;
            }
        }
    }
}
