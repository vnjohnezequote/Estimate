using DrawingModule.Enums;
using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public class CommandSysvarSearcher : HintSearcher
    {
        private AutoCompleteSearcher AutoCompleteSearcher { get; set; }
        public CommandSysvarSearcher()
        {
            this.AutoCompleteSearcher = new AutoCompleteSearcher();
            
            
        }
        public override HintSearchResult DoSearch()
        {
            HintSearchResult hintSearchResult = HintSearchResult.CreateEmptySearchResult(this);
            
            hintSearchResult = this.AutoCompleteSearcher.Search(base.Context);
            
            
            return hintSearchResult;
        }

        public static CommandSysvarHintItem GetFirstHintItem(HintSearchResult hr)
        {
            if (hr == null || !hr.HasResult)
            {
                return null;
            }
            CommandSysvarHintItem commandSysvarHintItem = null;
            commandSysvarHintItem = (hr.HundredPercentMatch as CommandSysvarHintItem);
            if (commandSysvarHintItem != null)
            {
                return commandSysvarHintItem;
            }
            if (CommandLineSettings.Instance.SeparateSysvar)
            {
                foreach (HintItem hintItem in hr.Hints)
                {
                    if (hintItem.Type == HintCategoryType.Command)
                    {
                        commandSysvarHintItem = (hintItem as CommandSysvarHintItem);
                        break;
                    }
                }
                if (commandSysvarHintItem == null)
                {
                    commandSysvarHintItem = (hr.Hints[0] as CommandSysvarHintItem);
                }
            }
            else
            {
                commandSysvarHintItem = (hr.Hints[0] as CommandSysvarHintItem);
            }
            return commandSysvarHintItem;
        }
    }
}