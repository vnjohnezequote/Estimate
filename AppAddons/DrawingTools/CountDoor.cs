using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels.CustomEntity;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Openings;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.Application;
using DrawingModule.CommandClass;
using DrawingModule.UserInteractive;

namespace AppAddons.DrawingTools
{
    public class CountDoor: DrawText
    {
        [CommandMethod("CountDoor")]
        public void DrawingCoutDoor()
        {
            while (true)
            {
                DynamicInput?.FocusDynamicInputTextBox(FocusType.TextContent);
                var acadEditor = Application.DocumentManager.MdiActiveDocument.Editor;
                var promptPointOption = new PromptPointOptions("Please enter Point to Insert Count Door");
                ToolMessage = "Please enter Point to Insert Count Door";
                var result = acadEditor.GetPoint(promptPointOption);
                if (result.Status == PromptStatus.Cancel) return;
                if (result.Status == PromptStatus.OK)
                {
                    var insertPoint = result.Value.Clone() as Point3D;
                    BasePoint = insertPoint;
                    ProcessDrawEntities(insertPoint);
                }
            }
        }
        protected override void ProcessDrawEntities(Point3D insertPoint)
        {
            LevelWall level = null;
            Opening newDoor = null;
            if (this.JobModel!=null)
            {
                foreach (var jobModelLevel in JobModel.Levels)
                {
                    if (jobModelLevel.LevelName == ActiveLevel)
                    {
                        level = jobModelLevel;
                    }
                }
            }

            if (level!=null)
            {
                var startDoorId = level.Openings.Count + 1;
                var door = new Opening(level.LevelInfo) { Id = startDoorId };
                level.AddOpening(door);
                var doorCount = new DoorCountEntity(insertPoint, "D" + door.Id.ToString(), TextHeight, ActiveLevel);
                doorCount.DoorReference = door;
                var rad = Utility.DegToRad(TextAngle);
                doorCount.Rotate(rad, Vector3D.AxisZ, insertPoint);
                //text.ColorMethod = colorMethodType.byEntity;
                //text.ColorMethod = Color.FromArgb(127, Color.BlueViolet);
                EntitiesManager.AddAndRefresh(doorCount, LayerManager.SelectedLayer.Name);
            }
           
        }

    }
}
