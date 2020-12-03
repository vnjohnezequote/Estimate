using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationService;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CustomControl.CanvasControl;
using DrawingModule.ViewModels;

namespace DrawingModule.UserInteractive
{
    public class PromptSelectionOptions : PromptEditorOptions
    {
        public PromptSelectionOptions() : base()
        {

        }
        public PromptSelectionOptions(string message) : base(message)
        {
        }

        protected internal override PromptResult DoIt(CanvasDrawing canvasDrawing, DynamicInputViewModel dynamicInput,bool isResetBeforeGet)
        {
            base.InitGet(dynamicInput);
            
            PromptStatus promptStatus = canvasDrawing.GetSelection(out var stringResult, out var pickedPoint, out var entity);

            return new PromptSelectionResult(promptStatus, stringResult, pickedPoint, entity);
        }
    }
}
