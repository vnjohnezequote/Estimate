
using ApplicationService;
using AppModels.CustomEntity;
using AppModels.Enums;
using AppModels.EventArg;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings.Blocking;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.Application;
using DrawingModule.CommandClass;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.UserInteractive;

namespace AppAddons.DrawingTools
{
    public class DrawBlocking: DrawText
    {
        public override string TextInput
        {
            get => "B";
            set { }
        }
        public virtual double TextHeight
        {
            get=>225;
            set {}
        }
        [CommandMethod("Blocking")]
        public void DrawingBlocking()
        {
            while (true)
            {
                DynamicInput?.FocusDynamicInputTextBox(FocusType.TextContent);
                var acadEditor = Application.DocumentManager.MdiActiveDocument.Editor;
                var promptPointOption = new PromptPointOptions("Please enter Point to Insert Blocking");
                ToolMessage = "Please enter Point to Insert Blocking";
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
            if (this.JobModel.ActiveFloorSheet != null)
            {
                var blockRef = new Blocking(this.JobModel.ActiveFloorSheet);
                blockRef.FullLength = JobModel.ActiveFloorSheet.FramingSpacing;
                blockRef.BlockingType = BlockingTypes.DoubleBlocking;
                var blocking = new Blocking2D(insertPoint,blockRef);
                var rad = Utility.DegToRad(TextAngle);
                blocking.Rotate(rad, Vector3D.AxisZ, insertPoint);
                EntitiesManager.AddAndRefresh(blocking, LayerManager.SelectedLayer.Name);
            }
        }
        protected override void DrawInteractiveText(ICadDrawAble canvas, DrawInteractiveArgs e)
        {
            if (string.IsNullOrEmpty(canvas.CurrentText) || e.CurrentPoint == null)
            {
                return;
            }
            var currentPoint = e.CurrentPoint;

            var text = new Text(currentPoint, TextInput, TextHeight,Text.alignmentType.MiddleCenter);
            var rad = Utility.DegToRad(TextAngle);
            text.Rotate(rad, Vector3D.AxisZ, e.CurrentPoint);

            var tempText = text.ConvertToLinearPaths(0.01, (Environment)canvas);
            foreach (var curve in tempText)
            {
                DrawInteractiveUntilities.DrawCurveOrBlockRef(curve, canvas);
            }

            if (BasePoint != null && e.CurrentPoint != null)
            {
                DrawInteractiveUntilities.DrawInteractiveSpotLine(BasePoint, e.CurrentPoint, canvas);
            }
        }
    }
}
