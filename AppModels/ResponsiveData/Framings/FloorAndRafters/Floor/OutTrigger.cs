using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.ResponsiveData.Openings;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Framings.FloorAndRafters.Floor
{
    public class OutTrigger: FramingBase
    {
        public override double QuoteLength => (double)FramingSpan.RoundUpTo300() / 1000;
    }
}
