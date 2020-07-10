using AppModels.CustomEntity;
using AppModels.ViewModelEntity;

namespace AppModels.Factories
{
    public static class Wall2DVmFactory
    {
        public static EntityVm GetEntityVm(this Wall2D entity)
        {
            return new Wall2DVm(entity);
        }
    }
}