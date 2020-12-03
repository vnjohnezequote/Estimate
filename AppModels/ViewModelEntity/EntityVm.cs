using System;
using devDept.Eyeshot.Entities;

namespace AppModels.ViewModelEntity
{
    [Serializable]
    public class EntityVm: EntityVmBase
    {
        public EntityVm(Entity entity): base(entity)
        {
        }

    }
}
