using System;

namespace DrawingModule.CommandLine
{
    public class DocumentDestroyedEventArgs: System.EventArgs
    {
        private string m_filename;

        public string FileName => m_filename;

        internal DocumentDestroyedEventArgs(string filename)
        {
            this.m_filename = filename;
        }
    }
}
