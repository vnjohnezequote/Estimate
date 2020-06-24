using System;
using DrawingModule.Application;

namespace DrawingModule.CommandLine
{
    public sealed class DocumentLockModeChangeVetoedEventArgs: System.EventArgs
    {
        private string m_globalcommandname;

        private Document m_document ;

        public string GlobalCommandName => m_globalcommandname;

        public Document Document => m_document;

        internal DocumentLockModeChangeVetoedEventArgs(string globalcommandname, Document document)
        {
            this.m_globalcommandname = globalcommandname;
            this.m_document = document;
        }
    }
}
