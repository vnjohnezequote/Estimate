using System.Collections.Generic;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.Interaface
{
    public interface IRectangleSolid:IEntity
    {
        Point3D StartPoint { get; }
        Point3D EndPoint { get;  }
        Point3D OuterStartPoint { get; set; }
        Point3D InnerStartPoint { get; }
        Point3D OuterEndPoint { get; set; }
        Point3D InnerEndPoint { get; }
        Point3D StartCenterLinePoint { get; }
        Point3D EndCenterLinePoint { get; }
        Point3D HangerACenterPoint { get; }
        Point3D HangerBCenterPoint { get; }
        int Thickness { get; }
        List<Point3D> FramingVertices { get;  }
        bool Project(Point3D point, out double t);
        Point3D PointAt(double t);


    }
}