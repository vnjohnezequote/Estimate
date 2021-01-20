using AppModels.CustomEntity;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.ViewModelEntity
{
    public class WallNameVm: BlockReferenceVm
    {
        public double ScaleWallName { 
            get{
                if (Entity is WallName wallName)
                {
                    return wallName.GetScaleFactorX();
                }

                return 1;
            }
            set
            {
                if (value!=0)
                {
                    if (Entity is WallName wallName)
                    {
                        var transformation = new Transformation(value);
                        wallName.Transformation = transformation;
                        //wallName.Scale(value);
                        //wallName.Transformation.ScaleFactorX = value;
                        //wallName.Transformation.ScaleFactorY = value;
                        //wallName.Transformation.ScaleFactorZ = value;
                        RaisePropertyChanged(nameof(ScaleWallName));
                    }
                }
            }
        }
        public WallNameVm(Entity entity) : base(entity)
        {
        }

        public override void NotifyPropertiesChanged()
        {
            base.NotifyPropertiesChanged();
            RaisePropertyChanged(nameof(ScaleWallName));
        }
    }
}
