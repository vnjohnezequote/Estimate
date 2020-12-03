using AppModels.CustomEntity;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings.Blocking;
using devDept.Eyeshot.Entities;

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
        public BlockingVm(Entity entity,IEntitiesManager entManger) : base(entity,entManger)
        {
        }
    }
}
