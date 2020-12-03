using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.PocoDataModel.Framings.FloorAndRafter;
using AppModels.ResponsiveData.Framings;

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
