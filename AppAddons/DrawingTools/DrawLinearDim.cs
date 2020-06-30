using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationService;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace AppAddons.DrawingTools
{
    public class DrawLinearDim : ToolBase
    {
        public override Point3D BasePoint { get; protected set; }
        private Plane _storePlane;
        private Point3D _currentPoint;
        private double _dimTextHeight;
        public sealed override string ToolMessage { get; set; }


        public DrawLinearDim()
        {

        }

        [CommandMethod("DLI")]
        public void LinearDim()
        {
            var dimTextHeight = 10.0;
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            ToolMessage = "Please enter first Point for Dim";
            var promptPointOp = new PromptPointOptions(ToolMessage);
            var res = acDoc.Editor.GetPoint(promptPointOp);
            if (res.Status == PromptStatus.Cancel)
            {
                //Application.DocumentManager.MdiActiveDocument.Editor.UnRegisterDrawInteractive(this);
                return;
            }
            var startPoint = res.Value;

            ToolMessage = "Please enter Second Point for Dim";
            res = acDoc.Editor.GetPoint(promptPointOp);
            if (res.Status == PromptStatus.Cancel)
            {
                //Application.DocumentManager.MdiActiveDocument.Editor.UnRegisterDrawInteractive(this);
                return;
            }

            var endPoint = res.Value;
            ToolMessage = "Please ender Dim Point Location";

            res = acDoc.Editor.GetPoint(promptPointOp);
            if (res.Status == PromptStatus.Cancel)
            {
                //Application.DocumentManager.MdiActiveDocument.Editor.UnRegisterDrawInteractive(this);
                return;
            }

            var dimPoint = res.Value;

            bool verticalDim = (dimPoint.X > startPoint.X && dimPoint.X > endPoint.X) || (dimPoint.X < startPoint.X && dimPoint.X < endPoint.X);

            Vector3D axisX;
            Point3D extPt1;
            Point3D extPt2;
            if (verticalDim)
            {

                axisX = Vector3D.AxisY;

                extPt1 = new Point3D(dimPoint.X, startPoint.Y);
                extPt2 = new Point3D(dimPoint.X, endPoint.Y);

                if (dimPoint.X > startPoint.X && dimPoint.X > endPoint.X)
                {
                    extPt1.X += dimTextHeight / 2;
                    extPt2.X += dimTextHeight / 2;
                }
                else
                {
                    extPt1.X -= dimTextHeight / 2;
                    extPt2.X -= dimTextHeight / 2;
                }
            }
            else
            {
                axisX = Vector3D.AxisX;

                extPt1 = new Point3D(startPoint.X, dimPoint.Y);
                extPt2 = new Point3D(endPoint.X, dimPoint.Y);

                if (dimPoint.Y > startPoint.Y && dimPoint.Y > endPoint.Y)
                {
                    extPt1.Y += dimTextHeight / 2;
                    extPt2.Y += dimTextHeight / 2;
                }
                else
                {
                    extPt1.Y -= dimTextHeight / 2;
                    extPt2.Y -= dimTextHeight / 2;
                }
            }

            Vector3D axisY = Vector3D.Cross(Vector3D.AxisZ, axisX);
            var storePlan = new Plane(startPoint, axisX, axisY);

            ProcessDim(extPt1,extPt2,dimPoint,dimTextHeight,storePlan);

        }

        private void ProcessDim(Point3D p1, Point3D p2, Point3D p3, double dimTextHeight,Plane storePlane)
        {
            var linearEntity = new LinearDim(storePlane, p1, p2, p3, dimTextHeight);
            EntitiesManager.AddAndRefresh(linearEntity, this.LayerManager.SelectedLayer.Name);
        }
}
}
