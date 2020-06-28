using ApplicationInterfaceCore;
using DrawingModule.Interface;

namespace DrawingModule.DrawToolBase
{
    public class ToolChangedArgs
    {
        internal IDrawInteractive CurrentTool { get; }
        internal ToolChangedArgs(IDrawInteractive currentTool)
        {
            CurrentTool = currentTool;
        }
    }
}
