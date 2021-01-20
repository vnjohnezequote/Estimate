using devDept.Eyeshot.Entities;

namespace AppModels.CustomEntity.CustomEntitySurrogate
{
    public class Blocking2DSurrogate : Framing2DSurrogate
    {
        public bool IsRotate { get; set; }
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

        protected override void CopyDataFromObject(Entity entity)
        {
            base.CopyDataFromObject(entity);
            if (entity is Blocking2D blocking)
            {
                IsRotate = blocking.IsRotate;
            }
        }

        protected override void CopyDataToObject(Entity entity)
        {
            base.CopyDataToObject(entity);
            if (entity  is Blocking2D blocking)
            {
                blocking.IsRotate = IsRotate;
            }
        }
    }
}
