using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ApplicationInterfaceCore;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels.EventArg;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.Application;
using DrawingModule.CommandClass;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;
using Environment = devDept.Eyeshot.Environment;

namespace AppAddons.DrawingTools
{
    public class MTextTool: DrawTextBase
    {
        public override string ToolName => "Draw MText";
        public MTextTool (): base()
        {
            IsUsingMultilineTextBox = true;
        }
        [CommandMethod("MText")]
        public void DrawingMText()
        {
            while (true)
            {
                DynamicInput?.FocusDynamicInputTextBox(FocusType.TextContent);
                var acadEditor = Application.DocumentManager.MdiActiveDocument.Editor;
                var promptPointOption = new PromptPointOptions("Please enter Point to Insert Text");
                ToolMessage = "Please enter Point to Insert Text";
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
        protected override void OnMoveNextTab()
        {
            if (this.DynamicInput == null) return;
            switch (DynamicInput.PreviusDynamicInputFocus)
            {
                case FocusType.TextContent:
                    DynamicInput.FocusTextHeight();
                    break;
                case FocusType.TextHeight:
                    DynamicInput.FocusTextAngle();
                    break;
                case FocusType.TextAngle:
                    DynamicInput.FocusTextContent();
                    break;
            }
        }
        protected virtual void ProcessDrawEntities(Point3D insertPoint)
        {
            var text = new MultilineText(Plane.XY, insertPoint,TextInput, 10000, TextHeight,1.3*TextHeight);
            text.Contents = TextInput;
            var rad = Utility.DegToRad(TextAngle);
            text.Rotate(rad, Vector3D.AxisZ, insertPoint);
            EntitiesManager.AddAndRefresh(text, LayerManager.SelectedLayer.Name);
        }

        //public override void NotifyPreviewKeyDown(object sender, KeyEventArgs e)
        //{
        //    switch (e.Key)
        //    {
        //        case Key.Tab:
        //            OnMoveNextTab();
        //            e.Handled = true;
        //            break;
        //        default:
        //            e.Handled = false;
        //            break;
        //    }
        //}

        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {
            DrawInteractiveText((ICadDrawAble)sender, e);
        }

        public void DrawInteractiveText(ICadDrawAble canvas, DrawInteractiveArgs e)
        {
            if (string.IsNullOrEmpty(canvas.CurrentText) || e.CurrentPoint == null)
            {
                return;
            }
            var currentPoint = e.CurrentPoint;
            var text = new Text(currentPoint, TextInput, TextHeight);
            var rad = Utility.DegToRad(TextAngle);
            text.Rotate(rad, Vector3D.AxisZ, e.CurrentPoint);

            var tempText = text.ConvertToLinearPaths(0.01,(Environment)canvas);
            foreach (var curve in tempText)
            {
                DrawInteractiveUntilities.DrawCurveOrBlockRef(curve,canvas);
                //DrawInteractiveUntilities.Draw(curve, canvas);
            }

            if (BasePoint != null && e.CurrentPoint != null)
            {
                DrawInteractiveUntilities.DrawInteractiveSpotLine(BasePoint, e.CurrentPoint, canvas);
            }
        }
    }
}
