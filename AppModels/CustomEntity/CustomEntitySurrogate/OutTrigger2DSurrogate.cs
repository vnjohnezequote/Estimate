using devDept.Eyeshot.Entities;

namespace AppModels.CustomEntity.CustomEntitySurrogate
{
    public class OutTrigger2DSurrogate: FramingRectangle2DSurrogate
    {
       
        public int InsideLength { get; set; }
        public int OutSideLength { get; set; }
        
        public OutTrigger2DSurrogate(PlanarEntity planarEntity) : base(planarEntity)
        {
        }
        protected override Entity ConvertToObject()
        {
            Entity ent=null;
            ent = new OutTrigger2D(OuterStartPoint, OuterEndPoint,Thickness, Flipped);
            CopyDataToObject(ent);
            return ent;
        }
        protected override void CopyDataToObject(Entity entity)
        {
            base.CopyDataToObject(entity);
            if (entity is OutTrigger2D outTrigger2D)
            {
                outTrigger2D.SetInsizeLength(InsideLength);
                outTrigger2D.SetOutSizeLength(OutSideLength);
            }
           
        }

        protected override void CopyDataFromObject(Entity entity)
        {
            base.CopyDataFromObject(entity);
            if (entity is OutTrigger2D outTrigger2D)
            {
                InsideLength = outTrigger2D.InSizeLength;
                OutSideLength = outTrigger2D.OutSizeLength;
            }
        }

    }
}

