using System;
using System.Drawing;
using ApplicationInterfaceCore;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels.EventArg;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.Control;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.DrawToolBase;
using DrawingModule.Helper;
using DrawingModule.UserInteractive;

namespace AppAddons.DrawingTools
{
    public class DrawingAngularDim : ToolBase
    {
        private bool _waitingForSelection;
        private Line _firstLineForAngularDim;
        private Line _secondLineForAngularDim ;
        public override string ToolName => "Angular Dim";

        public override Point3D BasePoint { get; protected set; }

        public DrawingAngularDim()
        {
            _waitingForSelection = true;
            EntityUnderMouseDrawingType = UnderMouseDrawingType.BySegment;
            IsUsingOrthorMode = false;
            IsSnapEnable = false;
        }
        [CommandMethod("AngleDim")]
        public void DrawAngularDim()
        {
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            while (true)
            {
                ToolMessage = "Please Select first line";
                var promptLineOption = new PromptSelectionOptions();
                var result = acDoc.Editor.GetSelection(promptLineOption);
                if (result.Status == PromptStatus.OK)
                {
                    this._firstLineForAngularDim = result.Value as Line;
                }
                else
                {
                    return;
                }

                ToolMessage = "Please Select Second line";
                result = acDoc.Editor.GetSelection(promptLineOption);
                if (result.Status == PromptStatus.OK)
                {
                    this._secondLineForAngularDim = result.Value as Line;
                }
                else
                {
                    return;
                }

                if (!CheckIntersctionOfTwoLine(_firstLineForAngularDim,_secondLineForAngularDim))
                {
                    ResetTool();
                    continue;
                }
                

                _waitingForSelection = false;
                //ToolMessage = "Please enter dim point";
                var promptPointOption = new PromptPointOptions(ToolMessage);
                Point3D dimPoint3D = null;
                var resultPoint = acDoc.Editor.GetPoint(promptPointOption);
                if (resultPoint.Status == PromptStatus.OK)
                {
                    dimPoint3D = resultPoint.Value;
                }
                else
                {
                    return;
                }

                if (_firstLineForAngularDim!= null && _secondLineForAngularDim!=null)
                {
                    this.ProcessAngularDim(Plane.XY, _firstLineForAngularDim, _secondLineForAngularDim, dimPoint3D);
                }
                
                this.ResetTool();
            }
            

        }

        private bool CheckIntersctionOfTwoLine(Line l1,Line l2)
        {
            Point3D p1 = this._firstLineForAngularDim.StartPoint;
            Point3D p2 = this._firstLineForAngularDim.EndPoint;
            Point3D p3 = this._secondLineForAngularDim.StartPoint;
            Point3D p4 = this._secondLineForAngularDim.EndPoint;
            Segment2D seg1 = new Segment2D(this._firstLineForAngularDim.StartPoint, this._firstLineForAngularDim.EndPoint);
            Segment2D seg2 = new Segment2D(this._secondLineForAngularDim.StartPoint, this._secondLineForAngularDim.EndPoint);
            return Segment2D.IntersectionLine(seg1, seg2, out var centerPoint);
        }
        private void ResetTool()
        {
            this._waitingForSelection = true;
            this._firstLineForAngularDim = null;
            this._secondLineForAngularDim = null;
        }

        private void ProcessAngularDim(Plane drawingPlane,Line firstLine, Line secondLine,Point3D dimPosition, double dimTextHeight = 10)
        {
            var angularDim = new AngularDim(drawingPlane, (Line)firstLine.Clone(), (Line)secondLine.Clone(), dimPosition, dimPosition, dimTextHeight) { TextSuffix = "°" };
            EntitiesManager.AddAndRefresh(angularDim, this.LayerManager.SelectedLayer.Name);
        }



        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {

            DrawInteractiveAngularLine((ICadDrawAble)sender, e);
        }


        private void DrawInteractiveAngularLine(ICadDrawAble canvas,DrawInteractiveArgs e)
        {
            if (e.CurrentPoint == null)
            {
                return;
            }
            if (this._firstLineForAngularDim != null)
            {
                DrawInteractiveUntilities.DrawCurveOrBlockRef((Entity)_firstLineForAngularDim, canvas);
            }

            if (this._secondLineForAngularDim != null)
            {
                DrawInteractiveUntilities.DrawCurveOrBlockRef((Entity)_secondLineForAngularDim, canvas);
            }

            if (this._waitingForSelection)
            {
                if (e.CurrentPoint!=null)
                {
                    Point3D currentScreen = canvas.WorldToScreen(e.CurrentPoint);

                    DrawInteractiveUntilities.DrawPickBox(new System.Drawing.Point((int)currentScreen.X, (int)(currentScreen.Y)),canvas);
                }

                return;
            }
            else
            {
                Point3D p1 = this._firstLineForAngularDim.StartPoint;
                Point3D p2 = this._firstLineForAngularDim.EndPoint;
                Point3D p3 = this._secondLineForAngularDim.StartPoint;
                Point3D p4 = this._secondLineForAngularDim.EndPoint;
                Segment2D seg1 = new Segment2D(this._firstLineForAngularDim.StartPoint, this._firstLineForAngularDim.EndPoint);
                Segment2D seg2 = new Segment2D(this._secondLineForAngularDim.StartPoint, this._secondLineForAngularDim.EndPoint);
                if (Segment2D.IntersectionLine(seg1, seg2, out var centerPoint))
                {
                    var centerP = centerPoint.ConvertPoint2DtoPoint3D();
                    double radius = centerP.DistanceTo(e.CurrentPoint);
                    if (Math.Abs(radius) < 0.0001 )
                    {
                        return;
                    }
                    Vector3D vector3D = new Vector3D(p1, p2);
                    //Vector3D y = Vector3D.Cross(dimPlane.AxisZ, vector3D);
                    Vector3D axisY = Vector3D.Cross(Vector3D.AxisZ, vector3D);
                    Circle circle = new Circle(new Plane(centerP, vector3D, axisY), radius);
                    Point3D point = p3;
                    if (p4.DistanceTo(centerPoint) > p3.DistanceTo(centerPoint))
                    {
                        point = p4;
                    }
                    double num;
                    circle.Project(point, out num);
                    double num2;
                    circle.Project(e.CurrentPoint, out num2);

                    double num3 = num;
                    if (num > 3.141592653589793)
                    {
                        num3 -= 3.141592653589793;
                    }
                    else
                    {
                        num3 += 3.141592653589793;
                    }
                    double num4 = (num < num3) ? num : num3;
                    double num5 = (num > num3) ? num : num3;
                    Point3D p6;
                    Point3D p7;
                    if (num2 <= num4)
                    {
                        p6 = circle.PointAt(0.0);
                        p7 = circle.PointAt(num4);
                    }
                    else if (num2 < 3.141592653589793)
                    {
                        p6 = circle.PointAt(3.141592653589793);
                        p7 = circle.PointAt(num4);
                    }
                    else if (num2 <= num5)
                    {
                        p6 = circle.PointAt(3.141592653589793);
                        p7 = circle.PointAt(num5);
                    }
                    else
                    {
                        p6 = circle.PointAt(0.0);
                        p7 = circle.PointAt(num5);
                    }
                    Vector3D vector3D2 = new Vector3D(centerP, p6);
                    vector3D2.Normalize();
                    Vector3D vector3D3 = new Vector3D(centerP, p7);
                    vector3D3.Normalize();
                    Vector3D vector3D4 = Vector3D.Cross(vector3D2, vector3D3);
                    vector3D4.Normalize();
                    double angular = Vector3D.AngleBetween(vector3D2, vector3D3).Round();
                    //double num6 = Vector3D.Dot(this._drawingPlane.AxisZ, vector3D4);
                    //double double_ = seg1.Project(this._drawingPlane.Project(p6));
                    //p6 = this.method_66(double_, p6, p1, p2, centerP, vector3D2);
                    //double double_2 = seg2.Project(this._drawingPlane.Project(p7));
                    //p7 = this.method_66(double_2, p7, p3, p4, centerP, vector3D3);
                    //renderContext.EnableXOR(true);
                    canvas.renderContext.SetColorWireframe(Color.RoyalBlue);
                    canvas.renderContext.SetLineStipple(1, 0x0F0F, canvas.Viewports[0].Camera);
                    canvas.renderContext.EnableLineStipple(true);
                    canvas.renderContext.DrawLine(canvas.WorldToScreen(centerP), canvas.WorldToScreen(p6));
                    canvas.renderContext.DrawLine(canvas.WorldToScreen(centerP), canvas.WorldToScreen(p7));
                    canvas.renderContext.EnableLineStipple(false);
                    Arc arc = new Arc(centerP, p6, p7);
                    string text = "A " + Utility.RadToDeg(arc.Domain.Length).ToString("f3") + "°";
                    canvas.DrawTextString(e.MousePosition.X, canvas.Size.Height - e.MousePosition.Y + 10, text, new Font("Tahoma", 8.25f), Color.Blue, ContentAlignment.BottomLeft);
                    canvas.renderContext.SetColorWireframe(Color.RoyalBlue);
                    canvas.renderContext.SetLineStipple(1, 0x0F0F, canvas.Viewports[0].Camera);
                    canvas.renderContext.EnableLineStipple(true);
                    DrawInteractiveUntilities.Draw(arc,canvas);
                    canvas.renderContext.EnableLineStipple(false);
                    //renderContext.EnableXOR(false);
                }
                else
                {
                    ToolMessage = "Two Line is Paralell";
                    ResetTool();

                }
            }
        }



    }
}
