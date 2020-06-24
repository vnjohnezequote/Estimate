using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace DrawingModule.Entities
{
    [Serializable]
    internal class MyCell : MultilineText
    {
        #region Constructor
        public MyCell(double x, double y, string textString, double width, double height, double lineSpaceDistance) : base(x, y, textString, width, height, lineSpaceDistance)
        {
        }

        public MyCell(double x, double y, double z, string textString, double width, double height, double lineSpaceDistance) : base(x, y, z, textString, width, height, lineSpaceDistance)
        {
        }

        public MyCell(double x, double y, double z, string textString, double width, double height, double lineSpaceDistance, alignmentType alignment) : base(x, y, z, textString, width, height, lineSpaceDistance, alignment)
        {
        }

        public MyCell(double x, double y, double z, string textString, double width, double height, double lineSpaceDistance, alignmentType alignment, string styleName) : base(x, y, z, textString, width, height, lineSpaceDistance, alignment, styleName)
        {
        }

        public MyCell(double x, double y, double z, string textString, double width, double height, double lineSpaceDistance, alignmentType alignment, string styleName, bool simplify, bool wrap = true) : base(x, y, z, textString, width, height, lineSpaceDistance, alignment, styleName, simplify, wrap)
        {
        }

        public MyCell(Point3D insPoint, string textString, double width, double height, double lineSpaceDistance) : base(insPoint, textString, width, height, lineSpaceDistance)
        {
        }

        public MyCell(Point3D insPoint, string textString, double width, double height, double lineSpaceDistance, alignmentType alignment) : base(insPoint, textString, width, height, lineSpaceDistance, alignment)
        {
        }

        public MyCell(Point3D insPoint, string textString, double width, double height, double lineSpaceDistance, alignmentType alignment, string styleName) : base(insPoint, textString, width, height, lineSpaceDistance, alignment, styleName)
        {
        }

        public MyCell(Point3D insPoint, string textString, double width, double height, double lineSpaceDistance, alignmentType alignment, string styleName, bool simplify, bool wrap = true) : base(insPoint, textString, width, height, lineSpaceDistance, alignment, styleName, simplify, wrap)
        {
        }

        public MyCell(Plane textPlane, string textString, double width, double height, double lineSpaceDistance) : base(textPlane, textString, width, height, lineSpaceDistance)
        {
        }

        public MyCell(Plane textPlane, string textString, double width, double height, double lineSpaceDistance, alignmentType alignment) : base(textPlane, textString, width, height, lineSpaceDistance, alignment)
        {
        }

        public MyCell(Plane textPlane, string textString, double width, double height, double lineSpaceDistance, alignmentType alignment, string styleName) : base(textPlane, textString, width, height, lineSpaceDistance, alignment, styleName)
        {
        }

        public MyCell(Plane textPlane, string textString, double width, double height, double lineSpaceDistance, alignmentType alignment, string styleName, bool simplify, bool wrap = true) : base(textPlane, textString, width, height, lineSpaceDistance, alignment, styleName, simplify, wrap)
        {
        }

        public MyCell(Plane textPlane, Point3D insPoint, string textString, double width, double height, double lineSpaceDistance) : base(textPlane, insPoint, textString, width, height, lineSpaceDistance)
        {
        }

        public MyCell(Plane textPlane, Point3D insPoint, string textString, double width, double height, double lineSpaceDistance, alignmentType alignment) : base(textPlane, insPoint, textString, width, height, lineSpaceDistance, alignment)
        {
        }

        public MyCell(Plane textPlane, Point3D insPoint, string textString, double width, double height, double lineSpaceDistance, alignmentType alignment, string styleName) : base(textPlane, insPoint, textString, width, height, lineSpaceDistance, alignment, styleName)
        {
        }

        public MyCell(Plane textPlane, Point3D insPoint, string textString, double width, double height, double lineSpaceDistance, alignmentType alignment, string styleName, bool simplify, bool wrap = true) : base(textPlane, insPoint, textString, width, height, lineSpaceDistance, alignment, styleName, simplify, wrap)
        {
        }

        public MyCell(Plane sketchPlane, Point2D insPoint, string textString, double width, double height, double lineSpaceDistance, alignmentType alignment) : base(sketchPlane, insPoint, textString, width, height, lineSpaceDistance, alignment)
        {
        }

        public MyCell(Plane sketchPlane, Point2D insPoint, string textString, double width, double height, double lineSpaceDistance, alignmentType alignment, string styleName) : base(sketchPlane, insPoint, textString, width, height, lineSpaceDistance, alignment, styleName)
        {
        }

        public MyCell(Plane sketchPlane, Point2D insPoint, string textString, double width, double height, double lineSpaceDistance, alignmentType alignment, string styleName, bool simplify, bool wrap = true) : base(sketchPlane, insPoint, textString, width, height, lineSpaceDistance, alignment, styleName, simplify, wrap)
        {
        }

        protected MyCell(Plane textPlane, Point3D insPoint, double width, double height, double lineSpaceDistance, alignmentType alignment) : base(textPlane, insPoint, width, height, lineSpaceDistance, alignment)
        {
        }

        public MyCell(MultilineText another) : base(another)
        {
        }

        public MyCell(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }


        #endregion

    }
}
