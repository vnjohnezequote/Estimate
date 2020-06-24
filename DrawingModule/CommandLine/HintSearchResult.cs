using System.Collections.Generic;
using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public class HintSearchResult
    {
        #region Constructor

        protected HintSearchResult(HintSearcher searcher) : this(searcher, new List<HintItem>())
        {
        }
        public HintSearchResult(HintSearcher searcher, IList<HintItem> hints) : this(searcher, hints, null)
        {
        }
        public HintSearchResult(HintSearcher searcher, IList<HintItem> hints, HintItem hundredPercentMatchItem)
        {
            this.Searcher = searcher;
            this.Hints = hints;
            this.HundredPercentMatch = hundredPercentMatchItem;
        }

        #endregion

        #region Public Properties
        public bool HasError { get; private set; }
        public HintSearcher Searcher { get; private set; }
        public HintItem HundredPercentMatch { get; private set; }
        public int Count => this.Hints.Count;
        public bool HasResult => this.Count > 0;
        public IList<HintItem> Hints;

        #endregion

        #region Public Method

        public static HintSearchResult CreateEmptySearchResult(HintSearcher searcher)
        {
            return new HintSearchResult(searcher);
        }
        
        public static HintSearchResult CreateErrorSearchResult(HintSearcher searcher)
        {
            return new HintSearchResult(searcher)
            {
                HasError = true
            };
        }

        #endregion
    }
}