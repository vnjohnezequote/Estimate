using System;
using System.Collections.Generic;
using System.Drawing;
using ApplicationInterfaceCore;
using devDept.Geometry;
using devDept.Graphics;
using DrawingModule.CustomControl;
using DrawingModule.Enums;
using DrawingModule.Interface;

namespace DrawingModule.DrawInteractiveUtilities
{
    internal static class DrawSnappPointUtl
    {
        public static void DisplaySnappedVertex(ICadDrawAble canvasDrawing,SnapPoint snap,RenderContextBase renderContext, double snapSymbolSize = 40)
        {
            renderContext.SetLineSize(2);

            // white color
            renderContext.SetColorWireframe(Color.FromArgb(0, 0, 255));
            renderContext.SetState(depthStencilStateType.DepthTestOff);

            Point2D onScreen =canvasDrawing.WorldToScreen(snap);

            //var snapPoint = snap;
            var drawPoint = new System.Drawing.Point((int) onScreen.X, (int) (onScreen.Y));
            
            switch (snap.Type)
            {
                case ObjectSnapType.Point:
                    DrawCircle(drawPoint,snapSymbolSize,renderContext);
                    DrawCross(drawPoint, snapSymbolSize, renderContext);
                    break;
                case ObjectSnapType.Center:
                    DrawCircle(drawPoint, snapSymbolSize, renderContext);
                    break;
                case ObjectSnapType.End:
                    DrawQuad(drawPoint, snapSymbolSize, renderContext);
                    break;
                case ObjectSnapType.Mid:
                    DrawTriangle(drawPoint, snapSymbolSize, renderContext);
                    break;
                case ObjectSnapType.Quad:
                    renderContext.SetLineSize(3.0f);
                    DrawRhombus(drawPoint, snapSymbolSize, renderContext);
                    break;
                case ObjectSnapType.Nearest:
                    DrawNearestIcon(drawPoint, snapSymbolSize, renderContext);
                    break;
                case ObjectSnapType.Intersect:
                    DrawIntersection(drawPoint, snapSymbolSize, renderContext);
                    break;
                case ObjectSnapType.Perpendicular:
                    DrawPerpendicular(drawPoint, snapSymbolSize, renderContext);
                    break;
            }

            renderContext.SetLineSize(1);
        }

        public static void DisplayTrackingPoint(ICadDrawAble canvasDrawing, HashSet<Point3D> polarPoints,
            RenderContextBase renderContext, double trackingPointSize = 40)
        {
            renderContext.SetLineSize(2);

            // white color
            renderContext.SetColorWireframe(Color.FromArgb(0, 255, 0));
            renderContext.SetState(depthStencilStateType.DepthTestOff);
            renderContext.SetLineSize(1);
            foreach (var polarPoint in polarPoints)
            {
                if (polarPoint == null)
                {
                    return;
                }
                Point2D onScreen =canvasDrawing.WorldToScreen(polarPoint);
                if (onScreen == null)
                {
                    return;
                }
                var drawPoint = new System.Drawing.Point((int) onScreen.X, (int) (onScreen.Y));
                DrawCrossTrackingpoint(drawPoint,trackingPointSize,renderContext);
            }
        }

        public static void DisplayTrackedPoint(ICadDrawAble canvasDrawing, Point3D potaTrackedPoint,RenderContextBase renderContext, double trackingPointSize = 40)
        {
            renderContext.SetLineSize(2);

            // white color
            renderContext.SetColorWireframe(Color.FromArgb(0, 255, 0));
            renderContext.SetState(depthStencilStateType.DepthTestOff);
            renderContext.SetLineSize(1);
            Point2D onScreen =canvasDrawing.WorldToScreen(potaTrackedPoint);
            var drawPoint = new System.Drawing.Point((int) onScreen.X, (int) (onScreen.Y));
            DrawCrossTrackingpoint(drawPoint,trackingPointSize,renderContext);


        }
        private static void DrawCross(System.Drawing.Point onScreen, double snapSymbolSize, RenderContextBase renderContext)
        {
            double dim1 = onScreen.X + (snapSymbolSize / 2);
            double dim2 = onScreen.Y + (snapSymbolSize / 2);
            double dim3 = onScreen.X - (snapSymbolSize / 2);
            double dim4 = onScreen.Y - (snapSymbolSize / 2);

            Point3D topLeftVertex = new Point3D(dim3, dim2);
            Point3D topRightVertex = new Point3D(dim1, dim2);
            Point3D bottomRightVertex = new Point3D(dim1, dim4);
            Point3D bottomLeftVertex = new Point3D(dim3, dim4);

            renderContext.DrawLines(
                new Point3D[]
                {
                    bottomLeftVertex,
                    topRightVertex,

                    topLeftVertex,
                    bottomRightVertex,

                });
        }

        private static void DrawCrossTrackingpoint(System.Drawing.Point onScreen, double snapSymbolSize, RenderContextBase renderContext)
        {
            double dim1 = onScreen.X + (snapSymbolSize / 2);
            double dim2 = onScreen.Y + (snapSymbolSize / 2);
            double dim3 = onScreen.X - (snapSymbolSize / 2);
            double dim4 = onScreen.Y - (snapSymbolSize / 2);
            var top = new Point3D(onScreen.X,dim2);
            var bottom = new Point3D(onScreen.X,dim4);
            var left = new Point3D(dim3,onScreen.Y);
            var right = new Point3D(dim1,onScreen.Y);
            renderContext.DrawLines(
               new Point3D[]
               {
                    top,
                    bottom,

                    left,
                    right,

               });

        }
        private static void DrawIntersection(System.Drawing.Point onScreen, double snapSymbolSize, RenderContextBase renderContext)
        {
            double dim1 = onScreen.X + (snapSymbolSize);
            double dim2 = onScreen.Y + (snapSymbolSize);
            double dim3 = onScreen.X - (snapSymbolSize);
            double dim4 = onScreen.Y - (snapSymbolSize);
            Point3D topLeftVertex = new Point3D(dim3, dim2);
            Point3D topRightVertex = new Point3D(dim1, dim2);
            Point3D bottomRightVertex = new Point3D(dim1, dim4);
            Point3D bottomLeftVertex = new Point3D(dim3, dim4);

            renderContext.DrawLine(topLeftVertex, bottomRightVertex);
            renderContext.DrawLine(bottomLeftVertex, topRightVertex);
        }
        private static void DrawNearestIcon(System.Drawing.Point onScreen, double snapSymbolSize, RenderContextBase renderContext)
        {
            double dim1 = onScreen.X + (snapSymbolSize);
            double dim2 = onScreen.Y + (snapSymbolSize);
            double dim3 = onScreen.X - (snapSymbolSize);
            double dim4 = onScreen.Y - (snapSymbolSize);
            Point3D topLeftVertex = new Point3D(dim3, dim2);
            Point3D topRightVertex = new Point3D(dim1, dim2);
            Point3D bottomRightVertex = new Point3D(dim1, dim4);
            Point3D bottomLeftVertex = new Point3D(dim3, dim4);

            renderContext.DrawLineLoop(new Point3D[]
            {
                topLeftVertex,bottomRightVertex,bottomLeftVertex,topRightVertex
            });
        }
        private static void DrawCircle(System.Drawing.Point onScreen, double snapSymbolSize, RenderContextBase renderContext)
        {
            double radius = snapSymbolSize;

            double x2 = 0, y2 = 0;

            List<Point3D> pts = new List<Point3D>();

            for (int angle = 0; angle < 360; angle += 10)
            {
                double rad_angle = Utility.DegToRad(angle);

                x2 = onScreen.X + radius * Math.Cos(rad_angle);
                y2 = onScreen.Y + radius * Math.Sin(rad_angle);

                Point3D circlePoint = new Point3D(x2, y2);
                pts.Add(circlePoint);
            }

            renderContext.DrawLineLoop(pts.ToArray());
        }

        /// <summary>
        /// Draw quad with renderContext to rapresent END snap point
        /// </summary>
        private static void DrawQuad(System.Drawing.Point onScreen, double snapSymbolSize, RenderContextBase renderContext)
        {
            double dim1 = onScreen.X + (snapSymbolSize);
            double dim2 = onScreen.Y + (snapSymbolSize);
            double dim3 = onScreen.X - (snapSymbolSize);
            double dim4 = onScreen.Y - (snapSymbolSize);

            Point3D topLeftVertex = new Point3D(dim3, dim2);
            Point3D topRightVertex = new Point3D(dim1, dim2);
            Point3D bottomRightVertex = new Point3D(dim1, dim4);
            Point3D bottomLeftVertex = new Point3D(dim3, dim4);

            renderContext.DrawLineLoop(new Point3D[]
            {
                bottomLeftVertex,
                bottomRightVertex,
                topRightVertex,
                topLeftVertex
            });
        }

        private static void DrawTriangle(System.Drawing.Point onScreen, double snapSymbolSize, RenderContextBase renderContext)
        {
            double h = 1.5 * (snapSymbolSize + (snapSymbolSize / 2));
            double halfa = h / Math.Sqrt(3);
            double dim1 = onScreen.Y + 1.5 * snapSymbolSize;
            double dim2 = onScreen.Y - (1.5 * snapSymbolSize / 2);
            double dim3 = onScreen.X + halfa;
            double dim4 = onScreen.X - halfa;

            Point3D topVertex = new Point3D(onScreen.X, dim1);
            Point3D bottomLeftVertex = new Point3D(dim4, dim2);
            Point3D bottomRightVertex = new Point3D(dim3, dim2);


            renderContext.DrawLineLoop(new Point3D[]
            {
                topVertex,bottomRightVertex,bottomLeftVertex
            });
        }
        private static void DrawPerpendicular(System.Drawing.Point onScreen, double snapSymbolSize, RenderContextBase renderContext)
        {
            double dim1 = onScreen.X + (snapSymbolSize);
            double dim2 = onScreen.Y + (snapSymbolSize);
            double dim3 = onScreen.X - (snapSymbolSize);
            double dim4 = onScreen.Y - (snapSymbolSize);

            Point3D topVerTex = new Point3D(onScreen.X, onScreen.Y);
            Point3D bottomVerTex = new Point3D(onScreen.X, dim4);
            Point3D bottomLeftVerTex = new Point3D(dim3, dim4);
            Point3D leffVertex = new Point3D(dim3, onScreen.Y);
            Point3D topLeft = new Point3D(dim3, dim2);
            Point3D BottomRight = new Point3D(dim1, dim4);

            renderContext.DrawLine(topLeft, bottomLeftVerTex);
            renderContext.DrawLine(bottomLeftVerTex, BottomRight);
            renderContext.DrawLine(topVerTex, leffVertex);
            renderContext.DrawLine(topVerTex, bottomVerTex);
        }
        private static void DrawRhombus(System.Drawing.Point onScreen, double snapSymbolSize, RenderContextBase renderContext)
        {
            double dim1 = onScreen.X + (snapSymbolSize / 1.5);
            double dim2 = onScreen.Y + (snapSymbolSize / 1.5);
            double dim3 = onScreen.X - (snapSymbolSize / 1.5);
            double dim4 = onScreen.Y - (snapSymbolSize / 1.5);

            Point3D topVertex = new Point3D(onScreen.X, dim2);
            Point3D bottomVertex = new Point3D(onScreen.X, dim4);
            Point3D rightVertex = new Point3D(dim1, onScreen.Y);
            Point3D leftVertex = new Point3D(dim3, onScreen.Y);

            renderContext.DrawLineLoop(new Point3D[]
            {
                bottomVertex,
                rightVertex,
                topVertex,
                leftVertex,
            });
        }

    }
}
