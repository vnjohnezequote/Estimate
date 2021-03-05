using System;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;

namespace AppModels.CustomEntity.CustomEntitySurrogate
{
    public class Hanger2DSurrogate : FramingBase2DSurrogate
    {
        public Hanger2DSurrogate(Text text) : base(text)
        {
        }
        protected override Entity ConvertToObject()
        {
            Entity ent;
            ent = new Hanger2D((Text)base.ConvertToObject());
            CopyDataToObject(ent);
            return ent;
        }

    }
}
