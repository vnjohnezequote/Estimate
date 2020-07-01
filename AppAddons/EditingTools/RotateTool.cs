
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
using DrawingModule.Application;
using DrawingModule.CommandClass;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;
using Environment = devDept.Eyeshot.Environment;

namespace AppAddons.EditingTools
{
    public class RotateTool : SelectAbleToolBase
    {
        public override Point3D BasePoint { get; protected set; }
        public override string ToolName => "Rotate";
        private Point3D _startPoint;
        private Point3D _refPoint2;
        private Point3D _endPoint;
        private double _arcSpanAngle;
        private List<Point3D> _clickPoints;
        public RotateTool() : base()
        {
            _clickPoints = new List<Point3D>();
        }
        [CommandMethod("Rotate")]
        public void Rotate()
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

            ToolMessage = "Please enter next point to " + ToolName.ToLower();
            promptPointOption.Message = "Please enter next point to " + ToolName.ToLower();
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
                selEntity.Rotate(_arcSpanAngle, Vector3D.AxisZ, _clickPoints[0]);
            }
            EntitiesManager.EntitiesRegen();
        }
        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {
            DrawInteractiveRotate((ICadDrawAble)sender, e);
        }

        private void DrawInteractiveRotate(ICadDrawAble canvas, DrawInteractiveArgs e)
        {
            if (WaitingForSelection)
            {
                return;
            }

            if (_clickPoints.Count ==0) return;
            
            DrawInteractiveUntilities.DrawInteractiveArc(canvas,_clickPoints,e.CurrentPoint, out _arcSpanAngle);
            // Show temp entities for current rotation state
            foreach (var ent in this.SelectedEntities)
            {
                Entity tempEntity = (Entity)ent.Clone();
                tempEntity.Rotate(_arcSpanAngle, Vector3D.AxisZ, _startPoint);

                if (tempEntity is Text)
                    tempEntity.Regen(new RegenParams(0, (Environment)canvas));
                canvas.renderContext.EnableXOR(true);
                DrawInteractiveUntilities.DrawCurveOrBlockRef(tempEntity,canvas);
                canvas.renderContext.EnableXOR(false);
                //this.Draw((ICurve)tempEntity); 
            }

            
                    

        }
    }
}
