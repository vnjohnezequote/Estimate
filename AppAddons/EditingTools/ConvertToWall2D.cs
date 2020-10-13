using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels.CustomEntity;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.Application;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace AppAddons.EditingTools
{
    public class ConvertToWall2D: ToolBase
    {
        public override string ToolName => "Convert To WallLine2D";
        public List<Entity> SelectedEntities { get; protected set; }
        public bool WaitingForSelection { get; protected set; }
        public override Point3D BasePoint { get; protected set; }
        public ConvertToWall2D() : base()
        {
            SelectedEntities = new List<Entity>();
            WaitingForSelection = true;
            InitSelect();
        }
        [CommandMethod("ConvertWall2D")]
        public void ConvertWall2D()
        {
            OnProcessCommand();
        }

        private void InitSelect()
        {
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            var selectedEntities = acDoc.Editor.CanvasDrawing.GetEntities();
            if (selectedEntities.Count <= 0)
            {
                ToolMessage = "Please select Entities to " + ToolName.ToLower();
                IsSnapEnable = false;
                return;
            }
            WaitingForSelection = false;
            IsSnapEnable = true;
            SelectedEntities.AddRange(selectedEntities);
            ToolMessage = "Enter To excute " + ToolName.ToLower();
        }

        private void OnProcessCommand()
        {
            var acDocEditor = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument.Editor;
            while (true)
            {
                if (!PrepareSelectionSet(acDocEditor)) return;
                //if (!PrepairPoint(acDocEditor)) return;
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

        //protected virtual bool PrepairPoint(Editor editor)
        //{
        //    IsSnapEnable = true;
        //    IsUsingOrthorMode = true;
        //    ToolMessage = "Please press enter BasePoint to" + ToolName.ToLower();
        //    this.IsSnapEnable = true;
        //    var promptPointOption = new PromptPointOptions("Please press enter to " + ToolName.ToLower());
        //    var promptPointResult = editor.GetPoint(promptPointOption);
        //    switch (promptPointResult.Status)
        //    {
        //        case PromptStatus.OK:
        //            this.BasePoint = promptPointResult.Value;
        //            break;
        //        case PromptStatus.Cancel:
        //            return false;
        //    }
        //    return true;
        //}


        private void ProcessEntities()
        {
            foreach (var selectedEntity in SelectedEntities)
            {
                switch (selectedEntity)
                {
                    case LinearPath linePath:
                    {
                        var lines = linePath.ConvertToLines();
                        foreach (var line in lines)
                        {
                            EntitiesManager.AddAndRefresh(new WallLine2D(line),line.LayerName );
                        }
                        EntitiesManager.RemoveEntity(selectedEntity);
                            continue;
                    }
                    case Line line2:
                        EntitiesManager.AddAndRefresh(new WallLine2D(line2),line2.LayerName );
                        EntitiesManager.RemoveEntity(selectedEntity);
                        continue;
                }
            }
            EntitiesManager.EntitiesRegen();
        }
        private void ResetTool()
        {
            EntitiesManager.ResetSelection();
            this.SelectedEntities.Clear();
            this.WaitingForSelection = true;
            this.BasePoint = null;
        }

    }
}
