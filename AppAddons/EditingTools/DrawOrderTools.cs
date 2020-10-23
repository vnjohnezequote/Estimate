using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationService;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace AppAddons.EditingTools
{
    public class DrawOrderTools: ToolBase
    {
        private Entity _selectedEntity;
        public DrawOrderTools()
        {
            
        }

        [CommandMethod("DrawOther")]
        public void DrawOrder()

        {
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            ToolMessage = "Please Select object to offset";
            var promptLineOption = new PromptSelectionOptions();
            var result = acDoc.Editor.GetSelection(promptLineOption);
            if (result.Status == PromptStatus.OK)
            {
                this._selectedEntity = result.Value;
            }
            else
            {
                return;
            }
            if (this._selectedEntity != null)
            {
                var index = EntitiesManager.Entities.IndexOf(_selectedEntity);
               EntitiesManager.RemoveEntity(_selectedEntity);
               EntitiesManager.AddAndRefresh(_selectedEntity,_selectedEntity.LayerName);
               //EntitiesManager.Insert(index - 1, _selectedEntity);

            }
        }
        public override Point3D BasePoint { get; protected set; }
    }
}
