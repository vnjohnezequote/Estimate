using System;
using DrawingModule.Enums;
using DrawingModule.Interface;

namespace DrawingModule.CommandClass
{
    public class CommandPropertiesAttribute: Attribute, ICommandLineCallable
    {
        public string GlobalName { get; }
        public string LocalizedNameId { get; }
        public string GroupName { get; }
        public CommandFlags Flags { get; }
        public string HelpFileName { get; }
        public Type ContextMenuExtensionType { get; }
    }
}
