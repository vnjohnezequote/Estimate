using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.ViewModelEntity
{
    public class FloorQuantitiesVm: BlockReferenceVm
    {
        public double ScaleWallName
        {
            get
            {
                if (Entity is FloorQuantity floorQty)
                {
                    return floorQty.GetScaleFactorX();
                }

                return 1;
            }
            set
            {
                if (value != 0)
                {
                    if (Entity is FloorQuantity floorQty)
                    {
                        var transformation = new Transformation(value);
                        floorQty.Transformation = transformation;
                        //wallName.Scale(value);
                        //wallName.Transformation.ScaleFactorX = value;
                        //wallName.Transformation.ScaleFactorY = value;
                        //wallName.Transformation.ScaleFactorZ = value;
                        RaisePropertyChanged(nameof(ScaleWallName));
                    }
                }
            }
        }

        public FloorQuantitiesVm(Entity entity) : base(entity)
        {
        }
    }
}
