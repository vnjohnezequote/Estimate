using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.EditingTools;

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
                EntitiesManager.AddAndRefresh(cloneEntity,LayerManager.SelectedLayer.Name);
            }
            EntitiesManager.Refresh();
        }
    }
}
