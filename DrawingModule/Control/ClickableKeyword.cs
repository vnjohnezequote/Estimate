using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace DrawingModule.Control
{
    public class ClickableKeyword: Button
    {
        public static string GetHighlightKeyword(DependencyObject obj)
		{
			return (string)obj.GetValue(ClickableKeyword.HighlightKeywordProperty);
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x000288DE File Offset: 0x000278DE
		public static void SetHighlightKeyword(DependencyObject obj, string value)
		{
			obj.SetValue(ClickableKeyword.HighlightKeywordProperty, value);
		}

		public string Keyword { get; set; }

		
		public string KeywordToHighlight { get; set; }

		private static void OnHighlightKeywordChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			TextBlock textBlock = sender as TextBlock;
			if (textBlock == null)
			{
				return;
			}
			string text = textBlock.Text;
			string text2 = e.NewValue as string;
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
			{
				return;
			}
			int num = text.IndexOf(text2);
			if (num < 0)
			{
				return;
			}
			int length = text2.Length;
			TextPointer textPointer = textBlock.ContentStart;
			while (textPointer.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
			{
				textPointer = textPointer.GetPositionAtOffset(1, LogicalDirection.Forward);
			}
			TextRange textRange = new TextRange(textPointer.GetPositionAtOffset(num), textPointer.GetPositionAtOffset(num + length));
			Color value = Color.FromRgb(0,0,0);
			textRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(value));
		}

		public static readonly DependencyProperty HighlightKeywordProperty = DependencyProperty.RegisterAttached("HighlightKeyword", typeof(string), typeof(ClickableKeyword), new PropertyMetadata(null, new PropertyChangedCallback(ClickableKeyword.OnHighlightKeywordChanged)));
    }
}
