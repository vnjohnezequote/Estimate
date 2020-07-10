using AppModels.ViewModelEntity;
using devDept.Eyeshot.Entities;

namespace AppModels.Factories
{
    public static class EntitiyVmFactory
    {
        public static EntityVm GetEntityVmFactory(this Entity entity)
        {
            return new EntityVm(entity);
        }
    }
}
