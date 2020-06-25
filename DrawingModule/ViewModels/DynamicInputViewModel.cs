﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ApplicationCore.BaseModule;
using ApplicationService;
using DrawingModule.CommandLine;
using DrawingModule.Enums;
using DrawingModule.Helper;
using DrawingModule.Interface;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Unity;

namespace DrawingModule.ViewModels
{
    public class DynamicInputViewModel : BaseViewModel, ICommandLine
    {
        #region Field

        private readonly Dispatcher _dispatcher;
        public bool _hintViewerVisible;
        private string _commandTextInput=string.Empty;
        private IDrawInteractive _curentTool;
        private string _lastCommandString=string.Empty;
        public ICadDrawAble CurrentCanvas { get; private set; }
        private bool IsValid => mInputBuffer != null;
        
        private InputBuffer mInputBuffer;
        
        private HintViewerViewModel HintViewerVM
        {
            get
            {
                return HintViewerViewModel.Instance;
            }
        }
        private HintListMode CurrentHintListMode
        {
            get;
            set;
        }
        private CommandLineSearcher Searcher { get; set; }
        
        private CommandEditor mCommandEditor;
        private PromptAndInput mPromptAndInput;
        #endregion

        #region Visibility Region
        public Visibility DynamicInputVisibility
        {
            get => this._dynamicInputVisibility;
            set => this.SetProperty(ref _dynamicInputVisibility, value);
        }
        public Visibility ToolNameVisibility => string.IsNullOrEmpty(ToolName) ? Visibility.Collapsed : Visibility.Visible;
        public Visibility ToolMessageVisibility => string.IsNullOrEmpty(ToolMessage) ? Visibility.Collapsed : Visibility.Visible;
        private Visibility _dynamicInputVisibility = Visibility.Collapsed;
        internal IDrawInteractive CurrentTool
        {
            get => _curentTool;
            set => SetProperty(ref _curentTool, value);
        }
        public Visibility CommandTextVisibility => this.CurrentTool == null ? Visibility.Visible : Visibility.Collapsed;
        public Visibility LengthVisibility
        {
            get
            {
                if (this._curentTool == null)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return this._curentTool.IsUsingLengthTextBox ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
        public Visibility HeightVisibility
        {
            get
            {
                if (this._curentTool == null)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return this._curentTool.IsUsingHeightTextBox ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
        public Visibility WidthVisibility
        {
            get
            {
                if (this._curentTool == null)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return this._curentTool.IsUsingWidthTextBox ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
        public Visibility AngleVisibility
        {
            get
            {
                if (this._curentTool == null)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return this._curentTool.IsUsingAngleTextBox ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
        public Visibility TextContentVisibility
        {
            get
            {
                if (this._curentTool == null)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return this._curentTool.IsUsingTextStringTextBox ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
        public Visibility TextStringHeightVisibility
        {
            get
            {
                if (this._curentTool == null)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return this._curentTool.IsUsingTextStringHeightTextBox ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
        public Visibility TextStringAngleVisibility
        {
            get
            {
                if (this._curentTool == null)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return this._curentTool.IsUsingTextStringAngleTextBox ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
        public Visibility LeaderSegmentVisibility
        {
            get
            {
                if (this._curentTool == null)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return this._curentTool.IsUsingLeaderSegmentTextBox ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
        public Visibility ArrowHeadSizeVisibility
        {
            get
            {
                if (this._curentTool == null)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return this._curentTool.IsUsingArrowHeadSizeTextBox ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }


        #endregion
        #region Drawing Properties Region
        public string LengthDimension
        {
            get
            {
                if (CurrentCanvas == null)
                {
                    return "0";
                }
                else
                {
                    return CurrentCanvas.CurrentLengthDimension.ToString(CultureInfo.InvariantCulture);
                }

            }
            set
            {
                var checkConvertDouble = Utils.ConvertStringToDouble(value, out var outputDouble);
                if (checkConvertDouble && Math.Abs(outputDouble) > 0.0001 & this.CurrentCanvas!=null)
                {
                    this.CurrentCanvas.UpdateCurrentPointByLengthAndAngle(outputDouble, Convert.ToDouble(AngleDimension));
                }
            } 
        }
        public string AngleDimension
        {
            get
            {
                if (CurrentCanvas == null)
                {
                    return "";
                }
                return CurrentCanvas.CurrentAngleDimension.ToString(CultureInfo.CurrentCulture);

            }
            set
            {
                //var inputString = value.ToString(CultureInfo.CurrentCulture);
                var angle = Utils.ConvertAngleStringToDouble(value);
                if (this.CurrentCanvas != null)
                {
                    this.CurrentCanvas.UpdateCurrentPointByLengthAndAngle(Convert.ToDouble(this.LengthDimension), angle);
                }
                

            }
        }
        #endregion
        #region Properties
        public bool IsBusyforTool => CurrentTool != null;
        public string ToolName => _curentTool == null ? string.Empty : _curentTool.ToolName;
        public string ToolMessage => _curentTool == null ? string.Empty : _curentTool.ToolMessage;
        public bool HintViewerVisible
        {
            get => _hintViewerVisible;
            set => this.SetProperty(ref _hintViewerVisible, value);
        }
        public string CommandTextInput
        {
            get=>_commandTextInput;
            set
            {
                this.SetProperty(ref _commandTextInput, value);
                this.OnHintListCommand(value);
            }
        }
        #endregion
        #region Constructor
        public DynamicInputViewModel():base()
        {}
        public DynamicInputViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator)
        : base(unityContainer, regionManager, eventAggregator)
        {
            this.EventAggre.GetEvent<DynamicInputViewEvent>().Subscribe(ShowOrHideDynamicInput);
            this.InitSuggestionHints();
            this._dispatcher = System.Windows.Application.Current.Dispatcher;
            //this.SetupCommandLineHints();
            this.SwitchEditor();


        }
        #endregion

        #region Private Method
        private void SwitchEditor()
        {
            this.HintViewerVisible = false;
            //if (this.mCommandEditor != null)
            //{
            //    this.mCommandEditor.PropertyChanged -= this.CommandEditor_PropertyChanged;
            //    this.mCommandEditor.History.CollectionChanged -= this.CommandEditor_HistoryChanged;
            //}

            //if (this.mPromptAndInput != null)
            //{
            //    this.mPromptAndInput.PropertyChanged -= this.PromptAndInput_PropertyChanged;
            //}

            if (this.mInputBuffer != null)
            {
                //this.mInputBuffer.PropertyChanged -= this.InputBuffer_PropertyChanged;
            }

            this.mCommandEditor = Application.Application.UiBindings.CommandEditorManager.ActiveEditor;
            this.mPromptAndInput = null;
            this.mInputBuffer = null;
            if (this.mCommandEditor != null)
            {
                //this.mCommandEditor.PropertyChanged += this.CommandEditor_PropertyChanged;
                //this.mCommandEditor.History.CollectionChanged += this.CommandEditor_HistoryChanged;
                this.mPromptAndInput = this.mCommandEditor.PromptAndInput;
            }

            if (this.mPromptAndInput != null)
            {
                //this.mPromptAndInput.PropertyChanged += this.PromptAndInput_PropertyChanged;
                this.mInputBuffer = this.mPromptAndInput.InputBuffer;
            }
            if (this.mInputBuffer != null)
            {
                //this.mInputBuffer.PropertyChanged += this.InputBuffer_PropertyChanged;
            }
            //this.RefereshBindings();
        }
        private void InitSuggestionHints()
        {
            AcHintItem.CmdExecuteHintItem = new DelegateCommand<object>(this.ExecuteHintItem);
            this.Searcher = new CommandLineSearcher();
            this.CurrentHintListMode = HintListMode.None;
            this.HintViewerVM.CurrentHintChanged += HintViewerVM_CurrentHintChanged;
            //Application.Idle += Application_Idle;
        }
        private void ExecuteHintItem(object parameter)
        {

        }
        private void HintViewerVM_CurrentHintChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            this.UpdateInputBoxText();
        }
        private void UpdateInputBoxText()
        {
            
        }
        private void OnHintListCommand(string commandText)
        {
            if (string.IsNullOrEmpty(commandText))
            {
                this.HintViewerVM.CleanUp();
                this.HintViewerVisible = false;
                return;
            }
            //if (!this.mInputBuffer.IsPromptingForCommandName())
            //{
            //    return;
            //}
            if (CommandLineSettings.Instance.DisplaySuggestionList)
            {
                this.ShowSuggestionHints();
                return;
            }
        }
        private void ShowOrHideDynamicInput(bool isVisible)
        {
            if (isVisible == true)
            {
                this.DynamicInputVisibility = Visibility.Visible;
            }
            else
            {
                this.DynamicInputVisibility = Visibility.Collapsed;
            }
        }
        private HintListMode GetCurrentHintListMode(HintSearchContext context)
        {
            if (context.HasResult(context.AutoCompleteSearchResult))
            {
                return HintListMode.AutoComplete;
            }
            if (context.HasResult(context.AutoCorrectSearchResult))
            {
                return HintListMode.AutoCorrect;
            }
            return HintListMode.None;
        }
        private void ShowSuggestionHints()
        {
            this.HintViewerVM.CleanUp();

            HintSearchContext hintSearchContext = new HintSearchContext(this.CommandTextInput);
            HintSearchResult hintSearchResult = this.Searcher.Search(hintSearchContext);
            IList<HintItem> hints = hintSearchResult.Hints;
            if (hints.Count <= 0)
            {
                this.HintViewerVisible = false;
                return;
            }

            this.CurrentHintListMode = this.GetCurrentHintListMode(hintSearchContext);

            foreach (HintItem hint in hints)
            {
                this.HintViewerVM.AddHint(hint);
            }
            this.HintViewerVisible = true;
            if (hintSearchResult.HundredPercentMatch != null)
            {
                this.HintViewerVM.CurrentHint = hintSearchResult.HundredPercentMatch;
            }
            else if (this.CurrentHintListMode == HintListMode.AutoComplete)
            {
                HintItem firstStartWithItemInAutoComplete = hintSearchContext.FirstStartWithItemInAutoComplete;
                if (firstStartWithItemInAutoComplete != null)
                {
                    this.HintViewerVM.CurrentHint = firstStartWithItemInAutoComplete;
                }
                else
                {
                    this.HintViewerVM.SelectFirstCategory();
                }
            }
            else if (this.CurrentHintListMode == HintListMode.AutoCorrect)
            {
                if (hintSearchContext.HasResult(hintSearchContext.DwgContentSearchResult))
                {
                    this.HintViewerVM.SelectCategory(HintCategoryType.DwgContent);
                }
                else
                {
                    this.HintViewerVM.CurrentHint = hintSearchContext.AutoCorrectSearchResult.Hints[0];
                    if (!hintSearchContext.AutoCorrectHasClearWinner)
                    {
                        this.HintViewerVM.CurrentHint = null;
                    }
                }
            }
            else
            {
                this.HintViewerVM.SelectFirstCategory();
            }
            
        }
        //public bool ProcessMessages(ref MSG msg, ref bool handled)
        //{
        //    if (!this.IsValid)
        //    {
        //        return true;
        //    }
        //    if ((msg.message == Win32Consts.WM_SYSKEYDOWN || msg.message == Win32Consts.WM_KEYDOWN || msg.message == Win32Consts.WM_KEYUP || msg.message == Win32Consts.WM_CHAR))
        //    {
        //        //handled = Utils.PreRouteMessageToDwgView(msg.hwnd, msg.message, msg.wParam, msg.lParam);
        //        handled = false;
        //        if (!handled)
        //        {
        //            this.ProcessMessagesForInput(ref msg, ref handled);
        //        }
        //        //Utils.PostRouteMessageToDwgView(msg.hwnd, msg.message, msg.wParam, msg.lParam);
        //    }
        //    return true;
        //}
        //private void ProcessMessagesForInput(ref MSG msg, ref bool handled)
        //{

        //    switch (msg.message)
        //    {
        //        case 256:
        //        {

        //            int num = msg.wParam.ToInt32();
        //            if (!this.HandleKeyDownForHintList(ref msg, ref handled))
        //            {
        //                this.HandleKeyDownForCommandEdit(ref msg, ref handled);
        //            }
        //            break;
        //        }
        //        case 257:
        //            HandleKeyUpForHintList(ref msg, ref handled);
        //            break;
        //        case 258:
        //        {
        //            if (!this.HandleCharForHintList(ref msg, ref handled))
        //            {
        //                this.HandleCharForCommandEdit(ref msg, ref handled);
        //            }
        //            if (msg.wParam.ToInt32() == 32)
        //            {
        //                //this.mNeedRefreshWithHistoryUpdate = true;
        //            }
        //            //this.IsExpandedHistoryOpen = false;
        //            return;
        //        }
        //        case 259:
        //            break;
        //        case 260:
        //            HandleKeyDownForCommandEdit(ref msg, ref handled);
        //            break;
        //        default:
        //            return;
        //    }

        //}
        //private void HandleKeyUpForHintList(ref MSG msg, ref bool handled)
        //{
        //    switch (msg.wParam.ToInt32())
        //    {
        //        case 37:
        //        case 39:
        //            return;
        //        case 9:
        //        case 38:
        //        case 40:
        //            //mHintListTimer.Pause();
        //            return;
        //    }
        //    //if (NeedRefreshHints)
        //    //{
        //    //    RestartHintSearchTimer(bForceRestart: false);
        //    //}
        //}
        private void ChangeHintListSelectionByOffset(int offset, bool wrappingSelection)
        {
            if (this.HintViewerVM.CurrentCategory == null)
            {
                return;
            }
            if (offset == 1)
            {
                this.HintViewerVM.SelectNextHintInCurrentCategory();
                return;
            }
            if (offset == -1)
            {
                this.HintViewerVM.SelectPreviousHintInCurrentCategory();
                return;
            }
            this.HintViewerVM.SelectHintInCurrentCategoryByOffset(offset);
        }
        //private void ChangeHintCategorySelection(bool bMoveForward)
        //{
        //    if (bMoveForward)
        //    {
        //        this.HintViewerVM.SelectNextCategory();
        //        return;
        //    }
        //    this.HintViewerVM.SelectPreviousCategory();
        //}
        //private void HandleKeyDownForCommandEdit(ref MSG msg, ref bool handled)
        //{

        //    uint inputChar = (uint)msg.wParam.ToInt32();
        //    long num = (long)msg.lParam;
        //    uint repCnt = (uint)(num & 65535L);
        //    uint flags = (uint)(num & 65535L);
        //    if (msg.message == 260)
        //    {
        //        handled = this.mInputBuffer.OnSysKeyDownQueue(inputChar, repCnt, flags);
        //        return;
        //    }
        //    if (msg.message == 256)
        //    {
        //        this.mInputBuffer.OnKeyDownQueue(inputChar, repCnt, flags);
        //    }

        //}
        //private bool HandleKeyDownForHintList(ref MSG msg, ref bool handled)
        //{
        //    InputBuffer inputBuffer = Application.Application.UiBindings.CommandEditorManager.ActiveEditor.PromptAndInput.InputBuffer;
        //    int num = msg.wParam.ToInt32();
        //    switch (num)
        //    {
        //        case 33:
        //            handled = true;
        //            if (!HintViewerVisible)
        //            {
        //                return false;
        //            }
        //            ChangeHintListSelectionByOffset(-1 * HintViewerViewModel.MaxItemsCount, wrappingSelection: false);
        //            return true;
        //        case 34:
        //            handled = true;
        //            if (!HintViewerVisible)
        //            {
        //                return false;
        //            }
        //            ChangeHintListSelectionByOffset(HintViewerViewModel.MaxItemsCount, wrappingSelection: false);
        //            return true;
        //        case 13:
        //        case 32:
        //            HandleEnterAndSpaceKey(ref msg, ref handled);
        //            return handled;
        //        case 27:
        //            return true;
        //        case 38:
        //        case 40:
        //            if (!HintViewerVisible)
        //            {
        //                return false;
        //            }
        //            //mAllowAppend = true;
        //            ChangeHintListSelectionByOffset((num == 40) ? 1 : (-1), wrappingSelection: true);
        //            handled = true;
        //            return true;
        //        case 9:
        //            if (!HintViewerVisible)
        //            {
        //                return false;
        //            }
        //            handled = true;
        //            //mAllowAppend = true;
        //            if (HintViewerVM.HintCategories.Count > 1)
        //            {
        //                ChangeHintCategorySelection((!Keyboard.Modifiers.HasFlag(ModifierKeys.Shift)) ? true : false);
        //            }
        //            else
        //            {
        //                ChangeHintListSelectionByOffset((!Keyboard.Modifiers.HasFlag(ModifierKeys.Shift)) ? 1 : (-1), wrappingSelection: true);
        //            }
        //            return true;
        //        case 8:
        //        case 46:
        //            //NeedRefreshHints = true;
        //            //mAllowAppend = false;
        //            return false;
        //        default:
        //            return false;
        //    }

        //}
        //private void HandleCharForCommandEdit(ref MSG msg, ref bool handled)
        //{
        //    uint num = (uint)msg.wParam.ToInt32();
        //    long num2 = (long)msg.lParam;
        //    uint repCnt = (uint)(num2 & 0xFFFF);
        //    uint flags = (uint)(num2 & 0xFFFFF);
        //    /*if (CommandLineService.IsInKoreanImeMode() && (num == 8u || (num >= 33u && num <= 126u)))
        //    {
        //        handled = false;
        //        return;
        //    }*/
        //    handled = this.mInputBuffer.OnCharQueue(num, repCnt, flags);
        //}
        //private bool HandleCharForHintList(ref MSG msg, ref bool handled)
        //{
        //    //NeedRefreshHints = true;
        //    switch ((int)msg.wParam)
        //    {
        //        case 27:
        //            //NeedRefreshHints = false;
        //            if (HintViewerVisible)
        //            {
        //                mInputBuffer.ReplaceInputQueue("", null);
        //                HintViewerVM.CleanUp();
        //                HintViewerVisible = false;
        //                handled = true;
        //                return true;
        //            }
        //            break;
        //        case 9:
        //            if (HintViewerVisible)
        //            {
        //                //NeedRefreshHints = false;
        //                handled = true;
        //                return true;
        //            }
        //            //mAllowAppend = false;
        //            break;
        //        case 8:
        //        case 46:
        //            //mAllowAppend = false;
        //            break;
        //        default:
        //            //mAllowAppend = true;
        //            break;
        //    }
        //    return false;
        //}
        //private void HandleEnterAndSpaceKey(ref MSG msg, ref bool handled)
        //{
        //    int num = msg.wParam.ToInt32();
        //    if (num != 13 && num != 32)
        //    {
        //        return;
        //    }
        //    if (!this.mInputBuffer.IsPromptingForCommandName())
        //    {
        //        return;
        //    }

        //    if (this.HintViewerVisible /*&& !this.NeedRefreshHints*/)
        //    {
        //        HintItem currentHint = this.HintViewerVM.CurrentHint;
        //        if (currentHint == null)
        //        {
        //            this.HintViewerVisible = false;
        //            return;
        //        }
        //        if (currentHint.IsCommandOrSysVarHint)
        //        {
        //            CommandSysvarHintItem commandSysvarHintItem = currentHint as CommandSysvarHintItem;

        //            if (this.CurrentHintListMode == HintListMode.AutoCorrect)
        //            {
                        
        //                if (commandSysvarHintItem != null)
        //                {
        //                    //AutoCorrectorService.UpdateErrInputCount(this.mInputBuffer.Text, commandSysvarHintItem.GlobalName, commandSysvarHintItem.LocalName);
        //                }
        //                //this.mInputBuffer.ReplaceInputQueue(commandSysvarHintItem.Value+'\n',commandSysvarHintItem.UnderlyingCommand);
                        
        //            }
        //            this.mInputBuffer.ReplaceInputQueue(currentHint.Value + '\n', currentHint.Name);
        //            this.EventAggre.GetEvent<CommandExcuteStringEvent>().Publish(commandSysvarHintItem.GlobalName);
        //            handled = true;

        //            //currentHint.Command.Execute(null);
        //        }
        //        else if (currentHint.Type == HintCategoryType.DwgContent && currentHint.Command != null)
        //        {
        //            this.mInputBuffer.ReplaceInputQueue(currentHint.Value + '\n', currentHint.Name);
        //            this.mPromptAndInput.Prompt = this.mPromptAndInput.Text;
        //            //currentHint.Command.Execute(currentHint);
                    
        //            handled = true;
        //        }
        //    }
        //    else if (string.IsNullOrEmpty(CommandTextInput))
        //    {
        //        if (this.mCommandEditor!=null && this.mCommandEditor.RecentCommands.Count!=0)
        //        {
        //            HintSearchContext hintSearchContext = new HintSearchContext(this.mCommandEditor.RecentCommands.Last());
        //            hintSearchContext.FastSearch = true;
        //            HintSearchResult hr = this.Searcher.Search(hintSearchContext);
        //            HintItem hintItem = null;
        //            if (this.GetCurrentHintListMode(hintSearchContext) == HintListMode.AutoComplete)
        //            {
        //                hintItem = hintSearchContext.FirstStartWithItemInAutoComplete;
        //            }
        //            if (hintItem == null)
        //            {
        //                hintItem = CommandSysvarSearcher.GetFirstHintItem(hr);
        //            }
        //            if (hintItem != null)
        //            {
        //                this.mInputBuffer.ReplaceInputQueue(hintItem.Value + '\n', hintItem.Name);
        //                hintItem.Command.Execute(null);
        //            }
        //            else
        //            {
        //                this.mInputBuffer.ReplaceInputQueue(this.mInputBuffer.Text + '\n', null);
        //            }
        //        }

                
        //    }
        //    //AutoCorrectorService.IsInputCommand(true);
        //    this.HintViewerVisible = false;
        //    this.CommandTextInput = String.Empty;
        //}
        private void HandleEnterAndSpaceKey()
        {
            if (this.IsBusyforTool)
            {
                return;
            }
            if (this.HintViewerVisible)
            {
                HintItem currentHint = this.HintViewerVM.CurrentHint;
                if (currentHint == null)
                {
                    this.HintViewerVisible = false;
                    return;
                }
                if (currentHint.IsCommandOrSysVarHint)
                {
                    var commandSysvarHintItem = currentHint as CommandSysvarHintItem;
                    if (commandSysvarHintItem != null)
                        this.EventAggre.GetEvent<CommandExcuteStringEvent>().Publish(commandSysvarHintItem.GlobalName);
                    if (commandSysvarHintItem != null) this._lastCommandString = commandSysvarHintItem.GlobalName;
                }
            }
            else if (string.IsNullOrEmpty(CommandTextInput) && !string.IsNullOrEmpty(_lastCommandString))
            {
                //this.EventAggre.GetEvent<CommandExcuteStringEvent>().Publish(_lastCommandString);
            }
            this.HintViewerVisible = false;
            this.CommandTextInput = string.Empty;
        }
        public void NotifyToolChanged()
        {
            RaisePropertyChanged(nameof(ToolName));
            RaisePropertyChanged(nameof(ToolMessage));
            RaisePropertyChanged(nameof(ToolNameVisibility));
            RaisePropertyChanged(nameof(ToolMessageVisibility));
            RaisePropertyChanged(nameof(CommandTextVisibility));
            RaisePropertyChanged(nameof(TextContentVisibility));
            RaisePropertyChanged(nameof(LeaderSegmentVisibility));
            RaisePropertyChanged(nameof(TextStringAngleVisibility));
            RaisePropertyChanged(nameof(TextStringHeightVisibility));
            RaisePropertyChanged(nameof(LengthVisibility));
            RaisePropertyChanged(nameof(WidthVisibility));
            RaisePropertyChanged(nameof(HeightVisibility));
            RaisePropertyChanged(nameof(AngleVisibility));
            RaisePropertyChanged(nameof(ArrowHeadSizeVisibility));
        }

        private void OnToolTabEvent()
        {

        }
        #endregion
        #region public method
        internal void SetCurrentTool(IDrawInteractive currentTool)
        {
           this.CurrentTool = currentTool;
           NotifyToolChanged();
        }

        internal void SetCurrentCanvas(ICadDrawAble cadDraw)
        {
            switch (CurrentCanvas)
            {
                case null when cadDraw ==null:
                    return;
                case null when true:
                    CurrentCanvas = cadDraw;
                    CurrentCanvas.PropertyChanged += CurrentCanvasOnPropertyChanged;
                    break;
                default:
                {
                    if (CurrentCanvas != null && cadDraw == null)
                    {
                        CurrentCanvas.PropertyChanged -= CurrentCanvasOnPropertyChanged;
                        CurrentCanvas = null;
                    }
                    else if(CurrentCanvas!=null && cadDraw!=null)
                    {
                        CurrentCanvas.PropertyChanged -= CurrentCanvasOnPropertyChanged;
                        CurrentCanvas = cadDraw;
                        CurrentCanvas.PropertyChanged += CurrentCanvasOnPropertyChanged;
                    }

                    break;
                }
            }
        }

        private void CurrentCanvasOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
           
            RaisePropertyChanged(nameof(LengthDimension));
            RaisePropertyChanged(nameof(AngleDimension));
            //UpdateSelectAllTExtInTextBox();
            
        }

       
        public bool ProcessKeyDown(KeyEventArgs e)
        {
            e.Handled = false;
            switch (e.Key)
            {
                case Key.Enter:
                    this.HandleEnterAndSpaceKey();
                    e.Handled = true;
                    return true;
                case Key.Up:
                    if (!HintViewerVisible)
                    {
                        e.Handled = false;
                        return false;
                    }
                    ChangeHintListSelectionByOffset(-1, wrappingSelection: true);
                    e.Handled = true;
                    return true;
                case Key.Down:
                    if (!HintViewerVisible)
                    {
                        e.Handled = false;
                        return false;
                    }
                    ChangeHintListSelectionByOffset(1, wrappingSelection: true);
                    e.Handled = true;
                    return true;
                case Key.Escape:
                    this.ResetProcesscingTool();
                    e.Handled = true;
                    return true;
            }

            return false;
        }
        public void ResetProcesscingTool()
        {
            this._dispatcher.Invoke((Action)(() =>
            {//this refer to form in WPF application 
                this.CommandTextInput = string.Empty;
                HintViewerVM.CleanUp();
                HintViewerVisible = false;
            }));

        }
        #endregion
    }
}