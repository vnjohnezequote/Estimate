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
    public class DrawAlignDim : ToolBase
    {
        public override Point3D BasePoint { get; protected set; }
        private Point3D _startPoint;
        private Point3D _endPoint;
        private Point3D _dimPoint;
        private Point3D _extPt1;
        private Point3D _extPt2;
        private Plane _storePlane;
        private Point3D _currentPoint;
        private double _dimTextHeight;
        public sealed override string ToolMessage { get; set; }


        public DrawAlignDim()
        {

        }

        [CommandMethod("DLA")]
        public void AlignDim()
        {
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            ToolMessage = "Please enter first Point for Dim";
            var promptPointOp = new PromptPointOptions(ToolMessage);
            var res = acDoc.Editor.GetPoint(promptPointOp);
            if (res.Status == PromptStatus.Cancel)
            {
                //Application.DocumentManager.MdiActiveDocument.Editor.UnRegisterDrawInteractive(this);
                return;
            }
            _startPoint = res.Value;

            ToolMessage = "Please enter Second Point for Dim";
            res = acDoc.Editor.GetPoint(promptPointOp);
            if (res.Status == PromptStatus.Cancel)
            {
                //Application.DocumentManager.MdiActiveDocument.Editor.UnRegisterDrawInteractive(this);
                return;
            }

            _endPoint = res.Value;
            ToolMessage = "Please ender Dim Point Location";

            res = acDoc.Editor.GetPoint(promptPointOp);
            if (res.Status == PromptStatus.Cancel)
            {
                //Application.DocumentManager.MdiActiveDocument.Editor.UnRegisterDrawInteractive(this);
                return;
            }

            _dimPoint = res.Value;

            if (_extPt1 != null && _extPt2 != null && _storePlane != null)
            {
                ProcessDim(_extPt1, _extPt2, _dimPoint, _storePlane);
            }



        }

        private void ProcessDim(Point3D p1, Point3D p2, Point3D p3, Plane storePlane, double dimTextHeight = 10.0)
        {
            var linearEntity = new LinearDim(storePlane, p1, p2, p3, dimTextHeight);
            EntitiesManager.AddAndRefresh(linearEntity, this.LayerManager.SelectedLayer.Name);
        }

        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {

            DrawInteractiveLinearDim((ICadDrawAble)sender, e);
        }

        private void DrawInteractiveLinearDim(ICadDrawAble canvas, DrawInteractiveArgs e)
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
