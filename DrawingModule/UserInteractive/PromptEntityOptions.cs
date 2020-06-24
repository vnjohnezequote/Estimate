using System.Collections.Generic;
using ApplicationService;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.ViewModels;
using CanvasDrawing = DrawingModule.CustomControl.CanvasControl.CanvasDrawing;

namespace DrawingModule.UserInteractive
{
    public class PromptEntityOptions : PromptEditorOptions
    {
        public PromptEntityOptions():base()
        {

        }
        public PromptEntityOptions(string message) : base(message)
        {
        }
        

        protected internal override PromptResult DoIt(CanvasDrawing canvasDrawing, DynamicInputViewModel dynamicInput)
        {
            base.InitGet(dynamicInput);
            var entities = new List<Entity>();
            PromptStatus promptStatus = canvasDrawing.GetEntities(out string stringResult, out Point3D pickedPoint,entities);

            return new PromptEntityResult(promptStatus, stringResult,pickedPoint, entities);
        }
    }
}
