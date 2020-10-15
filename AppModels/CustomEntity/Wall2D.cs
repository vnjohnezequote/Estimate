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
using Attribute = devDept.Eyeshot.Entities.Attribute;
using Point = System.Drawing.Point;

namespace AppModels.CustomEntity
{
    public class Wall2D: BlockReference
    {
        public int WallThickness { get; set; }
        public bool IsLoadBearingWall { get; set; }
        public bool IsShowDemension { get; set; }
        public Point3D StartPoint { get; set; }
        public Point3D EndPoint { get; set; }
        public Point3D DimensionPoint { get; set; }
        public double Distance { get; set; }
        public Block WallBlock { get; set; }
        public Line CenterLine { get; set; }
        public Line WallLine1 { get; set; }
        public Line WallLine2 { get; set; }
        public Line StartWallLine { get; set; }
        public Line EndWallLine { get; set; }
        public Attribute WallDimentionAttribute { get; set; }



        public Wall2D(Point3D startPoint,Point3D endPoint, Point3D insPoint, string blockName, double rotationAngleInRadians, int wallThickness=90, bool isLoadBearingWall = true, bool isShowDimension = false) : base(insPoint, blockName, rotationAngleInRadians)
        {
            this.WallThickness = wallThickness;
            IsLoadBearingWall = isLoadBearingWall;
            IsShowDemension = isShowDimension;
            StartPoint = startPoint;
            EndPoint = endPoint;
            CenterLine = new Line(startPoint,endPoint);
            InitializerWallLine(startPoint,endPoint);
            WallBlock = new Block(blockName);
            WallBlock.Entities.Add(WallLine1);
            WallBlock.Entities.Add(WallLine2);
            WallBlock.Entities.Add(StartWallLine);
            WallBlock.Entities.Add(EndWallLine);

        }

        private void InitializerWallLine(Point3D startPoint, Point3D endPoint)
        {
            var offsetLine = CenterLine.Offset(WallThickness / 2, Vector3D.AxisZ);
            Utility.OffsetPoint(offsetLine.StartPoint,offsetLine.EndPoint,WallThickness/2,out var  endPoint1);
            Utility.OffsetPoint(endPoint,startPoint,45,out var startPoint1);
            WallLine1 = new Line(startPoint1,endPoint1);
            offsetLine = CenterLine.Offset(-WallThickness / 2, Vector3D.AxisZ);
            Utility.OffsetPoint(offsetLine.StartPoint, offsetLine.EndPoint, WallThickness / 2, out var endPoint2);
            Utility.OffsetPoint(endPoint, startPoint, 45, out var startPoint2);
            WallLine2 = new Line(startPoint2,endPoint2);
            StartWallLine = new Line(WallLine1.StartPoint,WallLine2.StartPoint);
            EndWallLine = new Line(WallLine1.EndPoint, WallLine1.EndPoint);
        }
    }
}
