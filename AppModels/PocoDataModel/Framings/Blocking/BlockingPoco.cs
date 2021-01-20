using AppModels.Enums;
using AppModels.PocoDataModel.Framings.FloorAndRafter;

namespace AppModels.PocoDataModel.Framings.Blocking
{
    public class BlockingPoco: FramingBasePoco
    {
        public BlockingTypes BlockingType { get; set; }
        public BlockingPoco()
        { }
        public BlockingPoco(ResponsiveData.Framings.Blocking.Blocking blocking):base(blocking)
        {
            BlockingType = blocking.BlockingType;
        }
    }
}
