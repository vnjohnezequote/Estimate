using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ApplicationInterfaceCore;
using AppModels;
using AppModels.EventArg;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using DrawingModule.CommandClass;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.DrawToolBase;
using DrawingModule.Helper;

namespace AppAddons.EditingTools
{
    public class FilletTool : ToolBase
    {
       
        public override string ToolName => "Fillet";
        public override Point3D BasePoint { get; protected set; }
        private bool _processingTool;
        private Entity _firstSelectedEntity;
        private Entity _secondSelectedEntity;
        private int _selectedEntityIndex;
        private double _extensionLength = 10000;
        private Point3D _firstMousePoint;
        private Point3D _secondMousePoint;
        private System.Drawing.Point _clickPoint;
        public FilletTool()
        {
            _processingTool = true;
            IsSnapEnable = false;
            IsUsingOrthorMode = false;
            _selectedEntityIndex = -1;
        }
        [CommandMethod("Fillet")]
        public void Fillet()
        {
            OnProcessCommand();
        }
        protected virtual void OnProcessCommand()
        {
            while (_processingTool)
            {

            }

        }
        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {
            DrawInteractiveFillet((ICadDrawAble)sender, e);

        }
        public override void NotifyMouseDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = (ICadDrawAble)sender;
            _clickPoint = RenderContextUtility.ConvertPoint(canvas.GetMousePosition(e));
            var index = canvas.GetEntityUnderMouseCursor(_clickPoint);
            if (_firstSelectedEntity != null && _secondSelectedEntity != null) return;
            if (index > -1)
            {
                _selectedEntityIndex = index;
            }

            if (_secondSelectedEntity != null) return;
            //canvas.ScreenToPlane(_clickPoint, canvas.DrawingPlane, out clickPoint);
        }
        public override void NotifyPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this._processingTool = false;
                e.Handled = true;
            }
        }
        private void DrawInteractiveFillet(ICadDrawAble canvas, DrawInteractiveArgs e)
        {
            if (_firstSelectedEntity == null)
            {
                DrawInteractiveUntilities.DrawSelectionMark(canvas, e.MousePosition);
                ToolMessage = "Select boundary entity";
                if (_selectedEntityIndex != -1)
                {
                    _firstSelectedEntity = EntitiesManager.GetEntity(_selectedEntityIndex);
                    //var firstMousePosition = RenderContextUtility.ConvertPoint(canvas.GetMousePosition(e.MousePosition));
                    canvas.ScreenToPlane(_clickPoint, Plane.XY, out _firstMousePoint);
                    _selectedEntityIndex = -1;
                    return;
                }
            }
            else if (_secondSelectedEntity == null)
            {
                DrawInteractiveUntilities.DrawSelectionMark(canvas, e.MousePosition);
                canvas.renderContext.EnableXOR(false);
                ToolMessage = "Select entity to Fillet";
            }

            if (_secondSelectedEntity == null)
            {
                if (_selectedEntityIndex != -1)
                {
                    _secondSelectedEntity = EntitiesManager.GetEntity(_selectedEntityIndex);
                    canvas.ScreenToPlane(_clickPoint, Plane.XY, out _secondMousePoint);
                    _selectedEntityIndex = -1;
                }
            }

            if (_firstSelectedEntity != null && _secondSelectedEntity != null)
            {
                if (_firstSelectedEntity is ICurve && _secondSelectedEntity is ICurve)
                {
                    if (_firstSelectedEntity is Line firstLine && _secondSelectedEntity is Line secondLine)
                    {
                        var firstSegment = new Segment2D(firstLine.StartPoint, firstLine.EndPoint);
                        var secondSegment = new Segment2D(secondLine.StartPoint, secondLine.EndPoint);

                        //Segment2D.Intersection(firstSegment, secondSegment, out var interSectPTest);
                        //Segment2D.IntersectionAndT(firstSegment, secondSegment, out var interSectPTest2);
                        if (Segment2D.IntersectionAndT(firstSegment, secondSegment, out var interSectP))
                        {
                            SetNewPoint(firstLine,_firstMousePoint,interSectP.ConvertPoint2DtoPoint3D());
                            SetNewPoint(secondLine, _secondMousePoint, interSectP.ConvertPoint2DtoPoint3D());
                        }
                        else
                        {
                            Segment2D.IntersectionLine(firstSegment, secondSegment, out var intersecPoint);
                            if (intersecPoint!=null)
                            {
                                ApendLine(firstLine, intersecPoint.ConvertPoint2DtoPoint3D());
                                ApendLine(secondLine, intersecPoint.ConvertPoint2DtoPoint3D());
                            }
                            
                        }
                    }
                    //bool success = false;
                    //bool success2 = false;
                    //success = FilletProcess(_firstSelectedEntity, _secondSelectedEntity);
                    //success2 = FilletProcess(_secondSelectedEntity, _firstSelectedEntity);
                    //if (success)
                    //{
                    //    EntitiesManager.RemoveEntity(_secondSelectedEntity);
                    //}

                    //if (success2)
                    //{
                    //    EntitiesManager.RemoveEntity(_firstSelectedEntity);
                    //}
                    EntitiesManager.Refresh();
                }
                _firstSelectedEntity = null;
                _secondSelectedEntity = null;
                _selectedEntityIndex = -1;

            }

        }

        private void ApendLine(Line line, Point3D apendPoint)
        {
            var distance = line.StartPoint.DistanceTo(apendPoint);
            var distance2 = line.EndPoint.DistanceTo(apendPoint);
            Point3D movePoint = null;
            if (distance>distance2)
            {
                line.EndPoint = apendPoint;
            }
            else
            {
                line.StartPoint = apendPoint;
            }
        }

        private void SetNewPoint(Line line, Point3D mousePoint,Point3D intersecPoint)
        {
            var points = new List<Point3D>() {line.StartPoint, intersecPoint, line.EndPoint};
            var linearPath = new LinearPath(points);
            var keepLine = Utils.GetClosestSegment(linearPath, mousePoint);
            if (line.StartPoint == keepLine.StartPoint)
            {
                line.EndPoint = intersecPoint;
            }
            else
            {
                line.StartPoint = intersecPoint;
            }


        }
        private bool FilletProcess(Entity firstEntity,Entity secondEntity)
        {
            //TH1

            ICurve boundary = firstEntity as ICurve;
            ICurve curve = secondEntity as ICurve;
            
            // Check which end of curve is near to boundary
            double t1, t2;
            boundary.ClosestPointTo(curve.StartPoint, out t1);
            boundary.ClosestPointTo(curve.EndPoint, out t2);

            Point3D projStartPt = boundary.PointAt(t1);
            Point3D projEndPt = boundary.PointAt(t2);

            double curveStartDistance = curve.StartPoint.DistanceTo(projStartPt);
            double curveEndDistance = curve.EndPoint.DistanceTo(projEndPt);

            var success = false;
            if (curveStartDistance < curveEndDistance)
            {
                if (curve is Line)
                {
                    success = ExtendLine(curve, boundary, true);
                }
                else if (curve is LinearPath)
                {
                    success = ExtendPolyLine(curve, boundary, true);
                }
                else if (curve is Arc)
                {
                    success = ExtendCircularArc(curve, boundary, true);
                }
                else if (curve is EllipticalArc)
                {
                    success = ExtendEllipticalArc(curve, boundary, true);
                }
            }
            else
            {
                if (curve is Line)
                {
                    success = ExtendLine(curve, boundary, false);
                }
                else if (curve is LinearPath)
                {
                    success = ExtendPolyLine(curve, boundary, false);
                }
            }

            return success;

        }

        private bool ExtendEllipticalArc(ICurve ellipticalArcCurve, ICurve boundary, bool start)
        {
            EllipticalArc selEllipseArc = ellipticalArcCurve as EllipticalArc;
            Ellipse tempEllipse = new Ellipse(selEllipseArc.Plane, selEllipseArc.Center, selEllipseArc.RadiusX, selEllipseArc.RadiusY);
#if NURBS
            Point3D[] intersetionPoints = Curve.Intersection(boundary, tempEllipse);
            if (intersetionPoints.Length == 0)
                intersetionPoints = Curve.Intersection(GetExtendedBoundary(boundary), tempEllipse);

            EllipticalArc newArc = null;

            if (intersetionPoints.Length > 0)
            {
                Plane arcPlane = selEllipseArc.Plane;
                if (start)
                {
                    Point3D intPoint = GetClosestPoint(selEllipseArc.StartPoint, intersetionPoints);

                    newArc = new EllipticalArc(arcPlane, selEllipseArc.Center, selEllipseArc.RadiusX,
                                                    selEllipseArc.RadiusY, selEllipseArc.EndPoint, intPoint, false);
                    // If start point is not on the new arc, flip needed
                    double t;
                    newArc.ClosestPointTo(selEllipseArc.StartPoint, out t);
                    Point3D projPt = newArc.PointAt(t);
                    if (projPt.DistanceTo(selEllipseArc.StartPoint) > 0.1)
                    {
                        newArc = new EllipticalArc(arcPlane, selEllipseArc.Center, selEllipseArc.RadiusX,
                                                selEllipseArc.RadiusY, selEllipseArc.EndPoint, intPoint, true);
                    }
                    AddAndRefresh(newArc);
                }
                else
                {
                    Point3D intPoint = GetClosestPoint(selEllipseArc.EndPoint, intersetionPoints);
                    newArc = new EllipticalArc(arcPlane, selEllipseArc.Center, selEllipseArc.RadiusX,
                                                selEllipseArc.RadiusY, selEllipseArc.StartPoint, intPoint, false);

                    // If end point is not on the new arc, flip needed
                    double t;
                    newArc.ClosestPointTo(selEllipseArc.EndPoint, out t);
                    Point3D projPt = newArc.PointAt(t);
                    if (projPt.DistanceTo(selEllipseArc.EndPoint) > 0.1)
                    {
                        newArc = new EllipticalArc(arcPlane, selEllipseArc.Center, selEllipseArc.RadiusX,
                                               selEllipseArc.RadiusY, selEllipseArc.StartPoint, intPoint, true);
                    }
                }
                if (newArc != null)
                {
                    AddAndRefresh(newArc);
                    return true;
                }
            }
#endif
            return false;
        }
        private bool ExtendLine(ICurve lineCurve, ICurve boundary, bool nearStart)
        {
            Line line = lineCurve as Line;

            // Create temp line which will intersect boundary curve depending on which end to extend
            Line tempLine = null;
            Vector3D direction = null;
            if (nearStart)
            {
                tempLine = new Line(line.StartPoint, line.StartPoint);
                direction = new Vector3D(line.EndPoint, line.StartPoint);
            }
            else
            {
                tempLine = new Line(line.EndPoint, line.EndPoint);
                direction = new Vector3D(line.StartPoint, line.EndPoint);
            }
            direction.Normalize();
            tempLine.EndPoint = tempLine.EndPoint + direction * _extensionLength;

            // Get intersection points for input line and boundary
            // If not intersecting and boundary is line, we can try with extended boundary

            Point3D[] intersetionPoints = boundary.IntersectWith(tempLine);
            //Curve.Intersection(boundary, tempLine);
            if (intersetionPoints.Length == 0)
                intersetionPoints = Utils.GetExtendedBoundary(boundary).IntersectWith(tempLine);

            // Modify line start/end point as closest intersection point

            if (intersetionPoints.Length > 0)
            {
                if (nearStart)
                    line.StartPoint = Utils.GetClosestPoint(line.StartPoint, intersetionPoints);
                else
                    line.EndPoint = Utils.GetClosestPoint(line.EndPoint, intersetionPoints);

                var tempEntity = (Entity)line.Clone();
                EntitiesManager.AddAndRefresh(tempEntity, tempEntity.LayerName);
                return true;
            }
            return false;
        }
        private bool ExtendSpline(ICurve curve, ICurve boundary, bool nearStart)
        {
            return false;
        }
        private bool ExtendCircularArc(ICurve arcCurve, ICurve boundary, bool nearStart)
        {
            return false;
        }
        private bool ExtendPolyLine(ICurve lineCurve, ICurve boundary, bool nearStart)
        {
            LinearPath line = _secondSelectedEntity as LinearPath;
            Point3D[] tempVertices = line.Vertices;

            // create temp line with proper direction
            Line tempLine = new Line(line.StartPoint, line.StartPoint);
            Vector3D direction = new Vector3D(line.Vertices[1], line.StartPoint);

            if (!nearStart)
            {
                tempLine = new Line(line.EndPoint, line.EndPoint);
                direction = new Vector3D(line.Vertices[line.Vertices.Length - 2], line.EndPoint);
            }

            direction.Normalize();
            tempLine.EndPoint = tempLine.EndPoint + direction * _extensionLength;

            Point3D[] intersetionPoints =boundary.IntersectWith(tempLine);
            if (intersetionPoints.Length == 0)
                intersetionPoints = Utils.GetExtendedBoundary(boundary).IntersectWith( tempLine);
            if (intersetionPoints.Length > 0)
            {
                if (nearStart)
                    tempVertices[0] = Utils.GetClosestPoint(line.StartPoint, intersetionPoints);
                else
                    tempVertices[tempVertices.Length - 1] = Utils.GetClosestPoint(line.EndPoint, intersetionPoints);

                line.Vertices = tempVertices;
                var tempEntity = (Entity)line.Clone();
                EntitiesManager.AddAndRefresh(tempEntity, tempEntity.LayerName);
                return true;
            }


            return false;
        }


    }
}

