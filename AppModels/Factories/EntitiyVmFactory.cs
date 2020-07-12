using AppModels.Interaface;
using AppModels.ViewModelEntity;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;

namespace AppModels.Factories
{
    public class EntitiyVmFactory
    {
        public IEntityVm creatEntityVm(IEntity entity)
        {
            switch (entity)
            {
                case IEntityVmCreateAble entityVmCreateAble:
                    return entityVmCreateAble.CreateEntityVm();
                case Line line:
                    line.CreateEntityVm();
                    break;
                case Entity entiti:
                    entiti.CreateEntityVm();
                    break;
                default: return new EntityVm((Entity)entity);
            }
            return new EntityVm((Entity)entity);
        }
       
    }
}
