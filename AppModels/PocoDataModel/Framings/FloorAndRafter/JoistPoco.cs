using System;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;

namespace AppModels.PocoDataModel.Framings.FloorAndRafter
{
    public class JoistPoco : FramingBasePoco
    {
        public Guid? HangerAId { get; set; }
        public Guid? HangerBId { get; set; }
        public Guid? OutTriggerAId { get; set; }
        public Guid? OutTriggerBId { get; set; }

        public JoistPoco()
        {

        }
        public JoistPoco(Joist joist):base(joist)
        {
            if (joist.HangerA!=null)
            {
                HangerAId = joist.HangerA.Id;
            }
            if (joist.HangerB != null)
            {
                HangerBId = joist.HangerB.Id;
            }

            if (joist.OutTriggerA!=null)
            {
                OutTriggerAId = joist.OutTriggerA.Id;
            }

            if (joist.OutTriggerB!=null)
            {
                OutTriggerBId = joist.OutTriggerB.Id;
            }
        }
    }
}
