using devDept.Eyeshot.Entities;

namespace AppModels.CustomEntity.CustomEntitySurrogate
{
    public class Joist2DSurrogate: FramingRectangleContainHangerAndOutTriggerSurrogate
    {
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
    }
}
