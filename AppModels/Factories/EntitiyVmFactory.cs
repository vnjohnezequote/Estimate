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
            if (entity is IEntityVmCreateAble ebEntityVmCreateAble)
            {
                return ebEntityVmCreateAble.CreateEntityVm(entitiesManager);
            }
            else if(entity is Line line)
            {
                return line.CreateLineVm();
            }
            else if(entity is LinearPath linearPath)
            {
                return linearPath.CreateLinearPathVm();
            }
            else if (entity is VectorView view)
            {
                return view.CreateBlockReferenceVm();
            }
            else if (entity is Text text)
            {
                return text.CreateTextVm();
            }
            else if (entity is Leader leader)
            {
                return leader.CreateLeaderVm();
            }

            else if (entity is BlockReference block)
            {
                return block.CreateBlockReferenceVm();
            }
            else if (entity is Entity entiy)
            {
                return entiy.CreateEntityVm();
            }

            return null;
        }
       
    }
}
