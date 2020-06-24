using System;

namespace DrawingModule.Enums
{
    [Flags]
    public enum InputSearchOptions
    {
        None = 0x0,
        AutoComplete = 0x1,
        AutoCorrect = 0x2,
        SysvarSearch = 0x4,
        ContentSearch = 0x8,
        MidString = 0x10
    }
}