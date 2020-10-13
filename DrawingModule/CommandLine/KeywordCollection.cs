using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public sealed class KeywordCollection : ICollection, IEnumerable
    {
        private ArrayList m_imp;

        private Keyword m_default;

        bool ICollection.IsSynchronized
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                return false;
            }
        }
        object ICollection.SyncRoot => null;

        public Keyword this[int index] => (Keyword)m_imp[index];

        public string Default
        {
            get
            {
                return m_default?.GlobalName;
            }
            set
            {
                int num = 0;
                if (0 < m_imp.Count)
                {
                    do
                    {
                        Keyword keyword = this[num];
                        if (!keyword.Enabled || string.CompareOrdinal(keyword.GlobalName, value) != 0)
                        {
                            num++;
                            continue;
                        }
                        m_default = keyword;
                        return;
                    }
                    while (num < m_imp.Count);
                }
                throw new ArgumentException();
            }
        }

        public bool IsReadOnly
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                return m_imp.IsReadOnly;
            }
        }

        public int Count => m_imp.Count;

        internal string GetInteropString()
        {
            if (Count == 0)
            {
                return null;
            }
            StringBuilder stringBuilder = new StringBuilder();
            int num = 0;
            if (0 < Count)
            {
                do
                {
                    Keyword keyword = this[num];
                    if (keyword.Enabled)
                    {
                        stringBuilder.Append(keyword.LocalName);
                        stringBuilder.Append(" ");
                    }
                    num++;
                }
                while (num < Count);
            }
            stringBuilder.Append("_ ");
            int num2 = 0;
            if (0 < Count)
            {
                do
                {
                    Keyword keyword2 = this[num2];
                    if (keyword2.Enabled)
                    {
                        stringBuilder.Append(keyword2.GlobalName);
                    }
                    if (num2 < Count - 1)
                    {
                        stringBuilder.Append(" ");
                    }
                    num2++;
                }
                while (num2 < Count);
            }
            return stringBuilder.ToString();
        }

        public KeywordCollection()
        {
            m_imp = new ArrayList();
        }

        

        internal Keyword GetDefaultKeyword()
        {
            return m_default;
        }

        public IEnumerator GetEnumerator()
        {
            return m_imp.GetEnumerator();
        }

        public void Add(string globalName)
        {
            Add(globalName, globalName);
        }

        public void Add(string globalName, string localName)
        {
            Add(globalName, localName, localName);
        }

        public void Add(string globalName, string localName, string displayName)
        {
            Add(globalName, localName, displayName, visible: true, enabled: true);
        }

        public void Add(string globalName, string localName, string displayName, [MarshalAs(UnmanagedType.U1)] bool visible, [MarshalAs(UnmanagedType.U1)] bool enabled)
        {
            Keyword keyword = new Keyword(this);
            keyword.GlobalName = globalName;
            keyword.LocalName = localName;
            keyword.DisplayName = displayName;
            keyword.Visible = visible;
            keyword.Enabled = enabled;
            m_imp.Add(keyword);
        }

        public void Clear()
        {
            m_default = null;
            m_imp.Clear();
        }

        private void CopyTo(Array array, int index)
        {
            int num = 0;
            if (0 < array.Length)
            {
                do
                {
                    array.SetValue(this[num], num + index);
                    num++;
                }
                while (num < array.Length);
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this.CopyTo(array, index);
        }

        public void CopyTo(Keyword[] array, int index)
        {
            //int num = 0;
            //if (0L < (long)(IntPtr)(void*)array.LongLength)
            //{
            //    do
            //    {
            //        array[num + index] = this[num];
            //        num++;
            //    }
            //    while ((long)num < (long)(IntPtr)(void*)array.LongLength);
            //}
        }

        public string GetDisplayString([MarshalAs(UnmanagedType.U1)] bool showNoDefault)
        {
            if (Count == 0)
            {
                return null;
            }
            bool flag = false;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[");
            int num = 0;
            if (0 < Count)
            {
                do
                {
                    Keyword keyword = this[num];
                    if (keyword.Visible && keyword.Enabled && string.CompareOrdinal(keyword.GlobalName, "dummy") != 0)
                    {
                        flag = true;
                        stringBuilder.Append(keyword.DisplayName);
                        int num2 = num + 1;
                        if (num + 1 < Count)
                        {
                            do
                            {
                                Keyword keyword2 = this[num2];
                                if (!keyword2.Visible || !keyword2.Enabled || string.CompareOrdinal(keyword2.GlobalName, "dummy") == 0)
                                {
                                    num2++;
                                    continue;
                                }
                                stringBuilder.Append("/");
                                break;
                            }
                            while (num2 < Count);
                        }
                    }
                    num++;
                }
                while (num < Count);
                if (flag)
                {
                    stringBuilder.Append("]");
                    if (!showNoDefault && m_default != null)
                    {
                        stringBuilder.Append(" <");
                        stringBuilder.Append(m_default.DisplayName);
                        stringBuilder.Append(">");
                    }
                    return stringBuilder.ToString();
                }
            }
            return null;
        }
    }


}
