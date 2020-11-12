using System;
using AppModels.CustomEntity;
using AppModels.Interaface;
using AppModels.ViewModelEntity;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;

namespace AppModels.Factories
{
    [Serializable]
    public class EntitiyVmFactory
    {
        public IEntityVm creatEntityVm(IEntity entity,IEntitiesManager entitiesManager)
        {
            switch (entity)
            {
                case IEntityVmCreateAble entityVmCreateAble:
                    return entityVmCreateAble.CreateEntityVm(entitiesManager);
                case Line line:
                    return line.CreateLineVm();
                case LinearPath linePath:
                    return linePath.CreateLinearPathVm();
                case VectorView view:
                    return view.CreateVectorViewVm();
                case Text text:
                    return text.CreateTextVm();
                case Leader leader:
                    return leader.CreateLeaderVm();
                case BeamEntity beam:
                    return beam.CreateEntityVm();
                //case WallName wall:
                //    return wall.CreateEntityVm();
                case BlockReference block:
                    return block.CreateBlockReferenceVm();
                case Entity entiti:
                    entiti.CreateEntityVm();
                    break;
                default: return new EntityVm((Entity)entity);
            }
            return new EntityVm((Entity)entity);
        }
       
    }
}
