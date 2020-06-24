using System;
using DrawingModule.Application;

namespace DrawingModule.CommandLine
{
    public sealed class DocumentCollectionEventArgs:System.EventArgs
    {
        private Document m_document;

        public Document Document => m_document;

        internal DocumentCollectionEventArgs(Document doc)
        {
            this.m_document = doc;
        }
    }
}
