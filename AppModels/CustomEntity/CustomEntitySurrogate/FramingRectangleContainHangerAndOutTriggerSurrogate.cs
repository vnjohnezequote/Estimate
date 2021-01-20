using System;
using devDept.Eyeshot.Entities;

namespace AppModels.CustomEntity.CustomEntitySurrogate
{
    public class FramingRectangleContainHangerAndOutTriggerSurrogate: FramingRectangle2DSurrogate
    {
        public Guid? HangerAId { get; set; }
        public Guid? HangerBId { get; set; }
        public bool IsHangerA { get; set; }
        public bool IsHangerB { get; set; }
        public Guid? OutTriggerAId { get; set; }
        public Guid? OutTriggerBId { get; set; }
        public bool IsOutTriggerA { get; set; }
        public bool IsOutTriggerB { get; set; }
        public bool OutTriggerAFlipped { get; set; }
        public bool OutTriggerBFlipped { get; set; }

        public FramingRectangleContainHangerAndOutTriggerSurrogate(PlanarEntity text) : base(text)
        {

        }

        protected override Entity ConvertToObject()
        {
            Entity ent;
            ent = new FramingRectangleContainHangerAndOutTrigger(OuterStartPoint, OuterEndPoint, Thickness, Flipped);
            CopyDataToObject(ent);
            return ent;
        }

        protected override void CopyDataToObject(Entity entity)
        {
            base.CopyDataToObject(entity);
            if (!(entity is FramingRectangleContainHangerAndOutTrigger framing)) return;
            framing.Thickness = Thickness;
            framing.SetFlippedOutriggerA(OutTriggerAFlipped);
            framing.SetFlippedOutriggerB(OutTriggerBFlipped);
            framing.IsHangerA = IsHangerA;
            framing.IsHangerB = IsHangerB;
            framing.IsOutTriggerA = IsOutTriggerA;
            framing.IsOutTriggerB = IsOutTriggerB;
            framing.HangerAId = HangerAId;
            framing.HangerBId = HangerBId;
            framing.OutTriggerAId = OutTriggerAId;
            framing.OutTriggerBId = OutTriggerBId;
            framing.OuterStartPoint = OuterStartPoint;
            framing.OuterEndPoint = OuterEndPoint;
        }

        protected override void CopyDataFromObject(Entity entity)
        {
            base.CopyDataFromObject(entity);
            if (!(entity is FramingRectangleContainHangerAndOutTrigger framing)) return;
            this.OuterStartPoint = framing.OuterStartPoint;
            this.OuterEndPoint = framing.OuterEndPoint;
            this.Thickness = framing.Thickness;
            HangerAId = framing.HangerAId;
            HangerBId = framing.HangerBId;
            IsHangerA = framing.IsHangerA;
            IsHangerB = framing.IsHangerB;
            OutTriggerAId = framing.OutTriggerAId;
            OutTriggerBId = framing.OutTriggerBId;
            IsOutTriggerA = framing.IsOutTriggerA;
            IsOutTriggerB = framing.IsOutTriggerB;
            OutTriggerAFlipped = framing.OutTriggerAFlipped;
            OutTriggerBFlipped = framing.OutTriggerBFlipped;
        }
    }
}
