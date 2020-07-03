using System;
using ApplicationService;
using AppModels.EventArg;
using devDept.Geometry;
using DrawingModule.Application;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace AppAddons.EditingTools
{
    public sealed class MoveTool : EditingToolBase
    {
        public override string ToolName => "Move";

        public MoveTool() : base()
        {
        }
        [CommandMethod("Move")]
        public void Move()
        {
            OnProcessCommand();

        }
        protected override void ProcessEntities()
        {
            var movement = new Vector3D(_startPoint, _endPoint);
            foreach (var selEntity in this.SelectedEntities)
            {
                selEntity.Translate(movement);
                selEntity.Selected = false;
            }
            EntitiesManager.Refresh();
        }

    }

}
