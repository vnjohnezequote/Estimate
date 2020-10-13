using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace DrawingModule.CommandLine
{
    public class PromptParser
    {
        private protected string m_prompt;

        private protected KeywordCollection m_keywords;

        internal KeywordCollection Keywords => m_keywords;

        internal string Prompt => m_prompt;

        internal void Parse(string promptAndKeywords, string globalKeywords)
        {
            //int num = promptAndKeywords.LastIndexOf("[");
            //int num2 = promptAndKeywords.LastIndexOf("]");
            //if (num >= 0 && num2 > num + 1)
            //{
            //    if (num > 0)
            //    {
            //        m_prompt = promptAndKeywords.Substring(0, num).TrimEnd(null);
            //    }
            //    else
            //    {
            //        m_prompt = "";
            //    }
            //    string[] array = promptAndKeywords.Substring(num + 1, num2 - num - 1).Split("/".ToCharArray());
            //    int num3 = array.Length;
            //    string[] array2 = new string[num3];
            //    int num4 = num3;
            //    if (num4 > 0)
            //    {
            //        int num5 = 0;
            //        if (0 < num4)
            //        {
            //            do
            //            {
            //                array2[num5] = ParseLocalKeyword(array[num5]);
            //                num5++;
            //            }
            //            while (num5 < num4);
            //        }
            //        string[] array3 = SplitByWhitespace(globalKeywords);
            //        if ((IntPtr)num4 == (IntPtr)(void*)array3.LongLength)
            //        {
            //            m_keywords = new KeywordCollection();
            //            int num6 = 0;
            //            if (0 < num4)
            //            {
            //                do
            //                {
            //                    m_keywords.Add(array3[num6], array2[num6], array[num6]);
            //                    num6++;
            //                }
            //                while (num6 < num4);
            //            }
            //            return;
            //        }
            //        throw new ArgumentException("Mismatched number of global and local keywords");
            //    }
            //    throw new ArgumentException("Bracketed keyword list is empty");
            //}
            //throw new ArgumentException("No bracketed keyword list");
        }

        private protected static string[] SplitByWhitespace(string text)
        {
            return text.Split(" \t\n\r".ToCharArray());
        }

        private protected static string LastBracketedString(string text, [MarshalAs(UnmanagedType.U2)] char startChar, [MarshalAs(UnmanagedType.U2)] char endChar)
        {
            int num = text.LastIndexOf(startChar);
            int num2 = text.LastIndexOf(endChar);
            if (num > 0)
            {
                int num3 = num + 1;
                if (num2 > num3)
                {
                    return text.Substring(num3, num2 - num - 1);
                }
            }
            return null;
        }

        private protected static string ParseLocalKeyword(string displayKeyword)
        {
            //Discarded unreachable code: IL_00f7
            string text = LastBracketedString(displayKeyword, '(', ')');
            if (text != (string)null)
            {
                return text;
            }
            string[] array = SplitByWhitespace(displayKeyword);
            IEnumerator enumerator = array.GetEnumerator();
            if (enumerator.MoveNext())
            {
                do
                {
                    string text2 = (string)enumerator.Current;
                    int num = 0;
                    if (0 >= text2.Length)
                    {
                        continue;
                    }
                    do
                    {
                        if (!char.IsUpper(text2[num]))
                        {
                            num++;
                            continue;
                        }
                        return text2;
                    }
                    while (num < text2.Length);
                }
                while (enumerator.MoveNext());
            }
            enumerator = array.GetEnumerator();
            if (enumerator.MoveNext())
            {
                do
                {
                    string text3 = (string)enumerator.Current;
                    if (char.IsDigit(text3[0]))
                    {
                        return text3;
                    }
                }
                while (enumerator.MoveNext());
            }
            enumerator = array.GetEnumerator();
            if (enumerator.MoveNext())
            {
                do
                {
                    string text4 = (string)enumerator.Current;
                    if (text4.Length == 1)
                    {
                        char c = text4[0];
                        if (!char.IsLetterOrDigit(c))
                        {
                            return array[0];
                        }
                    }
                }
                while (enumerator.MoveNext());
            }
            throw new ArgumentException("Couldn't parse local keyword");
        }
    }
}
