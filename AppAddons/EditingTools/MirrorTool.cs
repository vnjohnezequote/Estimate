using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationInterfaceCore;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels.EventArg;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.Application;
using DrawingModule.CommandClass;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;
using OpenGL.Delegates;

namespace AppAddons.EditingTools
{
    public class MirrorTool : EditingToolBase
    {
        public override string ToolName => "Mirror";
        private List<Point3D> _clickPoints;
        private Point3D _startPoint;
        private Point3D _refPoint2;
        private Point3D _endPoint;
        private Plane _mirrorPlane;
        public MirrorTool(): base()
        {
            _clickPoints = new List<Point3D>();
        }
        [CommandMethod("Mirror")]
        public void Mirror()
        {
            OnProcessCommand();
        }

        protected override void ProcessEntities()
        {
            foreach (var selectedEntity in SelectedEntities)
            {
                var mirrorEntity = (Entity)selectedEntity.Clone();
                var mirror = new Mirror(_mirrorPlane);
                mirrorEntity.TransformBy(mirror);
                EntitiesManager.AddAndRefresh(mirrorEntity, mirrorEntity.LayerName);
            }

        }

        protected override bool PrepairPoint(Editor editor)
        {
            ToolMessage = "Please enter start refPoint to" + ToolName.ToLower();
            this.IsSnapEnable = true;
            var promptPointOption = new PromptPointOptions("Please enter basePoint to " + ToolName.ToLower());
            var promptPointResult = editor.GetPoint(promptPointOption);
            switch (promptPointResult.Status)
            {
                case PromptStatus.OK:
                    this._startPoint = promptPointResult.Value;
                    this._clickPoints.Add(_startPoint);
                    this.BasePoint = promptPointResult.Value;
                    IsUsingOrthorMode = true;
                    break;
                case PromptStatus.Cancel:
                    return false;
            }
            ToolMessage = "Please enter next point to " + ToolName.ToLower();
            promptPointOption.Message = "Please enter next point to " + ToolName.ToLower();
            promptPointResult = editor.GetPoint(promptPointOption);
            switch (promptPointResult.Status)
            {
                case PromptStatus.OK:
                    this._refPoint2 = promptPointResult.Value;
                    this._clickPoints.Add(_refPoint2);
                    break;
                case PromptStatus.Cancel:
                    return false;
            }

            return true;

        }
        protected override void ResetTool()
        {
            base.ResetTool();
            _clickPoints.Clear();
        }

        protected override void DrawInteractiveCommand(ICadDrawAble canvas, DrawInteractiveArgs e)
        {
            if (_clickPoints.Count == 0)
            {
                return;
            }
            if (e.CurrentPoint == null || this._clickPoints[0].Equals(e.CurrentPoint))
            {
                return;
            }
            Point3D p0 = null, p1 = null;
            
            // We need to have two reference points selected, might be snapped vertices
            
            

            DrawInteractiveUntilities.DrawInteractiveSpotLine(_clickPoints[0], e.CurrentPoint,canvas);
            p0 = _clickPoints[0];
            p1 = e.CurrentPoint;
            if (e.CurrentPoint.X < _clickPoints[0].X || e.CurrentPoint.Y < _clickPoints[0].Y)
            {
                Utility.Swap(ref p0, ref p1);

                //_clickPoints[0] = p0;
                //_clickPoints[1] = p1;
            }
            var axisX = new Vector3D(p0, p1);
            _mirrorPlane = new Plane(_clickPoints[0], axisX, Vector3D.AxisZ);

            foreach (var selectedEntity in SelectedEntities)
            {
                var mirrorEntity = (Entity)selectedEntity.Clone();
                var mirror = new Mirror(_mirrorPlane);
                mirrorEntity.TransformBy(mirror);
                DrawInteractiveUntilities.DrawCurveOrBlockRef(mirrorEntity,canvas);
            }
        }
    }
}
