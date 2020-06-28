using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using ApplicationInterfaceCore;
using DrawingModule.Enums;
using DrawingModule.ViewModels;
using MahApps.Metro.Converters;

namespace DrawingModule.Views
{
    /// <summary>
    /// Interaction logic for DynamicInputView
    /// </summary>
    public partial class DynamicInputView : UserControl, INotifyPropertyChanged, IDynamicInputView
    {
        #region Dependency Property

        public static readonly DependencyProperty CommantTextProperty =
            DependencyProperty.Register("CommandText", typeof(string), typeof(DynamicInputView),
                new PropertyMetadata(null));
                // Heigth
        public static readonly DependencyProperty HeightDimensionProperty =
            DependencyProperty.Register("HeightDimension", typeof(string), typeof(DynamicInputView),
                new PropertyMetadata("0"));
        public static readonly DependencyProperty TextHeightVisibleProperty =
            DependencyProperty.Register("TextHeightVisible", typeof(Visibility), typeof(DynamicInputView),
                new PropertyMetadata(Visibility.Collapsed));
        // Width
        public static readonly DependencyProperty WidthDimensionProperty =
            DependencyProperty.Register("WidthDimension", typeof(string), typeof(DynamicInputView),
                new PropertyMetadata("0"));

        public static readonly DependencyProperty TextWidthVisibleProperty =
            DependencyProperty.Register("TextWidthVisible", typeof(Visibility), typeof(DynamicInputView),
                new PropertyMetadata(Visibility.Collapsed));
        // Length
        public static readonly DependencyProperty LengthDimensionProperty =
            DependencyProperty.Register("LengthDimension", typeof(string), typeof(DynamicInputView),
                new FrameworkPropertyMetadata("0") { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        public static readonly DependencyProperty TextLengthVisibleProperty =
            DependencyProperty.Register("TextLengthVisible", typeof(Visibility), typeof(DynamicInputView),
                new PropertyMetadata(System.Windows.Visibility.Collapsed));
        // Angle
        public static readonly DependencyProperty AngleDimensionProperty =
            DependencyProperty.Register("AngleDimension", typeof(string), typeof(DynamicInputView),
                new PropertyMetadata("0"));
        public static readonly DependencyProperty TextAngleVisibleProperty =
            DependencyProperty.Register("TextAngleVisible", typeof(Visibility), typeof(DynamicInputView),
                new PropertyMetadata(Visibility.Collapsed));
        public static readonly DependencyProperty TextLabelVisibilityProperty =
            DependencyProperty.Register("TextLabelVisibility", typeof(Visibility), typeof(DynamicInputView),
            new PropertyMetadata(System.Windows.Visibility.Collapsed));
        public static readonly DependencyProperty TextInputContentVisibilityProperty =
            DependencyProperty.Register("TextInputContentVisibility", typeof(Visibility), typeof(DynamicInputView),
                new PropertyMetadata(System.Windows.Visibility.Collapsed));
        public static readonly DependencyProperty TextInputContentProperty =
            DependencyProperty.Register("TextInputContent", typeof(string), typeof(DynamicInputView),
                new PropertyMetadata("Please Input String Here"));
        public static readonly DependencyProperty TextInputHeightProperty =
            DependencyProperty.Register("TextInputHeight", typeof(string), typeof(DynamicInputView),
                new PropertyMetadata("0"));
        public static readonly DependencyProperty TextInputHeightVisibilityProperty =
            DependencyProperty.Register("TextInputHeightVisibility", typeof(Visibility), typeof(DynamicInputView),
                new PropertyMetadata(Visibility.Collapsed));
        public static readonly DependencyProperty TextInputAngleVisibilityProperty =
            DependencyProperty.Register("TextInputAngleVisibility", typeof(Visibility), typeof(DynamicInputView),
                new PropertyMetadata(Visibility.Collapsed));
        public static readonly DependencyProperty TextInputAngleProperty = DependencyProperty.Register("TextInputAngle",
            typeof(double), typeof(DynamicInputView), new PropertyMetadata(0.0));
        public static readonly DependencyProperty LeaderSegmentVisibilityProperty =
            DependencyProperty.Register("LeaderSegmentVisibility", typeof(Visibility), typeof(DynamicInputView),
                new PropertyMetadata(Visibility.Collapsed));
        public static readonly DependencyProperty LeaderSegmentNumberProperty =
            DependencyProperty.Register("LeaderSegmentNumber", typeof(int), typeof(DynamicInputView),
                new PropertyMetadata(1));

        public static readonly DependencyProperty ArrowHeadSizeVisibilityProperty =
            DependencyProperty.Register("ArrowHeadSizeVisibility", typeof(Visibility), typeof(DynamicInputView),
                new PropertyMetadata(Visibility.Collapsed));
        public static readonly DependencyProperty ArrowSizeProperty = DependencyProperty.Register("ArrowSize",
            typeof(double), typeof(DynamicInputView), new PropertyMetadata(default(double)));
        public static readonly DependencyProperty CommandTextVisibilityProperty =
            DependencyProperty.Register("CommandTextVisibility", typeof(Visibility), typeof(DynamicInputView),
                new PropertyMetadata(Visibility.Collapsed));

        #endregion


        #region Properties
        public bool IsValid => this.DataContext != null;
        private FocusType _previusFocusType;
        public string CommandText
        {
            get => (string)GetValue(CommantTextProperty);
            set => SetValue(CommantTextProperty, value);
        }
        public string TextInputContent
        {
            get => (string)GetValue(TextInputContentProperty);
            set => SetValue(TextInputContentProperty, value);
        }
        public string TextInputHeight
        {
            get => (string)GetValue(TextInputHeightProperty);
            set => SetValue(TextInputHeightProperty, value);
        }
        public Visibility TextInputHeightVisibility
        {
            get => (Visibility)GetValue(TextInputHeightVisibilityProperty);
            set => SetValue(TextInputHeightVisibilityProperty, value);
        }
        public Visibility TextInputContentVisibility
        {
            get => (Visibility)GetValue(TextInputContentVisibilityProperty);
            set => SetValue(TextInputContentVisibilityProperty, value);
        }
        public Visibility CommandTextVisibility
        {
            get => (Visibility)GetValue(CommandTextVisibilityProperty);
            set => SetValue(CommandTextVisibilityProperty, value);
        }
        public Visibility TextLabelVisibility
        {
            get => (Visibility) GetValue(TextLabelVisibilityProperty);
            set => SetValue(TextLabelVisibilityProperty, value);
        }
        
        public FocusType PreviusDynamicInputFocus { get=>_previusFocusType; private set=>SetProperty(ref _previusFocusType,value); }

        public Visibility TextLengthVisible
        {
            get=>(Visibility)GetValue(TextLengthVisibleProperty);
            set=>SetValue(TextLengthVisibleProperty,value);
        }
        public Visibility TextAngleVisible
        {
            get => (Visibility)GetValue(TextAngleVisibleProperty);
            set => SetValue(TextAngleVisibleProperty, value);
        }
        public Visibility TextHeightVisible
        {
            get => (Visibility)GetValue(TextHeightVisibleProperty);
            set => SetValue(TextHeightVisibleProperty, value);
        }
        public Visibility TextWidthVisible
        {
            get => (Visibility)GetValue(TextWidthVisibleProperty);
            set => SetValue(TextWidthVisibleProperty, value);
        }
        public string HeightDimension
        {
            get=> (string)GetValue(HeightDimensionProperty);
            set=>SetValue(HeightDimensionProperty,value);
        }

        public string WidthDimension
        {
            get => (string)GetValue(WidthDimensionProperty);
            set => SetValue(WidthDimensionProperty, value);
        }

        public string LengthDimension
        {
            get => (string)GetValue(LengthDimensionProperty);
            set => SetValue(LengthDimensionProperty,value);
        }
        public string AngleDimension
        {
            get => (string)GetValue(AngleDimensionProperty);
            set => SetValue(AngleDimensionProperty, value);
        }

        public Visibility TextInputAngleVisibility
        {
            get => (Visibility) GetValue(TextInputAngleVisibilityProperty);
            set => SetValue(TextInputAngleVisibilityProperty, value);
        }

        public double TextInputAngle
        {
            get => (double) GetValue(TextInputAngleProperty);
            set => SetValue(TextInputAngleProperty, value);
        }

        public Visibility LeaderSegmentVisibility
        {
            get => (Visibility) GetValue(LeaderSegmentVisibilityProperty);
            set => SetValue(LeaderSegmentVisibilityProperty, value);
        }

        public int LeaderSegmentNumber
        {
            get => (int) GetValue(LeaderSegmentNumberProperty);
            set => SetValue(LeaderSegmentNumberProperty, value);
        }

        public Visibility ArrowHeadSizeVisibility
        {
            get => (Visibility) GetValue(ArrowHeadSizeVisibilityProperty);
            set => SetValue(ArrowHeadSizeVisibilityProperty, value);
        }

        public double ArrowSize
        {
            get => (double) GetValue(ArrowSizeProperty);
            set => SetValue(ArrowSizeProperty, value);
        }

        #endregion

        #region Command Line Properties

        private DynamicInputViewModel mCommandLine;

        #endregion
        #region Constructor

        public DynamicInputView()
        {
            this.InitializeComponent();
            
            this.mCommandLine = this.DataContext as DynamicInputViewModel;
            
            if (this.IsValid)
            {
                //ContainerMessageDispatcher.Win32Message += OnContainerMessages;
                //ComponentDispatcher.ThreadPreprocessMessage += OnKeyMessages;
            }
        }
        #endregion

        #region Public Method
        public bool PreProcessKeyboardInput(KeyEventArgs e)
        {
            if (!this.IsValid)
            {
                return false;
            }

            return this.mCommandLine.ProcessKeyDown(e);
            
        }
        //private void OnContainerMessages(ref MSG msg, ref bool handled)
        //{
        //    if (!this.IsValid)
        //        return;

        //    if (msg.message != 260 && msg.message != 256 && msg.message != 257 && msg.message != 258)
        //    {
        //        this.mCommandLine.ProcessMessages(ref msg, ref handled);
        //    }
        //}
        //private void OnKeyMessages(ref MSG msg, ref bool handled)
        //{
        //    if (!this.IsValid)
        //    {
        //        return;
        //    }
        //    //if (!this.IsCommandLineMessage(ref msg))
        //    //{
        //    //    return;
        //    //}

        //    //if ((!base.IsKeyboardFocusWithin && !this.mHistoryPopup.IsKeyboardFocusWithin && !this.mAutoCompletePopup.IsKeyboardFocusWithin) || !base.IsEnabled)
        //    //{
        //    //    return;
        //    //}
        //    //if (this.mCommandLine != null && this.mCommandLine.AreKeytipsActive && (msg.message != 260 || msg.wParam.ToInt32() != 121))
        //    //{
        //    //    return;
        //    //}
        //    if (this.mCommandLine != null && this.PreProcessKeyDown(ref msg, ref handled))
        //    {
        //        return;
        //    }
        //    if (msg.message == 260 || msg.message == 256 || msg.message == 257 || msg.message == 258)
        //    {
        //        this.mCommandLine?.ProcessMessages(ref msg, ref handled);
        //    }
        //    if (msg.message == 256 && msg.wParam.ToInt32() == 9 && this.CommandLine.IsKeyboardFocusWithin)
        //    {
        //        handled = true;
        //    }
        //}
        //private bool PreProcessKeyDown(ref MSG msg, ref bool handled)
        //{
        //    if (!this.IsValid)
        //    {
        //        return false;
        //    }
        //    if (msg.message != 256 || (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.None)
        //    {
        //        return false;
        //    }
        //    if ((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
        //    {
        //        return false;
        //    }

        //    return true;

        //    //this.UpdatePopupHistoryStatus(ref msg);
        //    var num = msg.wParam.ToInt32();
        //    if (num != 67)
        //    {
        //        if (num == 86)
        //        {
        //            //this.mCommandLine.DoPaste();
        //            handled = true;
        //            return true;
        //        }
        //        if (num != 88)
        //        {
        //            return false;
        //        }
        //        string selection = this.GetSelection();
        //        if (!string.IsNullOrEmpty(selection))
        //        {
        //            //this.mCommandLine.UpdateSelectionForMenu(selection);
        //        }
        //        //this.mCommandLine.DoCut();
        //        handled = true;
        //        return true;
        //    }
        //    else
        //    {
        //        string selection2 = this.GetSelection();
        //        if (!string.IsNullOrEmpty(selection2))
        //        {
        //            //this.mCommandLine.UpdateSelectionForMenu(selection2);
        //            //this.mCommandLine.DoCopy();
        //            handled = true;
        //            return true;
        //        }
        //        return false;
        //    }
        //}
        private string GetSelection()
        {
            return "";
        }

        internal void FocusDynamicInputTextBox(FocusType focusType)
        {
            if (focusType == FocusType.Previous)
            {
                focusType = PreviusDynamicInputFocus;
            }
            switch (focusType)
            {
                case FocusType.Length:
                    FocusTextLength();
                    break;
                case FocusType.Width:
                    TextWidth.SelectAll();
                    break;
                case FocusType.Height:
                    TextHeight.SelectAll();
                    break;
                case FocusType.Angle:
                    FocusTextAngle();
                    break;
                case FocusType.TextContent:
                    this.TextInput.SelectAll();
                    break;
                case FocusType.TextHeight:
                    TextStringHeight.SelectAll();
                    break;
                case FocusType.TextAngle:
                    TextStringAngle.SelectAll();
                    break;
                case FocusType.LeaderSegment:
                    LeaderSegment.SelectAll();
                    break;
                case FocusType.ArrowSize:
                    ArrowHeadSize.SelectAll();
                    break;
                case FocusType.CommandLine:
                    CommandLine.SelectAll();
                    break;
                default:
                    break;
            }
        }
        public void FocusTextLength()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {//this refer to form in WPF application 
                this.TextLength.Focus();
                this.TextLength.SelectAll();
                this.PreviusDynamicInputFocus = FocusType.Length;
            }));
            
        }
        public void FocusCommandTextbox()
        {
            //if (PreviusDynamicInputFocus != FocusType.CommandLine)
            //{
            //    this.CommandLine.SelectAll();
            //}
            this.CommandLine.Focus();
            this.PreviusDynamicInputFocus = FocusType.CommandLine;
        }
        public void FocusTextContentInput()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {//this refer to form in WPF application 
                this.TextInput.Focus();
                this.TextInput.SelectAll();
                this.PreviusDynamicInputFocus = FocusType.TextContent;
            }));
            
        }

        public void FocusTextHeightInput()
        {
            this.Dispatcher.Invoke((Action) (() =>
            {
                //this refer to form in WPF application 
                this.TextStringHeight.Focus();
                this.TextStringHeight.SelectAll();
                this.PreviusDynamicInputFocus = FocusType.TextHeight;
            }));
        }


        public void FocusTextArrowSize()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                //this refer to form in WPF application 
                this.ArrowHeadSize.Focus();
                this.ArrowHeadSize.SelectAll();
                this.PreviusDynamicInputFocus = FocusType.ArrowSize;
            }));
           

        }

        public void FocusTextAngle()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                //this refer to form in WPF application 
                this.TextAngle.Focus();
                this.TextAngle.SelectAll();
                this.PreviusDynamicInputFocus = FocusType.Angle;
            }));
            
        }

        public void FocusTextWidth()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                //this refer to form in WPF application 
                this.TextWidth.Focus();
                this.TextWidth.SelectAll();
                this.PreviusDynamicInputFocus = FocusType.Width;
            }));
        }

        public void FocusTextStringAngle()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                //this refer to form in WPF application 
               this.TextStringAngle.Focus();
               this.TextStringAngle.SelectAll();
                this.PreviusDynamicInputFocus = FocusType.TextAngle;
            }));
            
        }

        public void FocusLeaderSegment()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                this.LeaderSegment.Focus();
                this.LeaderSegment.SelectAll();
                this.PreviusDynamicInputFocus = FocusType.LeaderSegment;
            }));
            
        }
        public void FocusTextHeight()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                this.TextHeight.Focus();
                this.TextHeight.SelectAll();
                this.PreviusDynamicInputFocus = FocusType.Height;
            }));
        }
        #endregion

        #region Private Method
        
        //private static void OnTextLengthChanged(DependencyObject source,
        //    DependencyPropertyChangedEventArgs e)
        //{

        //}


        #endregion

        #region Implement NotifyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;
            RaisePropertyChanged(propertyName);

            return true;
        }
        protected virtual bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;
            onChanged?.Invoke();
            RaisePropertyChanged(propertyName);

            return true;
        }
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }

        #endregion
    }
}
