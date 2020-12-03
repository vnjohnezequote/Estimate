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
    public class FramingRectangle2DSurrogate: PlanarEntitySurrogate
    {
        
        public int Thickness { get; set; }
        public int Depth { get; set; }
        public Guid Id { get; set; }
        public Point3D OuterStartPoint { get; set; }
        public Point3D OuterEndPoint { get; set; }
        public Guid FramingReferenceId { get; set; }
        public Guid? HangerAId { get; set; }
        public Guid? HangerBId { get; set; }
        public bool IsHangerA { get; set; }
        public bool IsHangerB { get; set; }
        public Guid? OutTriggerAId { get; set; }
        public Guid? OuTriggerBId { get; set; }
        public bool IsOutTriggerA { get; set; }
        public bool IsOutTriggerB { get; set; }
        public bool OutTriggerAFlipped { get; set; }
        public bool OutTriggerBFlipped { get; set; }
        public FramingRectangle2DSurrogate(PlanarEntity planarEntity) : base(planarEntity)
        {
        }

        protected override void CopyDataToObject(Entity entity)
        {
            base.CopyDataToObject(entity);
            var joist2D = entity as Joist2D;
            if (joist2D != null)
            {
                joist2D.Id = Id;
                joist2D.HangerAId = HangerAId;
                joist2D.HangerBId = HangerBId;
                joist2D.IsHangerA = IsHangerA;
                joist2D.IsHangerB = IsHangerB;
                joist2D.OutTriggerAId = OutTriggerAId;
                joist2D.OutTriggerBId = OuTriggerBId;
                joist2D.IsOutTriggerA = IsOutTriggerA;
                joist2D.IsOutTriggerB = IsOutTriggerB;
                joist2D.SetFlippedOutriggerA(OutTriggerAFlipped);
                joist2D.SetFlippedOutriggerB(OutTriggerBFlipped);
                joist2D.FramingReferenceId = FramingReferenceId;
            }

        }

        protected override void CopyDataFromObject(Entity entity)
        {
            var joist2D = entity as Joist2D;
            if (joist2D != null)
            {
                Thickness = joist2D.Thickness;
                Depth = joist2D.Depth;
                Id = joist2D.Id;
                OuterStartPoint = joist2D.OuterStartPoint;
                OuterEndPoint = joist2D.OuterEndPoint;
                FramingReferenceId = joist2D.FramingReference.Id;
                HangerAId = joist2D.HangerAId;
                HangerBId = joist2D.HangerBId;
                IsHangerA = joist2D.IsHangerA;
                IsHangerB = joist2D.IsHangerB;
                OutTriggerAId = joist2D.OutTriggerAId;
                OuTriggerBId = joist2D.OutTriggerBId;
                IsOutTriggerA = joist2D.IsOutTriggerA;
                IsOutTriggerB = joist2D.IsOutTriggerB;
                OutTriggerAFlipped = joist2D.OutTriggerAFlipped;
                OutTriggerBFlipped = joist2D.OutTriggerBFlipped;

            }
            base.CopyDataFromObject(entity);
        }
    }
}
