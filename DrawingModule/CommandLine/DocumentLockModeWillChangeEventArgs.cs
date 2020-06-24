using System;
using DrawingModule.Application;
using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public sealed class DocumentLockModeWillChangeEventArgs: System.EventArgs
    {
        private DocumentLockMode m_mycurrentmode;

        private DocumentLockMode m_mynewmode;

        private DocumentLockMode m_currentmode;

        private Document m_document;

        private string m_globalcommandname ;

        public string GlobalCommandName => m_globalcommandname;

        public DocumentLockMode CurrentMode => m_currentmode;

        public DocumentLockMode MyCurrentMode => m_mycurrentmode;

        public DocumentLockMode MyNewMode => m_mynewmode;

        public Document Document => m_document;

        internal DocumentLockModeWillChangeEventArgs(DocumentLockMode mycurrentmode, DocumentLockMode mynewmode, DocumentLockMode currentmode, Document document, string globalcommandname)
        {
            this.m_mycurrentmode = mycurrentmode;
            this.m_mynewmode = mynewmode;
            this.m_currentmode = currentmode;
            this.m_document = document;
            this.m_globalcommandname = globalcommandname;
        }
    }
}
