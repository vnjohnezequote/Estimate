using System;
using System.Runtime.InteropServices;
using DrawingModule.Application;
using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public class DocumentLockModeChangedEventArgs : System.EventArgs
    {
        private DocumentLockMode m_mycurrentmode;

        private DocumentLockMode m_mypreviousmode ;

        private DocumentLockMode m_currentmode;

        private Document m_document;

        private string m_globalcommandname;

        private bool m_veto = false;

        internal bool IsVetoed
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                return m_veto;
            }
        }

        public string GlobalCommandName => m_globalcommandname;

        public DocumentLockMode CurrentMode => m_currentmode;

        public DocumentLockMode MyCurrentMode => m_mycurrentmode;

        public DocumentLockMode MyPreviousMode => m_mypreviousmode;

        public Document Document => m_document;

        internal DocumentLockModeChangedEventArgs(DocumentLockMode mycurrentmode, DocumentLockMode mypreviousmode, DocumentLockMode currentmode, Document document, string globalcommandname)
        {
            this.m_mycurrentmode = mycurrentmode;
            this.m_mypreviousmode = mypreviousmode;
            this.m_currentmode = currentmode;
            this.m_document = document;
            this.m_globalcommandname = globalcommandname;
        }

        public void Veto()
        {
            m_veto = true;
        }
    }
}
