using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using devDept.Serialization;
using Point = System.Drawing.Point;

namespace AppModels.CustomEntity
{
    public class Wall2D: Text
    {
        public int WallThickness { get; set; }
        public bool IsLoadBearingWall { get; set; }
        public bool IsShowDemension { get; set; }
        public Point3D StartPoint { get; set; }
        public Point3D EndPoint { get; set; }
        public Point3D PointDimension { get; set; }
        public double Distance { get; set; }
        public new Point3D InsertionPoint => Plane.Origin;

        public Wall2D(Plane textPlane, Point3D startPoint, Point3D endPoint,int wallThickness, bool isLoadBearingWall = true, bool isShowDimension =true, alignmentType alignment = alignmentType.BaselineCenter) : base(textPlane, startPoint, 300, alignment)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            Plane.Origin = new Point3D((StartPoint.X+EndPoint.X)/2,(StartPoint.Y+EndPoint.Y)/2);
            this.TextString = "T";
        }

        protected override void Draw(DrawParams data)
        {
           //data.RenderContext.DrawBufferedLine(Vertices[0],Vertices[1]);
           data.RenderContext.DrawBufferedLine(StartPoint,EndPoint);
           data.RenderContext.DrawQuads(Vertices,new Vector3D[]
           {
               Vector3D.AxisZ
           });
        }
        

    }
}
