using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Serialization;

namespace AppModels.CustomEntity.CustomEntitySurrogate
{
    public class Joist2DSurrogate: FramingRectangle2DSurrogate
    {
        public Joist2DSurrogate(PlanarEntity text) : base(text)
        {
            
        }

        protected override Entity ConvertToObject()
        {
            Entity ent;
            ent = new Joist2D(OuterStartPoint, OuterEndPoint, Thickness);
            CopyDataToObject(ent);
            return ent;
        }

        protected override void CopyDataToObject(Entity entity)
        {
            base.CopyDataToObject(entity);
            }

        protected override void CopyDataFromObject(Entity entity)
        {
           base.CopyDataFromObject(entity);
        }
    }
}
