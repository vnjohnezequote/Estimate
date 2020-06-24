namespace DrawingModule.CommandLine
{
    public class CCmdThroat
    {
        public void StuffKDB(char param1, int param2, bool param3, short param4)
        {
            
        }

        public void ConOut(string commandLine, bool param1,string command)
        {
            CommandEditor commandEditor;
            commandEditor = this.CommandLineEditor();
            if (commandEditor!=null && !string.IsNullOrEmpty(commandLine))
            {
                char c = commandLine[commandLine.Length - 1];
                commandEditor.PromptAndInput.Echo(c, command);
            }
        }

        private CommandEditor CommandLineEditor()
        {
            return Application.Application.UiBindings.CommandEditorManager.ActiveEditor;
        }
        public void StuffKeyBoardn(string newInput, int stringLength, int param1, bool param2, bool param3)
        {

        }

    }
}
