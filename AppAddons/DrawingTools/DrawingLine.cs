// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawingLine.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the CanvasDrawing type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Input;

using ApplicationService;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.Enums;
using DrawingModule.EventArgs;
using DrawingModule.Interface;
using DrawingModule.UserInteractive;
using DrawingModule.Views;

namespace AppAddons.DrawingTools
{
    /// <summary>
    /// The my model.
    /// </summary>
    public  class DrawingLine: ToolBase
    {
        //public DrawInteractiveDelegate DrawInteractiveHandler { get; private set; }
        public override string ToolName => "DrawingLine";
        private List<Point3D> _points = new List<Point3D>();
        public sealed override string ToolMessage => _points.Count == 0
            ? "Please enter start point. Escape to break tool"
            : "Please enter next point. Escape to break tool";

        public override Point3D BasePoint { get; protected set; }
        private PromptPointOptions promptPointOp { get; set; }
        #region Constructor

        public DrawingLine()
        {
           this.promptPointOp = new PromptPointOptions(ToolMessage);
           IsUsingOrthorMode = false;
           IsUsingLengthTextBox = true;
           IsUsingAngleTextBox = true;
        }
        #endregion
        [CommandMethod("Line")]
        public void DrawLine()
        {
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            //acDoc.Editor.FocusToLengthTextBox();
            DynamicInput?.FocusTextLength();
            while (true)
            {
                this.promptPointOp.Message = ToolMessage;
                var res = acDoc.Editor.GetPoint(this.promptPointOp);
                if (res.Status == PromptStatus.Cancel)
                {
                    //Application.DocumentManager.MdiActiveDocument.Editor.UnRegisterDrawInteractive(this);
                    return;
                }

                _points.Add(res.Value);
                if (_points.Count>0)
                {
                    var index = _points.Count - 1;
                    BasePoint = (Point3D)_points[index]; ;
                    IsUsingOrthorMode = true;
                }
                if (this._points.Count < 2) continue;
                var index2 = _points.Count - 1;
                var startPoint2 = (Point3D)_points[index2 - 1].Clone();
                var endPoint = (Point3D) _points[index2].Clone();
                var line = new Line(startPoint2,endPoint);
                acDoc.Editor.CanvasDrawing.AddAndRefresh(line);
                //DynamicInput?.FocusDynamicInputTextBox(this.DefaultDynamicInputTextBoxToFocus);
            }
        }
        #region Implement IDrawInteractive
        public override void NotifyMouseMove(object sender, MouseEventArgs e)
        {
            
            base.NotifyMouseMove(sender, e);
            if (DynamicInput == null) return;
            if (DynamicInput.PreviusDynamicInputFocus==FocusType.Length)
            {
                DynamicInput.FocusTextLength();
            }
            else
            {
                DynamicInput.FocusTextAngle();
            }

        }
        public override void NotifyPreviewKeyDown(object sender, KeyEventArgs e)
        {
            base.NotifyPreviewKeyDown(sender, e);
            switch (e.Key)
            {
                case Key.Tab:
                    OnMoveNextTab();
                    e.Handled = true;
                    break;
                default:
                    e.Handled = false;
                    break;
            }
            
        }
        protected virtual void OnMoveNextTab()
        {
            if (this.DynamicInput == null) return;
            switch (DynamicInput.PreviusDynamicInputFocus)
            {
                case FocusType.Length:
                    DynamicInput.FocusTextAngle();
                    break;
                case FocusType.Angle:
                    DynamicInput.FocusTextLength();
                    break;
            }
        }
        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {

            DrawInteractiveLine((ICadDrawAble)sender, e);
        }
        public void DrawInteractiveLine(ICadDrawAble drawTable, DrawInteractiveArgs e)
        {
            drawTable.renderContext.SetColorWireframe(Color.Blue);
            if (this._points.Count < 1) return;
            var index = this._points.Count - 1;
            var startPoint = drawTable.WorldToScreen(_points[index]);
            if (e.CurrentPoint == null)
            {
                return;
            }
            var endPoint = drawTable.WorldToScreen(e.CurrentPoint);
            drawTable.renderContext.DrawLine(startPoint, endPoint);

        }
        #endregion

    }
}
