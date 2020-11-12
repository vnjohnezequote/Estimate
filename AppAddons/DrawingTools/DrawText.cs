using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ApplicationInterfaceCore;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels.Enums;
using AppModels.EventArg;
using AppModels.Interaface;
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
    public class DrawText : DrawTextBase
    {
        public override string ToolName => "Draw Text";

        public DrawText(): base()
        {
        }

        [CommandMethod("Text")]
        public void DrawingText()
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
            var text = new Text(insertPoint, TextInput, TextHeight);
            var rad = Utility.DegToRad(TextAngle);
            text.Rotate(rad, Vector3D.AxisZ, insertPoint);
            //text.ColorMethod = colorMethodType.byEntity;
            //text.ColorMethod = Color.FromArgb(127, Color.BlueViolet);
            EntitiesManager.AddAndRefresh(text,LayerManager.SelectedLayer.Name);
        }

        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {
            DrawInteractiveText((ICadDrawAble) sender, e);
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
                //DrawInteractiveUntilities.Draw(curve,canvas);
                DrawInteractiveUntilities.DrawCurveOrBlockRef(curve, canvas);
            }

            if (BasePoint!=null && e.CurrentPoint!=null)
            {
                DrawInteractiveUntilities.DrawInteractiveSpotLine(BasePoint,e.CurrentPoint,canvas);
            }
        }
    }
}
