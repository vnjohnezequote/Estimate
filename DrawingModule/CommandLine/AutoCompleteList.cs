using System.Collections.Generic;
using DrawingModule.Application;
using DrawingModule.CommandClass;
using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public class AutoCompleteList
    {
        private List<CommandLineItem> mCommandLineItems;
        public AutoCompleteList(string rawSearch, CommandCompletionType searchType)
        {
            mCommandLineItems = new List<CommandLineItem>();
            this.Initialize(rawSearch,searchType);
        }

        public CommandLineItem this[int index]
        {
            get
            {
                if (index<this.Length && index>=0)
                {
                    return this.mCommandLineItems[index];
                }
                return null;
            }
        }

        private void Initialize(string rawSearch, CommandCompletionType searchType)
        {
            if (ApplicationHolder.CommandClass.CommandThunks.Count>0)
            {
                foreach (CommandThunk commandThunk in ApplicationHolder.CommandClass.CommandThunks)
                {
                    if (commandThunk.GlobalName.StartsWith(rawSearch,true,null))
                    {
                        CommandLineItem commandLineItem = new CommandLineItem(commandThunk,rawSearch);
                        this.mCommandLineItems.Add(commandLineItem);
                    }
                }
            }
        }
        
        

        public int Length => this.mCommandLineItems.Count;

        public void Sort()
        {
            
        }
    }
}