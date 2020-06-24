using devDept.Geometry;
using DrawingModule.Enums;

namespace DrawingModule.CustomControl
{
    public class SnapPoint : Point3D
    {
        public ObjectSnapType Type;
        public SnapPoint(): base()
        {
            Type = ObjectSnapType.None;
        }

        public SnapPoint(Point3D point3D, ObjectSnapType objectSnapType) : base(point3D.X, point3D.Y, point3D.Z)
        {
            this.Type = objectSnapType;
        }
        public override string ToString()
        {
            return base.ToString() + " | " + Type;
        }
    }
}
