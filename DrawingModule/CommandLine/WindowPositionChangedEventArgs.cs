using System;

namespace DrawingModule.CommandLine
{
    public class WindowPositionChangedEventArgs: System.EventArgs
    {
        public WindowPositionChangedEventArgs(bool moved, bool sized)
        {
            this.Moved = moved;
            this.Sized = sized;
        }

        
        public bool Moved { get; private set; }

        
        public bool Sized { get; private set; }
    }
    
}
