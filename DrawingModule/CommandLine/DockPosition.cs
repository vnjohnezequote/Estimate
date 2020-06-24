using System;

namespace DrawingModule.CommandLine
{
    [Flags]
    public enum DockPosition
    {
        None = 0x0,
        AnchoredTop = 0x401,
        AnchoredBottom = 0x404,
        AnchoredLeft = 0x402,
        AnchoredRight = 0x408,
        Anchored = 0x400,
        DockedInESW = 0x800,
        Floating = 0x1000
    }
}