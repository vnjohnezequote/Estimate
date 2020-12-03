using System;
using AppModels.CustomEntity.CustomEntitySurrogate;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.ViewModelEntity;
using devDept.Geometry;
using devDept.Serialization;

namespace AppModels.CustomEntity
{
    public class Joist2D : FramingRectangle2D,IFraming2DContaintHangerAndOutTrigger,IEntityVmCreateAble
    {
        private bool _outTriggerAFlipped;
        private bool _outTriggerBFlipped;
        
        public Guid? HangerAId { get; set; }
        public Guid? HangerBId { get; set; }
        public Hanger2D HangerA { get; set; }
        public Hanger2D HangerB { get; set; }
        public bool IsHangerA { get; set; }
        public bool IsHangerB { get; set; }
        public Guid? OutTriggerAId { get; set; }
        public Guid? OutTriggerBId { get; set; }
        public OutTrigger2D OutTriggerA { get; set; }
        public OutTrigger2D OutTriggerB { get; set; }
        public bool IsOutTriggerA { get; set; }
        public bool IsOutTriggerB { get; set; }
        public bool OutTriggerAFlipped
        {
            get => _outTriggerAFlipped;
            set
            {
                if (this.OutTriggerA == null) return;
                _outTriggerAFlipped = value;

                OutTriggerA?.Flipper(StartPoint, EndPoint);

            }
        }
        public bool OutTriggerBFlipped
        {
            get => _outTriggerBFlipped;
            set
            {
                if (this.OutTriggerB == null) return;
                _outTriggerBFlipped = value;

                OutTriggerB?.Flipper(StartPoint, EndPoint);
            }
        }

        public Joist2D(Point3D outerStartPoint, Point3D outerEndPoint, Joist joistReference,
            int thickness = 90) :
            base(outerStartPoint,outerEndPoint,joistReference,thickness)
        {
            
        }

        public Joist2D(Point3D outerStartPoint, Point3D outerEndPoint, int thickness) : base(outerStartPoint,
            outerEndPoint, thickness)
        {

        }

        public Joist2D(Joist2D another) : base(another)
        {

        }

        
        public void SetFlippedOutriggerA(bool outTriggerAFlipped)
        {
            _outTriggerAFlipped = outTriggerAFlipped;
        }
        public void SetFlippedOutriggerB(bool outTriggerBFlipped)
        {
            _outTriggerBFlipped = outTriggerBFlipped;
        }
       
        public IEntityVm CreateEntityVm(IEntitiesManager entitiesManager)
        {
            return new Joist2dVm(this, entitiesManager);
        }

        public override EntitySurrogate ConvertToSurrogate()
        {
            return new Joist2DSurrogate(this);
        }

        public override object Clone()
        {
            return new Joist2D(this);
        }
    }
}
