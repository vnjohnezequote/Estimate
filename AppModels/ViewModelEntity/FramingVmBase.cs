using AppModels.CustomEntity;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.EngineerMember;
using devDept.Eyeshot.Entities;

namespace AppModels.ViewModelEntity
{
    public abstract class FramingVmBase: EntityVm,IFramingVmBase
    {
        public IEntitiesManager EntitiesManager { get;  }
        public FramingTypes FramingType
        {
            get
            {
                if (Entity is IFraming2D framing && framing.FramingReference !=null)
                {
                    return framing.FramingReference.FramingType;
                }

                return FramingTypes.FloorJoist;
            }
            set
            {
                if (Entity is IFraming2D framing)
                {
                    framing.FramingReference.FramingType = value;
                    RaisePropertyChanged(nameof(FramingType));
                    RaisePropertyChanged(nameof(Name));
                }
            }
        }
        public virtual TimberBase FramingInfo
        {
            get
            {
                if (Entity is IFraming2D framing && framing.FramingReference != null)
                {
                    return framing.FramingReference.FramingInfo;
                }

                return null;
            }
            set
            {
                if (!(Entity is IFraming2D beam)) return;
                beam.FramingReference.FramingInfo = value;
                RaisePropertyChanged(nameof(FramingInfo));
            }
        }

        public EngineerMemberInfo EngineerMember
        {
            get
            {
                if (Entity is IFraming2D framing)
                {
                    if (framing.FramingReference != null)
                    {
                        return framing.FramingReference.EngineerMember;
                    }
                }

                return null;
            }
            set
            {
                if (Entity is IFraming2D framing)
                {
                    if (framing.FramingReference != null)
                    {
                        framing.FramingReference.EngineerMember = value;
                        RaisePropertyChanged(nameof(EngineerMember));
                        RaisePropertyChanged(nameof(FramingInfo));
                        RaisePropertyChanged(nameof(BeamGrade));
                    }
                }
            }
        }

        public string BeamGrade
        {
            get
            {
                if (Entity is IFraming2D framing)
                {
                    if (framing.FramingReference != null)
                    {
                        return framing.FramingReference.TimberGrade;
                    }
                }

                return string.Empty;
            }
            set
            {
                if (Entity is IFraming2D framing)
                {
                    if (framing.FramingReference != null)
                    {
                        framing.FramingReference.TimberGrade = value;
                        RaisePropertyChanged(nameof(BeamGrade));
                    }
                }
            }
        }
        public string Name
        {
            get
            {
                if (Entity is IFraming2D framing && framing.FramingReference !=null)
                {
                    return framing.FramingReference.Name;
                }

                if (Entity is Blocking2D blocking)
                {
                    return blocking.FramingReference.Name;
                }

                return string.Empty;
            }
            set
            {
                if (Entity is IFraming2D framing)
                {
                    if (value == framing.FramingReference.Name)
                    {
                        return;
                    }
                    framing.FramingReference.Name = value;
                    RaisePropertyChanged(nameof(Name));
                }
            }
        }
        public int Index
        {
            get
            {
                if (Entity is IFraming2D framing && framing.FramingReference!=null)
                {
                    return framing.FramingReference.Index;
                }

                return 0;
            }
            set
            {
                if (Entity is IFraming2D framing)
                {
                    framing.FramingReference.Index = value;
                    RaisePropertyChanged(nameof(Index));
                }

            }
        }
        public int SubFixIndex
        {
            get
            {
                if (Entity is IFraming2D framing && framing.FramingReference!=null)
                {
                    return framing.FramingReference.SubFixIndex;
                }

                return 0;
            }
            set
            {
                if (Entity is IFraming2D framing)
                {
                    framing.FramingReference.SubFixIndex = value;
                    RaisePropertyChanged(nameof(SubFixIndex));
                }
            }
        }
        public bool IsExisting
        {
            get
            {
                if (Entity is IFraming2D framing && framing.FramingReference!=null)
                {
                    return framing.FramingReference.IsExisting;
                }

                return false;
            }
            set
            {
                if (Entity is IFraming2D framing)
                {
                    framing.FramingReference.IsExisting = value;
                    RaisePropertyChanged(nameof(IsExisting));
                }
            }
        }

        public int FullLength
        {
            get
            {
                if (Entity is IFraming2D framing && framing.FramingReference!=null)
                {
                    return framing.FramingReference.FullLength;
                }

                return 0;
            }
            set
            {
                if (Entity is Blocking2D blocking)
                {
                    blocking.FramingReference.FullLength = value;
                    RaisePropertyChanged(nameof(FullLength));
                }

            }
        }

        public double QuoteLength
        {
            get {
                if (Entity is IFraming2D framing && framing.FramingReference!=null)
                {
                    return framing.FramingReference.QuoteLength;
                }

                return 0.0;
            }
        }

        public FramingVmBase(Entity entity, IEntitiesManager entityManager) : base(entity)
        {
            this.EntitiesManager = entityManager;
        }

        
    }
}
