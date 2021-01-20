using AppModels.Interaface;
using devDept.Eyeshot.Entities;

namespace AppModels.ViewModelEntity
{
    public class JoistArrowVm: EntityVmBase
    {
        //public string FramingName
        //{
        //    get {
        //        if (!(this.Entity is JoistArrowEntity joistArrow)) return string.Empty;
        //        return joistArrow.FramingReference!=null ? joistArrow.FramingReference.Name : string.Empty;
        //    }
        //}

        //public IFraming FramingReference
        //{
        //    get
        //    {
        //        if (this.Entity is JoistArrowEntity joistArrow)
        //        {
        //            return joistArrow.FramingReference;
        //        }

        //        return null;
        //    }
        //    set
        //    {
        //        if (!(this.Entity is JoistArrowEntity joistArrow)) return;
        //        joistArrow.FramingReference = value;
        //        RaisePropertyChanged(nameof(FramingReference));
        //        RaisePropertyChanged(nameof(FramingName));

        //    }
        //}
        
        public JoistArrowVm(IEntity entity,IEntitiesManager entitiesManager) : base(entity)
        {
        }
    }
}
