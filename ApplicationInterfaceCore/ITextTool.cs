using System;

namespace ApplicationInterfaceCore
{
    public interface ITextTool
    {
        string TextInput { get; set; }
        double TextHeight { get; set; }
        double TextAngle { get; set; }
    }
}