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
    public class OutTrigger2dVm : FramingVm
    {
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

        public OutTrigger2dVm(Entity entity, IEntitiesManager entityManager) : base(entity,entityManager)
        {
        }
    }
}
