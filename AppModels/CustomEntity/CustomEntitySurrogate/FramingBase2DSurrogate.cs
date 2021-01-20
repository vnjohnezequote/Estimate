using System;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;
using devDept.Serialization;

namespace AppModels.CustomEntity.CustomEntitySurrogate
{
    public class FramingBase2DSurrogate: TextSurrogate
    {
        public Guid Id { get; set; }
        public Guid FramingReferenceId { get; set; }
        public Guid LevelId { get; set; }
        public Guid FramingSheetId { get; set; }
        public double FullLength { get; set;  }
        
        public FramingBase2DSurrogate(Text text) : base(text)
        {
        }
        protected override void CopyDataToObject(Entity entity)
        {
            base.CopyDataToObject(entity);
            if (!(entity is IFraming2D framing2D)) return;
            framing2D.Id = Id;
            framing2D.FramingReferenceId = FramingReferenceId;
            framing2D.FullLength = FullLength;
            framing2D.LevelId = LevelId;
            framing2D.FramingSheetId = FramingSheetId;
        }
        protected override void CopyDataFromObject(Entity entity)
        {
            base.CopyDataFromObject(entity);
            if (entity is IFraming2D framing2D)
            {
                Id = framing2D.Id;
                FramingReferenceId = framing2D.FramingReference.Id;
                FullLength = framing2D.FullLength;
                LevelId = framing2D.LevelId;
                FramingSheetId = framing2D.FramingSheetId;


            }
            
        }
    }
}
