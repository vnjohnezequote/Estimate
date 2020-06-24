using System.Windows.Input;
using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public class AcHintItem : HintItem
    {
        public static ICommand CmdExecuteHintItem { get; set; }
        public static ICommand CmdSearchHintItemHelp { get; set; }
        public static ICommand CmdSearchHintItemOnInternet { get; set; }

        public AcHintItem()
        {
            base.ShowImage = true;
            base.ShowToolTip = true;
            base.Command = CmdExecuteHintItem;
        }
    }
}