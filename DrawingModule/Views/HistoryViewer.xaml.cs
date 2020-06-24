using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Shapes;
using DrawingModule.CommandLine;
using DrawingModule.Helper;
using DrawingModule.ViewModels;

namespace DrawingModule.Views
{
    /// <summary>
    /// Interaction logic for HistoryViewer.xaml
    /// </summary>
    public partial class HistoryViewer : UserControl, IComponentConnector
    {
        private bool mLeftButtonPressed;

	private double mHistoryLineHeight;

	public static readonly DependencyProperty HistorySourceProperty = DependencyProperty.Register("HistorySource", typeof(IList<CommandHistory>), typeof(HistoryViewer), new PropertyMetadata(null, OnHistorySourceChanged));

	private ScrollViewer mScrollViewer;

	private Rectangle mHiddenBar;

	internal HistoryViewer mUserControl;

    public IList<CommandHistory> HistorySource
	{
		get
		{
			return (IList<CommandHistory>)GetValue(HistorySourceProperty);
		}
		set
		{
			SetValue(HistorySourceProperty, value);
		}
	}

	public bool HasSelection
	{
		get
		{
			if (mDocumentViewer != null && mDocumentViewer.Selection != null)
			{
				return !string.IsNullOrEmpty(mDocumentViewer.Selection.Text);
			}
			return false;
		}
	}

	public string SelectionText
	{
		get
		{
			if (mDocumentViewer == null || mDocumentViewer.Selection == null)
			{
				return string.Empty;
			}
			return mDocumentViewer.Selection.Text;
		}
	}

	private static void OnHistorySourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		HistoryViewer historyViewer = sender as HistoryViewer;
		if (historyViewer != null)
		{
			INotifyCollectionChanged notifyCollectionChanged = e.OldValue as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				notifyCollectionChanged.CollectionChanged -= historyViewer.OnHistoryCollectionChanged;
			}
			notifyCollectionChanged = (e.NewValue as INotifyCollectionChanged);
			if (notifyCollectionChanged != null)
			{
				notifyCollectionChanged.CollectionChanged += historyViewer.OnHistoryCollectionChanged;
			}
			historyViewer.RepopulateHistory();
		}
	}

	public HistoryViewer()
	{
		InitializeComponent();
		base.Loaded += OnLoaded;
		base.IsVisibleChanged += OnIsVisibleChanged;
		base.SizeChanged += OnSizeChanged;
		mHistoryLineHeight = -1.0;
	}

	private void RepopulateHistory()
	{
		mParagraph.Inlines.Clear();
		if (HistorySource != null)
		{
			foreach (CommandHistory item in HistorySource)
			{
				if (!string.IsNullOrEmpty(item.Command))
				{
					AddOneLineAtLast(item.Command);
				}
			}
			if (mDocumentViewer.Selection != null && !mDocumentViewer.Selection.IsEmpty)
			{
				mDocumentViewer.Selection.Select(mDocumentViewer.Selection.Start, mDocumentViewer.Selection.Start);
			}
			ScrollToBottom();
		}
	}

	private void AddHistory(IList newItems)
	{
		if (newItems == null || newItems.Count == 0)
		{
			return;
		}
		for (int i = 0; i < newItems.Count; i++)
		{
			CommandHistory commandHistory = newItems[i] as CommandHistory;
			if (commandHistory != null && !string.IsNullOrEmpty(commandHistory.Command))
			{
				AddOneLineAtLast(commandHistory.Command);
			}
		}
	}

	private void RemoveHistory(IList oldItems)
	{
		if (oldItems == null || oldItems.Count == 0)
		{
			return;
		}
		for (int i = 0; i < oldItems.Count; i++)
		{
			CommandHistory commandHistory = oldItems[i] as CommandHistory;
			if (commandHistory != null && !string.IsNullOrEmpty(commandHistory.Command))
			{
				RemoveFirstLine();
			}
		}
	}

	private void AddOneLineAtLast(string strHistory)
	{
		if (mParagraph.Inlines.FirstInline != null)
		{
			mParagraph.Inlines.Add(new LineBreak());
		}
		mParagraph.Inlines.Add(new Run(strHistory));
	}

	private void RemoveFirstLine()
	{
		mParagraph.Inlines.Remove(mParagraph.Inlines.FirstInline);
		if (mParagraph.Inlines.FirstInline != null)
		{
			mParagraph.Inlines.Remove(mParagraph.Inlines.FirstInline);
		}
	}

	private void OnHistoryCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		switch (e.Action)
		{
		case NotifyCollectionChangedAction.Add:
			AddHistory(e.NewItems);
			break;
		case NotifyCollectionChangedAction.Remove:
			RemoveHistory(e.OldItems);
			break;
		case NotifyCollectionChangedAction.Reset:
			RepopulateHistory();
			break;
		}
		ScrollToBottom();
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		if (mScrollViewer != null)
		{
			return;
		}
		mScrollViewer = Utils.FindVisualChild<ScrollViewer>(mDocumentViewer);
		if (mScrollViewer != null)
		{
			if (mScrollViewer.Template != null)
			{
				mHiddenBar = (mScrollViewer.Template.FindName("PART_HiddenBar", mScrollViewer) as Rectangle);
			}
			if (mHiddenBar != null)
			{
				mScrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
			}
			UIElement uIElement = mScrollViewer.Template.FindName("PART_VerticalScrollBar", mScrollViewer) as UIElement;
			uIElement.AddHandler(ContentElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(ScrollViewerVerticalBar_ButtonEventHandler), handledEventsToo: true);
			uIElement.AddHandler(ContentElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(ScrollViewerVerticalBar_ButtonEventHandler), handledEventsToo: true);
			mDocument.AddHandler(ContentElement.MouseWheelEvent, new MouseWheelEventHandler(HistoryViewer_MouseWheel));
			base.Dispatcher.BeginInvoke(new Action(ScrollToBottom));
		}
	}

	private void HistoryViewer_MouseWheel(object sender, MouseWheelEventArgs e)
	{
		Application.Application.UiBindings.CommandEditorManager.ActiveEditor?.SyncHistoryViewList();
	}

	private void ScrollViewerVerticalBar_ButtonEventHandler(object sender, MouseButtonEventArgs e)
	{
		if (e.ChangedButton == MouseButton.Left)
		{
			if (e.ButtonState == MouseButtonState.Pressed)
			{
				mLeftButtonPressed = true;
			}
			else if (e.ButtonState == MouseButtonState.Released)
			{
				mLeftButtonPressed = false;
			}
		}
	}

	private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
	{
		double heightToCover = GetHeightToCover();
		if (heightToCover > 0.0)
		{
			mHiddenBar.Height = heightToCover;
		}
		if (mLeftButtonPressed)
		{
            Application.Application.UiBindings.CommandEditorManager.ActiveEditor?.SyncHistoryViewList();
		}
	}

	private double GetHeightToCover()
	{
		TextElement firstInline = mParagraph.Inlines.FirstInline;
		if (firstInline == null)
		{
			return 0.0;
		}
		TextPointer contentStart = firstInline.ContentStart;
		if (contentStart == null)
		{
			return 0.0;
		}
		double num = mHistoryLineHeight;
		if (mHistoryLineHeight < 1.0)
		{
			num = (mHistoryLineHeight = firstInline.ContentStart.GetCharacterRect(LogicalDirection.Forward).Height);
			int visibleHistoryItemsCount = (int)(mDocumentViewer.ActualHeight / mHistoryLineHeight);
			CommandEditor activeEditor = Application.Application.UiBindings.CommandEditorManager.ActiveEditor;
			if (activeEditor != null)
			{
				activeEditor.VisibleHistoryItemsCount = visibleHistoryItemsCount;
			}
		}
		if (num < 1.0)
		{
			return 0.0;
		}
		double num2 = mScrollViewer.VerticalOffset - 2.0;
		if (num2 <= 0.0)
		{
			return 0.0;
		}
		int num3 = (int)Math.Ceiling(num2 / num);
		return num * (double)num3 - num2;
	}

	private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		ScrollToBottom();
	}

	private void OnSizeChanged(object sender, SizeChangedEventArgs e)
	{
		ScrollToBottom();
		CommandEditor activeEditor = Application.Application.UiBindings.CommandEditorManager.ActiveEditor;
		if (activeEditor != null)
		{
			activeEditor.SyncHistoryViewList();
			if (mHistoryLineHeight > 0.0)
			{
				int num2 = activeEditor.VisibleHistoryItemsCount = (int)(mDocumentViewer.ActualHeight / mHistoryLineHeight);
			}
		}
	}

	private void ScrollToBottom()
	{
		if (mScrollViewer != null && base.IsVisible && !(base.ActualHeight < 1.0))
		{
			mScrollViewer.ScrollToEnd();
			mScrollViewer.ScrollToLeftEnd();
		}
	}
    }
}
