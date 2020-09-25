// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawingLine.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the CanvasDrawing type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using ApplicationInterfaceCore;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels.CustomEntity;
using AppModels.EventArg;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace AppAddons.DrawingTools
{
    /// <summary>
    /// The my model.
    /// </summary>
    public  class DrawingLine: ToolBase
    {
        //public DrawInteractiveDelegate DrawInteractiveHandler { get; private set; }
        public override string ToolName => "Drawing Line";
        public List<Point3D> Points { get; set; } = new List<Point3D>();
        public sealed override string ToolMessage => Points.Count == 0
            ? "Please enter start point. Escape to break tool"
            : "Please enter next point. Escape to break tool";

        public override Point3D BasePoint { get; protected set; }
        public PromptPointOptions PromptPointOp { get; set; }
        #region Constructor

        public DrawingLine()
        {
           this.PromptPointOp = new PromptPointOptions(ToolMessage);
           IsUsingOrthorMode = false;
           IsUsingLengthTextBox = true;
           IsUsingAngleTextBox = true;
        }
        #endregion
        [CommandMethod("Line")]
        public void DrawLine()
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
                if (Points.Count>0)
                {
                    var index = Points.Count - 1;
                    BasePoint = (Point3D)Points[index]; ;
                    IsUsingOrthorMode = true;
                }
                if (this.Points.Count < 2) continue;
                var index2 = Points.Count - 1;
                var startPoint = (Point3D)Points[index2 - 1].Clone();
                var endPoint = (Point3D) Points[index2].Clone();
                var line = new Line(startPoint,endPoint);
                line.LineTypeMethod = colorMethodType.byLayer;
                {
                    if (LayerManager.SelectedLayer.LineTypeName!="Continues")
                    {
                        line.LineTypeName = LayerManager.SelectedLayer.LineTypeName;
                    }
                }
                line.Color = LayerManager.SelectedLayer.Color;
                var wall2D = new Wall2D(line);
                wall2D.WallLevelName = this.ActiveLevel;
                wall2D.LineTypeScale = 1;
                //wall2D.WallLevelName = "Level 1";
                this.EntitiesManager.AddAndRefresh(wall2D,this.LayerManager.SelectedLayer.Name);
            }
        }
        #region Implement IDrawInteractive
        public override void NotifyMouseMove(object sender, MouseEventArgs e)
        {
            DynamicInput?.FocusDynamicInputTextBox(FocusType.Previous);
            //if (DynamicInput.PreviusDynamicInputFocus==FocusType.Length)
            //{
            //    DynamicInput.FocusLength();
            //}
            //else
            //{
            //    DynamicInput.FocusAngle();
            //}

        }
        protected override void OnMoveNextTab()
        {
            if (this.DynamicInput == null) return;
            switch (DynamicInput.PreviusDynamicInputFocus)
            {
                case FocusType.Length:
                    DynamicInput.FocusAngle();
                    break;
                case FocusType.Angle:
                    DynamicInput.FocusLength();
                    break;
            }
        }
        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {

            DrawInteractiveLine((ICadDrawAble)sender, e);
        }
        public void DrawInteractiveLine(ICadDrawAble drawTable, DrawInteractiveArgs e)
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
            drawTable.renderContext.DrawLine(startPoint, endPoint);
            drawTable.renderContext.SetLineSize(1);
        }
        #endregion

    }
}
