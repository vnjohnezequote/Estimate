using AppModels.CustomEntity;
using AppModels.ResponsiveData;
using devDept.Eyeshot.Entities;

namespace AppModels.ViewModelEntity
{
    public class Beam2DVm: EntityVmBase
    {
        #region Field



        #endregion
        #region Properties

        public TimberBase TimberInfo
        {
            get
            {
                if (Entity is Beam2D beam && beam.BeamReference != null)
                {
                    return beam.BeamReference.TimberInfo;
                }

                return null;
            }
            set
            {
                if (!(Entity is Beam2D beam)) return;
                beam.BeamReference.TimberInfo = value;
                RaisePropertyChanged(nameof(TimberInfo));
            }
        }

        public string BeamGrade
        {
            get
            {
                if (Entity is Beam2D beam)
                {
                    if (beam.BeamReference!=null)
                    {
                        return beam.BeamReference.TimberGrade;
                    }
                }

                return string.Empty;
            }
            set
            {
                if (Entity is Beam2D beam)
                {
                    if (beam.BeamReference!=null)
                    {
                        beam.BeamReference.TimberGrade = value;
                        RaisePropertyChanged(nameof(BeamGrade));
                    }
                }
            }
        }

        public bool BeamUnder
        {
            get
            {
                if (Entity is Beam2D beam)
                {
                    return beam.IsBeamUnder;
                }

                return false;
            }
            set
            {
                if (Entity is Beam2D beam)
                {
                    beam.IsBeamUnder = value;
                    RaisePropertyChanged(nameof(BeamUnder));
                    RaisePropertyChanged(nameof(Color));
                }
            }
        }

        public double Pitch
        {
            get
            {
                if (Entity is Beam2D beam && beam.BeamReference !=null)
                {
                    return beam.BeamReference.BeamPitch;
                }

                return 0;
            }
            set
            {
                if (Entity is Beam2D beam && beam.BeamReference!=null)
                {
                    beam.BeamReference.BeamPitch = value;
                    RaisePropertyChanged(nameof(Pitch));
                }
            }
        }

        public double ExtraLength
        {
            get
            {
                if (Entity is Beam2D beam && beam.BeamReference !=null)
                {
                    return beam.BeamReference.ExtraLength;
                }

                return 0;

            }
            set
            {
                if (Entity is Beam2D beam && beam.BeamReference!=null)
                {
                    beam.BeamReference.ExtraLength = value;
                    RaisePropertyChanged(nameof(ExtraLength));
                }
            }
        }

        #endregion


        public Beam2DVm(Entity entity) : base(entity)
        {
        }

        public override void NotifyPropertiesChanged()
        {
            base.NotifyPropertiesChanged();
        }
    }
}