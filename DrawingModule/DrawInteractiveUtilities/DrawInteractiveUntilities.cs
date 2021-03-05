using System;
using System.Collections.Generic;
using System.Drawing;
using ApplicationInterfaceCore;
using AppModels.CustomEntity;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.Enums;
using DrawingModule.Helper;
using Syncfusion.UI.Xaml.Grid;
using CanvasDrawing = DrawingModule.CustomControl.CanvasControl.CanvasDrawing;
using Environment = devDept.Eyeshot.Environment;
using Point = System.Drawing.Point;

namespace DrawingModule.DrawInteractiveUtilities
{
    public static class DrawInteractiveUntilities
    {
        public static void DrawInteractiveEntityUnderMouse(Entity ent, DrawEntitiesType drawType, ICadDrawAble canvas)
        {

        }

        public static void DrawRectangle(Point3D basePoint,double width,double height, ICadDrawAble canvas)
        {
            var top = basePoint.Y + height;
            var right = basePoint.X + width;
            var topleft = new Point3D(basePoint.X,top);
            var bottomRight = new Point3D(right,basePoint.Y);
            var topRight = new Point3D(right,top);
            var verticies = new Point3D[]
            {
                basePoint,
                bottomRight,
                topRight,
                topleft
            };
            var scrVertices = Utils.GetScreenVertices(verticies, canvas);

            canvas.renderContext.DrawLineLoop(scrVertices);
        }

        public static void DrawCurveOrBlockRef(Entity entity, ICadDrawAble canvas)
        {
            if (entity is AngularDim angularDim)
            {
                Draw(angularDim.UnderlyingArc,canvas);
            }
            switch (entity)
            {
                case Line line:
                    {
                        var screenpts = Utils.GetScreenVertices(line.Vertices,canvas);
                        canvas.renderContext.DrawLineStrip(screenpts);
                        //this.Draw((ICurve)line);

                        break;
                    }

                case LinearPath linear:
                    {
                        var screenpts = Utils.GetScreenVertices(linear.Vertices,canvas);
                        canvas.renderContext.DrawLineStrip(screenpts);

                        break;
                    }

                case ICurve curve:
                    Draw(curve,canvas);
                    break;
                case LinearDim dim:
                    {
                        Draw(new Line(dim.Vertices[0], dim.Vertices[1]),canvas);
                        Draw(new Line(dim.Vertices[2], dim.Vertices[3]),canvas);
                        Draw(new Line(dim.Vertices[4], dim.Vertices[5]),canvas);
                        //Draw(new Line(dim.Vertices[6], dim.Vertices[7]));


                        var text = new Text(dim.Vertices[6], dim.TextString, dim.Height);
                        text.Rotate(Utils.GetAngleRadian(dim.Vertices[2], dim.Vertices[3]), Vector3D.AxisZ, dim.Vertices[6]);

                        var tempText = text.ConvertToLinearPaths(0.01, (Environment) canvas);
                        //text.ConvertToCurves((Environment)canvas);
                        foreach (var curve in tempText)
                        {
                            var screenpts = Utils.GetScreenVertices(curve.Vertices, canvas);
                            canvas.renderContext.DrawLineStrip(screenpts);
                            //Draw(curve,canvas);
                        }

                        break;
                    }
                case Text text:
                    {
                        var tempText = text.ConvertToLinearPaths(0.01, (Environment)canvas);
                        foreach (var curve in tempText)
                        {
                            var screenpts = Utils.GetScreenVertices(curve.Vertices, canvas);
                            canvas.renderContext.DrawLineStrip(screenpts);
                            //Draw(curve,canvas);
                        }
                        break;
                    }
                case Leader leader:
                    {
                        var screenpts = Utils.GetScreenVertices(leader.Vertices,canvas);
                        canvas.renderContext.DrawLineStrip(screenpts);
                        //canvas.DrawInteractiveArrowHeader(leader.Vertices[0], leader.Vertices[1], leader.ArrowheadSize);
                        break;
                    }
                case Picture picture:
                {
                    var vertices = new List<Point3D>();
                    vertices.AddRange(picture.Vertices);
                    vertices.Add(picture.Vertices[0]);
                    var screenpts = Utils.GetScreenVertices(vertices, canvas);
                        canvas.renderContext.DrawLineStrip(screenpts);
                }
                    break;
                case FramingRectangle2D framing:
                {
                    var vertices = new List<Point3D>() { framing.OuterStartPoint,framing.OuterEndPoint,framing.InnerEndPoint,framing.InnerStartPoint,framing.OuterStartPoint};
                    var screenpts = Utils.GetScreenVertices(vertices, canvas);
                    canvas.renderContext.DrawLineStrip(screenpts);
                }
                    break;
            }
        }

        public static void DrawPositionMark(Point3D current,ICadDrawAble canvas, double crossSize = 40, bool drawingMode = true)
        {
            canvas.HideCursor(true);
            if (current == null)
                return;

            //if (IsPolygonClosed())
            //{
            //    current = points[0];
            //}

            Point3D currentScreen = canvas.WorldToScreen(current);

            // Compute the direction on screen of the horizontal line
            Point2D left = canvas.WorldToScreen(current.X - 1, current.Y, 0);
            Vector2D dirHorizontal = Vector2D.Subtract(left, currentScreen);
            dirHorizontal.Normalize();

            // Compute the position on screen of the line endpoints
            left = currentScreen + dirHorizontal * crossSize;
            Point2D right = currentScreen - dirHorizontal * crossSize;
            Point2D endLeft = currentScreen + dirHorizontal * ((double)canvas.PickBoxSize / 2);
            Point2D endRight = currentScreen - dirHorizontal * ((double)canvas.PickBoxSize / 2);

            // Compute the direction on screen of the vertical line
            Point2D bottom = canvas.WorldToScreen(current.X, current.Y - 1, 0);
            Vector2D dirVertical = Vector2D.Subtract(bottom, currentScreen);
            dirVertical.Normalize();

            // Compute the position on screen of the line endpoints
            bottom = currentScreen + dirVertical * crossSize;
            Point2D top = currentScreen - dirVertical * crossSize;
            Point2D endTop = currentScreen - dirVertical * ((double)canvas.PickBoxSize / 2);
            Point2D endBottom = currentScreen + dirVertical * ((double)canvas.PickBoxSize / 2);
            if (drawingMode)
            {
                canvas.renderContext.DrawLine(bottom, top);
                canvas.renderContext.DrawLine(left, right);
            }
            else
            {
                DrawPickBox(new System.Drawing.Point((int)currentScreen.X, (int)(currentScreen.Y)),canvas);
                //if (!IsPickBoxMode)
                //{
                //    renderContext.DrawLine(top, endTop);
                //    renderContext.DrawLine(bottom, endBottom);
                //    renderContext.DrawLine(left, endLeft);
                //    renderContext.DrawLine(endRight, right);
                //}

            }
            canvas.renderContext.SetLineSize(1);

        }
        public static void DrawSelectionMark(ICadDrawAble canvas,System.Drawing.Point current)
        {
            double num = (double)canvas.PickBoxSize;
            double x = (double)current.X + num / 2.0;
            double y = (double)(canvas.Size.Height - current.Y) + num / 2.0;
            double x2 = (double)current.X - num / 2.0;
            double y2 = (double)(canvas.Size.Height - current.Y) - num / 2.0;
            Point3D point3D = new Point3D(x2, y);
            Point3D point3D2 = new Point3D(x, y);
            Point3D point3D3 = new Point3D(x, y2);
            Point3D point3D4 = new Point3D(x2, y2);
            canvas.renderContext.DrawLines(new Point3D[]
            {
                point3D4,
                point3D3,
                point3D3,
                point3D2,
                point3D2,
                point3D,
                point3D,
                point3D4
            });
            canvas.renderContext.SetLineSize(1f, true, false);
        }

        public static void DrawPickBox(System.Drawing.Point onScreen,ICadDrawAble canvas)
        {
            double dim1 = onScreen.X + ((double)canvas.PickBoxSize / 2);
            double dim2 = onScreen.Y + ((double)canvas.PickBoxSize / 2);
            double dim3 = onScreen.X - ((double)canvas.PickBoxSize / 2);
            double dim4 = onScreen.Y - ((double)canvas.PickBoxSize / 2);
            Point3D topLeftVertex = new Point3D(dim3, dim2);
            Point3D topRightVertex = new Point3D(dim1, dim2);
            Point3D bottomRightVertex = new Point3D(dim1, dim4);
            Point3D bottomLeftVertex = new Point3D(dim3, dim4);

            canvas.renderContext.DrawLineLoop(new Point3D[]
            {
                bottomLeftVertex,
                bottomRightVertex,
                topRightVertex,
                topLeftVertex
            });
        }
        public static void Draw(ICurve theCurve, ICadDrawAble canvas)
        {
            if (theCurve is CompositeCurve)
            {
                CompositeCurve compositeCurve = theCurve as CompositeCurve;
                Entity[] explodedCurves = compositeCurve.Explode();
                foreach (Entity ent in explodedCurves)

                    DrawScreenCurve((ICurve)ent,canvas);
            }
            else
            {
                DrawScreenCurve(theCurve,canvas);
            }
        }
        public static void DrawScreenCurve(ICurve curve,ICadDrawAble canvas)
        {

           canvas.renderContext.SetColorWireframe(Color.Red);
            const int subd = 100;

            Point3D[] pts = new Point3D[subd + 1];

            for (var i = 0; i <= subd; i++)
            {
                pts[i] = canvas.WorldToScreen(curve.PointAt(curve.Domain.ParameterAt((double)i / subd)));
            }

            canvas.renderContext.DrawLineStrip(pts);
        }

        public static void DrawInteractiveSpotLine(Point3D firstPoint, Point3D secondPoint, ICadDrawAble canvas)
        {
            if (firstPoint == null || secondPoint == null)
                return;
            var startPoint = canvas.WorldToScreen(firstPoint);
            var endPoint = canvas.WorldToScreen( secondPoint);
            canvas.renderContext.SetColorWireframe(Color.RoyalBlue);
            canvas.renderContext.SetLineStipple(1, 0x0F0F, canvas.Viewports[0].Camera);
            canvas.renderContext.EnableLineStipple(true);
            canvas.renderContext.DrawLine(startPoint, endPoint);
            canvas.renderContext.EnableLineStipple(false);

        }

        public static void DrawInteractiveLines(List<Point3D> clickPoints, ICadDrawAble canvas, Point3D currentPoint)
        {
            canvas.renderContext.EnableXOR(false);
            if (clickPoints.Count == 0)
                return;
            Point3D[] screenvertices = Utils.GetScreenVertices(clickPoints,canvas);
            canvas.renderContext.DrawLineStrip(screenvertices);
            var index = clickPoints.Count-1;
            var lastClickPoint = clickPoints[index];
            var startPoint = canvas.WorldToScreen(lastClickPoint);
            var endPoint = canvas.WorldToScreen(currentPoint);

            canvas.renderContext.DrawLine(startPoint, endPoint);
        }

        public static void DrawInteractiveTextForLeader(ICadDrawAble canvas, Point3D currentPoint, Point3D startPoint,string textString,double textHeight)
        {
            if (string.IsNullOrEmpty(canvas.CurrentText))
            {
                
                return;
            }
            //if (string.IsNullOrEmpty(text) || this.CurrentPoint == null)
            //{
            //    return;
            //}
            //if (this.LastClickPoint == null || this.CurrentPoint == null)
            //{
            //    return;
            //}
            //if (this.LastClickPoint == this.CurrentPoint)
            //{
            //    return;
            //}


            var textOutput = Utils.CreateNewTextLeader(startPoint, currentPoint,textString,textHeight);
            var rad = Utility.DegToRad(canvas.CurrentTextAngle);
            textOutput.Rotate(rad, Vector3D.AxisZ, currentPoint);

            var tempText = textOutput.ConvertToLinearPaths(0.01,(Environment)canvas);
            foreach (var curve in tempText)
            {
                var screenpts = Utils.GetScreenVertices(curve.Vertices, canvas);
                canvas.renderContext.DrawLineStrip(screenpts);
                //Draw(curve,canvas);
            }
        }

        public static void DrawInteractiveArc(ICadDrawAble canvas, List<Point3D> clickPoints, Point3D currentPoint, out double arcSpanAngle)
        {
            Point2D[] screenPts = Utils.GetScreenVertices(clickPoints,canvas);

            canvas.renderContext.DrawLineStrip(screenPts);
            arcSpanAngle = 0;
           
                // Draw elastic line
                canvas.renderContext.DrawLine(canvas.WorldToScreen(clickPoints[0]), canvas.WorldToScreen(currentPoint));

                //draw three point arc
                if (clickPoints.Count == 2)
                {

                    var radius = clickPoints[0].DistanceTo(clickPoints[1]);

                    if (radius > 1e-3)
                    {
                        var drawingPlane = Utils.GetPlane(clickPoints[1],clickPoints);

                        Vector2D v1 = new Vector2D(clickPoints[0], clickPoints[1]);
                        v1.Normalize();
                        Vector2D v2 = new Vector2D(clickPoints[0], currentPoint);
                        v2.Normalize();

                        arcSpanAngle = Vector2D.SignedAngleBetween(v1, v2);

                        if (Math.Abs(arcSpanAngle) > 1e-3)
                        {

                            Arc tempArc = new Arc(drawingPlane, drawingPlane.Origin, radius, 0, arcSpanAngle);

                            Draw(tempArc,canvas);

                        }

                    }
                }

            
        }

        public static void DrawInteractiveArrowHeader(ICadDrawAble canvas,Point3D p1, Point3D p2, double arrowSize)
        {
            var basePoint = p1;
            var midPoint = new Point3D(p1.X + arrowSize, p1.Y);
            var topPoint = new Point3D(midPoint.X, midPoint.Y + arrowSize / 4);
            var bottomPoint = new Point3D(midPoint.X, midPoint.Y - arrowSize / 4);
            var points = new[] { basePoint, topPoint, bottomPoint, (Point3D)basePoint.Clone() };

            var arrowShape = new LinearPath(points);
            arrowShape.Rotate(Utils.GetAngleRadian(p1, p2), Vector3D.AxisZ, basePoint);
            var screenVertices = Utils.GetScreenVertices(arrowShape.Vertices,canvas);
            //renderContext.DrawTriangles(screenVertices,Vector3D.AxisZ);
            canvas.renderContext.DrawLineStrip(screenVertices);

        }

    }
}
