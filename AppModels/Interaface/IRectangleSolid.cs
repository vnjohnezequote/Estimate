using System.Collections.Generic;
using devDept.Geometry;

namespace AppModels.Interaface
{
    public interface IRectangleSolid
    {
        Point3D StartPoint { get; }
        Point3D EndPoint { get;  }
        Point3D OuterStartPoint { get; }
        Point3D InnerStartPoint { get; }
        Point3D OuterEndPoint { get; }
        Point3D InnerEndPoint { get; }
        int Thickness { get; }
        List<Point3D> FramingVertices { get;  }


    }
}