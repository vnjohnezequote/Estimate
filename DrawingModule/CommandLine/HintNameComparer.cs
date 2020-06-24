using DrawingModule.Enums;

namespace DrawingModule.CommandLine
{
    public class HintNameComparer
    {
        internal static HintMatchType Match(string wholeWord, string matchWord, bool removePrefix)
        {
            if (removePrefix)
            {
                matchWord = matchWord.TrimStart(new char[]
                				{
                					'\'',
                					'_',
                					'"'
                				});
                wholeWord = wholeWord.TrimStart(new char[]
                				{
                					'\'',
                					'_',
                					'"'
                				});
            }
            wholeWord = wholeWord.ToUpper();
            matchWord = matchWord.ToUpper();
            if (wholeWord == matchWord)
            {
	            return HintMatchType.HundredPercentMatch;
            }
            if (wholeWord.StartsWith(matchWord))
            {
	            return HintMatchType.PrefixMatch;
            }
            if (wholeWord.Contains(matchWord))
            {
	            return HintMatchType.MidStringMatch;
            }
            return HintMatchType.NoMatch;
        }
    }
}