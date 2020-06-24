using System;
using System.Windows;

namespace DrawingModule.CommandLine
{
    public class CommandLineDragEventArgs : System.EventArgs
    {
        public CommandLineDragEventArgs(bool isDocked, Window oldHost, Window newHost)
        {
            this.IsDocked = this.IsDocked;
            this.OldHost = oldHost;
            this.NewHost = newHost;
        }

       
        public bool IsDocked { get; private set; }

       
        public Window OldHost { get; private set; }

       
        public Window NewHost { get; private set; }
    }
}
