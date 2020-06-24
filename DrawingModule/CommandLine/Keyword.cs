using System;
using System.Runtime.InteropServices;

namespace DrawingModule.CommandLine
{
    public sealed class Keyword
    {
        private string m_localName;

        private string m_globalName;

        private string m_displayName;

        private bool m_visible;

        private bool m_enabled;

        private bool m_readOnly = false;

        private KeywordCollection m_col;

        public bool Enabled
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                return m_enabled;
            }
            [param: MarshalAs(UnmanagedType.U1)]
            set
            {
                throwIfReadOnly();
                if (!value && m_col.GetDefaultKeyword() == this)
                {
                    throw new InvalidOperationException();
                }
                m_enabled = value;
            }
        }

        public bool Visible
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                return m_visible;
            }
            [param: MarshalAs(UnmanagedType.U1)]
            set
            {
                throwIfReadOnly();
                m_visible = value;
            }
        }

        public string DisplayName
        {
            get
            {
                return m_displayName;
            }
            set
            {
                throwIfReadOnly();
                m_displayName = value;
            }
        }

        public string GlobalName
        {
            get
            {
                return m_globalName;
            }
            set
            {
                throwIfReadOnly();
                m_globalName = value;
            }
        }

        public string LocalName
        {
            get
            {
                return m_localName;
            }
            set
            {
                throwIfReadOnly();
                m_localName = value;
            }
        }

        public bool IsReadOnly
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                return m_readOnly;
            }
            [param: MarshalAs(UnmanagedType.U1)]
            internal set
            {
                m_readOnly = true;
            }
        }

        private void throwIfReadOnly()
        {
            if (m_readOnly)
            {
                throw new InvalidOperationException();
            }
        }

        public Keyword(KeywordCollection parent)
        {
            this.m_col = parent;
        }
    }
}
