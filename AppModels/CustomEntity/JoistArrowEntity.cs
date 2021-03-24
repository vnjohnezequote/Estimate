using System;
using AppModels.Interaface;
using AppModels.Undo;
using AppModels.ViewModelEntity;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.CustomEntity
{
    public class JoistArrowEntity: Line, IEntityVmCreateAble,ICloneAbleToUndo
    {
        public FramingNameEntity FramingName { get; set; }
        public Guid FramingNameId { get; set; }
        public JoistArrowEntity(Point3D start, Point3D end) : base(start, end)
        {
            
        }

        protected JoistArrowEntity(Line another) : base(another)
        {
        }

        protected override void Draw(DrawParams data)
        {
            base.Draw(data);
            DrawArrow(data);

        }

        private void DrawArrow(DrawParams data)
        {
            var lineSegment = new Segment2D((Point3D)StartPoint.Clone(), (Point3D)EndPoint.Clone());
            lineSegment.ExtendBy(-500,-500);
            var upperSeg = lineSegment.Offset(-135);
            var lowerSeg = lineSegment.Offset(135);
            data.RenderContext.DrawBufferedLine(StartPoint,upperSeg.P0.ConvertPoint2DtoPoint3D());
            data.RenderContext.DrawBufferedLine(EndPoint, lowerSeg.P1.ConvertPoint2DtoPoint3D());
            StartArrow = upperSeg.P0.ConvertPoint2DtoPoint3D();
            EndArrow = lowerSeg.P1.ConvertPoint2DtoPoint3D();
        }
        public Point3D StartArrow { get; set; }
        public Point3D EndArrow { get; set; }
        public override object Clone()
        {
            return new JoistArrowEntity(this);
        }

        public IEntityVm CreateEntityVm(IEntitiesManager entitiesManager)
        {
            return new JoistArrowVm(this, entitiesManager);
        }
    }
}
