using System.Collections.Generic;
using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public class AutoCompleteSearcher : CommandLineItemSearcher
    {
        public override HintSearchResult DoSearch()
        {
            HintSearchResult result = base.Context.AutoCompleteSearchResult = base.DoSearch();
            return result;
        }
        protected override IEnumerable<CommandLineItem> GetCommandLineItemCollection()
        {
            AutoCompleteList autoComplete = new AutoCompleteList(base.Context.UserInput, CommandCompletionType.Command);
            int num;
            for (int index = 0; index < autoComplete.Length; index = num + 1)
            {
                yield return autoComplete[index];
                num = index;
            }
        }
        
    }
}