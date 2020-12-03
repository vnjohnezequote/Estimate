using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Serialization;

namespace AppModels.CustomEntity.CustomEntitySurrogate
{
    public class OutTrigger2DSurrogate: FramingRectangle2DSurrogate
    {
       
        public int InsideLength { get; set; }
        public int OutSideLength { get; set; }
        public bool Flipped { get; set; }
        public OutTrigger2DSurrogate(PlanarEntity planarEntity) : base(planarEntity)
        {
        }
        protected override Entity ConvertToObject()
        {
            Entity ent=null;
            //ent = new OutTrigger2D(OuterStartPoint, OuterEndPoint,false, Thickness, Depth);
            //CopyDataToObject(ent);
            return ent;
        }
        protected override void CopyDataToObject(Entity entity)
        {
            base.CopyDataToObject(entity);
            var outTrigger = entity as OutTrigger2D;
            if (outTrigger != null)
            {
                outTrigger.Id = Id;
                //outTrigger.ReferenceId = ReferenceID;
                outTrigger.Thickness = Thickness;
                outTrigger.Depth = Depth;
                outTrigger.SetInsizeLength(InsideLength);
                outTrigger.SetOutSizeLength(OutSideLength);
                outTrigger.Flipped = Flipped;
            }
        }

        protected override void CopyDataFromObject(Entity entity)
        {
            var outTrigger = entity as OutTrigger2D;
            if (outTrigger != null)
            {
                Id = outTrigger.Id;
                Thickness = outTrigger.Thickness;
                Depth = outTrigger.Depth;
                OuterStartPoint = outTrigger.OuterStartPoint;
                OuterEndPoint = outTrigger.OuterEndPoint;
                InsideLength = outTrigger.InSizeLength;
                OutSideLength = outTrigger.OutSizeLength;
                Flipped = outTrigger.Flipped;
            }
            base.CopyDataFromObject(entity);
        }

    }
}

