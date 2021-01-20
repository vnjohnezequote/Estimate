using devDept.Eyeshot.Entities;

namespace AppModels.CustomEntity.CustomEntitySurrogate
{
    public class Joist2DSurrogate: FramingRectangleContainHangerAndOutTriggerSurrogate
    {
        //public Guid? HangerAId { get; set; }
        //public Guid? HangerBId { get; set; }
        //public bool IsHangerA { get; set; }
        //public bool IsHangerB { get; set; }
        //public Guid? OutTriggerAId { get; set; }
        //public Guid? OutTriggerBId { get; set; }
        //public bool IsOutTriggerA { get; set; }
        //public bool IsOutTriggerB { get; set; }
        //public bool OutTriggerAFlipped { get; set; }
        //public bool OutTriggerBFlipped { get; set; }
       
        public Joist2DSurrogate(FramingRectangleContainHangerAndOutTrigger text) : base(text)
        {
            
        }

        protected override Entity ConvertToObject()
        {
            Entity ent;
            ent = new Joist2D(OuterStartPoint, OuterEndPoint, Thickness, Flipped);
            CopyDataToObject(ent);
            return ent;
        }

        //protected override void CopyDataToObject(Entity entity)
        //{
        //    //base.CopyDataToObject(entity);
        //    //if (!(entity is Joist2D joist)) return;
        //    //joist.Thickness = Thickness;
        //    //joist.SetFlippedOutriggerA(OutTriggerAFlipped);
        //    //joist.SetFlippedOutriggerB(OutTriggerBFlipped);
        //    //joist.IsHangerA = IsHangerA;
        //    //joist.IsHangerB = IsHangerB;
        //    //joist.IsOutTriggerA = IsOutTriggerA;
        //    //joist.IsOutTriggerB = IsOutTriggerB;
        //    //joist.HangerAId = HangerAId;
        //    //joist.HangerBId = HangerBId;
        //    //joist.OutTriggerAId = OutTriggerAId;
        //    //joist.OutTriggerBId = OutTriggerBId;
        //    //joist.OuterStartPoint = OuterStartPoint;
        //    //joist.OuterEndPoint = OuterEndPoint;
        //}

        //protected override void CopyDataFromObject(Entity entity)
        //{
        //   //base.CopyDataFromObject(entity);
        //   //if (!(entity is Joist2D joist)) return;
        //   //this.OuterStartPoint = joist.OuterStartPoint;
        //   //this.OuterEndPoint = joist.OuterEndPoint;
        //   //this.Thickness = joist.Thickness;
        //   //HangerAId = joist.HangerAId;
        //   //HangerBId = joist.HangerBId;
        //   //IsHangerA = joist.IsHangerA;
        //   //IsHangerB = joist.IsHangerB;
        //   //OutTriggerAId = joist.OutTriggerAId;
        //   //OutTriggerBId = joist.OutTriggerBId;
        //   //IsOutTriggerA = joist.IsOutTriggerA;
        //   //IsOutTriggerB = joist.IsOutTriggerB;
        //   //OutTriggerAFlipped = joist.OutTriggerAFlipped;
        //   //OutTriggerBFlipped = joist.OutTriggerBFlipped;
        //}
    }
}
