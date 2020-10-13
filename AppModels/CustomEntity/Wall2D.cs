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
        public Attribute WallDimentionAttribute { get; set; }



        public Wall2D(Point3D insPoint, string blockName, double rotationAngleInRadians) : base(insPoint, blockName, rotationAngleInRadians)
        {

        }
    }
}
