using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using ApplicationInterfaceCore;
using ApplicationService;
using AppModels.Enums;
using AppModels.EventArg;
using AppModels.Interaface;
using AppModels.Undo;
using AppModels.Undo.Backup;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using DrawingModule.Application;
using DrawingModule.CommandClass;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.DrawToolBase;
using DrawingModule.Helper;
using DrawingModule.UserInteractive;

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
        private List<Entity> _selectedEntities = new List<Entity>();
        private bool _waitingForSelectBoundary;
        private bool _waltingForSelectExtendEntities;

        public ExtendTool()
        {
            _processingTool = true;
            _waitingForSelectBoundary = true;
            _waltingForSelectExtendEntities = true;
            IsSnapEnable = false;
            IsUsingOrthorMode = false;
            //_selectedEntityIndex = -1;

            var aDoc = Application.DocumentManager.MdiActiveDocument;
            var _firSelectionEntity = aDoc.Editor.CanvasDrawing.GetSelectionEntity();
            if (_firSelectionEntity ==null)
            {
                _waitingForSelectBoundary = true;
                ToolMessage = "Please select Boundary Entity for extend";
            }
            else
            {
                _waitingForSelectBoundary = false;
                ToolMessage = "Please Select Entity to Extend";
            }
            
            
        }

        [CommandMethod("Extend")]
        public void Extend()
        {
            OnProcessCommand();
        }

        protected virtual void OnProcessCommand()
        {
            var aDoc = Application.DocumentManager.MdiActiveDocument;
            while (_processingTool)
            {
                while (_waitingForSelectBoundary)
                {
                    ToolMessage = "Please select Boundary Entity for extend";
                    var promptLineOption = new PromptSelectionOptions();
                    var promptResults = aDoc.Editor.GetSelection(promptLineOption);
                    if (promptResults.Status == PromptStatus.OK)
                    {
                        _firstSelectedEntity = promptResults.Value;
                        _waitingForSelectBoundary = false;
                    }
                    else return;

                }

                while (_waltingForSelectExtendEntities)
                {
                    ToolMessage = "Please select Entity to extend";
                    var promp = new PromptEntityOptions("Please select Bondary Entity for extend");
                    var promptResults = aDoc.Editor.GetEntities(promp,true);
                    if (promptResults.Status == PromptStatus.OK && promptResults.Entities!=null && promptResults.Entities.Count!=null)
                    {
                        _selectedEntities = promptResults.Entities;
                        ProcessCommand();
                    }
                    else if (promptResults.Entities != null && promptResults.Status == PromptStatus.OK && (promptResults.Entities != null || promptResults.Entities.Count !=0 ))
                    {
                        continue;
                    }
                    else return;
                }

            }
        }

        private void ProcessCommand()
        {
            var i = 0;
            var undoItem = new UndoList() {ActionType = ActionTypes.Edit};
            while (i<_selectedEntities.Count)
            {
                var selEntity = _selectedEntities[i];
                var backup = BackupEntitiesFactory.CreateBackup(selEntity, undoItem, EntitiesManager);
                backup?.Backup();
                ExtendProcess(_firstSelectedEntity, _selectedEntities[i],undoItem);
                i++;
            }
            this.UndoEngineer.SaveSnapshot(undoItem);
            EntitiesManager.Refresh();
            ResetTool();
        }

        private void ResetTool()
        {
            foreach (var selectedEntity in _selectedEntities)
            {
                selectedEntity.Selected = false;
            }
            _selectedEntities.Clear();

        }

        private void ExtendProcess(Entity boundaryEntity, Entity extendEntity,UndoList undoItem)
        {
            if (boundaryEntity is ICurve && extendEntity is ICurve)
            {
                ICurve boundary = boundaryEntity as ICurve;
                ICurve curve = extendEntity as ICurve;

                // Check which end of curve is near to boundary
                double t1, t2;
                boundary.ClosestPointTo(curve.StartPoint, out t1);
                boundary.ClosestPointTo(curve.EndPoint, out t2);

                Point3D projStartPt = boundary.PointAt(t1);
                Point3D projEndPt = boundary.PointAt(t2);

                double curveStartDistance = curve.StartPoint.DistanceTo(projStartPt);
                double curveEndDistance = curve.EndPoint.DistanceTo(projEndPt);

                bool success = false;
                Entity successEntity = null;
                if (curveStartDistance < curveEndDistance)
                {
                    if (curve is Line)
                    {
                        success = ExtendLine(curve, boundary, true,ref successEntity);
                    }
                    else if (curve is LinearPath)
                    {
                        success = ExtendPolyLine(curve, boundary, true,ref successEntity);
                    }
                }
                else
                {
                    if (curve is Line)
                    {
                        success = ExtendLine(curve, boundary, false,ref successEntity);
                    }
                    else if (curve is LinearPath)
                    {
                        success = ExtendPolyLine(curve, boundary, false,ref successEntity);
                    }
                }
                
                if (success)
                {
                    //EntitiesManager.EntitiesRegen();
                    //var backup =
                    //    BackupEntitiesFactory.CreateBackup(successEntity, extendEntity, undoItem, EntitiesManager);
                    //backup.Backup();
                    //EntitiesManager.AddAndRefresh(successEntity, extendEntity.LayerName);
                    //EntitiesManager.RemoveEntity(extendEntity);
                }
            }
        }
        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {
            DrawInteractiveExtend((ICadDrawAble)sender, e);

        }

        public override void NotifyMouseDown(object sender, MouseButtonEventArgs e)
        {
            //var canvas = (ICadDrawAble)sender;
            //var mousePosition = RenderContextUtility.ConvertPoint(canvas.GetMousePosition(e));
            //var index = canvas.GetEntityUnderMouseCursor(mousePosition);
            //if (_firstSelectedEntity != null && _secondSelectedEntity != null) return;
            //if (index > -1)
            //{
            //    _selectedEntityIndex = index;
            //}

            //if (_secondSelectedEntity != null) return;
            //canvas.ScreenToPlane(mousePosition, canvas.DrawingPlane, out var clickPoint);
        }

        public override void NotifyPreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Escape)
            //{
            //    this._processingTool = false;
            //    e.Handled = true;
            //}
        }

        private void DrawInteractiveExtend(ICadDrawAble canvas, DrawInteractiveArgs e)
        {

        }

        private bool ExtendLine(ICurve lineCurve, ICurve boundary, bool nearStart,ref Entity successEntity)
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

                successEntity = (Entity)line.Clone();
                //EntitiesManager.AddAndRefresh(tempEntity, tempEntity.LayerName);
                return true;
            }
            return false;
        }
        private bool ExtendPolyLine(ICurve lineCurve, ICurve boundary, bool nearStart,ref Entity successEntity)
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
                successEntity = (Entity)line.Clone();
                //EntitiesManager.AddAndRefresh(tempEntity, tempEntity.LayerName);
                return true;
            }

            return false;
        }


    }
}
