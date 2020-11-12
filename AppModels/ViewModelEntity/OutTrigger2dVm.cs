using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using devDept.Eyeshot.Entities;

namespace AppModels.ViewModelEntity
{
    public class OutTrigger2dVm : EntityVm,ITimberVm
    {
        public string BeamGrade
        {
            get
            {
                if (Entity is OutTrigger2D outTrigger)
                {
                    if (outTrigger.OutTriggerReference != null)
                    {
                        return outTrigger.OutTriggerReference.TimberGrade;
                    }
                }

                return string.Empty;
            }
            set
            {
                if (Entity is OutTrigger2D outTrigger)
                {
                    if (outTrigger.OutTriggerReference != null)
                    {
                        outTrigger.OutTriggerReference.TimberGrade = value;
                        RaisePropertyChanged(nameof(BeamGrade));
                    }
                }
            }
        }
        public TimberBase TimberInfo
        {
            get
            {
                if (Entity is OutTrigger2D outTrigger)
                {
                    return outTrigger.OutTriggerReference.FramingInfo;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (Entity is OutTrigger2D outTrigger)
                {
                    outTrigger.OutTriggerReference.FramingInfo = value;
                    RaisePropertyChanged(nameof(TimberInfo));
                }
            }
        }
        public int OutTriggerOutSize
        {
            get
            {
                if (Entity is OutTrigger2D outTrigger)
                {
                    return outTrigger.OutSizeLength;
                }

                return 0;
            }
            set
            {
                if (Entity is OutTrigger2D outTrigger)
                {
                   outTrigger.OutSizeLength = value;
                   RaisePropertyChanged(nameof(OutTriggerOutSize));
                }
            }
        }
        public int OutTriggerInSize
        {
            get
            {
                if (Entity is OutTrigger2D outTrigger)
                {
                    return outTrigger.InSizeLength;
                }

                return 0;
            }
            set
            {
                if (Entity is OutTrigger2D outTrigger)
                {
                    outTrigger.InSizeLength = value;
                    RaisePropertyChanged(nameof(OutTriggerInSize));
                }
            }
        }
        public double ExtraLength
        {
            get
            {
                if (Entity is OutTrigger2D outTrigger)
                {
                    return outTrigger.OutTriggerReference.ExtraLength;
                }

                return 0;

            }
            set
            {
                if (Entity is OutTrigger2D outTrigger)
                {
                    outTrigger.OutTriggerReference.ExtraLength = value;
                    RaisePropertyChanged(nameof(ExtraLength));
                }
            }
        }

        public double QuoteLength
        {
            get
            {
                if (Entity is OutTrigger2D outTrigger)
                {
                    return outTrigger.OutTriggerReference.QuoteLength;
                }

                return 0;
            }
        }

        public OutTrigger2dVm(Entity entity, IEntitiesManager entityManager) : base(entity)
        {
        }
    }
}
