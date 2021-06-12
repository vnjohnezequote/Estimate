using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationService;
using AppModels;
using AppModels.CustomEntity;
using AppModels.Enums;
using AppModels.EventArg;
using AppModels.Interaface;
using AppModels.Undo;
using AppModels.Undo.Backup;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.UserInteractive;

namespace AppAddons.DrawingTools
{
    public class DrawJoistArrowName: DrawingLine
    {
        public override string ToolName => "Drawing Arrow Joist Line";

        public override string ToolMessage
        {
            get
            {
                if (Points.Count==0)
                {
                    return "Please enter start point. Escape to break tool";
                }
                else if(Points.Count == 1)
                {
                    return "Please enter end point. Escape to break tool";
                }
                else
                {
                    return "Please select Framing";
                }
            }
            
        }
            
        [CommandMethod("JoistArrow")]
        public void DrawJoistArrow()
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
                var line = new JoistArrowEntity(startPoint, endPoint);
                line.LineTypeMethod = colorMethodType.byEntity;
                line.LineWeightMethod = colorMethodType.byEntity;
                line.LineWeight = 0.5f;
                {
                    if (LayerManager.SelectedLayer.LineTypeName != "Continues")
                    {
                        line.LineTypeName = LayerManager.SelectedLayer.LineTypeName;
                    }
                }
                line.Color = LayerManager.SelectedLayer.Color;

                var isSelectedFraming = false;
                FramingRectangle2D selectEntity = null;
                while (isSelectedFraming == false)
                {
                    this.ToolMessage = "Please select Framing";
                    var promptSelectOption = new PromptSelectionOptions();
                    var result = acDoc.Editor.GetSelection(promptSelectOption);
                    if (result.Status == PromptStatus.OK)
                    {
                        if (result.Value is FramingRectangle2D framingRectangle)
                        {
                            selectEntity = framingRectangle;
                            isSelectedFraming = true;
                        }
                        else
                        {
                            ToolMessage = "Please select Framing Against";
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                var undoItem = new UndoList() { ActionType = ActionTypes.Add };
                var backup = BackupEntitiesFactory.CreateBackup(line, undoItem, EntitiesManager);
                backup?.Backup();
                this.GeneralFramingName(selectEntity.FramingReference,line,undoItem);
                
                this.UndoEngineer.SaveSnapshot(undoItem);
                this.EntitiesManager.AddAndRefresh(line, this.LayerManager.SelectedLayer.Name);
                return;
            }
        }
        private void GeneralFramingName(IFraming framingReference,JoistArrowEntity joistArrow,UndoList undoItem)
        {
            if (framingReference == null) return;
            var p0 = joistArrow.StartPoint;
            var p1 = joistArrow.EndPoint;
            if (p0.X > p1.X)
            {
                Utility.Swap(ref p0, ref p1);
            }
            else if (Math.Abs(p0.X - p1.X) < 0.00001 && p0.Y > p1.Y)
            {
                Utility.Swap(ref p0, ref p1);
            }
            var framingBaseLine = new Segment2D(p0, p1);
            framingBaseLine = framingBaseLine.Offset(-100);
            var initPoint = framingBaseLine.MidPoint.ConvertPoint2DtoPoint3D();
            var framingName = new FramingNameEntity(initPoint, framingReference.Name, 200,
                Text.alignmentType.BaselineCenter, framingReference);
            joistArrow.FramingName = framingName;
            joistArrow.FramingNameId = framingName.Id;
            Vector2D v = new Vector2D(p0, p1);
            var radian = v.Angle;
            framingName.Rotate(radian, Vector3D.AxisZ, framingName.InsertionPoint);
            //framingName.Color =;
            framingName.ColorMethod = colorMethodType.byEntity;
            var backup = BackupEntitiesFactory.CreateBackup(framingName, undoItem, EntitiesManager);
            backup?.Backup();
            EntitiesManager.AddAndRefresh(framingName, this.LayerManager.SelectedLayer.Name);
        }
        protected override void DrawInteractiveLine(ICadDrawAble drawTable, DrawInteractiveArgs e)
        {
            drawTable.renderContext.SetColorWireframe(LayerManager.SelectedLayer.Color);
            drawTable.renderContext.SetLineSize(LayerManager.SelectedLayer.LineWeight);
            //drawTable.renderContext.SetLineStipple();
            if (this.Points.Count < 1) return;
            var index = this.Points.Count - 1;
            var startPoint = drawTable.WorldToScreen(Points[index]);
            if (e.CurrentPoint == null)
            {
                return;
            }
            var endPoint = drawTable.WorldToScreen(e.CurrentPoint);
            if (Points.Count>=2)
            {
                drawTable.renderContext.SetLineSize(1);
                DrawInteractiveUntilities.DrawSelectionMark(drawTable,e.MousePosition);
            }
            else
            {
                drawTable.renderContext.DrawLine(startPoint, endPoint);
                drawTable.renderContext.SetLineSize(1);
            }
            
        }
    }
}
