using System.Windows.Media;
using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public class CommandSysvarHintItem : LazyLoadHintItem
    {
        public string UnderlyingCommand { get; set; }
        public string GlobalName { get; set; }
        public string LocalName { get; set; }
        public string ImageKey { get; }
        //public IntPtr ToolTipHandle { get; set; }
        
        public CommandSysvarHintItem(CommandLineItem item)
        {
            base.SupportOnlineSearch = true;
            base.Name = item.DisplayText;
            this.UnderlyingCommand = item.UnderlyingCommand;
            this.GlobalName = item.GlobalName;
            this.LocalName = item.LocalName;
            base.Value = item.Value;
            base.SearchHelp = AcHintItem.CmdSearchHintItemHelp;
            this.ImageKey = item.ImageKey;
            this.Command = item.Command;

        }
        
        protected override ImageSource CreateItemIcon()
        {
            //return CommandLineItem.ImageFromKey(this.ImageKey);
            return null;
        }
        protected override object CreateTooltip()
        {
            /*if (this.ToolTipHandle != IntPtr.Zero)
            {
                return Util.CreateCommandToolTip(this.ToolTipHandle);
            }*/
            return null;
        }
    }
}