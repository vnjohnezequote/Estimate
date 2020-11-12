using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ApplicationInterfaceCore;
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
    public class ExtendTool : ToolBase
    {

        public override Point3D BasePoint { get; protected set; }
        public override string ToolName => "Extend";
        private bool _processingTool;
        private Entity _firstSelectedEntity;
        private Entity _secondSelectedEntity;
        private int _selectedEntityIndex;
        private double _extensionLength = 10000;

        public ExtendTool()
        {
            _processingTool = true;
            IsSnapEnable = false;
            IsUsingOrthorMode = false;
            _selectedEntityIndex = -1;
        }

        [CommandMethod("Extend")]
        public void Extend()
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
            DrawInteractiveExtend((ICadDrawAble)sender, e);

        }

        public override void NotifyMouseDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = (ICadDrawAble)sender;
            var mousePosition = RenderContextUtility.ConvertPoint(canvas.GetMousePosition(e));
            var index = canvas.GetEntityUnderMouseCursor(mousePosition);
            if (_firstSelectedEntity != null && _secondSelectedEntity != null) return;
            if (index > -1)
            {
                _selectedEntityIndex = index;
            }

            if (_secondSelectedEntity != null) return;
            canvas.ScreenToPlane(mousePosition, canvas.DrawingPlane, out var clickPoint);
        }

        public override void NotifyPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this._processingTool = false;
                e.Handled = true;
            }
        }

        private void DrawInteractiveExtend(ICadDrawAble canvas, DrawInteractiveArgs e)
        {
            if (_firstSelectedEntity == null)
            {
                DrawInteractiveUntilities.DrawSelectionMark(canvas, e.MousePosition);
                ToolMessage = "Select boundary entity";
                if (_selectedEntityIndex != -1)
                {
                    _firstSelectedEntity = EntitiesManager.GetEntity(_selectedEntityIndex);
                    _selectedEntityIndex = -1;
                    return;
                }
            }
            else if (_secondSelectedEntity == null)
            {
                DrawInteractiveUntilities.DrawSelectionMark(canvas, e.MousePosition);
                canvas.renderContext.EnableXOR(false);
                ToolMessage = "Select entity to extend";
            }

            if (_secondSelectedEntity == null)
            {
                if (_selectedEntityIndex != -1)
                {
                    _secondSelectedEntity = EntitiesManager.GetEntity(_selectedEntityIndex);
                    _selectedEntityIndex = -1;
                }
            }

            if (_firstSelectedEntity != null && _secondSelectedEntity != null)
            {
                if (_firstSelectedEntity is ICurve && _secondSelectedEntity is ICurve)
                {
                    ICurve boundary = _firstSelectedEntity as ICurve;
                    ICurve curve = _secondSelectedEntity as ICurve;

                    // Check which end of curve is near to boundary
                    double t1, t2;
                    boundary.ClosestPointTo(curve.StartPoint, out t1);
                    boundary.ClosestPointTo(curve.EndPoint, out t2);

                    Point3D projStartPt = boundary.PointAt(t1);
                    Point3D projEndPt = boundary.PointAt(t2);

                    double curveStartDistance = curve.StartPoint.DistanceTo(projStartPt);
                    double curveEndDistance = curve.EndPoint.DistanceTo(projEndPt);

                    bool success = false;
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
#if NURBS
                        else if (curve is Curve)
                        {
                            success = ExtendSpline(curve, boundary, true);
                        }
#endif
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
#if NURBS
                        else if (curve is Arc)
                        {
                            success = ExtendCircularArc(curve, boundary, false);
                        }
                        else if (curve is EllipticalArc)
                        {
                            success = ExtendEllipticalArc(curve, boundary, false);
                        }

                        else if (curve is Curve)
                        {
                            success = ExtendSpline(curve, boundary, false);
                        }
#endif
                    }

                    if (success)
                    {
                        EntitiesManager.RemoveEntity(_secondSelectedEntity);
                    }
                }

                _firstSelectedEntity = null;
                _secondSelectedEntity = null;
                _selectedEntityIndex = -1;


            }

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
#if NURBS
            Curve originalSpline = curve as Curve;

            Line tempLine = null;
            Vector3D direction = null;
            if (nearStart)
            {
                tempLine = new Line(curve.StartPoint, curve.StartPoint);
                direction = curve.StartTangent; direction.Normalize(); direction.Negate();
                tempLine.EndPoint = tempLine.EndPoint + direction * _extensionLength;
            }
            else
            {
                tempLine = new Line(curve.EndPoint, curve.EndPoint);
                direction = curve.EndTangent; direction.Normalize();
                tempLine.EndPoint = tempLine.EndPoint + direction * _extensionLength;
            }

            Point3D[] intersetionPoints = Curve.Intersection(boundary, tempLine);
            if (intersetionPoints.Length == 0)
                intersetionPoints = Curve.Intersection(GetExtendedBoundary(boundary), tempLine);

            if (intersetionPoints.Length > 0)
            {
                List<Point4D> ctrlPoints = originalSpline.ControlPoints.ToList();
                List<Point3D> newCtrlPoints = new List<Point3D>();
                if (nearStart)
                {
                    newCtrlPoints.Add(GetClosestPoint(curve.StartPoint, intersetionPoints));
                    foreach (Point4D ctrlPt in ctrlPoints)
                    {
                        Point3D point = new Point3D(ctrlPt.X, ctrlPt.Y, ctrlPt.Z);
                        if (!point.Equals(originalSpline.StartPoint))
                            newCtrlPoints.Add(point);
                    }
                }
                else
                {
                    foreach (Point4D ctrlPt in ctrlPoints)
                    {
                        Point3D point = new Point3D(ctrlPt.X, ctrlPt.Y, ctrlPt.Z);
                        if (!point.Equals(originalSpline.EndPoint))
                            newCtrlPoints.Add(point);
                    }
                    newCtrlPoints.Add(GetClosestPoint(curve.EndPoint, intersetionPoints));
                }

                Curve newCurve = new Curve(originalSpline.Degree, newCtrlPoints);
                if (newCurve != null)
                {
                    AddAndRefresh(newCurve);
                    return true;
                }
            }
#endif
            return false;
        }
        private bool ExtendCircularArc(ICurve arcCurve, ICurve boundary, bool nearStart)
        {
            Arc selCircularArc = arcCurve as Arc;
            Circle tempCircle = new Circle(selCircularArc.Plane, selCircularArc.Center, selCircularArc.Radius);
#if NURBS
            Point3D[] intersetionPoints = Curve.Intersection(boundary, tempCircle);
            if (intersetionPoints.Length == 0)
                intersetionPoints = Curve.Intersection(GetExtendedBoundary(boundary), tempCircle);

            if (intersetionPoints.Length > 0)
            {
                if (nearStart)
                {
                    Point3D intPoint = GetClosestPoint(selCircularArc.StartPoint, intersetionPoints);
                    Vector3D xAxis = new Vector3D(selCircularArc.Center, selCircularArc.EndPoint);
                    xAxis.Normalize();
                    Vector3D yAxis = Vector3D.Cross(Vector3D.AxisZ, xAxis);
                    yAxis.Normalize();
                    Plane arcPlane = new Plane(selCircularArc.Center, xAxis, yAxis);

                    Vector2D v1 = new Vector2D(selCircularArc.Center, selCircularArc.EndPoint);
                    v1.Normalize();
                    Vector2D v2 = new Vector2D(selCircularArc.Center, intPoint);
                    v2.Normalize();

                    double arcSpan = Vector2D.SignedAngleBetween(v1, v2);
                    Arc newArc = new Arc(arcPlane, arcPlane.Origin, selCircularArc.Radius, 0, arcSpan);
                    AddAndRefresh(newArc);
                }
                else
                {
                    Point3D intPoint = GetClosestPoint(selCircularArc.EndPoint, intersetionPoints);

                    //plane
                    Vector3D xAxis = new Vector3D(selCircularArc.Center, selCircularArc.StartPoint);
                    xAxis.Normalize();
                    Vector3D yAxis = Vector3D.Cross(Vector3D.AxisZ, xAxis);
                    yAxis.Normalize();
                    Plane arcPlane = new Plane(selCircularArc.Center, xAxis, yAxis);

                    Vector2D v1 = new Vector2D(selCircularArc.Center, selCircularArc.StartPoint);
                    v1.Normalize();
                    Vector2D v2 = new Vector2D(selCircularArc.Center, intPoint);
                    v2.Normalize();

                    double arcSpan = Vector2D.SignedAngleBetween(v1, v2);
                    Arc newArc = new Arc(arcPlane, arcPlane.Origin, selCircularArc.Radius, 0, arcSpan);
                    AddAndRefresh(newArc);
                }
                return true;
            }
#endif
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

            Point3D[] intersetionPoints = boundary.IntersectWith(tempLine);
            if (intersetionPoints.Length == 0)
                intersetionPoints = Utils.GetExtendedBoundary(boundary).IntersectWith(tempLine);
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
