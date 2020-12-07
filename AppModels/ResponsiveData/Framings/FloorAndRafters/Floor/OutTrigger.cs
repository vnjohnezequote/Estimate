using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.PocoDataModel.Framings.FloorAndRafter;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Openings;
using devDept.Geometry;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Framings.FloorAndRafters.Floor
{
    public class OutTrigger: FramingBase
    {
        private IFraming _parrent;
        public override double QuoteLength
        {
            get
            {
                var fullLength = FullLength / Math.Cos(Utility.DegToRad(Pitch));
                var quoteLengthinMM = ((int)fullLength).RoundUpTo300();
                var quoteLength = (double)quoteLengthinMM / 1000 + ExtraLength;
                return quoteLength;
            }
            
        } 
        public IFraming Parrent { 
            get=>_parrent;
            set
            {
                SetProperty(ref _parrent, value);
                if (value !=null)
                {
                    _parrent.PropertyChanged+=ParrentOnPropertyChanged;
                }
            }
        }

        private void ParrentOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Parrent.FramingType):
                    SetOutTriggerType(Parrent.FramingType);
                    break;
                default:break;
                }
            }
        

        public OutTrigger() : base()
        {
        }
        public OutTrigger(FramingSheet framingSheet):base(framingSheet)
        { }

        public OutTrigger(IFraming framingRef):base(framingRef.FramingSheet)
        {
            Parrent = framingRef;
            SetOutTriggerType(Parrent.FramingType);
            
        }
        public OutTrigger(OutTriggerPoco outTrigger,FramingSheet framingSheet, List<TimberBase> timberList,List<EngineerMemberInfo> engineerMemberInfos) : base(outTrigger,framingSheet, timberList,engineerMemberInfos)
        {
        }
        public OutTrigger(OutTrigger another) : base(another) { }

        public override object Clone()
        {
            return new OutTrigger(this);
        }

        private void SetOutTriggerType(FramingTypes framingType)
        {
            switch (framingType)
            {
                case FramingTypes.FloorJoist:
                case FramingTypes.FloorBeam:
                    FramingType = FramingTypes.OutTrigger;
                    break;
                case FramingTypes.RafterBeam:
                case FramingTypes.RafterJoist:
                    FramingType = FramingTypes.RafterOutTrigger;
                    break;
                case FramingTypes.BoundaryJoist:
                    break;
                case FramingTypes.CeilingJoist:
                    break;
                case FramingTypes.Purlin:
                    break;
                case FramingTypes.PolePlate:
                    break;
                case FramingTypes.Fascia:
                    break;
                case FramingTypes.TieDown:
                    break;
                case FramingTypes.Hanger:
                    break;
                case FramingTypes.Trimmer:
                    break;
                case FramingTypes.Rimboard:
                    break;
                case FramingTypes.HipRafter:
                    break;
                case FramingTypes.LintelBeam:
                    break;
                case FramingTypes.TrussBeam:
                    break;
                case FramingTypes.Blocking:
                    break;
                case FramingTypes.RidgeBeam:
                    break;
                case FramingTypes.OutTrigger:
                    break;
                case FramingTypes.RafterOutTrigger:
                    break;
                case FramingTypes.DeckJoist:
                    break;
                default:
                    break;
            }
        }
    }
}
