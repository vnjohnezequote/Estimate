using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public class HintSearchContext
    {
        #region Constructor

        public HintSearchContext(string userInput)
        {
            this.UserInput = userInput;
            this.FastSearch = false;
            this.ContextualSearch = true;
        }

        #endregion

        #region Public Properties
        
        public string UserInput { get; set; }
        public bool FastSearch { get; set; }
        public bool ContextualSearch { get; set; }
        public HintSearchResult CommandLineSearchResult { get; set; }
        public HintSearchResult AutoCompleteSearchResult { get; set; }
        public HintSearchResult InternalCommandSearchResult { get; set; }
        public HintSearchResult AutoCorrectSearchResult { get; set; }
        public HintSearchResult DwgContentSearchResult { get; set; }
        public bool AutoCorrectHasClearWinner { get; set; }
        public string RelativeCommandName { get; set; }
        public CommandSysvarHintItem FirstStartWithItemInAutoComplete { get; set; }
       

        #endregion

        #region Methods
        public bool HasResult(HintSearchResult hr)
        {
            return hr != null && hr.HasResult;
        }
        #endregion
    }
}