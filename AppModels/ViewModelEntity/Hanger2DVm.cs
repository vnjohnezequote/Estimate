using AppModels.CustomEntity;
using AppModels.Interaface;
using AppModels.ResponsiveData.Openings;
using devDept.Eyeshot.Entities;

namespace AppModels.ViewModelEntity
{
    public class Hanger2DVm: EntityVmBase
    {
        public HangerMat Material
        {
            get
            {
                if (Entity is Hanger2D hanger && hanger.FramingReference !=null)
                {
                    return ((Hanger)hanger.FramingReference).HangerMaterial;
                }

                return null;
            }
            set
            {
                if (Entity is Hanger2D hanger && hanger.FramingReference!=null)
                {
                    ((Hanger)hanger.FramingReference).HangerMaterial = value;
                    RaisePropertyChanged(nameof(Material));
                }
            }
        }

        public string Name
        {
            get
            {
                if (Entity is Hanger2D hanger && hanger.FramingReference!=null)
                {
                    return hanger.FramingReference.Name;
                }

                return string.Empty;
            }
            set
            {
                if (Entity is Hanger2D hanger && hanger.FramingReference!=null) 
                {
                    if (value == hanger.FramingReference.Name)
                    {
                        return;
                    }

                    hanger.FramingReference.Name = value;
                    RaisePropertyChanged(nameof(Name));
                }
            }
        }

        public int SubFixIndex
        {
            get
            {
                if (Entity is Hanger2D hanger && hanger.FramingReference!=null)
                {
                    return hanger.FramingReference.SubFixIndex;
                }

                return 0;
            }
            set
            {
                if (Entity is Hanger2D hanger && hanger.FramingReference!=null)
                {
                    hanger.FramingReference.SubFixIndex = value;
                    RaisePropertyChanged(nameof(SubFixIndex));
                }

            }
        }

        public int Index 
        { 
            get
            {
                if (Entity is Hanger2D hanger && hanger.FramingReference!=null)
                {
                    return hanger.FramingReference.Index;
                }

                return 0;
            }
            set
            {
                if (Entity is Hanger2D hanger && hanger.FramingReference!=null)
                {
                    hanger.FramingReference.Index = value;
                    RaisePropertyChanged(nameof(Index));
                }
            }
        }


        public Hanger2DVm(IEntity entity,IEntitiesManager entityManager) : base(entity)
        {
        }
    }
}