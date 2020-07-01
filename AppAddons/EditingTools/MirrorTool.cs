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
using OpenGL.Delegates;

namespace AppAddons.EditingTools
{
    public class MirrorTool : SelectAbleToolBase
    {
        public override string ToolName => "Mirror";
        public override Point3D BasePoint { get; protected set; }
        private List<Point3D> _clickPoints;
        private Point3D _startPoint;
        private Point3D _refPoint2;
        private Point3D _endPoint;
        private Plane _mirrorPlane;
        public MirrorTool()
        {
            _clickPoints = new List<Point3D>();
        }
        [CommandMethod("Mirror")]
        public void Mirror()
        {
            OnProcessCommand();
        }

        protected virtual void OnProcessCommand()
        {
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            if (WaitingForSelection)
            {
                var promptEntityOptions = new PromptEntityOptions("Please Select Entity for " + ToolName.ToLower());
                PromptEntityResult result = null;
                while (WaitingForSelection)
                {
                    result = acDoc.Editor.GetEntities(promptEntityOptions);
                    switch (result.Status)
                    {
                        case PromptStatus.Cancel:
                            return;
                        case PromptStatus.OK when result.Entities.Count > 0:
                            WaitingForSelection = false;
                            break;
                        case PromptStatus.None:
                            break;
                        case PromptStatus.Error:
                            break;
                        case PromptStatus.Keyword:
                            break;
                        case PromptStatus.Modeless:
                            break;
                        case PromptStatus.Other:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (result != null)
                {
                    this.SelectedEntities.AddRange(result.Entities);
                }
            }
            ToolMessage = "Please enter startRefPoint to" + ToolName.ToLower();
            this.IsSnapEnable = true;
            var promptPointOption = new PromptPointOptions("Please enter basePoint to " + ToolName.ToLower());
            var promptPointResult = acDoc.Editor.GetPoint(promptPointOption);
            switch (promptPointResult.Status)
            {
                case PromptStatus.OK:
                    this._startPoint = promptPointResult.Value;
                    this._clickPoints.Add(_startPoint);
                    //this.BasePoint = promptPointResult.Value;
                    break;
                case PromptStatus.Cancel:
                    return;
            }
            ToolMessage = "Please enter next point to " + ToolName.ToLower();
            promptPointOption.Message = "Please enter next point to " + ToolName.ToLower();
            promptPointResult = acDoc.Editor.GetPoint(promptPointOption);
            switch (promptPointResult.Status)
            {
                case PromptStatus.OK:
                    this._refPoint2 = promptPointResult.Value;
                    this._clickPoints.Add(_refPoint2);
                    break;
                case PromptStatus.Cancel:
                    return;
            }

            this.ProcessEntities();
            IsSnapEnable = false;
        }

        private void ProcessEntities()
        {
            foreach (var selectedEntity in SelectedEntities)
            {
                var mirrorEntity = (Entity)selectedEntity.Clone();
                var mirror = new Mirror(_mirrorPlane);
                mirrorEntity.TransformBy(mirror);
                EntitiesManager.AddAndRefresh(mirrorEntity, mirrorEntity.LayerName);
            }

        }


        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {
            DrawInteractiveMirror((ICadDrawAble)sender, e);
        }

        private void DrawInteractiveMirror(ICadDrawAble canvas, DrawInteractiveArgs e)
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
