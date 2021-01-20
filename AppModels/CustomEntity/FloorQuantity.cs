using AppModels.Interaface;
using AppModels.ViewModelEntity;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.CustomEntity
{
    public class FloorQuantity: BlockReference, IEntityVmCreateAble
    {
        public FloorQuantity(Point3D insPoint, string blockName, double rotationAngleInRadians=0) : base(insPoint, blockName, rotationAngleInRadians)
        {
        }

       
        public FloorQuantity(string blockName) : base(blockName)
        {
        }


        public IEntityVm CreateEntityVm(IEntitiesManager entitiesManager)
        {
            return new FloorQuantitiesVm(this);
        }
    }
}
