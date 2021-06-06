using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ApplicationInterfaceCore;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels.CustomEntity;
using AppModels.Enums;
using AppModels.EventArg;
using AppModels.Interaface;
using AppModels.Undo;
using AppModels.Undo.Backup;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace AppAddons.EditingTools
{
    public class OffsetTool : ToolBase, IOffsetDisance
    {
        public override Point3D BasePoint { get; protected set; }
        public override string ToolName => "Offset";
        private bool _waitingForSelection;
        private int _offsetDistance;

        public int OffsetDistance { get => _offsetDistance; set => SetProperty(ref _offsetDistance, value); }
        private Entity _selectedEntity;
        //private Entity _offetEntity;

        private Point3D _offsetPoint;

        public OffsetTool() : base()
        {
            IsUsingOffsetDistance = true;
            IsUsingOrthorMode = true;
            IsSnapEnable = true;
        }

        [CommandMethod("Offset")]
        public void Offset()
        {
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            while (true)
            {
                ToolMessage = "Please Select object to offset";
                var promptLineOption = new PromptSelectionOptions();
                var result = acDoc.Editor.GetSelection(promptLineOption);
                if (result.Status == PromptStatus.OK)
                {
                    this._selectedEntity = result.Value;
                }
                else
                {
                    return;
                }

                IsUsingLengthTextBox = true;
                DynamicInput?.FocusDynamicInputTextBox(FocusType.Length);

                var promptPointOption = new PromptPointOptions(ToolMessage);
                var resultPoint = acDoc.Editor.GetPoint(promptPointOption);
                if (resultPoint.Status == PromptStatus.OK)
                {
                    _offsetPoint = resultPoint.Value;
                }
                else
                {
                    return;
                }

                ProcessOffsetEntity();
            }

        }

        public override void NotifyMouseMove(object sender, MouseEventArgs e)
        {
            DynamicInput?.FocusDynamicInputTextBox(FocusType.Length);
        }

        private void ProcessOffsetEntity()
        {
            if (this._selectedEntity != null && this._offsetPoint != null)
            {
                var offetEntity = CalculatorOffsetEntity(_selectedEntity, _offsetPoint);
                if (offetEntity != null)
                {
                    var undoItem = new UndoList() {ActionType = ActionTypes.Add};
                    var backup = BackupEntitiesFactory.CreateBackup(offetEntity, undoItem, EntitiesManager);
                    backup?.Backup();
                    EntitiesManager.AddAndRefresh(offetEntity, LayerManager.SelectedLayer.Name);
                    UndoEngineer.SaveSnapshot(undoItem);
                }

            }

            ResetTool();
        }

        private void ResetTool()
        {
            //this._offetEntity = null;
            _offsetPoint = null;
            this._selectedEntity = null;
            this._waitingForSelection = true;
        }
        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {
            DrawInteractiveOffset((ICadDrawAble)sender, e);
        }

        private void DrawInteractiveOffset(ICadDrawAble canvas, DrawInteractiveArgs e)
        {
            if (this._selectedEntity == null)
            {
                return;
            }

            if (e.CurrentPoint == null)
            {
                return;
            }


            var offetEntity = CalculatorOffsetEntity(this._selectedEntity, e.CurrentPoint);


            if (offetEntity is ICurve|| offetEntity is FramingRectangle2D)
            {
                DrawInteractiveUntilities.DrawCurveOrBlockRef(offetEntity, canvas);
            }

            if (BasePoint != null)
            {
                DrawInteractiveUntilities.DrawInteractiveSpotLine(BasePoint, e.CurrentPoint, canvas);
            }
            //DrawInteractiveUntilities.DrawPositionMark(e.CurrentPoint,canvas);
        }

        private Entity CalculatorOffsetEntity(Entity selEntity, Point3D offsetPoint)
        {
            if (selEntity is ICurve selCurve)
            {
                double t;
                bool success = selCurve.Project(offsetPoint, out t);
                Point3D projectedPt = selCurve.PointAt(t);
                BasePoint = projectedPt;
                double offsetDist = 0;
                if (this.OffsetDistance != 0)
                {
                    var trackingLine = new Line(projectedPt, offsetPoint);
                    offsetPoint = trackingLine.PointAt(OffsetDistance);
                }
                offsetDist = projectedPt.DistanceTo(offsetPoint);
                ICurve offsetCurve = selCurve.Offset(offsetDist, Vector3D.AxisZ, 0.01, true);
                success = offsetCurve.Project(offsetPoint, out t);
                projectedPt = offsetCurve.PointAt(t);
                var disc = projectedPt.DistanceTo(offsetPoint);
                if (disc > 1e-3)
                    offsetCurve = selCurve.Offset(-offsetDist, Vector3D.AxisZ, 0.01, true);
                return offsetCurve as Entity;
            }

            if (selEntity is FramingRectangle2D framingRectangle2D)
            {
                double t;
                bool success = framingRectangle2D.Project(offsetPoint, out t);
                Point3D projectedPt = framingRectangle2D.PointAt(t);
                BasePoint = projectedPt;
                double offsetDist = 0;
                if (this.OffsetDistance != 0)
                {
                    var trackingLine = new Line(projectedPt, offsetPoint);
                    offsetPoint = trackingLine.PointAt(OffsetDistance);
                }
                offsetDist = projectedPt.DistanceTo(offsetPoint);

                var offsetJoist = framingRectangle2D.Offset(offsetDist, Vector3D.AxisZ, 0.01, true);
                success = offsetJoist.Project(offsetPoint, out t);
                projectedPt = offsetJoist.PointAt(t);
                if (projectedPt.DistanceTo(offsetPoint) > 1e-3)
                {
                    offsetJoist = framingRectangle2D.Offset(-offsetDist, Vector3D.AxisZ, 0.01, true);
                }
                return (Entity) offsetJoist;
            }


            return null;

        }

    }
}
