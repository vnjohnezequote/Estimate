using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;
using devDept.Serialization;

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
