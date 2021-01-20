using System.Collections.Generic;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.Interaface
{
    public interface IRectangleSolid:IEntity
    {
        Point3D StartPoint { get; }
        Point3D EndPoint { get;  }
        Point3D OuterStartPoint { get; }
        Point3D InnerStartPoint { get; }
        Point3D OuterEndPoint { get; }
        Point3D InnerEndPoint { get; }
        Point3D StartTopPoint { get; }
        Point3D EndTopPoint { get; }
        Point3D HangerACenterPoint { get; }
        Point3D HangerBCenterPoint { get; }
        int Thickness { get; }
        List<Point3D> FramingVertices { get;  }


    }
}