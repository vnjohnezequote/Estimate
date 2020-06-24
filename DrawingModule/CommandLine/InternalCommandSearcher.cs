using System.Collections.Generic;
using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public class InternalCommandSearcher : CommandLineItemSearcher
    {
        public override HintSearchResult DoSearch()
        {
            return base.Context.InternalCommandSearchResult = base.DoSearch();
        }

        protected override IEnumerable<CommandLineItem> GetCommandLineItemCollection()
        {
            AutoCorrectList internalCmds = new AutoCorrectList(base.Context.UserInput,AutoCorrectType.InternalCommand);
            
                int num;
                for (int index = 0; index < internalCmds.Length; index = num + 1)
                {
                    yield return internalCmds[index];
                    num = index;
                }
        }
    }
}