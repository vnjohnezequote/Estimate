using System.Collections.Generic;
using AppModels.ResponsiveData.Openings;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Framings.FloorAndRafters.Floor
{
    public class Joist: BindableBase
    {
        private TimberBase _joistInfo;
        private TimberBase _outTrigger;
        private double _quoteLength;
        private int _joistSpan;
        private double _joistPitch;
        public TimberBase JoistInfo { get=>_joistInfo; set=>SetProperty(ref _joistInfo,value); }
        public double QuoteLength { get; set; }
        public int JoistSpan { get=>_joistSpan; set=>SetProperty(ref _joistSpan,value); }
        public double JoistPitch { get=>_joistPitch; set=>SetProperty(ref _joistPitch,value); }
        public List<Hanger> Hangers { get; set; }

        //public List<>

    }
}
