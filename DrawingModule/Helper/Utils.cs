using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using ApplicationInterfaceCore;
using AppModels.CustomEntity;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CustomControl;
using DrawingModule.Enums;
using DrawingModule.Interface;

namespace DrawingModule.Helper
{
    public static class Utils
    {
        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            if (obj != null)
            {
                int childrenCount = VisualTreeHelper.GetChildrenCount(obj);
                for (int i = 0; i < childrenCount; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                    {
                        return (T)((object)child);
                    }
                    T childItem = Utils.FindVisualChild<T>(child);
                    if (childItem != null)
                    {
                        return childItem;
                    }
                }
            }
            return default(T);
        }

        public static Point3D ConvertPoint2DtoPoint3D(this Point2D point)
        {
            return new Point3D(point.X,point.Y,0);
        }

        public static bool ConvertStringToDouble(string inputString, out double outputDouble)
        {
            outputDouble = 0;
            
            if (string.IsNullOrEmpty(inputString))
            {
                outputDouble = 0;
                return false;
            }
            var match = Regex.Match(inputString, @"([+|-]?[\d+]?[+|\-|\*|\/]?\d+)+");
            if (match.Success)
            {
                var dataTable = new DataTable();
                var output = dataTable.Compute(match.Value,String.Empty);
                outputDouble = Convert.ToDouble(output);

            }

            if (Math.Abs(outputDouble) > 0.0001)
            {
                return true;
            }

            return false;
        }
        public static double ConvertAngleStringToDouble(string inputString)
        {
            var outputAngle = 0.0;

            if (string.IsNullOrEmpty(inputString))
            {
                return outputAngle;
            }

            var match = Regex.Match(inputString, @"([+|\-]?\d{0,3}\.?\d+)");
            if (!match.Success) return 0;
            outputAngle = Convert.ToDouble(match.Value);
            if (!(outputAngle > 360)) return outputAngle;
            var factor = (int) (outputAngle / 360);
            outputAngle = outputAngle - factor * 360;
            return outputAngle;

        }
        public static T GetSysVarSafely<T>(string name, T defaultValue)
        {
            try
            {
                object systemVariable = null; /*Application.GetSystemVariable(name)*/;
                if (systemVariable != null)
                {
                    if (systemVariable is T)
                    {
                        return (T)systemVariable;
                    }
                    if (systemVariable is IConvertible)
                    {
                        return (T)Convert.ChangeType(systemVariable, typeof(T));
                    }
                }
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string GetCommandPromptString()
         {
            string resultString;
            resultString = Application.Application.UiBindings.CommandEditorManager.ActiveEditor.PromptAndInput
                .GetCommandPromptString();

            return resultString;
        }

        public static Point3D CalculatorPointWithLengthAndAngle(Point3D basePoint,Point3D refPoint, double length, double angle)
        {
            if (basePoint == null)
            {
                return null;
            }

            //if (refPoint !=null)
            //{
            //    var vector = new Vector2D(basePoint,refPoint);
            //    var angleBetwenXaxist = -Vector2D.SignedAngleBetween(vector, Vector2D.AxisX) * (180 / Math.PI);
            //    angle += angleBetwenXaxist;
            //}
            var startPoint = basePoint;
            var endPoint = new Point3D(basePoint.X + length, basePoint.Y);
            var tempLine = new Line(startPoint, endPoint);
            var angleRadians = angle * (Math.PI / 180);
            tempLine.Rotate(angleRadians, Vector3D.AxisZ, startPoint);
            return new Point3D(tempLine.EndPoint.X, tempLine.EndPoint.Y);
        }

        public static bool IsEntityContainPoint3D(Entity entity, Point3D point)
        {
            return entity.Vertices.Any(entVertex => Math.Abs(entVertex.X - point.X) < 0.0001 && Math.Abs(entVertex.Y - point.Y) < 0.0001) || false;
        }
        public static Point3D GetEndPoint(Point3D basePoint, Point3D currentPoint)
        {
            var endPoint = currentPoint;
            var vector = new Vector2D(basePoint, currentPoint);
            var angle = -Vector2D.SignedAngleBetween(vector, Vector2D.AxisX) * (180 / Math.PI);
            var length = basePoint.DistanceTo(currentPoint);
            if (angle >= -40 && angle <= 40)
            {
                angle = 0;
            }
            else if (angle > 40 && angle < 50)
            {
                angle = 45;
            }
            else if (angle >= 50 && angle <= 130)
            {
                angle = 90;
            }
            else if (angle > 130 && angle < 140)
            {
                angle = 135;
            }
            else if (angle >= 140 && angle <= 180)
            {
                angle = 180;
            }
            else if (angle >= -180 && angle < -140)
            {
                angle = -180;
            }
            else if (angle >= -140 && angle <= -120)
            {
                angle = -135;
            }
            else if (angle > -120 && angle < -50)
            {
                angle = -90;
            }
            else if (angle >= -50 && angle < -40)
            {
                angle = -45;
            }

            //if (this.CurrentTool == ETool.Move)
            //{
            //    angle = (-1) * angle;
            //}

            endPoint = CalculatorPointWithLengthAndAngle(basePoint,null, length, angle);

            return endPoint;
        }
        public static HashSet<SnapPoint> GetPerpenticalSnapPoints(Point3D basePoint, ICurve entity)
        {
            //var oldSize = PickBoxSize;
            //PickBoxSize = 10;
            var snapPoints = new HashSet<SnapPoint>();
            //Find Perpendicular snappoint
            if (basePoint != null)
            {
                if (entity is LinearPath linearPath1)
                {
                    var tempsPerdicularPoints = GetPerpendicularPoints(linearPath1, basePoint);
                    if (tempsPerdicularPoints.Count > 0)
                    {
                        foreach (var tempsPerdicularPoint in tempsPerdicularPoints)
                        {
                            if (!CheckPointInSnapPoints(tempsPerdicularPoint, snapPoints))
                            {
                                snapPoints.Add(
                                    new SnapPoint(tempsPerdicularPoint, ObjectSnapType.Perpendicular));
                            }
                        }
                    }
                }
                else if (entity is Line line)
                {
                    var tempsPerdicularPoint = GetPerpendicularPoints(line, basePoint);
                    if (tempsPerdicularPoint != null)
                    {
                        snapPoints.Add(
                            new SnapPoint(tempsPerdicularPoint, ObjectSnapType.Perpendicular));

                    }
                }
            }

            return snapPoints;
        }
        public static HashSet<Point3D> GetPerpendicularPoints(LinearPath linearPath, Point3D basePoint)
        {
            var lines = linearPath.ConvertToLines();
            var tempoints = new HashSet<Point3D>();
            foreach (var line in lines)
            {
                tempoints.Add(GetPerpendicularPoints(line, basePoint));

            }

            return tempoints;

        }
        public static Point3D GetPerpendicularPoints(Line line, Point3D basePoint)
        {
            var tempSegment = new Segment2D(line.StartPoint, line.EndPoint);
            var temptDistance = tempSegment.ClosestPointTo(basePoint);
            var tempPoint = tempSegment.PointAt(temptDistance);
            return tempPoint.ConvertPoint2DtoPoint3D();

        }
        public static double GetDistanceFromPointToLine(Line line, Point3D point)
        {
            if (point == null)
            {
                return 0;
            }
            var canhday = line.Length();
            var canh1 = point.DistanceTo(line.StartPoint);
            var canh2 = point.DistanceTo(line.EndPoint);
            var chuvi = (canh1 + canh2 + canhday) / 2;
            var distance = 2 * (Math.Sqrt(chuvi * (chuvi - canh1) * (chuvi - canh2) * (chuvi - canhday))) / canhday;
            return distance;
        }
        public static Line GetClosestSegment(LinearPath linearPath, System.Drawing.Point mousePosition, Plane sketchPlant, ICadDrawAble cadDraw)
        {
            var lines = linearPath.ConvertToLines();
            var point =
            System.Windows.Application.Current.Dispatcher.Invoke((Func<Point3D>)(() =>
            {//this refer to form in WPF application 
                cadDraw.ScreenToPlane(mousePosition, sketchPlant, out var tempPoint);
                return tempPoint;
            }));

            var minDist = Double.MaxValue;
            Line tempLine = null;
            foreach (var line in lines)
            {
                var distance = GetDistanceFromPointToLine(line, point);

                if (!(distance < minDist)) continue;
                tempLine = line;
                minDist = distance;
            }
            //var count = lines.Length;

            //for (int i = 0; i < lines.Length; i++)
            //{
            //    var distance = this.GetDistanceFromPointToLine(lines[i], point);
            //    if (distance<minDist)
            //    {
            //        tempLine = lines[i];
            //        minDist = distance;
            //    }
            //}
            return tempLine;


        }
        public static Point3D GetMidPoint3D(Point3D pointA, Point3D pointB)
        {
            return new Point3D((pointA.X + pointB.X) / 2, (pointA.Y + pointB.Y) / 2);
        }
        public static HashSet<Point3D> GetIntersectBySeft(LinearPath linearPath)
        {
            var lines = linearPath.ConvertToLines();
            var temPoint3Ds = new HashSet<Point3D>();
            for (var i = 0; i < lines.Length; i++)
            {
                for (var j = i + 2; j < lines.Length; j++)
                {
                    temPoint3Ds.UnionWith(lines[i].IntersectWith(lines[j]));
                }
            }

            var tempPoints2 = new HashSet<Point3D>();
            foreach (var temPoint in temPoint3Ds.Where(temPoint => !tempPoints2.Contains(temPoint)))
            {
                tempPoints2.Add(temPoint);
            }

            return tempPoints2;
        }
        public static bool CheckPointInSnapPoints(Point3D point, HashSet<SnapPoint> snapPoints)
        {
            return snapPoints.Any(snapPoint => point.X == snapPoint.X && point.Y == snapPoint.Y);
        }
        public static HashSet<SnapPoint> GetSnapPoints(System.Drawing.Point mouseLocation, ICadDrawAble cadDraw)
        {
            var snapPoints = new HashSet<SnapPoint>();
            Line tempLine = null;

            var entityIndexs = cadDraw.GetAllEntitiesUnderMouseCursor(mouseLocation, false);
            if (entityIndexs.Length <= 0) return snapPoints;
            var countIndex = 0;
            foreach (var index in entityIndexs)
            {
                if (cadDraw.Entities[index] is ICurve entCurve)
                {
                    var indexVertex = 0;

                    if (Utils.GetPerpenticalSnapPoints(cadDraw.LastClickPoint, entCurve).Count > 0)
                    {
                        snapPoints.UnionWith(Utils.GetPerpenticalSnapPoints(cadDraw.LastClickPoint, entCurve));
                    }

                    // Find Intersecpoint by seft if it is linearPath


                    switch (entCurve)
                    {
                        case LinearPath linearPath:
                            {
                                var temptPoints = GetIntersectBySeft(linearPath);
                                tempLine = Utils.GetClosestSegment(linearPath, mouseLocation, cadDraw.DrawingPlane, cadDraw);
                                if (temptPoints.Count > 0)
                                {
                                    foreach (var temptPoint in temptPoints.Where(temptPoint => !Utils.CheckPointInSnapPoints(temptPoint, snapPoints)))
                                    {
                                        snapPoints.Add(new SnapPoint(temptPoint, ObjectSnapType.Intersect));
                                    }
                                }

                                break;
                            }
                        case Line line:
                            tempLine = line;
                            break;
                    }

                    // Find Intersecpoint betwen orther object
                    if (countIndex > 0)
                    {
                        if (cadDraw.Entities[entityIndexs[countIndex - 1]] is ICurve line1 && cadDraw.Entities[entityIndexs[countIndex]] is ICurve line2)
                        {
                            if (line1 is LinearPath linePath1)
                            {
                                var intersectPoints = linePath1.IntersectWith(line2);
                                foreach (var intersectPoint in intersectPoints)
                                {
                                    snapPoints.Add(new SnapPoint(intersectPoint, ObjectSnapType.Intersect));
                                }
                            }
                            else if (line2 is LinearPath linePath2)
                            {
                                var intersectPoints = linePath2.IntersectWith(line1);
                                foreach (var intersectPoint in intersectPoints)
                                {
                                    snapPoints.Add(new SnapPoint(intersectPoint, ObjectSnapType.Intersect));
                                }
                            }
                            else
                            {
                                if (CompositeCurve.IntersectionLineLine(line1, line2, out var intersectPoint3D))
                                {
                                    snapPoints.Add(new SnapPoint(intersectPoint3D, ObjectSnapType.Intersect));
                                }
                            }
                        }

                    }

                    // Find EndPoit and Midpoint 
                    foreach (var vertex in cadDraw.Entities[index].Vertices)
                    {
                        snapPoints.Add(new SnapPoint(vertex, ObjectSnapType.End));
                        if (indexVertex > 0)
                        {
                            var midPoint = Utils.GetMidPoint3D(cadDraw.Entities[index].Vertices[indexVertex - 1], vertex);
                            snapPoints.Add(new SnapPoint(midPoint, ObjectSnapType.Mid));
                        }

                        indexVertex++;
                    }

                    cadDraw.ScreenToPlane(mouseLocation, cadDraw.DrawingPlane, out var mousePos);
                    if (tempLine != null)
                    {
                        var nearPoint = Utils.GetPerpendicularPoints(tempLine, mousePos);
                        if (!Utils.CheckPointInSnapPoints(nearPoint, snapPoints))
                        {
                            snapPoints.Add(new SnapPoint(nearPoint, ObjectSnapType.Nearest));
                        }
                    }


                    //find Nearest Point

                    //var nearPts = this.GetNearestPoints(Entities[index]);

                    //foreach (var entPoint in nearPts)
                    //{
                    //    if (!CheckPointInSnapPoints(entPoint, snapPoints))
                    //    {
                    //        snapPoints.Add(new SnapPoint(entPoint, ObjectSnapType.Nearest));
                    //    }
                    //}

                }
                else
                {
                    if (cadDraw.Entities[index] is BeamEntity beamEntity)
                    {
                        if (Utils.GetPerpenticalSnapPoints(cadDraw.LastClickPoint, beamEntity.BeamLine as ICurve).Count > 0)
                        {
                            snapPoints.UnionWith(Utils.GetPerpenticalSnapPoints(cadDraw.LastClickPoint, beamEntity.BeamLine as ICurve));
                        }

                        var vertices = beamEntity.BeamLine.Vertices;


                        foreach (var vertex in vertices)
                        {
                            snapPoints.Add(new SnapPoint(vertex, ObjectSnapType.End));
                        }

                        if(!Utils.CheckPointInSnapPoints(beamEntity.BeamLine.MidPoint, snapPoints))
                            snapPoints.Add(new SnapPoint(beamEntity.BeamLine.MidPoint,ObjectSnapType.Nearest));

                        tempLine = beamEntity.BeamLine;

                        cadDraw.ScreenToPlane(mouseLocation, cadDraw.DrawingPlane, out var mousePos);
                        if (tempLine != null)
                        {
                            var nearPoint = Utils.GetPerpendicularPoints(tempLine, mousePos);
                            if (!Utils.CheckPointInSnapPoints(nearPoint, snapPoints))
                            {
                                snapPoints.Add(new SnapPoint(nearPoint, ObjectSnapType.Nearest));
                            }
                        }


                    }
                    else
                    {
                        foreach (var vertex in cadDraw.Entities[index].Vertices)
                        {
                            snapPoints.Add(new SnapPoint(vertex, ObjectSnapType.End));
                        }
                    }
                    
                }

                countIndex++;
            }


            return snapPoints;
        }
        public static SnapPoint GetClosestPoint(HashSet<SnapPoint> snapPoints, System.Drawing.Point mousePosition,ICadDrawAble cadDraw)
        {
            var minDist = double.MaxValue;
            SnapPoint snap = null;
            //var index = 0;
            foreach (var verTex in snapPoints)
            {
                var vertexScreen = cadDraw.WorldToScreen(verTex);
                var currentScreen = new Point2D(mousePosition.X, cadDraw.Size.Height - mousePosition.Y);
                var dist = Point2D.Distance(vertexScreen, currentScreen);
                if (verTex.Type == ObjectSnapType.Perpendicular && dist < cadDraw.PickBoxSize)
                {
                    snap = verTex;
                    break;
                }

                if ((verTex.Type == ObjectSnapType.End || verTex.Type == ObjectSnapType.Mid || verTex.Type == ObjectSnapType.Intersect) && dist < cadDraw.PickBoxSize)
                {
                    snap = verTex;
                    break;
                }
                if (verTex.Type == ObjectSnapType.Nearest && dist < minDist)
                {
                    snap = verTex;
                    minDist = dist;
                }


                //i++;
            }

            return snap;
        }
        public static Point3D[] GetScreenVertices(IList<Point3D> vertices,ICadDrawAble canvas)
        {
            Point3D[] screenPoints = new Point3D[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
                screenPoints[i] = canvas.WorldToScreen(vertices[i]);
            return screenPoints;
        }
        public static double GetAngleRadian(Point3D p1, Point3D p2)
        {
            var vector = new Vector2D(p1, p2);
            //vector.Normalize();
            var angle = -Vector2D.SignedAngleBetween(vector, Vector2D.AxisX);
            return angle;
        }
        public static Text CreateNewTextLeader(Point3D p1, Point3D p2, string textString,double textHeight )
        {
            var textLeaderAlliment = Text.alignmentType.BaselineCenter;
            if (p2.X > p1.X)
            {

                textLeaderAlliment = Text.alignmentType.MiddleLeft;
            }
            else if (p2.X < p1.X)
            {
                textLeaderAlliment = Text.alignmentType.MiddleRight;
            }
            else
            {
                textLeaderAlliment = p2.Y > p1.Y ? Text.alignmentType.BaselineCenter : Text.alignmentType.TopCenter;
            }
            return new Text((Point3D)p2.Clone(), textString, textHeight, textLeaderAlliment);
        }
        public static Plane GetPlane(Point3D next, List<Point3D> clickPoints)
        {
            Vector3D xAxis = new Vector3D(clickPoints[0], next);
            xAxis.Normalize();
            Vector3D yAxis = Vector3D.Cross(Vector3D.AxisZ, xAxis);
            yAxis.Normalize();

            Plane plane = new Plane(clickPoints[0], xAxis, yAxis);

            return plane;
        }


        public static ICurve GetExtendedBoundary(ICurve boundary,double extensionLength = 10000)
        {
            if (boundary is Line)
            {
                Line tempLine = new Line(boundary.StartPoint, boundary.EndPoint);
                Vector3D dir1 = new Vector3D(tempLine.StartPoint, tempLine.EndPoint);
                dir1.Normalize();
                tempLine.EndPoint = tempLine.EndPoint + dir1 * extensionLength;

                Vector3D dir2 = new Vector3D(tempLine.EndPoint, tempLine.StartPoint);
                dir2.Normalize();
                tempLine.StartPoint = tempLine.StartPoint + dir2 * extensionLength;

                boundary = tempLine;
            }
            return boundary;
        }

        public static Point3D GetClosestPoint(Point3D point3D, Point3D[] intersetionPoints)
        {
            double minsquaredDist = Double.MaxValue;
            Point3D result = null;

            foreach (Point3D pt in intersetionPoints)
            {
                double distSquared = Point3D.DistanceSquared(point3D, pt);
                if (distSquared < minsquaredDist && !point3D.Equals(pt))
                {
                    minsquaredDist = distSquared;
                    result = pt;
                }
            }
            return result;
        }



    }
}
