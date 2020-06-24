// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlatWindow.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the FlatWindow type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CustomControls.Controls
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Forms;
    using System.Windows.Input;
    using System.Windows.Interop;

    using CustomControls.Helper;

    using Microsoft.Win32;

    using Prism.Regions;
    using Rectangle = System.Drawing.Rectangle;

    /// <summary>
    /// The flat window.
    /// </summary>
    public partial class FlatWindow : Window
    {
        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private static int mOuterMarginSize = 1;

        /// <summary>
        /// The _hwnd source.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private HwndSource _hwndSource;

        /// <summary>
        /// The is mouse button down.
        /// </summary>
        private bool isMouseButtonDown;

        /// <summary>
        /// The is manual drag.
        /// </summary>
        private bool isManualDrag;

        /// <summary>
        /// The mouse down position.
        /// </summary>
        private System.Windows.Point mouseDownPosition;

        /// <summary>
        /// The position before drag.
        /// </summary>
        private System.Windows.Point positionBeforeDrag;

        /// <summary>
        /// The previous screen bounds.
        /// </summary>
        private System.Windows.Point previousScreenBounds;

        /// <summary>
        /// Gets or sets the window root.
        /// </summary>
        private Grid WindowRoot { get; set; }

        /// <summary>
        /// Gets or sets the layout root.
        /// </summary>
        private Grid LayoutRoot { get; set; }

        /// <summary>
        /// Gets the minimize button.
        /// </summary>
        public System.Windows.Controls.Button MinimizeButton { get; private set; }

        /// <summary>
        /// Gets the maximize button.
        /// </summary>
        public System.Windows.Controls.Button MaximizeButton { get; private set; }

        /// <summary>
        /// Gets the restore button.
        /// </summary>
        public System.Windows.Controls.Button RestoreButton { get; private set; }

        public System.Windows.Controls.Button CloseButton { get; private set; }

        public Grid HeaderBar { get; private set; }

        public double HeightBeforeMaximize { get; private set; }

        public double WidthBeforeMaximize { get; private set; }

        public WindowState PreviousState { get; private set; }

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        public Thickness OuterMarginSizeThickness
        {
            get => (Thickness)this.GetValue(FlatWindow.OuterMarginSizeThicknessProperty);
            set => this.SetValue(FlatWindow.OuterMarginSizeThicknessProperty, (Thickness)value);
        }

        /// <summary>
        /// get or set Title height for Window 
        /// </summary>
        public GridLength TitleHeight
        {
            get => (GridLength)this.GetValue(FlatWindow.TitleHeightProperty);
            set => this.SetValue(FlatWindow.TitleHeightProperty, (GridLength)value);
        }

        #region Dependency Property

        /// <summary>
        /// get or set Title height for Window 
        /// </summary>
        public static DependencyProperty TitleHeightProperty = DependencyProperty.Register(
            nameof(TitleHeight),
            typeof(GridLength),
            typeof(FlatWindow),
            new PropertyMetadata(new GridLength(40)));
        /// <summary>
        /// The outer margin size thickness property.
        /// </summary>
        public static DependencyProperty OuterMarginSizeThicknessProperty = DependencyProperty.Register(
            nameof(OuterMarginSizeThickness),
            typeof(Thickness),
            typeof(FlatWindow),
            new PropertyMetadata(new Thickness(mOuterMarginSize)));

        public IRegionManager RegionManager { get; set; }
        
        #endregion

        /// <summary>
        /// Initializes static members of the <see cref="FlatWindow"/> class.
        /// </summary>
        static FlatWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(FlatWindow),
                new FrameworkPropertyMetadata(typeof(FlatWindow)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlatWindow"/> class.
        /// </summary>
        public FlatWindow()
        {
            double currentDPIScaleFactor = (double)SystemHelper.GetCurrentDPIScaleFactor();
            Screen screen = Screen.FromHandle((new WindowInteropHelper(this)).Handle);
            base.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
            base.StateChanged += new EventHandler(this.OnStateChanged);
            base.Loaded += new RoutedEventHandler(this.OnLoaded);
            Rectangle workingArea = screen.WorkingArea;
            base.MaxHeight = (double)(workingArea.Height + 16) / currentDPIScaleFactor;
            SystemEvents.DisplaySettingsChanged += new EventHandler(this.SystemEvents_DisplaySettingsChanged);
            this.AddHandler(Window.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.OnMouseButtonUp), true);
            this.AddHandler(Window.MouseMoveEvent, new System.Windows.Input.MouseEventHandler(this.OnMouseMove));
        }

        public void CreateRegionManager(IRegionManager regionManager)
        {
            this.RegionManager = RegionManager;
        }

        public T GetRequiredTemplateChild<T>(string childName) where T : DependencyObject
        {
            return (T)base.GetTemplateChild(childName);
        }

        public override void OnApplyTemplate()
        {
            this.WindowRoot = this.GetRequiredTemplateChild<Grid>("WindowRoot");
            this.LayoutRoot = this.GetRequiredTemplateChild<Grid>("LayoutRoot");
            this.MinimizeButton = this.GetRequiredTemplateChild<System.Windows.Controls.Button>("MinimizeButton");
            this.MaximizeButton = this.GetRequiredTemplateChild<System.Windows.Controls.Button>("MaximizeButton");
            this.RestoreButton = this.GetRequiredTemplateChild<System.Windows.Controls.Button>("RestoreButton");
            this.CloseButton = this.GetRequiredTemplateChild<System.Windows.Controls.Button>("CloseButton");
            this.HeaderBar = this.GetRequiredTemplateChild<Grid>("PART_HeaderBar");

            if (this.LayoutRoot != null && this.WindowState == WindowState.Maximized)
            {
                this.LayoutRoot.Margin = GetDefaultMarginForDpi();
            }

            if (this.CloseButton != null)
            {
                this.CloseButton.Click += CloseButton_Click;
            }

            if (this.MinimizeButton != null)
            {
                this.MinimizeButton.Click += MinimizeButton_Click;
            }

            if (this.RestoreButton != null)
            {
                this.RestoreButton.Click += RestoreButton_Click;
            }

            if (this.MaximizeButton != null)
            {
                this.MaximizeButton.Click += MaximizeButton_Click;
            }

            if (this.HeaderBar != null)
            {
                this.HeaderBar.AddHandler(Grid.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.OnHeaderBarMouseLeftButtonDown));
            }

            base.OnApplyTemplate();
        }

        protected override void OnInitialized(EventArgs e)
        {
            this.SourceInitialized += this.OnSourceInitialized;
            base.OnInitialized(e);
        }

        /// <summary>
        /// The on header bar mouse left button down.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected virtual void OnHeaderBarMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.isManualDrag)
            {
                return;
            }

            System.Windows.Point position = e.GetPosition(this);
            int headerBarHeight = 40;
            int leftmostClickableOffset = 50;

            if (position.X - this.LayoutRoot.Margin.Left <= leftmostClickableOffset && position.Y <= headerBarHeight)
            {
                if (e.ClickCount != 2)
                {
                    this.OpenSystemContextMenu(e);
                }
                else
                {
                    base.Close();
                }
                e.Handled = true;
                return;
            }

            if (e.ClickCount == 2 && base.ResizeMode == ResizeMode.CanResize)
            {
                this.ToggleWindowState();
                return;
            }

            if (base.WindowState == WindowState.Maximized)
            {
                this.isMouseButtonDown = true;
                this.mouseDownPosition = position;
            }
            else
            {
                try
                {
                    this.positionBeforeDrag = new System.Windows.Point(base.Left, base.Top);
                    this.DragMove();
                }
                catch
                {
                }
            }
        }

        protected void ToggleWindowState()
        {
            if (base.WindowState != WindowState.Maximized)
            {
                base.WindowState = WindowState.Maximized;
            }
            else
            {
                base.WindowState = WindowState.Normal;
            }
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.ToggleWindowState();
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            this.ToggleWindowState();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnSourceInitialized(object sender, EventArgs e)
        {
            _hwndSource = (HwndSource)PresentationSource.FromVisual(this);
        }

        private void SetMaximizeButtonsVisibility(Visibility maximizeButtonVisibility, Visibility reverseMaximizeButtonVisiility)
        {
            if (this.MaximizeButton != null)
            {
                this.MaximizeButton.Visibility = maximizeButtonVisibility;
            }
            if (this.RestoreButton != null)
            {
                this.RestoreButton.Visibility = reverseMaximizeButtonVisiility;
            }
        }

        private void OpenSystemContextMenu(MouseButtonEventArgs e)
        {
            System.Windows.Point position = e.GetPosition(this);
            System.Windows.Point screen = this.PointToScreen(position);
            int num = 40;
            if (position.Y < (double)num)
            {
                IntPtr handle = (new WindowInteropHelper(this)).Handle;
                IntPtr systemMenu = NativeUtils.GetSystemMenu(handle, false);
                if (base.WindowState != WindowState.Maximized)
                {
                    NativeUtils.EnableMenuItem(systemMenu, 61488, 0);
                }
                else
                {
                    NativeUtils.EnableMenuItem(systemMenu, 61488, 1);
                }
                int num1 = NativeUtils.TrackPopupMenuEx(systemMenu, NativeUtils.TPM_LEFTALIGN | NativeUtils.TPM_RETURNCMD, Convert.ToInt32(screen.X + 2), Convert.ToInt32(screen.Y + 2), handle, IntPtr.Zero);
                if (num1 == 0)
                {
                    return;
                }

                NativeUtils.PostMessage(handle, 274, new IntPtr(num1), IntPtr.Zero);
            }
        }

    }
}
