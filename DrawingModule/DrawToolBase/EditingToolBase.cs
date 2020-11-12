using System.Windows.Input;
using ApplicationInterfaceCore;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels.Enums;
using AppModels.EventArg;
using AppModels.Interaface;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.Application;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.UserInteractive;
using Environment = devDept.Eyeshot.Environment;

namespace DrawingModule.DrawToolBase
{
    public abstract class EditingToolBase: SelectAbleToolBase
    {
        protected Point3D _startPoint;
        protected Point3D _endPoint;

        public EditingToolBase() :base()
        {
            DefaultDynamicInputTextBoxToFocus = FocusType.Length;
        }

        protected virtual void OnProcessCommand()
        {
            var acDocEditor = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument.Editor;
            while (true)
            {
                if(!PrepareSelectionSet(acDocEditor)) return;
                if(!PrepairPoint(acDocEditor)) return;
                    this.ProcessEntities();
                    this.ResetTool();

            }
        }
        private bool PrepareSelectionSet(Editor editor)
        {
            if (!WaitingForSelection) return true;
            ToolMessage = "Please Select Entity for" + ToolName.ToLower();
            var promptEntityOptions = new PromptEntityOptions("Please Select Entity for " + ToolName.ToLower());
            PromptEntityResult result = null;
            while (WaitingForSelection)
            {
                result = editor.GetEntities(promptEntityOptions);
                switch (result.Status)
                {
                    case PromptStatus.Cancel:
                        return false;
                    case PromptStatus.OK when result.Entities.Count > 0:
                        WaitingForSelection = false;
                        break;
                    default:
                        return false;
                }
            }

            if (result != null)
            {
                this.SelectedEntities.AddRange(result.Entities);
                return true;
            }

            return true;
        }
        protected virtual bool PrepairPoint(Editor editor)
        {
            IsSnapEnable = true;
            IsUsingOrthorMode = true;
            ToolMessage = "Please enter BasePoint to" + ToolName.ToLower();
            this.IsSnapEnable = true;
            var promptPointOption = new PromptPointOptions("Please enter basePoint to " + ToolName.ToLower());
            var promptPointResult = editor.GetPoint(promptPointOption);
            switch (promptPointResult.Status)
            {
                case PromptStatus.OK:
                    this._startPoint = promptPointResult.Value;
                    this.BasePoint = promptPointResult.Value;
                    break;
                case PromptStatus.Cancel:
                    return false;
            }
            IsUsingLengthTextBox = true;
            IsUsingAngleTextBox = true;
            DynamicInput?.FocusDynamicInputTextBox(FocusType.Length);
            ToolMessage = "Please enter next point to " + ToolName.ToLower();
            promptPointOption.Message = "Please enter next point to " + ToolName.ToLower();
            promptPointResult = editor.GetPoint(promptPointOption);
            switch (promptPointResult.Status)
            {
                case PromptStatus.OK:
                    this._endPoint = promptPointResult.Value;
                    break;
                case PromptStatus.Cancel:
                    return false;
            }

            return true;
        }

        public override void NotifyMouseMove(object sender, MouseEventArgs e)
        {
            if (IsUsingAngleTextBox && IsUsingLengthTextBox)
            {
                DynamicInput?.FocusDynamicInputTextBox(FocusType.Previous);
            }
            
        }

        protected override void OnMoveNextTab()
        {
            if (this.DynamicInput == null) return;
            switch (DynamicInput.PreviusDynamicInputFocus)
            {
                case FocusType.Length:
                    DynamicInput.FocusAngle();
                    break;
                case FocusType.Angle:
                    DynamicInput.FocusLength();
                    break;
            }
        }

        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {
            this.DrawInteractiveCommand((ICadDrawAble)sender, e);
        }
        protected abstract void ProcessEntities();
        protected virtual void ResetTool()
        {
            EntitiesManager.ResetSelection();
            this.SelectedEntities.Clear();
            this.WaitingForSelection = true;
            this.BasePoint = null;
            IsUsingLengthTextBox = false;
            IsUsingAngleTextBox = false;
            IsSnapEnable = false;
            IsUsingOrthorMode = false;
        }
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

            if (e.CurrentPoint == null)
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
