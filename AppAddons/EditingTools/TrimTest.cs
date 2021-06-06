using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppModels;
using AppModels.CustomEntity;
using AppModels.Enums;
using AppModels.EventArg;
using AppModels.Interaface;
using AppModels.Undo;
using AppModels.Undo.Backup;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using DrawingModule.CommandClass;
using DrawingModule.CustomControl.CanvasControl;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.DrawToolBase;
using DrawingModule.Enums;
using DrawingModule.Helper;
using Point = System.Drawing.Point;

namespace AppAddons.EditingTools
{
    
    public class TrimTest: ToolBase
    {
        private PickState CurrentPickState { get; set; }
        public Point3D StartPoint { get; set; }
        public Point3D EndPoint { get; set; }
        private Entity _entityUnderMouse;
        public override Point3D BasePoint { get; protected set; }
        private bool _processingTool;
        public TrimTest()
        {
            this.CurrentPickState = PickState.Pick;
            IsSnapEnable = false;
            _processingTool = true;
        }

        [CommandMethod("Trim")]
        public void Trim()
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
            DrawInteractiveTrim((ICadDrawAble)sender, e);
        }

        public override void NotifyMouseDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = (ICadDrawAble)sender;
            var mousePosition = RenderContextUtility.ConvertPoint(canvas.GetMousePosition(e));
            var index = canvas.GetEntityUnderMouseCursor(mousePosition);
            var check = GetEntitiesUnderMouse(mousePosition,canvas);
            if (!canvas.ActiveViewport.ToolBar.Contains(mousePosition))
            {
                this.ProcessMouseDownForSelection(e,check,canvas);
            }

        }
        public void ProcessMouseDownForSelection(MouseButtonEventArgs e, bool isSelected, ICadDrawAble canvasDrawing)
        {
            if (isSelected)
            {
                this.ProcessTrim(e, true, canvasDrawing);
                return;
            }
            if (this.StartPoint == null)
            {
                canvasDrawing.ScreenToPlane(RenderContextUtility.ConvertPoint(canvasDrawing.GetMousePosition(e)), canvasDrawing.DrawingPlane, out var clickPoint);
                StartPoint = clickPoint;
            }
            else
            {
                canvasDrawing.ScreenToPlane(RenderContextUtility.ConvertPoint(canvasDrawing.GetMousePosition(e)), canvasDrawing.DrawingPlane, out var clickPoint);
                this.EndPoint = clickPoint;
                this.ProcessTrim(e, false, canvasDrawing);

            }
        }
        private void ResetTrimTool()
        {
            this.CurrentPickState = PickState.Pick;
            this.StartPoint = null;
            this.EndPoint = null;
        }
        internal static void NormalizeBox(ref Point3D p1, ref Point3D p2)
        {

            var firstX = (int)Math.Min(p1.X, p2.X);
            var firstY = (int)Math.Min(p1.Y, p2.Y);
            var secondX = (int)Math.Max(p1.X, p2.X);
            var secondY = (int)Math.Max(p1.Y, p2.Y);

            p1.X = firstX;
            p1.Y = firstY;
            p2.X = secondX;
            p2.Y = secondY;
        }
        public void ProcessTrim(MouseButtonEventArgs e, bool isSelected, ICadDrawAble canvas)
        {
            var currentLocation = RenderContextUtility.ConvertPoint(canvas.GetMousePosition(e));
            var undoItem = new UndoList() {ActionType = ActionTypes.AddAndRemove};

            int[] ents;
            if (isSelected)
            {
                canvas.ScreenToPlane(currentLocation, Plane.XY, out var clickedPoint);
                var ent = canvas.GetEntityUnderMouseCursor(currentLocation);
                if (ent>=0)
                {
                    var tempEntity = canvas.Entities[ent];
                    TrimEntites(tempEntity,clickedPoint,undoItem);
                   
                    
                }
                this.ResetTrimTool();
                return;
            }


            //var startPoint = canvas.WorldToScreen(this.StartPoint);
            //var endPoint = canvas.WorldToScreen(this.EndPoint);
            var segmentCross = new Segment2D(StartPoint, EndPoint);
            Dictionary<Point3D, Entity> crossEntityDicts = new Dictionary<Point3D, Entity>();
            foreach (var entity in EntitiesManager.Entities)
            {
                if (entity is Line line)
                {
                    var lineSeg = new Segment2D(line.StartPoint, line.EndPoint);
                    if (Segment2D.IntersectionAndT(lineSeg, segmentCross, out var intersecPoint)&& intersecPoint!=null)
                    {
                        crossEntityDicts.Add(intersecPoint.ConvertPoint2DtoPoint3D(),entity);
                    }
                }
            }

            if (crossEntityDicts.Count>0)
            {
                foreach (var crossEntity in crossEntityDicts)
                {
                    TrimEntites(crossEntity.Value,crossEntity.Key,undoItem);
                }
            }
            UndoEngineer.SaveSnapshot(undoItem);

            this.ResetTrimTool();

        }

        public void TrimEntites(Entity trimEntity, Point3D crossPoint, UndoList undoItem)
        {
            
            var intersectPoints = new List<Point3D>();
            Segment2D tempSegment = null;
            if (trimEntity is Line line)
            {
                tempSegment = new Segment2D(line.StartPoint, line.EndPoint);
            }
            else
            {

            }
            foreach (var canvasEntity in EntitiesManager.Entities)
            {
                if (canvasEntity is Line lineToCheck)
                {

                    var segmentToCheck = new Segment2D(lineToCheck.StartPoint, lineToCheck.EndPoint);
                    if (segmentToCheck.P0 == tempSegment.P0 && segmentToCheck.P1 == tempSegment.P1)
                    {
                        continue;
                    }
                    if (Segment2D.IntersectionAndT(tempSegment, segmentToCheck, out var intersectPoint)&& intersectPoint!=null)
                    {
                        intersectPoints.Add(intersectPoint.ConvertPoint2DtoPoint3D());
                    }
                }
                else
                {

                }
            }
            intersectPoints.Insert(0, tempSegment.P0.ConvertPoint2DtoPoint3D());
            intersectPoints.Add(tempSegment.P1.ConvertPoint2DtoPoint3D());
            intersectPoints = Helper.SortPointInLine(intersectPoints);
            this.FindTrimSegment(intersectPoints, crossPoint, out var point1, out var point2);
            var newEntities = new List<Entity>();
            this.RemoveLineBetween(trimEntity, newEntities, point1, point2);
            if (newEntities.Count > 0)
            {
                var bacup = BackupEntitiesFactory.CreateBackup(newEntities, trimEntity, undoItem, EntitiesManager);
                bacup?.Backup();
                this.EntitiesManager.RemoveEntity(trimEntity);
                foreach (var newEntity in newEntities)
                {
                    this.EntitiesManager.AddAndRefresh(newEntity, trimEntity.LayerName);
                }
            }

        }

        public void RemoveLineBetween(Entity entity,List<Entity> ents,Point3D p1,Point3D p2)
        {
            
            if (entity is WallLine2D wallLine)
            {
                var entityRemove = new WallLine2D(p1, p2);
                var points = new List<Point3D>()
                {
                    wallLine.StartPoint
                };
                if (!points.Contains(p1))
                {
                    points.Add(p1);
                }

                if (!points.Contains(p2))
                {
                    points.Add(p2);
                }

                if (!points.Contains(wallLine.EndPoint))
                {
                    points.Add(EndPoint);
                }

                var i = 0;
                while (i < points.Count)
                {
                    var newLine = new WallLine2D(points[i], points[i + 1]);
                    if (newLine != entityRemove)
                    {
                        ents.Add(newLine);
                    }
                    i++;
                }

            }
            else if (entity is Line line)
            {
                var entityRemove = new Line(p1, p2);
                var points = new List<Point3D>()
                {
                    line.StartPoint
                };
                if (!points.Contains(p1))
                {
                    points.Add(p1);
                }

                if (!points.Contains(p2))
                {
                    points.Add(p2);
                }

                if (!points.Contains(line.EndPoint))
                {
                    points.Add(line.EndPoint);
                }

                var i = 0;
                points = Helper.SortPointInLine(points);
                while (i < points.Count-1)
                {
                    var newLine = new Line(points[i], points[i + 1]);
                    if (!IsTwoLineTheSame(entityRemove,newLine))
                    {
                        ents.Add(newLine);
                    }
                    i++;
                }
            }
        }

        private bool IsTwoLineTheSame(Line line1, Line line2)
        {
            if (line1.StartPoint == line2.StartPoint && line1.EndPoint == line2.EndPoint)
            {
                return true;
            }

            return line1.EndPoint == line2.StartPoint && line1.StartPoint ==line2.EndPoint;
        }
        private void FindTrimSegment(List<Point3D> points,Point3D checkPoint, out Point3D p1, out Point3D p2)
        {
            p1 = null;
            p2 = null;
            var linearPath = new LinearPath(points);
            var line = Utils.GetClosestSegment(linearPath, checkPoint);
            if (line!=null)
            {
                p1 = line.StartPoint;
                p2 = line.EndPoint;
            }

            //p1 = ClosesPoint(points, checkPoint);
            //pointList.Remove(p1);
            //p2 = ClosesPoint(pointList, checkPoint);

        }

        //private Point3D ClosesPoint(List<Point3D> points, Point3D checkPoint)
        //{
        //    var minDist = Double.MaxValue;
        //    Point3D result = null;
        //    foreach (var point3D in points)
        //    {
        //        var distance = Point3D.Distance(point3D, checkPoint);
        //        if (distance<minDist)
        //        {
        //            minDist = distance;
        //            result = point3D;
        //        }
        //    }

        //    return result;
        //}
        private bool GetEntitiesUnderMouse(System.Drawing.Point mousePosition,ICadDrawAble cadDraw)
        {
            var entIndex = cadDraw.GetEntityUnderMouseCursor(mousePosition);
            if (entIndex > -1)
            {
                if (entIndex > cadDraw.Entities.Count - 1)
                {
                    _entityUnderMouse = null;
                    return false;
                }
                _entityUnderMouse = cadDraw.Entities[entIndex];
                if (_entityUnderMouse is Picture picture|| _entityUnderMouse is BlockReference)
                {
                    return false;
                }

                return true;
            }
            else
            {
                _entityUnderMouse = null;
                return false;
            }
        }

        public override void NotifyPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this._processingTool = false;
                e.Handled = true;
            }
        }

        private void DrawInteractiveTrim(ICadDrawAble canvas, DrawInteractiveArgs e)
        {
            if (this.StartPoint == null || this.StartPoint == e.CurrentPoint) return;
            
            if (StartPoint !=null && e.CurrentPoint!= null)
            {
                DrawInteractiveUntilities.DrawInteractiveSpotLine(StartPoint,e.CurrentPoint,canvas);
            }
            //switch (this.CurrentPickState)
            //{
            //    case PickState.Cross:
            //        DrawSelectionBox(StartPoint, e.CurrentPoint, Color.DarkBlue, true, true, canvas);
            //        break;
            //    case PickState.Enclose:
            //        DrawSelectionBox(StartPoint, e.CurrentPoint, Color.DarkRed, true, false, canvas);
            //        break;
            //    default:
            //        break;
            //}
        }
        private void DrawSelectionBox(Point3D p1, Point3D p2, Color transparentColor, bool drawBorder, bool dottedBorder, ICadDrawAble canvas)
        {
            //var pp1 = p1;
            //var pp2 = p2;
            //pp1.Y = (int)(Size.Height - pp1.Y);
            //pp2.Y = (int)(Size.Height - pp2.Y);

            var startPoint = canvas.WorldToScreen(p1);
            var endPoint = canvas.WorldToScreen(p2);

            var startPointForRec = new Point3D(startPoint.X, (startPoint.Y));
            var endPointForRec = new Point3D(endPoint.X, (endPoint.Y));

            NormalizeBox(ref startPointForRec, ref endPointForRec);

            //Adjust the bounds so that it doesn't exit from the current viewport frame

            int[] viewFrame = canvas.Viewports[canvas.ActiveViewportIndex].GetViewFrame();
            int left = viewFrame[0];
            int top = viewFrame[1] + viewFrame[3];
            int right = left + viewFrame[2];
            int bottom = viewFrame[1];

            if (endPointForRec.X > right - 1)
                endPointForRec.X = right - 1;

            if (endPointForRec.Y > top - 1)
                endPointForRec.Y = top - 1;

            if (startPointForRec.X < left + 1)
                startPointForRec.X = left + 1;

            if (startPointForRec.Y < bottom + 1)
                startPointForRec.Y = bottom + 1;




            canvas.renderContext.SetState(blendStateType.Blend);
            canvas.renderContext.SetColorWireframe(System.Drawing.Color.FromArgb(40, transparentColor.R, transparentColor.G, transparentColor.B));
            canvas.renderContext.SetState(rasterizerStateType.CCW_PolygonFill_CullFaceBack_NoPolygonOffset);

            //int w = (int)(pp2.X - pp1.X);
            //int h = (int)(pp2.Y - pp1.Y);

            var w1 = (int)(endPointForRec.X - startPointForRec.X);
            var h1 = (int)(endPointForRec.Y - startPointForRec.Y);




            canvas.renderContext.DrawQuad(new System.Drawing.RectangleF((float)(startPointForRec.X + 1), (float)(startPointForRec.Y + 1), w1 - 1, h1 - 1));
            canvas.renderContext.SetState(blendStateType.NoBlend);

            if (drawBorder)
            {
                canvas.renderContext.SetColorWireframe(System.Drawing.Color.FromArgb(255, transparentColor.R, transparentColor.G, transparentColor.B));

                List<Point3D> pts = null;

                if (dottedBorder)
                {
                    canvas.renderContext.SetLineStipple(1, 0x0F0F, canvas.Viewports[0].Camera);
                    canvas.renderContext.EnableLineStipple(true);
                }

                var l = (int)startPoint.X;
                var r = (int)endPoint.X;


                pts = new List<Point3D>(new Point3D[]
                {
                    new Point3D(l, startPoint.Y), new Point3D(endPoint.X, startPoint.Y),
                    new Point3D(r, startPoint.Y), new Point3D(r, endPoint.Y),
                    new Point3D(r, endPoint.Y), new Point3D(l, endPoint.Y),
                    new Point3D(l, endPoint.Y), new Point3D(l, startPoint.Y),
                });


                canvas.renderContext.DrawLines(pts.ToArray());

                if (dottedBorder)
                    canvas.renderContext.EnableLineStipple(false);
            }
        }


    }
}
