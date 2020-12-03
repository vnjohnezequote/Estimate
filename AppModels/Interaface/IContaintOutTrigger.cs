using System.Collections.Generic;
using AppModels.PocoDataModel.Framings.FloorAndRafter;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;

namespace AppModels.Interaface
{
    public interface IContaintOutTrigger
    {
        OutTrigger OutTriggerA { get; set; }
        OutTrigger OutTriggerB { get; set; }
        void LoadOutTriggers(List<IFraming> outTriggers, FramingBasePoco joistPoco);
    }
}