using System;
using ApplicationInterfaceCore;
using ApplicationService;
using AppModels.EventArg;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;
using Environment = devDept.Eyeshot.Environment;

namespace AppAddons.EditingTools
{
    public abstract class EditingToolBase: SelectAbleToolBase
    {
        protected Point3D _startPoint;
        protected Point3D _endPoint;

        public EditingToolBase() :base()
        {

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
            ToolMessage = "Please enter BasePoint to" + ToolName.ToLower();
            this.IsSnapEnable = true;
            var promptPointOption = new PromptPointOptions("Please enter basePoint to " + ToolName.ToLower());
            var promptPointResult = acDoc.Editor.GetPoint(promptPointOption);
            switch (promptPointResult.Status)
            {
                case PromptStatus.OK:
                    this._startPoint = promptPointResult.Value;
                    this.BasePoint = promptPointResult.Value;
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
                    break;
                case PromptStatus.Cancel:
                    return;
            }
            this.ProcessEntities();
            IsSnapEnable = false;
        }
        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {
            this.DrawInteractiveCommand((ICadDrawAble)sender, e);
        }

        protected abstract void ProcessEntities();

        //public void Dispose()
        //{
        //    Application.Application.DocumentManager.MdiActiveDocument.Editor.UnRegisterDrawInteractive(this);
        //}

        protected virtual void DrawInteractiveCommand(ICadDrawAble canvas, DrawInteractiveArgs e)
        {
            if (WaitingForSelection)
            {
                return;
            }

            if (BasePoint == null)
            {
                return;
            }
            foreach (var selectedEntity in SelectedEntities)
            {
                Entity tempEntity = (Entity)selectedEntity.Clone();
                Vector3D vectorMove = new Vector3D(BasePoint, e.CurrentPoint);
                tempEntity.Translate(vectorMove);
                if (tempEntity is Text)
                {
                    tempEntity.Regen(new RegenParams(0.0, (Environment)canvas));
                }

                DrawInteractiveUntilities.DrawCurveOrBlockRef(tempEntity,canvas);


            }

            DrawInteractiveUntilities.DrawInteractiveSpotLine(BasePoint, e.CurrentPoint,canvas);

        }
    }
}
