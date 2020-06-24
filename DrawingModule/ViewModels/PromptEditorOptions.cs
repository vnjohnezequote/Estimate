using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DrawingModule.Function;

namespace DrawingModule.ViewModels
{
    public abstract class PromptEditorOptions : PromptOptions
    {
        #region Constructor
        protected internal PromptEditorOptions(string message) : base(message)
        {
            
        }

        private protected void InitGet(DynamicInputViewModel dynamicInput)
        {
            dynamicInput.ToolDescription = this.Message;
        }


        #endregion
        }
}
