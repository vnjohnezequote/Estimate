// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLineView.xaml.cs" company="John Nguyen">
//  John Nguyen 
// </copyright>
// <summary>
//   Interaction logic for CommandLineView.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System.ComponentModel;
using System.Text;
using System.Windows.Automation;
using DrawingModule.CommandLine;
using DrawingModule.Control;
using DrawingModule.Interface;

namespace DrawingModule.Views
{
    using System.Windows.Controls;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interop;
    using ViewModels;
    using RawInputManager.Wind32;
    using System;
    using System.Collections.Generic;
    using System.Windows.Threading;
    public delegate void CommandLineDragEventHandler(object sender, CommandLineDragEventArgs e);
    /// <summary>
    /// Interaction logic for CommandLineView.xaml
    /// </summary>
    public partial class CommandLineView : UserControl, IHandleWindowPositionChanged, IResize
    {
        #region Private
        private CommandLineViewModel mCommandLine;

        private readonly bool mSetAutomationId;

        private DispatcherTimer mClosePromptTimer;

        private Size mOriginalSize;
        private static CommandLineView commandControl;

        public static CommandLineView CommandControl
        {
            get
            {
                if (commandControl == null)
                {
                    return new CommandLineView();
                }

                return commandControl;
            }
        }


        #endregion

        #region Public
        internal bool IsFocusWithin
        {
            get
            {
                if (!IsKeyboardFocusWithin && !mTempHistoryPopup.IsFocused && !mHistoryPopup.IsFocused)
                {
                    return mAutoCompletePopup.IsFocused;
                }
                return true;
            }
        }

        internal bool IsMouseWithin
        {
            get
            {
                if (!IsMouseOver && !mTempHistoryPopup.IsMouseOver && !mHistoryPopup.IsMouseOver)
                {
                    return mAutoCompletePopup.IsMouseOver;
                }
                return true;
            }
        }

        public double InPlaceHistoryHeight => mInplaceHistory.ActualHeight;

        public bool IsValid => this.mCommandLine != null;

        public event CommandLineDragEventHandler Dragged;
        #endregion

        #region Constructor
        public CommandLineView()
        {
            this.InitializeComponent();
            this.mCommandLine = this.DataContext as CommandLineViewModel;
            if (this.IsValid)
            {
                this.PART_CurrentCommand.Visibility = Visibility.Collapsed;
                this.PART_Input.SetCommandLine(this.mCommandLine);
                //this.PART_Input.IsEnabled = !mCommandLine.IsOEMReadOnly;
                //this.PART_Input.LostFocus += InputTextBox_LostFocus;
                //this.PART_Input.LostKeyboardFocus += InputTextBox_LostKeyboardFocus;
                //base.MouseEnter += CommandLineControl_MouseEnter;
                //base.MouseLeave += CommandLineControl_MouseLeave;
                //base.GotKeyboardFocus += CommandLineControl_GotKeyboardFocus;
                //base.LostKeyboardFocus += CommandLineControl_LostKeyboardFocus;
                //base.IsEnabledChanged += CommandLineControl_IsEnabledChanged;
                //ContainerMessageDispatcher.Win32Message += OnContainerMessages;
                //ComponentDispatcher.ThreadPreprocessMessage += OnKeyMessages;
                mCommandLine.PropertyChanged += mCommandLine_PropertyChanged;
                UpdateKeywords(null, usePlainKeywords: false);
                mCommandLine.IsDocked = (mBackgroundGrid.VerticalAlignment == VerticalAlignment.Stretch);
                DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(FrameworkElement.VerticalAlignmentProperty, typeof(Grid));
                dependencyPropertyDescriptor.AddValueChanged(mBackgroundGrid, OnBackgroundGridVerticalAlignmentChanged);
                GoToState("Inactive");
                //LoadSizeFromProfile();
                //mBackgroundGrid.SizeChanged += BackgroundGrid_SizeChanged;
                mHistoryPopupBorder.SizeChanged += HistoryPopupBorder_SizeChanged;
                mAutoCompletePopup.Opened += AutoCompletePopup_Opened;
                //PART_PromptPanel.PreviewMouseMove += PromptPanel_PreviewMouseMove;
                //mSetAutomationId = (string.Compare("True", Environment.GetEnvironmentVariable("CLISETAUTOMATIONID"), ignoreCase: true) == 0);
                /*mPromptFadeStoryboard = (base.Resources["TempPromptFadeStoryboard"] as Storyboard);
                if (mPromptFadeStoryboard != null)
                {
                    mPromptFadeStoryboard.Completed += mPromptFadeStoryboard_Completed;
                }*/
                base.Loaded += OnLoaded;



            }
        }


        #endregion

        #region private method

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        public void OnWindowPositionChanged(object sender, WindowPositionChangedEventArgs e)
        {

        }

        //protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        //{

        //}

        private void OnBackgroundGridVerticalAlignmentChanged(object sender, EventArgs e)
        {

        }

        private void mBackgroundBorder_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void mBackgroundBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void InputAreaBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        //private void OnContainerMessages(ref MSG msg, ref bool handled)
        //{
        //    if (!this.IsValid)
        //    {
        //        return;
        //    }
            
        //    if (!this.IsCommandLineMessage(ref msg))
        //    {
        //        return;
        //    }
        //    if (msg.message != Win32Consts.WM_SYSKEYDOWN && msg.message != Win32Consts.WM_KEYDOWN && msg.message != Win32Consts.WM_KEYUP && msg.message != Win32Consts.WM_CHAR)
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
        //    if (!this.IsCommandLineMessage(ref msg))
        //    {
        //        return;
        //    }
            
        //    if ((!base.IsKeyboardFocusWithin && !this.mHistoryPopup.IsKeyboardFocusWithin && !this.mAutoCompletePopup.IsKeyboardFocusWithin) || !base.IsEnabled)
        //    {
        //        return;
        //    }
        //    if (this.mCommandLine != null && this.mCommandLine.AreKeytipsActive && (msg.message != 260 || msg.wParam.ToInt32() != 121))
        //    {
        //        return;
        //    }
        //    if (this.mCommandLine != null && this.PreProcessKeyDown(ref msg, ref handled))
        //    {
        //        return;
        //    }
        //    if (msg.message == 260 || msg.message == 256 || msg.message == 257 || msg.message == 258)
        //    {
        //        this.mCommandLine?.ProcessMessages(ref msg, ref handled);
        //    }
        //    if (msg.message == 256 && msg.wParam.ToInt32() == 9 && this.PART_Input.IsKeyboardFocusWithin)
        //    {
        //        handled = true;
        //    }
        //}

        private bool PreProcessKeyDown(ref MSG msg, ref bool handled)
        {
            if (!this.IsValid)
            {
                return false;
            }
            if (msg.message != 256 || (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.None)
            {
                return false;
            }
            if ((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
            {
                return false;
            }

            return true;
            
            this.UpdatePopupHistoryStatus(ref msg);
            int num = msg.wParam.ToInt32();
            if (num != 67)
            {
                if (num == 86)
                {
                    //this.mCommandLine.DoPaste();
                    handled = true;
                    return true;
                }
                if (num != 88)
                {
                    return false;
                }
                string selection = this.GetSelection();
                if (!string.IsNullOrEmpty(selection))
                {
                    //this.mCommandLine.UpdateSelectionForMenu(selection);
                }
                //this.mCommandLine.DoCut();
                handled = true;
                return true;
            }
            else
            {
                string selection2 = this.GetSelection();
                if (!string.IsNullOrEmpty(selection2))
                {
                    //this.mCommandLine.UpdateSelectionForMenu(selection2);
                    //this.mCommandLine.DoCopy();
                    handled = true;
                    return true;
                }
                return false;
            }
        }

        private void UpdateAutoCompleteOrientation()
        {

        }

        private void UpdatePopupHistoryStatus(ref MSG msg)
        {

        }

        private bool IsCommandLineMessage(ref MSG msg)
        {
            return true;
        }

        private void CommandLineControl_MouseEnter(object sender, MouseEventArgs e)
        {
            this.UpdateIsActive();
        }

        private void CommandLineControl_MouseLeave(object sender, MouseEventArgs e)
        {
            this.UpdateIsActive();
        }

        private void CommandLineControl_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {

        }

        private void CommandLineControl_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {

        }

        private void CommandLineControl_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void HistoryPopupBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void AutoCompletePopup_Opened(object sender, EventArgs e)
        {

        }

        private void BackgroundGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void UpdateIsActive()
        {
            if (!this.IsValid)
            {
                return;
            }

            bool flag = base.IsEnabled && (this.IsFocusWithin || this.IsMouseOver ||!string.IsNullOrEmpty(this.mCommandLine.CurrentCommand));
            if (flag)
            {
                this.GoToState("Active");
                return;
            }
            this.GoToState("InActive");
        }

        private void GoToState(string state)
        {
            if (!this.IsValid)
            {
                return;
            }
            switch (state)
            {
                case "Docked":
                    {
                        Size? localSize = mBackgroundGrid.GetLocalSize();
                        if (localSize.HasValue)
                        {
                            mOriginalSize = localSize.Value;
                            mBackgroundGrid.SetValue(FrameworkElement.WidthProperty, DependencyProperty.UnsetValue);
                            mBackgroundGrid.SetValue(FrameworkElement.HeightProperty, DependencyProperty.UnsetValue);
                        }
                        mBackgroundGrid.MinWidth = 0.0;
                        base.Opacity = 1.0;
                        mTempHistoryPopup.IsOpen = false;
                        VisualStateManager.GoToState(this, "Docked", useTransitions: false);
                        break;
                    }
                case "Undocked":
                    if (!mBackgroundGrid.GetLocalSize().HasValue)
                    {
                        mBackgroundGrid.Width = mOriginalSize.Width;
                        mBackgroundGrid.Height = mOriginalSize.Height;
                    }
                    mBackgroundGrid.MinWidth = 250.0;
                    VisualStateManager.GoToState(this, "Undocked", useTransitions: false);
                    break;
                case "AlignTop":
                    VisualStateManager.GoToState(this, "AnchorTop", useTransitions: false);
                    break;
                case "ButtonsInOneRow":
                    VisualStateManager.GoToState(this, "OneRow", useTransitions: false);
                    mInputAreaGrid.MinHeight = mBackgroundGrid.ActualHeight - 2.0;
                    break;
                /*
                case "ButtonsInTwoRows":
                    VisualStateManager.GoToState(this, "TwoRows", useTransitions: false);
                    mInputAreaGrid.SetValue(FrameworkElement.MinHeightProperty, DependencyProperty.UnsetValue);
                    break;*/
                case "Active":
                    if (!mCommandLine.IsDocked)
                    {
                        if (!string.IsNullOrEmpty(mCommandLine.CurrentCommand) || (mCommandLine.TempPromptCollection != null && mCommandLine.TempPromptCollection.Count != 0))
                        {
                            ShowTempPrompt();
                        }
                        //base.Opacity = CommandLineService.ProfileData.RolloverOpacity;
                    }
                    else
                    {
                        base.Opacity = 1.0;
                    }
                    break;
                case "Inactive":
                    PendingCloseTempPrompt();
                    if (!mCommandLine.IsDocked)
                    {
                        //base.Opacity = CommandLineService.ProfileData.InactiveOpacity;
                    }
                    else
                    {
                        base.Opacity = 1.0;
                    }
                    break;
                default:
                    break;
                    //throw new NotSupportedException("Unknown State: " + state);
            }
        }

        private void ShowTempPrompt()
        {
            if (mClosePromptTimer != null && mClosePromptTimer.IsEnabled)
            {
                mClosePromptTimer.Stop();
            }
            mTempHistoryPopup.IsOpen = true;
            /*
            if (mPromptFadeStoryboard != null)
            {
                mPromptFadeStoryboard.Stop(this);
            }*/
        }

        private void PendingCloseTempPrompt()
        {
            
            if (mClosePromptTimer == null)
            {
                mClosePromptTimer = new DispatcherTimer();
                mClosePromptTimer.Interval = TimeSpan.FromSeconds(3.0);
                mClosePromptTimer.Tick += ClosePromptTimerTick;
            }
            try
            {
                if (!mClosePromptTimer.IsEnabled)
                {
                    mClosePromptTimer.Start();
                }
            }
            catch
            {
                mClosePromptTimer = null;
            }
        }

        private void ClosePromptTimerTick(object sender, EventArgs e)
        {
            if (mClosePromptTimer != null && mClosePromptTimer.IsEnabled)
            {
                mClosePromptTimer.Stop();
            }
            /*
            if (mPromptFadeStoryboard != null)
            {
                mPromptFadeStoryboard.Begin(this, isControllable: true);
            }
            else
            {
                mPromptFadeStoryboard_Completed(null, null);
            }*/
        }

        private void mPromptFadeStoryboard_Completed(object sender, EventArgs e)
        {

        }

        private void LayoutRoot_PreviewDragEnter(object sender, DragEventArgs e)
        {

        }

        private void LayoutRoot_PreviewDragOver(object sender, DragEventArgs e)
        {

        }

        private void LayoutRoot_PreviewDrop(object sender, DragEventArgs e)
        {

        }

        private bool IsDroppableData(DataObject dataObj)
        {
            return true;
        }

        private void DropFiles(DataObject dataObj)
        {

        }

        private void SetTransparency(object sender, RoutedEventArgs e)
        {

        }

        private void OnCloseButtonClicked(object sender, RoutedEventArgs e)
        {

        }

        private void OnHistoryToggleButtonClicked(object sender, RoutedEventArgs e)
        {

        }

        private void mCommandLine_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
            string propertyName;
            if ((propertyName = e.PropertyName) != null)
            {
                if (propertyName == "SelectionRange")
                {
                    base.Dispatcher.BeginInvoke(new Action(this.PART_Input.UpdateSelection), null);
                    return;
                }
                if (propertyName == "PromptKeywords")
                {
                    this.UpdateKeywords(this.mCommandLine.PromptKeywords, this.mCommandLine.ImprovePerformance);
                    return;
                }
                if (propertyName == "CurrentCommand")
                {
                    this.PART_CurrentCommand.Visibility = (string.IsNullOrEmpty(this.mCommandLine.CurrentCommand) ? Visibility.Collapsed : Visibility.Visible);
                    this.UpdateIsActive();
                    return;
                }
                if (!(propertyName == "InputText"))
                {
                    if (!(propertyName == "ShowTempHistoryNow"))
                    {
                        return;
                    }
                    if (!this.mCommandLine.IsDocked)
                    {
                        this.ShowTempPrompt();
                        this.UpdateIsActive();
                        return;
                    }
                    this.mCommandLine.ClearTempPrompt(true);
                }
                else if (this.mCloseButton.IsKeyboardFocused || this.mConfigButton.IsKeyboardFocused)
                {
                    this.PART_Input.Focus();
                    return;
                }
            }
        }

        private void UpdateKeywords(IList<IPromptKeyword> keywords, bool usePlainKeywords)
        {
            int num = keywords?.Count ?? 0;
            int num2 = PART_PromptPanel.Children.IndexOf(PART_PlainKeywords);
            int num3 = PART_PromptPanel.Children.IndexOf(PART_EndOfKeywords);
            PART_PromptPanel.Children.RemoveRange(num2 + 1, num3 - num2 - 1);
            if (usePlainKeywords)
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < num; i++)
                {
                    stringBuilder.Append(keywords[i].Keyword);
                    if (i != num - 1)
                    {
                        stringBuilder.Append('/');
                    }
                }
                PART_PlainKeywords.Text = stringBuilder.ToString();
                PART_PlainKeywords.Visibility = Visibility.Visible;
            }
            else
            {
                PART_PlainKeywords.Visibility = Visibility.Collapsed;
                for (int j = 0; j < num; j++)
                {
                    ClickableKeyword clickableKeyword = new ClickableKeyword
                    {
                        Focusable = false,
                        Keyword = keywords[j].Keyword,
                        KeywordToHighlight = keywords[j].Highlight,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    if (mSetAutomationId)
                    {
                        string value = "CmdOpt" + keywords[j].Keyword;
                        AutomationProperties.SetAutomationId(clickableKeyword, value);
                    }
                    clickableKeyword.Click += OnKeywordClick;
                    PART_PromptPanel.Children.Insert(num2 + 1 + j, clickableKeyword);
                }
            }
            Visibility visibility = (num == 0) ? Visibility.Collapsed : Visibility.Visible;
            TextBlock pART_BeginOfKeywords = PART_BeginOfKeywords;
            Visibility visibility3 = PART_EndOfKeywords.Visibility = visibility;
            pART_BeginOfKeywords.Visibility = visibility3;
        }

        private void OnKeywordClick(object sender, RoutedEventArgs e)
        {

        }

        private void General_ToolTipOpening(object sender, ToolTipEventArgs e)
        {

        }

        private void ContextMenu_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void InputTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!IsValid) return;
            //mCommandLine.Select(mCommandLine.InputText.Length, mCommandLine.InputText.Length);
            if (!mAutoCompletePopup.ContextMenu.IsOpen && !mAutoCompletePopup.IsKeyboardFocusWithin)
            {
                mCommandLine.HintViewerVisible = false;
            }
        }

        private void InputTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //Win32.ShowCursor((IntPtr)1);
            if (mAutoCompletePopup.ContextMenu.IsOpen)
            {
                mAutoCompletePopup.ContextMenu.Closed += AutoCompleteContextMenu_Closed;
            }
            else if (mCommandLine != null && !mAutoCompletePopup.IsKeyboardFocusWithin)
            {
                mCommandLine.HintViewerVisible = false;
            }
        }


        private void AutoCompleteContextMenu_Closed(object sender, RoutedEventArgs e)
        {

        }

        private string GetSelection()
        {
            return "";
        }

        public void Resize(double width, double height)
        {

        }

        #endregion

        #region Public Method

        #endregion



    }
}
