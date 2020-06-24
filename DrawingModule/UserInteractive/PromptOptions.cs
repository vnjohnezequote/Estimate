using System;
using DrawingModule.CustomControl;
using DrawingModule.ViewModels;
using CanvasDrawing = DrawingModule.CustomControl.CanvasControl.CanvasDrawing;

namespace DrawingModule.UserInteractive
{
    public class PromptOptions
    {
        #region Field

        #endregion

        #region Properties
        public string Message { get; set; }

        #endregion
        #region Constructor
        protected internal PromptOptions()
        {
            
        }
        protected internal PromptOptions(string message)
        {
            this.CommonInit(message);
            if (message == null)
            {
                throw new ArgumentException();
            }
        }


        #endregion

        #region private Method
        private void CommonInit(string message)
        {
            this.Message = message;
        }
        protected internal virtual PromptResult DoIt(CanvasDrawing canvas,DynamicInputViewModel dynamicInputViewModel)
        {
            return null;
        }


        #endregion
    }
}
