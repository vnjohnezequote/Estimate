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
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace AppAddons.DrawingTools
{
    public class DrawLinearDim : DimToolBase
    {
        public override string ToolName => "Linear Dim";
        public DrawLinearDim():base()
        {

        }

        [CommandMethod("DLI")]
        public void LinearDim()
        {
            OnProcessCommand();
        }

        protected override void DrawInteractiveDim(ICadDrawAble canvas, DrawInteractiveArgs e)
        {
            if(_startPoint == null ) return;
            if (e.CurrentPoint ==null) return;
            Point3D click0 = null;
            Point3D click1 = null;

            click0 = _startPoint;
            if (_endPoint == null )
            {
                click1 = e.CurrentPoint;
            }
            else
            {
                click1 = _endPoint;
            }


            //if (this._selectedEntityForDim != null)
            //{
            //    if (_selectedEntityForDim is Line line)
            //    {
            //        click0 = line.StartPoint;
            //        click1 = line.EndPoint;
            //    }
            //}
            // We need to have two reference _clickPoints selected, might be snapped vertices

            bool verticalDim = (e.CurrentPoint.X > click0.X && e.CurrentPoint.X > click1.X) || (e.CurrentPoint.X < click0.X && e.CurrentPoint.X < click1.X);

            Vector3D axisX;

            if (verticalDim)
            {

                axisX = Vector3D.AxisY;

                _extPt1 = new Point3D(e.CurrentPoint.X, click0.Y);
                _extPt2 = new Point3D(e.CurrentPoint.X, click1.Y);

                if (e.CurrentPoint.X > click0.X && e.CurrentPoint.X > click1.X)
                {
                    _extPt1.X += canvas.DimTextHeight / 2;
                    _extPt2.X += canvas.DimTextHeight / 2;
                }
                else
                {
                    _extPt1.X -= canvas.DimTextHeight / 2;
                    _extPt2.X -= canvas.DimTextHeight / 2;
                }

            }
            else//for horizontal
            {

                axisX = Vector3D.AxisX;

                _extPt1 = new Point3D(click0.X, e.CurrentPoint.Y);
                _extPt2 = new Point3D(click1.X, e.CurrentPoint.Y);

                if (e.CurrentPoint.Y > click0.Y && e.CurrentPoint.Y > click1.Y)
                {
                    _extPt1.Y += canvas.DimTextHeight / 2;
                    _extPt2.Y += canvas.DimTextHeight / 2;
                }
                else
                {
                    _extPt1.Y -= canvas.DimTextHeight / 2;
                    _extPt2.Y -= canvas.DimTextHeight / 2;
                }

            }

            Vector3D axisY = Vector3D.Cross(Vector3D.AxisZ, axisX);


            List<Point3D> pts = new List<Point3D>();

            // Draw extension line1
            pts.Add(canvas.WorldToScreen(click0));
            pts.Add(canvas.WorldToScreen(_extPt1));

            // Draw extension line2
            pts.Add(canvas.WorldToScreen(click1));
            pts.Add(canvas.WorldToScreen(_extPt2));

            //Draw dimension line
            Segment3D extLine1 = new Segment3D(click0, _extPt1);
            Segment3D extLine2 = new Segment3D(click1, _extPt2);
            Point3D pt1 = e.CurrentPoint.ProjectTo(extLine1);
            Point3D pt2 = e.CurrentPoint.ProjectTo(extLine2);

            pts.Add(canvas.WorldToScreen(pt1));
            pts.Add(canvas.WorldToScreen(pt2));
            canvas.renderContext.SetColorWireframe(Color.Brown);
            canvas.renderContext.DrawLines(pts.ToArray());

            //store dimensioning plane
            _storePlane = new Plane(click0, axisX, axisY);

            //draw dimension text
            canvas.renderContext.EnableXOR(false);

            string dimText = "L " + _extPt1.DistanceTo(_extPt2).ToString("f3");
            canvas.DrawTextString(e.MousePosition.X, (int)canvas.Size.Height - e.MousePosition.Y + 10, dimText,
               new Font("Tahoma", 16f), Color.Chartreuse, ContentAlignment.BottomLeft);
        }



    }
}
