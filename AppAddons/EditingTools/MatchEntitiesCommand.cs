using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels.Factories;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace AppAddons.EditingTools
{
    public class MatchEntitiesCommand: ToolBase
    {
        public override Point3D BasePoint { get; protected set; }
        private bool _waitingForSelection;
        private Entity _entityToMatch;
        //private List<Entity> _entitiyToBeMatch;
        public override string ToolName => "Math Entities";
        public MatchEntitiesCommand(): base()
        {
            _waitingForSelection = true;
            EntityUnderMouseDrawingType = UnderMouseDrawingType.ByObject;
            IsUsingOrthorMode = false;
            IsSnapEnable = false;
            InitSelectAbleBase();
        }
        private void InitSelectAbleBase()
        {
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            var promptLineOption = new PromptSelectionOptions();
            _entityToMatch = acDoc.Editor.CanvasDrawing.GetSelectionEntity();
            if (_entityToMatch==null)
            {
                ToolMessage = "Please select Entities to " + ToolName.ToLower();
                return;
            }
            _waitingForSelection = false;
            ToolMessage = "Please Enter Entities To Be Matched";
        }

        [CommandMethod("Match")]
        public void MatchEntities()
        {
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            if (_entityToMatch==null)
            {
                ToolMessage = "Please select Entities to " + ToolName.ToLower();
                var promptLineOption = new PromptSelectionOptions();
                var result = acDoc.Editor.GetSelection(promptLineOption);
                if (result.Status == PromptStatus.OK)
                {
                    _entityToMatch = result.Value;
                }
                else
                {
                    return;
                }
            }
            
            while (true)
            {
                ToolMessage = "Please Select Entity To Be " + ToolName.ToLower();
                var promptEntityOptions = new PromptEntityOptions("Please Select Entity To Be " + ToolName.ToLower());
                var result2 = acDoc.Editor.GetEntities(promptEntityOptions);
                if (result2.Status == PromptStatus.OK && result2.Entities!=null && result2.Entities.Count!=0)
                {
                    ProcessCommand(result2.Entities);
                }
                else if(result2.Status == PromptStatus.OK && result2.Entities!=null && result2.Entities.Count==0)
                {
                    continue;
                }
                else
                {
                    return;
                }
            }

        }

        private void ProcessCommand(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                entity.LayerName = _entityToMatch.LayerName;
                entity.Selected = false;
            }
            EntitiesManager.Refresh();
            EntitiesManager.SelectedEntities.Clear();
        }
    }
}
