using System.Runtime.Serialization;
using AppModels.Interaface;
using AppModels.ViewModelEntity;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.CustomEntity
{
    public class WallName: BlockReference, IEntityVmCreateAble
    {
        public WallName(Point3D insPoint, string blockName, double rotationAngleInRadians = 0) : base(insPoint, blockName, rotationAngleInRadians)
        {

        }

        protected WallName(BlockReference another) : base(another)
        {
        }


        public IEntityVm CreateEntityVm()
        {
            return new WallNameVm(this);
        }
    }
}
