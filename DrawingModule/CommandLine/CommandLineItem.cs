using System.Drawing;
using System.Windows.Input;
using DrawingModule.CommandClass;

namespace DrawingModule.CommandLine
{
    public class CommandLineItem
    {
        public ICommand Command { get; set; }
        public string Value { get; set; }
        public string DisplayText { get; set; }
        public string UnderlyingCommand { get; set; }
        public string GlobalName { get; set; }
        public string ImageKey { get; set; }
        public string LocalName { get; set; }
        public int Flags { get; set; }
        public Image  Image { get; set; }

        public object ToolTip { get; set; }

        public CommandLineItem(CommandThunk commandThunk,string rawSearch)
        {
            this.Value = commandThunk.LocalName.ToUpper();
            this.DisplayText = $"{rawSearch}({commandThunk.GlobalName})";
            this.GlobalName = commandThunk.GlobalName;
            this.LocalName = commandThunk.LocalName;
            this.UnderlyingCommand = commandThunk.LocalName;
            this.ImageKey = commandThunk.LocalName;
            this.Command = commandThunk.Command;
        }

    }
}