using AppModels.CustomEntity;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.ResponsiveData.Openings;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.ViewModelEntity
{
    public class Joist2dVm : EntityContainhangerAndOutTriggerVm
    {
        public Joist2dVm(Entity entity, IEntitiesManager entitiesManager) : base(entity,entitiesManager)
        {
        }
    }
}
