using System;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Serialization;

namespace AppModels.CustomEntity.CustomEntitySurrogate
{
    public class FramingRectangle2DSurrogate: PlanarEntitySurrogate
    {
        
        public int Thickness { get; set; }
        public int Depth { get; set; }
        public Guid Id { get; set; }
        public Guid LevelId { get; set; }
        public Guid FramingSheetId { get; set; }
        public bool Flipped { get; set; }
        public Point3D OuterStartPoint { get; set; }
        public Point3D OuterEndPoint { get; set; }
        public Guid FramingReferenceId { get; set; }
        
        public FramingRectangle2DSurrogate(PlanarEntity planarEntity) : base(planarEntity)
        {
        }

        protected override void CopyDataToObject(Entity entity)
        {
            base.CopyDataToObject(entity);
            if (entity is FramingRectangle2D framingRectangle2D)
            {
                framingRectangle2D.FramingReferenceId = FramingReferenceId;
                framingRectangle2D.Id = Id;
                framingRectangle2D.LevelId = LevelId;
                framingRectangle2D.FramingSheetId = FramingSheetId;
                framingRectangle2D.Flipped = Flipped;
                framingRectangle2D.Thickness = Thickness;
                framingRectangle2D.Depth = Depth;
                framingRectangle2D.OuterStartPoint = OuterStartPoint;
                framingRectangle2D.OuterEndPoint = OuterEndPoint;
                
            }

        }

        protected override void CopyDataFromObject(Entity entity)
        {
            base.CopyDataFromObject(entity);
            if (entity is FramingRectangle2D framingRectangle2D)
            {
                Thickness = framingRectangle2D.Thickness;
                Depth = framingRectangle2D.Depth;
                Id = framingRectangle2D.Id;
                LevelId = framingRectangle2D.LevelId;
                FramingSheetId = framingRectangle2D.FramingSheetId;
                OuterStartPoint = framingRectangle2D.OuterStartPoint;
                OuterEndPoint = framingRectangle2D.OuterEndPoint;
                FramingReferenceId = framingRectangle2D.FramingReference.Id;
                Flipped = framingRectangle2D.Flipped;
               
            }
            
        }
    }
}
