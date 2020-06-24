using System;
using DrawingModule.Enums;

namespace DrawingModule.Interface
{
    public interface ICommandLineCallable
    {
        string GlobalName { get; }
        string LocalizedNameId { get; }
        string GroupName { get; }
        CommandFlags Flags { get; }
        string HelpFileName { get; }
        Type ContextMenuExtensionType { get; }
    }
}