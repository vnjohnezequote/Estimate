using ApplicationInterfaceCore;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.Enums;
using DrawingModule.Helper;
using CanvasDrawing = DrawingModule.CustomControl.CanvasControl.CanvasDrawing;

namespace DrawingModule.DrawInteractiveUtilities
{
    public static class DrawInteractiveUntilities
    {
        public static void DrawInteractiveEntityUnderMouse(Entity ent, DrawEntitiesType drawType, ICadDrawAble canvas)
        {

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

                        var tempText = text.ConvertToCurves((Environment)canvas);
                        foreach (var curve in tempText)
                        {
                            Draw(curve,canvas);
                        }

                        break;
                    }
                case Text text:
                    {
                        var tempText = text.ConvertToCurves((Environment)canvas);
                        foreach (var curve in tempText)
                        {
                            Draw(curve,canvas);
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
            const int subd = 100;

            Point3D[] pts = new Point3D[subd + 1];

            for (var i = 0; i <= subd; i++)
            {
                pts[i] = canvas.WorldToScreen(curve.PointAt(curve.Domain.ParameterAt((double)i / subd)));
            }

            canvas.renderContext.DrawLineStrip(pts);
        }





    }
}
