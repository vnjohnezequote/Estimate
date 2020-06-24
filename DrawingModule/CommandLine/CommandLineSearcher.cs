using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public class CommandLineSearcher : CompositeHintSearcher
    {
        #region Fields

        private InternalCommandSearcher InternalCommandSearcher { get; set; }
        
        private bool IsLispCommandString(string userInput)
        {
            return userInput.IndexOfAny(new char[]
            {
                '(',
                ')',
                ' ',
                '"',
                '!'
            }) != -1;
        }

        #endregion

        public CommandLineSearcher()
        {
            this.InternalCommandSearcher = new InternalCommandSearcher();
            base.SubSearchers.Add(new CommandSysvarSearcher());
        }
        
        public override HintSearchResult DoSearch()
        {
            if (this.IsLispCommandString(base.Context.UserInput))
            {
                return HintSearchResult.CreateEmptySearchResult(this);
            }
            HintSearchResult hintSearchResult = this.InternalCommandSearcher.Search(base.Context);
            if (hintSearchResult.HasResult)
            {
                return HintSearchResult.CreateEmptySearchResult(this);
            }
            return base.DoSearch();
        }
    }
}