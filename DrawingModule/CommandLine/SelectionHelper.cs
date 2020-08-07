using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DrawingModule.CommandLine
{
    // Token: 0x02000131 RID: 305
	internal static class SelectionHelper
	{
		// Token: 0x06000CBA RID: 3258 RVA: 0x0002B7BD File Offset: 0x0002A7BD
		public static bool GetKeepSelectionInView(DependencyObject obj)
		{
			return (bool)obj.GetValue(SelectionHelper.KeepSelectionInViewProperty);
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0002B7CF File Offset: 0x0002A7CF
		public static void SetKeepSelectionInView(DependencyObject obj, bool value)
		{
			obj.SetValue(SelectionHelper.KeepSelectionInViewProperty, value);
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0002B7E4 File Offset: 0x0002A7E4
		private static void OnKeepSelectionInViewChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			ListBox listBox = sender as ListBox;
			if (listBox == null)
			{
				return;
			}
			if ((bool)e.NewValue)
			{
				listBox.SelectionChanged += SelectionHelper.List_SelectionChanged;
				return;
			}
			listBox.SelectionChanged -= SelectionHelper.List_SelectionChanged;
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x0002B82F File Offset: 0x0002A82F
		private static void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems == null || e.AddedItems.Count == 0)
			{
				return;
			}
			((ListBox)sender).ScrollIntoView(e.AddedItems[0]);
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x0002B85E File Offset: 0x0002A85E
		public static bool GetHoverToSelect(DependencyObject obj)
		{
			return (bool)obj.GetValue(SelectionHelper.HoverToSelectProperty);
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x0002B870 File Offset: 0x0002A870
		public static void SetHoverToSelect(DependencyObject obj, bool value)
		{
			obj.SetValue(SelectionHelper.HoverToSelectProperty, value);
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0002B884 File Offset: 0x0002A884
		private static void OnHoverToSelectChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			ListBoxItem listBoxItem = sender as ListBoxItem;
			if (listBoxItem == null)
			{
				return;
			}
			if ((bool)e.NewValue)
			{
				listBoxItem.MouseMove += SelectionHelper.ListBoxItem_MouseMove;
				listBoxItem.MouseLeave += SelectionHelper.ListBoxItem_MouseLeave;
				return;
			}
			listBoxItem.MouseMove -= SelectionHelper.ListBoxItem_MouseMove;
			listBoxItem.MouseLeave -= SelectionHelper.ListBoxItem_MouseLeave;
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x0002B8F4 File Offset: 0x0002A8F4
		private static void ListBoxItem_MouseLeave(object sender, MouseEventArgs e)
		{
			ListBoxItem listBoxItem = sender as ListBoxItem;
			if (listBoxItem != null)
			{
				listBoxItem.Tag = null;
			}
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x0002B914 File Offset: 0x0002A914
		private static void ListBoxItem_MouseMove(object sender, MouseEventArgs e)
		{
			ListBoxItem listBoxItem = sender as ListBoxItem;
			if (listBoxItem == null || listBoxItem.IsSelected)
			{
				return;
			}
			if (listBoxItem.Tag == null)
			{
				listBoxItem.Tag = e.GetPosition(listBoxItem);
				return;
			}
			if (e.GetPosition(listBoxItem).Equals((Point)listBoxItem.Tag))
			{
				return;
			}
			listBoxItem.Tag = null;
			Point position = e.GetPosition(listBoxItem);
			Rect rect = new Rect(1.0, 1.0, listBoxItem.ActualWidth - 2.0, listBoxItem.ActualHeight - 2.0);
			if (rect.Contains(position))
			{
				listBoxItem.IsSelected = true;
			}
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x0002B9C5 File Offset: 0x0002A9C5
		public static bool GetClickToExecuteItem(DependencyObject obj)
		{
			return (bool)obj.GetValue(SelectionHelper.ClickToExecuteItemProperty);
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x0002B9D7 File Offset: 0x0002A9D7
		public static void SetClickToExecuteItem(DependencyObject obj, bool value)
		{
			obj.SetValue(SelectionHelper.ClickToExecuteItemProperty, value);
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x0002B9EC File Offset: 0x0002A9EC
		private static void OnClickToExecuteItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			ListBoxItem listBoxItem = sender as ListBoxItem;
			if (listBoxItem == null)
			{
				return;
			}
			if ((bool)e.NewValue)
			{
				listBoxItem.MouseLeftButtonUp += SelectionHelper.List_MouseLeftButtonUp;
				return;
			}
			listBoxItem.MouseLeftButtonUp -= SelectionHelper.List_MouseLeftButtonUp;
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x0002BA38 File Offset: 0x0002AA38
		private static void List_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			ListBoxItem listBoxItem = sender as ListBoxItem;
			if (listBoxItem == null)
			{
				return;
			}
			HintItem hintItem = listBoxItem.DataContext as HintItem;
			if (hintItem != null && hintItem.Command != null)
			{
				// need to fix this
				//hintItem.Command.Execute(hintItem);
			}
		}

		// Token: 0x0400044E RID: 1102
		public static readonly DependencyProperty KeepSelectionInViewProperty = DependencyProperty.RegisterAttached("KeepSelectionInView", typeof(bool), typeof(SelectionHelper), new PropertyMetadata(false, new PropertyChangedCallback(SelectionHelper.OnKeepSelectionInViewChanged)));

		// Token: 0x0400044F RID: 1103
		public static readonly DependencyProperty HoverToSelectProperty = DependencyProperty.RegisterAttached("HoverToSelect", typeof(bool), typeof(SelectionHelper), new PropertyMetadata(false, new PropertyChangedCallback(SelectionHelper.OnHoverToSelectChanged)));

		// Token: 0x04000450 RID: 1104
		public static readonly DependencyProperty ClickToExecuteItemProperty = DependencyProperty.RegisterAttached("ClickToExecuteItem", typeof(bool), typeof(SelectionHelper), new PropertyMetadata(false, new PropertyChangedCallback(SelectionHelper.OnClickToExecuteItemChanged)));
	}
}
