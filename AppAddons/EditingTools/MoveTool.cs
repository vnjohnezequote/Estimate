using System;
using ApplicationService;
using AppModels.Enums;
using AppModels.EventArg;
using AppModels.Undo;
using AppModels.Undo.Backup;
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
            var undoItem = new UndoList() { ActionType = ActionTypes.Edit };
            foreach (var selEntity in this.SelectedEntities)
            {
                var backup = BackupEntitiesFactory.CreateBackup(selEntity, undoItem, EntitiesManager);
                backup?.Backup();
                selEntity.Translate(movement);
                selEntity.Selected = false;
            }
            this.UndoEngineer.SaveSnapshot(undoItem);
            EntitiesManager.EntitiesRegen();
        }

    }

}
