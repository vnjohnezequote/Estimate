using System;
using devDept.Eyeshot.Entities;
using devDept.Serialization;

namespace AppModels.CustomEntity.CustomEntitySurrogate
{
    public class FramingNameSurrogate: TextSurrogate
    {
        public Guid Id { get; set; }
        public Guid LevelId { get; set; }
        public Guid FramingSheetId { get; set; }
        public Guid FramingReferenceId { get; set; }

        public FramingNameSurrogate(Text text) : base(text)
        {
        }

        protected override Entity ConvertToObject()
        {
            Entity enti;
            enti = new FramingNameEntity((Text) base.ConvertToObject());
            CopyDataToObject(enti);
            return enti;
        }

        protected override void CopyDataToObject(Entity ent)
        {
            base.CopyDataToObject(ent);
            if (ent is FramingNameEntity framingName)
            {
                framingName.Id = Id;
                framingName.LevelId = LevelId;
                framingName.FramingSheetId = FramingSheetId;
                framingName.FramingReferenceId = FramingReferenceId;
            }
        }

        protected override void CopyDataFromObject(Entity entity)
        {
            base.CopyDataFromObject(entity);
            if (entity is FramingNameEntity framingName)
            {
                Id = framingName.Id;
                LevelId = framingName.LevelId;
                FramingSheetId = framingName.FramingSheetId;
                FramingReferenceId = framingName.FramingReferenceId;
            }
        }
    }
}
