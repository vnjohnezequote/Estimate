using AppModels.Interaface;
using AppModels.ViewModelEntity;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;

namespace AppModels.CustomEntity
{
    public class CustomVectorView : VectorView,IEntityVmCreateAble
    {
        public bool KeepEntityLineWeight { get; set; }
        public bool KeepEntityLineType { get; set; }
        public CustomVectorView(double x, double y, viewType standardView, double scale, string name, double width = 0, double height = 0) : base(x, y, standardView, scale, name, width, height)
        {
        }

        public IEntityVm CreateEntityVm(IEntitiesManager entitiesManager)
        {
            return new VectorViewVm(this, entitiesManager);
        }
    }
}