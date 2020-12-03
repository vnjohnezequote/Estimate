using System;
using AppModels.CustomEntity.CustomEntitySurrogate;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.ViewModelEntity;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Serialization;

namespace AppModels.CustomEntity
{
    public class OutTrigger2D: FramingRectangle2D, IEntityVmCreateAble
    {
        private int _insizeLength;
        private int _outSizeLength;
        
        public int InSizeLength
        {
            get => _insizeLength;
            set
            {
                var extendLength = value - _insizeLength;
                _insizeLength = value;
                var outerLine = new Segment2D(OuterStartPoint, OuterEndPoint);
                outerLine.ExtendBy(0,extendLength);
                this.SetOuterEndPoint(outerLine.P1.ConvertPoint2DtoPoint3D());
                this.RegenFramingGeometry(OuterStartPoint,OuterEndPoint);
                this.RegenMode = regenType.RegenAndCompile;
            }
        }
        public int OutSizeLength
        {
            get => _outSizeLength;
            set
            {
                var extendLength = value - _outSizeLength;
                _outSizeLength = value;
                var outerLine = new Segment2D(OuterStartPoint, OuterEndPoint);
                outerLine.ExtendBy(extendLength, 0);
                this.SetOuterStartPoint(outerLine.P0.ConvertPoint2DtoPoint3D());
                this.RegenFramingGeometry(OuterStartPoint, OuterEndPoint,Flipped);
                this.RegenMode = regenType.RegenAndCompile;
            }
        }

        
        public OutTrigger2D(Point3D outerStartPoint, Point3D outerEndPoint, OutTrigger outrigger, int thickness, bool flipped = true,
            int outSizeLength = 450, int inSizeLength = 900) : base(outerStartPoint,outerEndPoint,outrigger,thickness,flipped)
        {
            _insizeLength = inSizeLength;
            _outSizeLength = outSizeLength;
        }

        public OutTrigger2D(OutTrigger2D another) : base(another)
        {
            _insizeLength = this.InSizeLength;
            _outSizeLength = this.OutSizeLength;
            Flipped = this.Flipped;
        }
        public IEntityVm CreateEntityVm(IEntitiesManager entitiesManager)
        {
            return new OutTrigger2dVm(this, entitiesManager);
        }
        public void SetInsizeLength(int inSize)
        {
            _insizeLength = inSize;
        }
        public void SetOutSizeLength(int outSize)
        {
            _outSizeLength = outSize;
        }
        public void Flipper(Point3D startPoint,Point3D endPoint)
        {
            Flipped = !Flipped;
            var line = new Line(this.OuterStartPoint, this.OuterEndPoint);
            var p0 = startPoint;
            var p1 = endPoint;
            if (endPoint.X < startPoint.X || endPoint.Y < startPoint.Y)
            {
                Utility.Swap(ref p0, ref p1);

                //_clickPoints[0] = p0;
                //_clickPoints[1] = p1;
            }
            var axisX = new Vector3D(p0, p1);
            var mirrorPlane = new Plane(startPoint, axisX, Vector3D.AxisZ);
            var mirror = new Mirror(mirrorPlane);
            line.TransformBy(mirror);
            SetOuterStartPoint(line.StartPoint);
            SetOuterEndPoint(line.EndPoint);
            this.RegenFramingGeometry(OuterStartPoint,OuterEndPoint,Flipped);
            this.RegenMode = regenType.RegenAndCompile;
        }

        public override EntitySurrogate ConvertToSurrogate()
        {
            return new OutTrigger2DSurrogate(this);
        }
        public override object Clone()
        {
            return new OutTrigger2D(this);
        }
    }
}
