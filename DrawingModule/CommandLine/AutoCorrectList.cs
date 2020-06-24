using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public class AutoCorrectList
    {
        public AutoCorrectList(string rawSearch, AutoCorrectType searchType)
        {
            
        }
        public int Length { get; set; }

        public CommandLineItem this[int index]
        {
            get { return null; }
        }
    }
}