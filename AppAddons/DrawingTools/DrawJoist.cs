using System;
using System.Collections.ObjectModel;
using System.Linq;
using ApplicationService;
using AppModels.CustomEntity;
using AppModels.EntityCreator;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.NewReposiveData;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.Undo;
using AppModels.Undo.Backup;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;

namespace AppAddons.DrawingTools
{
    public class DrawJoist: DrawingLine
    {
        public override string ToolName => "Drawing Single Joist";
        public DrawJoist() : base()
        {
            //IsUsingInsideLength = true;
            //InsideLengthDistance = 90;
        }

        [CommandMethod("Joist")]
        public void DrawJoistCommand()
        {
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            DynamicInput?.FocusLength();
            while (true)
            {
                this.PromptPointOp.Message = ToolMessage;
                var res = acDoc.Editor.GetPoint(this.PromptPointOp);
                if (res.Status == PromptStatus.Cancel)
                {
                    return;
                }

                Points.Add(res.Value);
                if (Points.Count > 0)
                {
                    var index = Points.Count - 1;
                    BasePoint = (Point3D)Points[index]; ;
                    IsUsingOrthorMode = true;
                }
                if (this.Points.Count < 2) continue;
                var index2 = Points.Count - 1;
                var startPoint = (Point3D)Points[index2 - 1].Clone();
                var endPoint = (Point3D)Points[index2].Clone();
                if (this.JobModel.ActiveFloorSheet == null) continue;
                var framingType = FramingTypes.FloorJoist;
                if (JobModel.ActiveFloorSheet.FramingSheetType == FramingSheetTypes.RafterFraming)
                {
                    framingType = FramingTypes.RafterJoist;
                }
                var joistCreator = new Joist2DCreator(JobModel.ActiveFloorSheet, startPoint, endPoint,
                    framingType, JobModel.SelectedJoitsMaterial);
                joistCreator.GetFraming2D().ColorMethod = colorMethodType.byEntity;
                var undoList = new UndoList() { ActionType = ActionTypes.Add };
                var backup = BackupEntitiesFactory.CreateBackup((Joist2D)joistCreator.GetFraming2D(), undoList, EntitiesManager);
                backup?.Backup();
                UndoEngineer.SaveSnapshot(undoList);
                this.EntitiesManager.AddAndRefresh((Joist2D)joistCreator.GetFraming2D(), LayerManager.SelectedLayer.Name);
            }
        }

    }
}
