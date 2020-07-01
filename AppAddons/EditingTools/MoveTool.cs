using System;
using ApplicationService;
using AppModels.EventArg;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace AppAddons.EditingTools
{
    public sealed class MoveTool : EditingToolBase
    {
        public override string ToolName => "Move";

        //protected Point3D _startPoint;
        //protected Point3D _endPoint;

        public MoveTool() : base()
        {
            IsUsingLengthTextBox = true;
            IsUsingAngleTextBox = true;
        }

        
        [CommandMethod("Move")]
        public void Move()
        {
            OnProcessCommand();

        }

        //private void OnProcessCommand()
        //{
        //    var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
        //    if (WaitingForSelection)
        //    {
        //        var promptEntityOptions = new PromptEntityOptions("Please Select Entity for move");
        //        PromptEntityResult result = null;
        //        while (WaitingForSelection)
        //        {
        //            result = acDoc.Editor.GetEntities(promptEntityOptions);
        //            switch (result.Status)
        //            {
        //                case PromptStatus.Cancel:
        //                    return;
        //                case PromptStatus.OK when result.Entities.Count > 0:
        //                    WaitingForSelection = false;
        //                    break;
        //                case PromptStatus.None:
        //                    break;
        //                case PromptStatus.Error:
        //                    break;
        //                case PromptStatus.Keyword:
        //                    break;
        //                case PromptStatus.Modeless:
        //                    break;
        //                case PromptStatus.Other:
        //                    break;
        //                default:
        //                    throw new ArgumentOutOfRangeException();
        //            }
        //        }

        //        if (result != null)
        //        {
        //            this.SelectedEntities.AddRange(result.Entities);
        //        }


        //    }
        //    ToolMessage = "Please enter BasePoint to" + ToolName.ToLower();
        //    var promptPointOption = new PromptPointOptions("Please enter basePoint to " + ToolName.ToLower());
        //    var promptPointResult = acDoc.Editor.GetPoint(promptPointOption);
        //    if (promptPointResult.Status == PromptStatus.OK)
        //    {
        //        this._startPoint = promptPointResult.Value;
        //    }
        //    ToolMessage = "Please enter next point to " + ToolName.ToLower();
        //    promptPointOption.Message = "Please enter next point to " + ToolName.ToLower();
        //    promptPointResult = acDoc.Editor.GetPoint(promptPointOption);
        //    if (promptPointResult.Status == PromptStatus.OK)
        //    {
        //        this._endPoint = promptPointResult.Value;
        //    }
        //    this.ProcessEntities();
        //}
        //public override void OnJigging(object sender, DrawInteractiveArgs e)
        //{
        //    //this.DrawInteractiveCommand((ICadDrawAble)sender, e);
        //}
        protected override void ProcessEntities()
        {
            var movement = new Vector3D(_startPoint, _endPoint);
            foreach (var selEntity in this.SelectedEntities)
            {
                selEntity.Translate(movement);
                selEntity.Selected = false;
            }
            EntitiesManager.Refresh();
            this.SelectedEntities.Clear();
        }

        //public void Dispose()
        //{
        //    Application.Application.DocumentManager.MdiActiveDocument.Editor.UnRegisterDrawInteractive(this);
        //}

        //protected virtual void DrawInteractiveCommand(ICadDrawAble canvas, DrawInteractiveArgs drawInteractiveArgs)
        //{
            //if (this._selectedEntities.Count == 0)
            //{
            //    if (this._dynamicInput == null) return;
            //    if (this._waitingForSelection)
            //    {
            //        this.DynamicInputLabel = "Please select object to moving";
            //        this.DrawInteractiveSelection();
            //        return;
            //    }
            //}
            //else
            //{
            //    if (_waitingForSelection)
            //    {
            //        this.DrawInteractiveSelection();
            //        return;
            //    }

            //    if (_clickPoints.Count == 0)
            //    {
            //        this.DynamicInputLabel = "Please select base point";
            //    }
            //    else
            //    {
            //        this.DynamicInputLabel = "Please select second point to move";
            //        foreach (var selectedEntity in _selectedEntities)
            //        {
            //            Entity tempEntity = (Entity) selectedEntity.Clone();
            //            Vector3D vectorMove = new Vector3D(LastClickPoint, CurrentPoint);
            //            tempEntity.Translate(vectorMove);
            //            if (tempEntity is Text)
            //            {
            //                tempEntity.Regen(new RegenParams(0.0, this));
            //            }

            //            DrawCurveOrBlockRef(tempEntity);


            //        }

            //        DrawInteractiveSpotLine(this.LastClickPoint, this.CurrentPoint);

            //    }
            //}
        //}


    }

}
