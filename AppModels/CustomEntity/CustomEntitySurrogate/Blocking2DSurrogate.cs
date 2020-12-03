using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;
using devDept.Serialization;

namespace AppModels.CustomEntity.CustomEntitySurrogate
{
    public class Blocking2DSurrogate : Framing2DSurrogate
    {
        public Blocking2DSurrogate(Text text) : base(text)
        {
        }

        protected override Entity ConvertToObject()
        {
            Entity blocking;
            blocking = new Blocking2D((Text) base.ConvertToObject());
            CopyDataToObject(blocking);
            return blocking;

        }
    }
}
