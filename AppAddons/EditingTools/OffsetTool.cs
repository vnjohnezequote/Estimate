using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationInterfaceCore;
using ApplicationService;
using AppModels.EventArg;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace AppAddons.EditingTools
{
    public class OffsetTool : ToolBase
    {
        public override Point3D BasePoint { get; protected set; }
        public override string ToolName => "Offset";
        private bool _waitingForSelection;
        private Entity _selectedEntity;
        private Entity _offetEntity;
        
        private Point3D _offsetPoint;

        public OffsetTool()
        {

        }

        [CommandMethod("Offset")]
        public void Offset()
        {
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
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

            var promptPointOption = new PromptPointOptions(ToolMessage);
            Point3D dimPoint3D = null;
            var resultPoint = acDoc.Editor.GetPoint(promptPointOption);
            if (resultPoint.Status == PromptStatus.OK)
            {
                dimPoint3D = resultPoint.Value;
            }
            else
            {
                return;
            }

            ProcessOffsetEntity();
        }

        private void ProcessOffsetEntity()
        {
            if (_offetEntity !=null)
            {
                EntitiesManager.AddAndRefresh(_offetEntity,LayerManager.SelectedLayer.Name);
            }
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

            if (e.CurrentPoint==null)
            {
                return;
            }

            _offsetPoint = e.CurrentPoint;
            ICurve selCurve = _selectedEntity as ICurve;
            double t;
            bool success = selCurve.Project(e.CurrentPoint, out t);
            Point3D projectedPt = selCurve.PointAt(t);
            double offsetDist = projectedPt.DistanceTo(e.CurrentPoint);

            ICurve offsetCurve = selCurve.Offset(offsetDist, Vector3D.AxisZ, 0.01, true);
            success = offsetCurve.Project(e.CurrentPoint, out t);
            projectedPt = offsetCurve.PointAt(t);
            if (projectedPt.DistanceTo(e.CurrentPoint) > 1e-3)
                offsetCurve = selCurve.Offset(-offsetDist, Vector3D.AxisZ, 0.01, true);
            _offetEntity = offsetCurve as Entity;
            DrawInteractiveUntilities.DrawCurveOrBlockRef(_offetEntity,canvas);
        }

    }
}
