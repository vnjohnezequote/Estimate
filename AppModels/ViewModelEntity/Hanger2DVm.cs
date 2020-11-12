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
                if (Entity is Hanger2D hanger)
                {
                    return hanger.HangerReference.HangerMaterial;
                }

                return null;
            }
            set
            {
                if (Entity is Hanger2D hanger)
                {
                    hanger.HangerReference.HangerMaterial = value;
                    RaisePropertyChanged(nameof(Material));
                }

                
            }
        }
        public Hanger2DVm(IEntity entity,IEntitiesManager entityManager) : base(entity)
        {
        }
    }
}