using System;
using System.Windows;
using System.Windows.Input;
using DrawingModule.ViewModels;
using Syncfusion.Windows.Controls;

namespace DrawingModule.Control
{
    internal delegate void BeginResizeEventHandler(object sender, BeginResizeEventArgs e);
    public class ResizeThumb : System.Windows.Controls.Control
    {
        public static readonly DependencyProperty ResizeTargetProperty;

	public static readonly DependencyProperty ResizeDirectionProperty;

	public static readonly RoutedEvent BeginResizeEvent;

	public static readonly RoutedEvent EndResizeEvent;

	private bool mDragging;

	private Size mMinSize;

	private Size mMaxSize;

	private Size mInitSize;

	private Point mInitPosition;

	private Direction mDirection;

	private FrameworkElement mTarget;

	public object ResizeTarget
	{
		get
		{
			return GetValue(ResizeTargetProperty);
		}
		set
		{
			SetValue(ResizeTargetProperty, value);
		}
	}

	public Direction ResizeDirection
	{
		get
		{
			return (Direction)GetValue(ResizeDirectionProperty);
		}
		set
		{
			SetValue(ResizeDirectionProperty, value);
		}
	}

	public event RoutedEventHandler BeginResize
	{
		add
		{
			AddHandler(BeginResizeEvent, value);
		}
		remove
		{
			RemoveHandler(BeginResizeEvent, value);
		}
	}

	public event RoutedEventHandler EndResize
	{
		add
		{
			AddHandler(EndResizeEvent, value);
		}
		remove
		{
			RemoveHandler(EndResizeEvent, value);
		}
	}

	static ResizeThumb()
	{
		ResizeTargetProperty = DependencyProperty.Register("ResizeTarget", typeof(object), typeof(ResizeThumb), new PropertyMetadata());
		ResizeDirectionProperty = DependencyProperty.Register("ResizeDirection", typeof(Direction), typeof(ResizeThumb), new PropertyMetadata(Direction.None));
		BeginResizeEvent = EventManager.RegisterRoutedEvent("BeginResize", RoutingStrategy.Bubble, typeof(BeginResizeEventHandler), typeof(ResizeThumb));
		EndResizeEvent = EventManager.RegisterRoutedEvent("EndResize", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ResizeThumb));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizeThumb), new FrameworkPropertyMetadata(typeof(ResizeThumb)));
	}

	public ResizeThumb()
	{
		base.MouseLeftButtonDown += ResizeThumb_MouseLeftButtonDown;
		base.MouseMove += ResizeThumb_MouseMove;
		base.MouseLeftButtonUp += ResizeThumb_MouseLeftButtonUp;
		base.LostMouseCapture += ResizeThumb_LostMouseCapture;
	}

	private void ResizeThumb_MouseMove(object sender, MouseEventArgs e)
	{
		OnDragDelta();
	}

	private void ResizeThumb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		OnDragStarted();
	}

	private void ResizeThumb_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		OnDragCompleted();
	}

	private void ResizeThumb_LostMouseCapture(object sender, MouseEventArgs e)
	{
		OnDragCompleted();
	}

	private void OnDragStarted()
	{
		FrameworkElement frameworkElement = ResizeTarget as FrameworkElement;
		if (frameworkElement != null && CaptureMouse() && (mDirection = ResizeDirection) != 0)
		{
			RaiseEvent(new BeginResizeEventArgs(mDirection));
			mTarget = frameworkElement;
			mMinSize = new Size(mTarget.MinWidth, mTarget.MinHeight);
			mMaxSize = new Size(mTarget.MaxWidth, mTarget.MaxHeight);
			mInitSize = new Size(mTarget.ActualWidth, mTarget.ActualHeight);
			//mInitPosition = Util.GetCursorPosition();
			mDragging = true;
		}
	}

	private void OnDragDelta()
	{
        /*
		if (mDragging)
		{
            //Point cursorPosition = Util.GetCursorPosition();
			Vector vector = new Vector(cursorPosition.X - mInitPosition.X, cursorPosition.Y - mInitPosition.Y);
			if (mDirection.HasFlag(Direction.Left))
			{
				vector.X = 0.0 - vector.X;
			}
			else if (!mDirection.HasFlag(Direction.Right))
			{
				vector.X = 0.0;
			}
			if (mDirection.HasFlag(Direction.Top))
			{
				vector.Y = 0.0 - vector.Y;
			}
			else if (!mDirection.HasFlag(Direction.Bottom))
			{
				vector.Y = 0.0;
			}
			Size s = new Size(Math.Max(0.0, mInitSize.Width + vector.X), Math.Max(0.0, mInitSize.Height + vector.Y));
			s = s.KeepInRange(mMinSize, mMaxSize);
			if (mDirection.HasFlag(Direction.Left) || mDirection.HasFlag(Direction.Right))
			{
				mTarget.Width = s.Width;
			}
			if (mDirection.HasFlag(Direction.Top) || mDirection.HasFlag(Direction.Bottom))
			{
				mTarget.Height = s.Height;
			}
		}*/
	}

	private void OnDragCompleted()
	{
		if (mDragging)
		{
			mDragging = false;
			mTarget = null;
			ReleaseMouseCapture();
			RaiseEvent(new RoutedEventArgs(EndResizeEvent));
		}
	}
    }
}
