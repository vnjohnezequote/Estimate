using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;

namespace AppModels.CustomEntity.CustomEntitySurrogate
{
    public class Framing2DSurrogate: FramingBase2DSurrogate
    {
        public Framing2DSurrogate(Text text) : base(text)
        {
        }
    }
}
