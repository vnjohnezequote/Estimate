using System.Collections.Generic;
using DrawingModule.Enums;
using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public abstract class CommandLineItemSearcher : HintSearcher
    {
        public override HintSearchResult DoSearch()
        {
            HintItem hundredPercentMatchItem = null;
            List<HintItem> listHintItem = new List<HintItem>();
            int num = 0;
            foreach (CommandLineItem commandLineItem in GetCommandLineItemCollection())
            {
                HintItem hintItem = new CommandSysvarHintItem(commandLineItem);
                listHintItem.Add(hintItem);
                if (num == 0 && HintNameComparer.Match(commandLineItem.Value, base.Context.UserInput, true) == HintMatchType.HundredPercentMatch)
                {
                    hundredPercentMatchItem = hintItem;
                }
            }
            return new HintSearchResult(this, listHintItem, hundredPercentMatchItem);
        }
        protected abstract IEnumerable<CommandLineItem> GetCommandLineItemCollection();
    }
}