using System.Collections.Generic;
using devDept.Geometry;
using DrawingModule.Enums;
using DrawingModule.Helper;

namespace DrawingModule.CustomControl.CanvasControl
{
    public partial class CanvasDrawing
    {
        public SnapPoint SnapPoint { get; private set; }
        public int SnapSymbolSize = 12;
        private HashSet<SnapPoint> _snapPoints = new HashSet<SnapPoint>();
        public HashSet<SnapPoint> SnapPoints => _snapPoints;
        private void SetCurrentPoint(System.Drawing.Point mouseLocation, bool getEndPoint = false)
        {
            _snapPoints = Utils.GetSnapPoints(mouseLocation,this);
            if (_snapPoints.Count == 0)
            {
                SnapPoint = null;
                return;
            }
            SnapPoint = GetClosestPoint(_snapPoints, mouseLocation);
            if (SnapPoint != null)
            {
                CurrentPoint = SnapPoint;
            }
           

        }
        private SnapPoint GetClosestPoint(HashSet<SnapPoint> snapPoints, System.Drawing.Point mousePosition)
        {
            var minDist = double.MaxValue;
            SnapPoint snap = null;
            //var index = 0;
            foreach (var verTex in snapPoints)
            {
                var vertexScreen = WorldToScreen(verTex);
                var currentScreen = new Point2D(mousePosition.X, Size.Height - mousePosition.Y);
                var dist = Point2D.Distance(vertexScreen, currentScreen);
                if (verTex.Type == ObjectSnapType.Perpendicular && dist < PickBoxSize)
                {
                    snap = verTex;
                    break;
                }

                if ((verTex.Type == ObjectSnapType.End || verTex.Type == ObjectSnapType.Mid || verTex.Type == ObjectSnapType.Intersect) && dist < PickBoxSize)
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
    }
}
