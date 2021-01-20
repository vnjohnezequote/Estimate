using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationInterfaceCore;
using ApplicationService;
using AppModels.EventArg;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using DrawingModule.CommandClass;
using DrawingModule.DrawInteractiveUtilities;
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

            if (click0.X<click1.X && click0.Y>click1.Y)
            {
                var tempP = click0;
                var temP1 = click1;
                Utility.Swap(ref tempP,ref temP1);
                click0 = tempP;
                click1 = temP1;
            }
            else
            {
                if (click1.X < click0.X || click1.Y < click0.Y)
                {
                    Point3D p0 = click0;
                    Point3D p1 = click1;

                    Utility.Swap(ref p0, ref p1);


                    click0 = p0;
                    click1 = p1;
                }
            }

            

            Vector3D axisX = new Vector3D(click0, click1);
            Vector3D axisY = Vector3D.Cross(Vector3D.AxisZ, axisX);
            //Vector3D axisY = Vector3D.Cross(axisX,Vector3D.AxisZ);
            //var angle = Vector3D.AngleBetween(axisX, axisY);
            //var finalAngle = Utility.RadToDeg(angle);

            _storePlane = new Plane(click0, axisX, axisY);
            //_storePlane = Plane.XY;

            Vector2D v1 = new Vector2D(click0, click1);
            //Vector2D v2 = Vector2D.AxisX;
            Vector2D v2 = new Vector2D(click0, e.CurrentPoint);
            //double angle90 = Math.PI / 2;
            //double angle90 = 0;
            double sign = Math.Sign(Vector2D.SignedAngleBetween(v1, v2));

            //offset p0-p1 at current
            Segment2D segment = new Segment2D(click0, click1);
            double offsetDist = e.CurrentPoint.DistanceTo(segment);

            _extPt1 = click0 + sign * _storePlane.AxisY * (offsetDist + canvas.DimTextHeight / 2);
            _extPt2 = click1 + sign * _storePlane.AxisY * (offsetDist + canvas.DimTextHeight / 2);
            //_extPt1 = click0;
            //_extPt2 = click1;
            Point3D dimPt1 = click0 + sign * _storePlane.AxisY * offsetDist;
            Point3D dimPt2 = click1 + sign * _storePlane.AxisY * offsetDist;
            //DrawInteractiveUntilities.DrawInteractiveSpotLine(_extPt1,_extPt2,canvas);
            //DrawInteractiveUntilities.DrawInteractiveSpotLine(dimPt1,dimPt2,canvas);
            DrawInteractiveUntilities.DrawInteractiveSpotLine(click0, click1, canvas);
            
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
            canvas.renderContext.SetColorWireframe(Color.Brown);

            canvas.renderContext.DrawLines(pts.ToArray());

            //draw dimension text
            canvas.renderContext.EnableXOR(false);

            string dimText = "L " + _extPt1.DistanceTo(_extPt2).ToString("f3")+"mm";
            canvas.DrawTextString(e.MousePosition.X, (int)canvas.Size.Height - e.MousePosition.Y + 10, dimText,
                new Font("Tahoma", 16f), Color.GreenYellow, ContentAlignment.BottomLeft);
            //canvas.DrawTextString(e.MousePosition.X, (int)canvas.Size.Height -e.MousePosition.Y + 10, finalAngle.ToString(),
            //    new Font("Tahoma", 16f), Color.GreenYellow, ContentAlignment.BottomLeft);
        }

    }
}
