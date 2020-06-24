using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DrawingModule.Function;

namespace DrawingModule.ViewModels
{
    public class PromptOptions
    {
        #region Field

        #endregion

        #region Properties
        public string Message { get; set; }

        #endregion
        #region Constructor
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
