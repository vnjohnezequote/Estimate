using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using ApplicationInterfaceCore;
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
    public class DrawText : ToolBase
    {
        public override string ToolName => "Draw Text";
        public override Point3D BasePoint { get; protected set; }
        private string _drawText = "Test";
        private double _drawTextHeight = 100;
        private double _drawTextAngle = 0;

        public DrawText()
        {
            
        }

        [CommandMethod("Text")]
        public void DrawingText()
        {
            var acadEditor = Application.DocumentManager.MdiActiveDocument.Editor;

            var promptPointOption = new PromptPointOptions("Please enter Point to Insert Text");
            ToolMessage = "Please enter Point to Insert Text";
            var result = acadEditor.GetPoint(promptPointOption);
            if (result.Status == PromptStatus.Cancel) return;
            if (result.Status == PromptStatus.OK)
            {
                var insertPoint = result.Value.Clone() as Point3D;
               ProcessDrawEntities(insertPoint);
            }

        }

        private void ProcessDrawEntities(Point3D insertPoint)
        {
            var text = new Text(insertPoint, _drawText, _drawTextHeight);
            var rad = Utility.DegToRad(_drawTextAngle);
            text.Rotate(rad, Vector3D.AxisZ, insertPoint);
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

            var text = new Text(currentPoint, canvas.CurrentText, canvas.CurrentTextHeight);
            var rad = Utility.DegToRad(canvas.CurrentTextAngle);
            text.Rotate(rad, Vector3D.AxisZ, e.CurrentPoint);

            var tempText = text.ConvertToCurves((Environment)canvas);
            foreach (var curve in tempText)
            {
                DrawInteractiveUntilities.Draw(curve,canvas);
            }
        }
    }
}
