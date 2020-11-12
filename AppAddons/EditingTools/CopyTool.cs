using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels.Enums;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.Application;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.EditingTools;
using DrawingModule.UserInteractive;

namespace AppAddons.EditingTools
{
    public class CopyTool : EditingToolBase
    {
        public override string ToolName => "Copy";

        public CopyTool() : base()
        {

        }
        [CommandMethod("Copy")]
        public void Copy()
        {
            OnProcessCommand();

        }
        protected override void ProcessEntities()
        {
           
            var movement = new Vector3D(_startPoint, _endPoint);
            foreach (var selEntity in this.SelectedEntities)
            {
                var cloneEntity = (Entity)selEntity.Clone();
                cloneEntity.Translate(movement);
                selEntity.Selected = false;
                EntitiesManager.AddAndRefresh(cloneEntity,cloneEntity.LayerName);
            }
            EntitiesManager.Refresh();
            IsSnapEnable = false;
        }

        protected override bool PrepairPoint(Editor editor)
        {
            IsSnapEnable = true;
            IsUsingOrthorMode = true;
            var promptPointOption = new PromptPointOptions("Please enter basePoint to " + ToolName.ToLower());
            PromptPointResult promptPointResult = null;
            if (this.BasePoint == null)
            {
                ToolMessage = "Please enter BasePoint to" + ToolName.ToLower();
                this.IsSnapEnable = true;
                promptPointResult = editor.GetPoint(promptPointOption);
                switch (promptPointResult.Status)
                {
                    case PromptStatus.OK:
                        this._startPoint = promptPointResult.Value;
                        this.BasePoint = promptPointResult.Value;
                        break;
                    case PromptStatus.Cancel:
                        return false;
                }
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

        protected override void ResetTool()
        {
            
        }
    }
}
