using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationInterfaceCore;
using ApplicationService;
using AppModels.EventArg;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace AppAddons.DrawingTools
{
    public class DrawAlignDim : DimToolBase
    {
        public override string ToolName => "Align Dim";
        public DrawAlignDim() : base()
        {

        }

        [CommandMethod("DLA")]
        public void AlignDim()
        {
            OnProcessCommand();
        }

        protected override void DrawInteractiveDim(ICadDrawAble canvas, DrawInteractiveArgs e)
        {
            if (_startPoint == null) return;
            if (e.CurrentPoint == null) return;
            Point3D click0 = null;
            Point3D click1 = null;

            click0 = _startPoint;
            if (_endPoint == null)
            {
                click1 = e.CurrentPoint;
            }
            else
            {
                click1 = _endPoint;
            }
            if (click0 == click1)
            {
                return;
            }

            if (click1.X < click0.X || click1.Y < click0.Y)
            {
                Point3D p0 = click0;
                Point3D p1 = click1;

                Utility.Swap(ref p0, ref p1);


                click0 = p0;
                click1 = p1;
            }

            Vector3D axisX = new Vector3D(click0, click1);
            Vector3D axisY = Vector3D.Cross(Vector3D.AxisZ, axisX);
            //if (click0 == this.CurrentPoint || click1 == this.CurrentPoint)
            //{
            //    _storePlane = Plane.XY;
            //}
            //else
            //{
            //    _storePlane = new Plane(click0, axisX, axisY);
            //}
            //_storePlane = Plane.XY;
            
            _storePlane = new Plane(click0, axisX, axisY);

            Vector2D v1 = new Vector2D(click0, click1);
            //Vector2D v2 = Vector2D.AxisX;
            Vector2D v2 = new Vector2D(click0, e.CurrentPoint);
            //double angle90 = Math.PI / 2;
            double angle90 = 0;
            double sign = Math.Sign(Vector2D.SignedAngleBetween(v1, v2)) + angle90;

            //offset p0-p1 at current
            Segment2D segment = new Segment2D(click0, click1);
            double offsetDist = e.CurrentPoint.DistanceTo(segment);
            _extPt1 = click0 + sign * _storePlane.AxisY * (offsetDist + canvas.DimTextHeight / 2);
            _extPt2 = click1 + sign * _storePlane.AxisY * (offsetDist + canvas.DimTextHeight / 2);
            Point3D dimPt1 = click0 + sign * _storePlane.AxisY * offsetDist;
            Point3D dimPt2 = click1 + sign * _storePlane.AxisY * offsetDist;

            List<Point3D> pts = new List<Point3D>();

            // Draw extension line1
            pts.Add(canvas.WorldToScreen(click0));
            pts.Add(canvas.WorldToScreen(_extPt1));

            // Draw extension line2
            pts.Add(canvas.WorldToScreen(click1));
            pts.Add(canvas.WorldToScreen(_extPt2));

            //Draw dimension line
            pts.Add(canvas.WorldToScreen(dimPt1));
            pts.Add(canvas.WorldToScreen(dimPt2));

            canvas.renderContext.DrawLines(pts.ToArray());

            //draw dimension text
            canvas.renderContext.EnableXOR(false);

            string dimText = "L " + _extPt1.DistanceTo(_extPt2).ToString("f3");
            canvas.DrawTextString(e.MousePosition.X, (int)canvas.Size.Height -e.MousePosition.Y + 10, dimText,
                new Font("Tahoma", 8.25f), Color.Blue, ContentAlignment.BottomLeft);
        }

    }
}
