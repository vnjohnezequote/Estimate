using DrawingModule.ViewModels;

namespace DrawingModule.UserInteractive
{
    public abstract class PromptEditorOptions : PromptOptions
    {
        #region Constructor

        protected internal PromptEditorOptions():base()
        {

        }
        protected internal PromptEditorOptions(string message) : base(message)
        {
            
        }

        private protected void InitGet(DynamicInputViewModel dynamicInput)
        {
            dynamicInput.NotifyToolChanged();   
        }


        #endregion
        }
}
