using System;

namespace DrawingModule.Enums
{
    [Flags]
    public enum CommandFlags
    {
        Modal = 0,
        Transparent = 1,
        UsePickSet = 2,
        Redraw = 4,
        NoPerspective = 8,
        NoMultiple = 16,
        NoTileMode = 32,
        NoPaperSpace = 64,
        NoOem = 256,
        Undefined = 512,
        InProgress = 1024,
        Defun = 2048,
        NoNewStack = 65536,
        NoInternalLock = 131072,
        DocReadLock = 524288,
        DocExclusiveLock = 1048576,
        Session = 2097152,
        Interruptible = 4194304,
        NoHistory = 8388608,
        NoUndoMarker = 16777216,
        NoBlockEditor = 33554432,
        NoActionRecording = 67108864,
        ActionMacro = 134217728,
        NoInferConstraint = 1073741824,
        TempShowDynDimension = -2147483648
    }
}