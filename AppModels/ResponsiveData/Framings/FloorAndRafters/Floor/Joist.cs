using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AppModels.ResponsiveData.Openings;
using devDept.Geometry;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Framings.FloorAndRafters.Floor
{
    public class Joist: FramingBase
    {
        private double _quoteLength;
        private int _joistSpan;
        private double _joistPitch;
        //public int JoistSpan { get=>_joistSpan; set=>SetProperty(ref _joistSpan,value); }
        public override double QuoteLength
        {
            get
            {
                var fullLength = FullLength / Math.Cos(Utility.DegToRad(JoistPitch));
                var quoteLengthinMM = ((int)fullLength).RoundUpTo300();
                var quoteLength = (double) quoteLengthinMM / 1000 + ExtraLength;
                return quoteLength;
            }
       
        } 
        public double JoistPitch { 
            get=>_joistPitch;
            set
            {
                SetProperty(ref _joistPitch, value);
                RaisePropertyChanged(nameof(QuoteLength));
            }
        }
        public ObservableCollection<Hanger> Hangers { get; set; } = new ObservableCollection<Hanger>();
        public ObservableCollection<OutTrigger> OutTriggers { get; set; } = new ObservableCollection<OutTrigger>();


    }
}
