using System.Collections.Generic;
using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public class CompositeHintSearcher : HintSearcher
    {
        public IList<HintSearcher> SubSearchers { get; set; }

        public CompositeHintSearcher()
        {
            this.SubSearchers = new List<HintSearcher>();
        }
        
        public override HintSearchResult DoSearch()
        {
            IList<HintSearchResult> results = this.CallSubSearchers();
            return this.CombineSearchResults(results);
        }
        

        protected IList<HintSearchResult> CallSubSearchers()
        {
            List<HintSearchResult> list = new List<HintSearchResult>();
            foreach (HintSearcher hintSearcher in this.SubSearchers)
            {
                HintSearchResult item = hintSearcher.Search(base.Context);
                list.Add(item);
            }
            return list;
        }
        protected HintSearchResult CombineSearchResults(IList<HintSearchResult> results)
        {
            HintItem hintItem = null;
            List<HintItem> list = new List<HintItem>();
            foreach (HintSearchResult hintSearchResult in results)
            {
                if (hintItem == null && hintSearchResult.HundredPercentMatch != null)
                {
                    hintItem = hintSearchResult.HundredPercentMatch;
                }
                list.AddRange(hintSearchResult.Hints);
            }
            return new HintSearchResult(this, list, hintItem);
        }
        
    }
}