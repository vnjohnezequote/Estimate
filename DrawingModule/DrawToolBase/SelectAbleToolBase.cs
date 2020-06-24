using System.Collections.Generic;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace DrawingModule.DrawToolBase
{
    public class SelectAbleToolBase : ToolBase
    {
        public List<Entity> SelectedEntities { get; protected set; }
        public bool WaitingForSelection { get; protected set; }
        public override Point3D BasePoint { get; protected set; }

        public SelectAbleToolBase()
        {
            SelectedEntities = new List<Entity>();
            WaitingForSelection = true;
            InitSelectAbleBase();
        }

        private void InitSelectAbleBase()
        {
            var acDoc = Application.Application.DocumentManager.MdiActiveDocument;
            var selectedEntities = acDoc.Editor.CanvasDrawing.GetEntities();
            if (selectedEntities.Count<= 0)
            {
                ToolMessage = "Please select Entities to " + ToolName.ToLower();
                IsSnapEnable = false;
                return;
            }
            WaitingForSelection = false;
            IsSnapEnable = true;
            SelectedEntities.AddRange(selectedEntities);
            ToolMessage = "Enter Base point to " + ToolName.ToLower();
        }

        
    }
}
