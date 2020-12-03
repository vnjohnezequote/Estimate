using System.Collections.Generic;
using AppModels.PocoDataModel.Framings.FloorAndRafter;
using AppModels.ResponsiveData.Openings;

namespace AppModels.Interaface
{
    public interface IContaintHanger
    {
        Hanger HangerA { get; set; }
        Hanger HangerB { get; set; }
        void LoadHangers(List<Hanger> hangers, FramingBasePoco framingPoco);
    }
}