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
    public class Beam2DSurrogate: Framing2DSurrogate
    {
        public Point3D OuterStartPoint { get; set; }
        public Point3D OuterEndPoint { get; set; }
        public int Thickness { get; set; }
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
        public bool IsBeamUnder { get; set; }
        public bool ShowDimension { get; set; }
        public Segment2D DimensionLine { get; set; }

        public Beam2DSurrogate(Text text) : base(text)
        {
        }

        protected override Entity ConvertToObject()
        {
            Entity beam2D = new Beam2D((Text)base.ConvertToObject());
            CopyDataToObject(beam2D);
            return beam2D;
        }

        protected override void CopyDataFromObject(Entity entity)
        {
            base.CopyDataFromObject(entity);
            if (!(entity is Beam2D beam)) return;
            this.OuterStartPoint = beam.OuterStartPoint;
            this.OuterEndPoint = beam.OuterEndPoint;
            this.Thickness = beam.Thickness;
            this.ShowDimension = beam.ShowDimension;
            this.IsBeamUnder = beam.IsBeamUnder;
            HangerAId = beam.HangerAId;
            HangerBId = beam.HangerBId;
            IsHangerA = beam.IsHangerA;
            IsHangerB = beam.IsHangerB;
            OutTriggerAId = beam.OutTriggerAId;
            OutTriggerBId = beam.OutTriggerBId;
            IsOutTriggerA = beam.IsOutTriggerA;
            IsOutTriggerB = beam.IsOutTriggerB;
            OutTriggerAFlipped = beam.OutTriggerAFlipped;
            OutTriggerBFlipped = beam.OutTriggerBFlipped;
            DimensionLine = beam.DimensionLine;
        }

        protected override void CopyDataToObject(Entity entity)
        {
            base.CopyDataToObject(entity);
            if (!(entity is Beam2D beam)) return;
            beam.DimensionLine = DimensionLine;
            beam.Thickness = Thickness;
            beam.SetFlippedOutriggerA(OutTriggerAFlipped);
            beam.SetFlippedOutriggerB(OutTriggerBFlipped);
            beam.IsBeamUnder = IsBeamUnder;
            beam.ShowDimension = ShowDimension;
            beam.IsHangerA = IsHangerA;
            beam.IsHangerB = IsHangerB;
            beam.IsOutTriggerA = IsOutTriggerA;
            beam.IsOutTriggerB = IsOutTriggerB;
            beam.HangerAId = HangerAId;
            beam.HangerBId = HangerBId;
            beam.OutTriggerAId = OutTriggerAId;
            beam.OutTriggerBId = OutTriggerBId;
            beam.OuterStartPoint = OuterStartPoint;
            beam.OuterEndPoint = OuterEndPoint;
            
        }
    }
}
