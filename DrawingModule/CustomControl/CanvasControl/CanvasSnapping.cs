using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Threading;
using AppModels.Enums;
using devDept.Geometry;
using DrawingModule.Enums;
using DrawingModule.Helper;

namespace DrawingModule.CustomControl.CanvasControl
{
    public partial class CanvasDrawing
    {
        public SnapPoint SnapPoint { get; private set; }

        public LineTypes TempLineType
        {
            get
            {
                if (CurrentPoint == null || LastClickPoint == null)
                    return AppModels.Enums.LineTypes.None;
                if (CurrentPoint == LastClickPoint )
                {
                    return AppModels.Enums.LineTypes.None;
                }

                if (Math.Abs(CurrentPoint.X - LastClickPoint.X) < 0.01)
                {
                    return AppModels.Enums.LineTypes.VerticalLine;
                }
                else if(Math.Abs(CurrentPoint.Y - LastClickPoint.Y) < 0.01)
                {
                    return AppModels.Enums.LineTypes.HorizontalLine;
                }

                return AppModels.Enums.LineTypes.SlantingLine;

            }
        }
        
        public int SnapSymbolSize = 12;
        private HashSet<SnapPoint> _snapPoints = new HashSet<SnapPoint>();
        private HashSet<Point3D> _polaTrackingPoint = new HashSet<Point3D>();
        public HashSet<SnapPoint> SnapPoints => _snapPoints;
        public HashSet<Point3D> PolaTrackingPoints=>_polaTrackingPoint;
        public Point3D PolaTrackedPoint;
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
                if (SnapPoint.Type !=ObjectSnapType.Point  && SnapPoint.Type != ObjectSnapType.Nearest)
                {
                    _isMouseMove = false;
                    var timer = new DispatcherTimer();
                    timer.Tick += Timer_Tick;
                    timer.Interval = new TimeSpan(0,0,2);
                    timer.Start();
                    
                }
                
            }
            
        }

        private bool _isMouseMove = false;

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!_isMouseMove)
            {
                if (PolaTrackingPoints.Contains(SnapPoint))
                {
                    PolaTrackingPoints.Remove(SnapPoint);
                }
                else
                {
                    PolaTrackingPoints.Add(SnapPoint);
                }
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
