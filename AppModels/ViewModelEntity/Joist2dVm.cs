using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using AppModels.Factories;
using AppModels.ResponsiveData;
using devDept.Eyeshot.Entities;

namespace AppModels.ViewModelEntity
{
    public class Joist2dVm: EntityVmBase
    {
        public TimberBase TimberInfo
        {
            get
            {
                if (Entity is Joist2D joist && joist.JoistReference != null)
                {
                    return joist.JoistReference.FramingInfo;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (Entity is Joist2D joist && joist.JoistReference!=null)
                {
                    joist.JoistReference.FramingInfo = value;
                    RaisePropertyChanged(nameof(TimberInfo));
                }
            }
        }

        public string BeamGrade
        {
            get
            {
                if (Entity is Joist2D joist)
                {
                    if (joist.JoistReference !=null)
                    {
                        return joist.JoistReference.TimberGrade;
                    }
                }

                return string.Empty;
            }
            set
            {
                if (Entity is Joist2D joist)
                {
                    if (joist.JoistReference!=null)
                    {
                        joist.JoistReference.TimberGrade = value;
                        RaisePropertyChanged(nameof(BeamGrade));
                    }
                }
            }
        }
        public double Pitch
        {
            get
            {
                if (Entity is Joist2D joist && joist.JoistReference != null)
                {
                    return joist.JoistReference.JoistPitch;
                }

                return 0;
            }
            set
            {
                if (Entity is Joist2D joist && joist.JoistReference != null)
                {
                    joist.JoistReference.JoistPitch = value;
                    RaisePropertyChanged(nameof(Pitch));
                }
            }
        }
        public double ExtraLength
        {
            get
            {
                if (Entity is Joist2D joist && joist.JoistReference != null)
                {
                    return joist.JoistReference.ExtraLength;
                }

                return 0;

            }
            set
            {
                if (Entity is Joist2D joist && joist.JoistReference != null)
                {
                    joist.JoistReference.ExtraLength = value;
                    RaisePropertyChanged(nameof(ExtraLength));
                }
            }
        }

        public Joist2dVm(IEntity entity) : base(entity)
        {
            
        }
    }
}
