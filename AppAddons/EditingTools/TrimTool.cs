using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationInterfaceCore;
using ApplicationService;
using AppModels.EventArg;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace AppAddons.EditingTools
{
    public class TrimTool : SelectAbleToolBase
    {
        public override Point3D BasePoint { get; protected set; }
        public override string ToolName => "Trim";
        private Entity _trimedEntity;
        private Point3D _trimPoint;
        public TrimTool()
        {

        }

        [CommandMethod("trim")]
        public void Trim()
        {
            OnProcessCommand();
        }

        protected virtual void OnProcessCommand()
        {
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            if (WaitingForSelection)
            {
                var promptEntityOptions = new PromptEntityOptions("Please Select Entity for " + ToolName.ToLower());
                ToolMessage = "Please select trim entity";
                PromptEntityResult result = null;
                while (WaitingForSelection)
                {
                    result = acDoc.Editor.GetEntities(promptEntityOptions);
                    switch (result.Status)
                    {
                        case PromptStatus.Cancel:
                            return;
                        case PromptStatus.OK when result.Entities.Count > 0:
                            WaitingForSelection = false;
                            break;
                        case PromptStatus.None:
                            break;
                        case PromptStatus.Error:
                            break;
                        case PromptStatus.Keyword:
                            break;
                        case PromptStatus.Modeless:
                            break;
                        case PromptStatus.Other:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (result != null)
                {
                    this.SelectedEntities.AddRange(result.Entities);
                }


            }
            ToolMessage = "Please Select Entity to be Trim";
            var promptLineOption = new PromptSelectionOptions();
            var resultEnt = acDoc.Editor.GetSelection(promptLineOption);
            if (resultEnt.Status == PromptStatus.OK)
            {
                this._trimedEntity = resultEnt.Value;
                _trimPoint = resultEnt.ClickedPoint;
            }
            else
            {
                return;
            }


            this.ProcessTrimedEntities();
            IsSnapEnable = false;
        }

        private void ProcessTrimedEntities()
        {
            
        }


        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {
            DrawInteractiveTrim((ICadDrawAble)sender,e);

        }

        private void DrawInteractiveTrim(ICadDrawAble canvas, DrawInteractiveArgs e)
        {




        }
    }
}
