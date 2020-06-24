using System.Collections.Generic;

namespace DrawingModule.CommandLine
{
    public class CommandLineHistory
    {
        public List<string> CommandHistoryList;
        public CommandLineHistory()
        {
            CommandHistoryList = new List<string>();
        }

        public void AddLine(string commandHistory)
        {
            this.CommandHistoryList.Add(commandHistory);
        }
    }
}
