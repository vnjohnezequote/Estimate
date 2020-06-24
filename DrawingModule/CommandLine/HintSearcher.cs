using DrawingModule.Interface;
using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public abstract class HintSearcher : IHintSearcher
    {
        public HintSearchContext Context { get; protected set; }

        public HintSearchResult Search(HintSearchContext context)
        {
            if (context == null)
            {
                return HintSearchResult.CreateEmptySearchResult(this);
            }

            this.Context = context;
            if (this.Context.ContextualSearch && !this.ContextCheck())
            {
                return HintSearchResult.CreateEmptySearchResult(this);
            }

            return this.DoSearch();
        }

        protected virtual bool ContextCheck()
        {
            return true;
        }
        public abstract HintSearchResult DoSearch();
        
    }
}