using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationInterfaceCore;
using ApplicationService;
using AppModels.EventArg;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;
using Environment = devDept.Eyeshot.Environment;

namespace AppAddons.EditingTools
{
    public class ScaleTool : SelectAbleToolBase
    {
        public override string ToolName => "Scale";
        public override Point3D BasePoint { get; protected set; }

        private Point3D _startPoint;
        private Point3D _refPoint2;
        private Point3D _endPoint;
        private List<Point3D> _clickPoints;
        private double _scaleFactor;

        public ScaleTool()
        {
            _clickPoints = new List<Point3D>();
        }
        [CommandMethod("Scale")]
        public void Scale()
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
            ToolMessage = "Please enter origin point to " + ToolName.ToLower();
            this.IsSnapEnable = true;
            var promptPointOption = new PromptPointOptions("Please enter origin point to " + ToolName.ToLower());
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
            ToolMessage = "Select first reference point to " + ToolName.ToLower();
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

            ToolMessage = "Select second reference point to " + ToolName.ToLower();
            promptPointResult = acDoc.Editor.GetPoint(promptPointOption);
            switch (promptPointResult.Status)
            {
                case PromptStatus.OK:
                    this._endPoint = promptPointResult.Value;
                    this._clickPoints.Add(_endPoint);
                    break;
                case PromptStatus.Cancel:
                    return;
            }

            this.ProcessEntities();
            IsSnapEnable = false;
        }

        private void ProcessEntities()
        {
            foreach (var selEntity in this.SelectedEntities)
            {
                selEntity.Scale(_clickPoints[0], _scaleFactor);
            }
            this.EntitiesManager.EntitiesRegen();
        }
        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {
            DrawInteractiveScale((ICadDrawAble)sender, e);
        }

        private void DrawInteractiveScale(ICadDrawAble canvas, DrawInteractiveArgs e)
        {
            if (_clickPoints.Count == 0)
            {
                return;
            }
            
            var worldToScreenVertices = _clickPoints.Select(point3D => canvas.WorldToScreen(point3D)).ToList();
            //renderContext.DrawLineStrip(worldToScreenVertices.ToArray());


            if (worldToScreenVertices.Count > 0)
            {
                canvas.renderContext.DrawLineStrip(new Point3D[]
                {
                    canvas.WorldToScreen(_clickPoints.First()),canvas.WorldToScreen(e.CurrentPoint)
                });
            }
            if (_clickPoints.Count < 2) return;
            _scaleFactor = _clickPoints[0].DistanceTo(e.CurrentPoint) / _clickPoints[0].DistanceTo(_clickPoints[1]);
            foreach (var selEntity in this.SelectedEntities)
            {
                Entity tempEntity = (Entity)selEntity.Clone();
                tempEntity.Scale(_clickPoints[0], _scaleFactor == 0 ? 1 : _scaleFactor);
                if (tempEntity is Text)
                {
                    tempEntity.Regen(new RegenParams(0, (Environment)canvas));

                }
                DrawInteractiveUntilities.DrawCurveOrBlockRef(tempEntity, canvas);
            }





        }

    }
}
