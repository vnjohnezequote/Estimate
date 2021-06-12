using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ApplicationService;
using AppModels;
using AppModels.CustomEntity;
using AppModels.EntityCreator;
using AppModels.Enums;
using AppModels.EventArg;
using AppModels.Interaface;
using AppModels.Undo;
using AppModels.Undo.Backup;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using DrawingModule.CommandClass;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.DrawToolBase;
using DrawingModule.Enums;
using DrawingModule.UserInteractive;

namespace AppAddons.EditingTools
{
    public class TrimJoist : ToolBase
    {
        private PickState CurrentPickState { get; set; }
        public override Point3D BasePoint { get; protected set; }
        public override string ToolName => "Trim Joist";
        private List<Line> _boundaryLines;
        private List<Joist2D> _framing2Ds;
        private bool _processingTool;
        public Point3D StartPoint { get; set; }
        public Point3D EndPoint { get; set; }
        private bool _isWaitingSelectBoundary;
        private Entity _entityUnderMouse;
        private Point3D _currentMouse;
        public TrimJoist()
        {
            CurrentPickState = PickState.Pick;
            _boundaryLines = new List<Line>();
            _framing2Ds = new List<Joist2D>();
            _processingTool = true;
            IsSnapEnable = false;
            IsUsingOrthorMode = false;
            _isWaitingSelectBoundary = true;

        }

        [CommandMethod("Trim Framing")]
        public void TrimFraming()
        {
            OnProcessCommand();
        }

        protected virtual void OnProcessCommand()
        {
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            while (_processingTool)
            {
                if (_isWaitingSelectBoundary)
                {
                    ToolMessage = "Please select boundary line to trim";
                    var promptEntities = new PromptEntityOptions();
                    var result = acDoc.Editor.GetEntities(promptEntities, false);
                    if (result.Status == PromptStatus.Cancel) return;
                    foreach (var entity in result.Entities)
                    {
                        if (entity is Line line)
                        {
                            _boundaryLines.Add(line);
                        }
                    }
                    _isWaitingSelectBoundary = false;
                }
                ToolMessage = "Please select Joist to trim";
            }

        }

        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {
            DrawInteractiveTrim((ICadDrawAble)sender, e);
        }

        public override void NotifyMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_isWaitingSelectBoundary) return;
            var canvas = (ICadDrawAble)sender;
            var mousePosition = RenderContextUtility.ConvertPoint(canvas.GetMousePosition(e));
            //var index = canvas.GetEntityUnderMouseCursor(mousePosition);
            var check = GetEntitiesUnderMouse(mousePosition, canvas);
            if (!canvas.ActiveViewport.ToolBar.Contains(mousePosition))
            {
                canvas.ScreenToPlane(mousePosition, Plane.XY, out _currentMouse);
                this.ProcessMouseDownForSelection(e, check, canvas);
            }

        }

        public override void NotifyPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this._processingTool = false;
                e.Handled = false;
            }
        }

        private bool GetEntitiesUnderMouse(System.Drawing.Point mousePosition, ICadDrawAble cadDraw)
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
                if (_entityUnderMouse is Joist2D)
                {
                    return true;
                }

                return false;
            }
            else
            {
                _entityUnderMouse = null;
                return false;
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

        public void ProcessTrim(MouseButtonEventArgs e, bool isSelected, ICadDrawAble canvas)
        {
            var undoItem = new UndoList() { ActionType = ActionTypes.AddAndRemove };
            if (isSelected)
            {
                if (_entityUnderMouse != null && _entityUnderMouse is Joist2D joist)
                {
                    
                    TrimingJoist(joist, _currentMouse,undoItem);
                    _processingTool = false;
                }
            }
            else
            {
                if (StartPoint != null && EndPoint != null)
                {
                    var crossLine = new Line(StartPoint, EndPoint);

                    var crossJoists = new Dictionary<Joist2D, Point3D>();
                    foreach (var entity in EntitiesManager.Entities)
                    {
                        if (entity is Joist2D joist)
                        {
                            if (joist.IntersectWith(crossLine, out var intersectPoint, out var intersectType) && intersectType == segmentIntersectionType.Cross)
                            {
                                crossJoists.Add(joist, intersectPoint);
                            }

                        }
                    }

                    if (crossJoists.Count > 0)
                    {
                        foreach (var crossJoist in crossJoists)
                        {
                            TrimingJoist(crossJoist.Key, crossJoist.Value,undoItem);
                        }
                    }

                    _processingTool = false;
                }
            }
            UndoEngineer.SaveSnapshot(undoItem);
        }

        private void RemoveFarestLine(List<Line> boundaryLines, Point3D crossPoint)
        {
            Line farestLine = null;
            var maxDisc = 0.0;
            foreach (var line in boundaryLines)
            {
                var disc = crossPoint.DistanceTo(new Segment2D(line.StartPoint, line.EndPoint));
                if (disc > maxDisc)
                {
                    maxDisc = disc;
                    farestLine = line;
                }

            }

            if (farestLine != null)
            {
                boundaryLines.Remove(farestLine);
            }
        }

        private void TrimingJoist(Joist2D joist, Point3D crossedPoint,UndoList undoItem)
        {
            var intersectPoints = new List<Point3D>();
            if (_boundaryLines.Count > 2)
            {
                RemoveFarestLine(_boundaryLines, crossedPoint);
            }
            //Segment2D tempSegment = new Segment2D(joist.StartCenterLinePoint,joist.EndCenterLinePoint);
            foreach (var boundaryLine in _boundaryLines)
            {
                if (joist.IntersectWith(boundaryLine, out var intersectPoint, out var intersectType))
                {
                    if (intersectType == segmentIntersectionType.Cross && intersectPoint != null)
                    {
                        intersectPoints.Add(intersectPoint);
                    }
                }
            }

            if (intersectPoints.Count > 0)
            {
                intersectPoints.Insert(0, joist.StartCenterLinePoint);
                intersectPoints.Add(joist.EndCenterLinePoint);
                intersectPoints = Helper.SortPointInLine(intersectPoints);
                var trimedSegments = new List<Segment2D>();
                FindTrimedSegment(intersectPoints, crossedPoint, trimedSegments);
                if (trimedSegments.Count > 0)
                {
                    var framingSheet = joist.FramingReference.FramingSheet;
                    var listAddNewJoist = new List<Entity>();
                    foreach (var segment in trimedSegments)
                    {
                        var joistSegment = ProjectSegmentOn(new Segment2D(joist.OuterStartPoint, joist.OuterEndPoint),
                            segment);
                        var joistCreator = new Joist2DCreator(joist, joistSegment.P0.ConvertPoint2DtoPoint3D(),
                            joistSegment.P1.ConvertPoint2DtoPoint3D());
                        listAddNewJoist.Add((Joist2D)joistCreator.GetFraming2D());
                        this.EntitiesManager.AddAndRefresh((Joist2D)joistCreator.GetFraming2D(), joist.LayerName);
                    }

                    var backup = BackupEntitiesFactory.CreateBackup(listAddNewJoist, joist, undoItem, EntitiesManager);
                    backup?.Backup();
                    this.EntitiesManager.RemoveEntity(joist);
                }
            }




        }

        private Segment2D ProjectSegmentOn(Segment2D segment1, Segment2D sourceSegment)
        {
            var disc1 = segment1.Project(sourceSegment.P0);
            var projectPt1 = segment1.PointAt(disc1);
            var disc2 = segment1.Project(sourceSegment.P1);
            var projectPt2 = segment1.PointAt(disc2);
            return new Segment2D(projectPt1, projectPt2);
        }
        private void FindTrimedSegment(List<Point3D> segments, Point3D crossedPoint, List<Segment2D> trimmedSegments)
        {
            for (int i = 0; i < segments.Count - 1; i++)
            {
                var segment = new Segment2D(segments[i], segments[i + 1]);
                var disc = segment.Project(crossedPoint);
                var projectPoint = segment.PointAt(disc);
                if (!UtilityEx.IsPointOnSegment(projectPoint, segment.P0, segment.P1, 0.0001))
                {
                    trimmedSegments.Add(segment);
                }
            }
        }
        private void DrawInteractiveTrim(ICadDrawAble canvas, DrawInteractiveArgs e)
        {
            if (this.StartPoint == null || this.StartPoint == e.CurrentPoint) return;

            if (StartPoint != null && e.CurrentPoint != null)
            {
                DrawInteractiveUntilities.DrawInteractiveSpotLine(StartPoint, e.CurrentPoint, canvas);
            }
        }


    }
}
