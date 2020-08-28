using System;
using AppModels.Interaface;
using AppModels.ViewModelEntity;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;

namespace AppModels.Factories
{
    [Serializable]
    public class EntitiyVmFactory
    {
        public IEntityVm creatEntityVm(IEntity entity)
        {
            switch (entity)
            {
                case IEntityVmCreateAble entityVmCreateAble:
                    return entityVmCreateAble.CreateEntityVm();
                case Line line:
                    line.CreateLineVm();
                    break;
                case LinearPath linePath:
                    linePath.CreateLinearPathVm();
                    break;
                case VectorView view:
                    view.CreateVectorViewVm();
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
