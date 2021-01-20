using AppModels.CustomEntity;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings.Blocking;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.ViewModelEntity
{
    public class BlockingVm:FramingVmBase
    {
        public BlockingTypes BlockingType
        {
            get
            {
                if (Entity is Blocking2D blocking)
                {
                    return ((Blocking)blocking.FramingReference).BlockingType;
                }

                return BlockingTypes.SingleBlocking;
            }
            set
            {
                if (Entity is Blocking2D blocking)
                {
                    ((Blocking)blocking.FramingReference).BlockingType = value;
                    RaisePropertyChanged(nameof(BlockingType));
                }

            }
        }

        public bool IsRotateBlocking
        {
            get
            {
                if (Entity is Blocking2D blocking)
                {
                    return blocking.IsRotate;
                }

                return false;
            }
            set
            {
                if (Entity is Blocking2D blocking)
                {
                    blocking.IsRotate = value;
                    var angleRadian = Utility.DegToRad(90);
                    if (value)
                    {
                       blocking.Rotate(angleRadian,Vector3D.AxisZ,blocking.InsertionPoint);
                    }
                    else
                    {
                        blocking.Rotate(-angleRadian, Vector3D.AxisZ, blocking.InsertionPoint);
                    }
                    RaisePropertyChanged(nameof(IsRotateBlocking));
                }
            }
        }

        public BlockingVm(Entity entity,IEntitiesManager entManger) : base(entity,entManger)
        {
        }
    }
}
