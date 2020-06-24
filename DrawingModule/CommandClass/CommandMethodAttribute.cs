using System;
using DrawingModule.Enums;
using DrawingModule.Interface;

namespace DrawingModule.CommandClass
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed class CommandMethodAttribute: Attribute, ICommandLineCallable
    {
        #region Fields

        private string m_globalName;
        private string m_groupName;
        private CommandFlags m_flags;
        private string m_localizedNameId;
        private string m_helpTopic;
        private string m_helpFileName;
        private Type m_contextMenuExtensionType;

        #endregion

        #region Constructor

        public CommandMethodAttribute(string globalName)
        {
            this.m_groupName = null;
            this.m_globalName = globalName;
            this.m_localizedNameId = null;
            this.m_flags = CommandFlags.Modal;
            this.m_contextMenuExtensionType = null;
            this.m_helpFileName = null;
            this.m_helpTopic = null;
        }
        public CommandMethodAttribute(string globalName, CommandFlags flags)
        {
            this.m_groupName = null;
            this.m_globalName = globalName;
            this.m_localizedNameId = null;
            this.m_flags = flags;
            this.m_contextMenuExtensionType = null;
            this.m_helpFileName = null;
            this.m_helpTopic = null;
        }
        
        public CommandMethodAttribute(string groupName, string globalName, CommandFlags flags)
        {
            this.m_groupName = groupName;
            this.m_globalName = globalName;
            this.m_localizedNameId = null;
            this.m_flags = flags;
            this.m_contextMenuExtensionType = null;
            this.m_helpFileName = null;
            this.m_helpTopic = null;
        }
        public CommandMethodAttribute(string groupName, string globalName, string localizedNameId, CommandFlags flags)
        {
            this.m_groupName = groupName;
            this.m_globalName = globalName;
            this.m_localizedNameId = localizedNameId;
            this.m_flags = flags;
            this.m_contextMenuExtensionType = null;
            this.m_helpFileName = null;
            this.m_helpTopic = null;
        }

        
        public CommandMethodAttribute(string groupName, string globalName, string localizedNameId, CommandFlags flags, Type contextMenuExtensionType)
        {
            this.m_groupName = groupName;
            this.m_globalName = globalName;
            this.m_localizedNameId = localizedNameId;
            this.m_flags = flags;
            this.m_contextMenuExtensionType = contextMenuExtensionType;
            this.m_helpFileName = null;
            this.m_helpTopic = null;
        }

        
        public CommandMethodAttribute(string groupName, string globalName, string localizedNameId, CommandFlags flags, string helpTopic)
        {
            this.m_groupName = groupName;
            this.m_globalName = globalName;
            this.m_localizedNameId = localizedNameId;
            this.m_flags = flags;
            this.m_contextMenuExtensionType = null;
            this.m_helpFileName = null;
            this.m_helpTopic = helpTopic;
        }

        
        public CommandMethodAttribute(string groupName, string globalName, string localizedNameId, CommandFlags flags, Type contextMenuExtensionType, string helpFileName, string helpTopic)
        {
            this.m_groupName = groupName;
            this.m_globalName = globalName;
            this.m_localizedNameId = localizedNameId;
            this.m_flags = flags;
            this.m_contextMenuExtensionType = contextMenuExtensionType;
            this.m_helpFileName = helpFileName;
            this.m_helpTopic = helpTopic;
        }
        #endregion

        #region Properties

        public string GlobalName => this.m_globalName;
        public string LocalizedNameId => this.m_localizedNameId;
        public string GroupName => this.m_groupName;
        public CommandFlags Flags => this.m_flags;
        public string HelpFileName => this.m_helpFileName;
        public Type ContextMenuExtensionType => this.m_contextMenuExtensionType;

        #endregion

        
    }
}