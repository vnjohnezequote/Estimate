using DrawingModule.Interface;
using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public sealed class DataBindings
    {
        #region private field

        private CommandEditorManager mCommandEditorManager;
        private SystemVariables mSystemVariables;

        #endregion
        #region public properties

        public CommandEditorManager CommandEditorManager
        {
            get
            {
                if (this.mCommandEditorManager == null)
                {
                    return this.mCommandEditorManager = new CommandEditorManager();
                }

                return this.mCommandEditorManager;
            }
        }

        public ILookup<SystemVariable> SystemVariables
        {
            get
            {
                if (mSystemVariables == null)
                {
                    mSystemVariables = new SystemVariables();
                }
                return mSystemVariables;
            }
        }


        #endregion
    }
}