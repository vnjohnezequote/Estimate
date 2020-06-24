// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLineViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   The command line view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using DrawingModule.CommandLine;
using DrawingModule.Enums;
using DrawingModule.Helper;
using DrawingModule.Interface;
using Prism.Commands;
using RawInputManager.Wind32;

namespace DrawingModule.ViewModels
{
    using Prism.Mvvm;

    /// <summary>
    /// The command line view model.
    /// </summary>
    public class CommandLineViewModel : BindableBase,ICommandLine
    {
        #region Private
        private class RecentCommand : ICommand
        {
            public string Name
            {
                get;
                set;
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                _ = this.CanExecuteChanged;
                return true;
            }

            public void Execute(object parameter)
            {
                ExecuteCommand(Name + "\n", echo: true);
            }
        }

        private class PromptKeyword : IPromptKeyword
        {
            public string Keyword
            {
                get;
                set;
            }

            public string Highlight
            {
                get;
                set;
            }

            public override string ToString()
            {
                return Keyword;
            }
        }

        private bool mHintViewerVisible;

        private static CommandLineViewModel mInstance;

        private const uint IDS_OPEN_READ_ONLY = 276u;

        ////private static bool mIsOEMReadOnly = Utils.IsFlagOn(276u, bDefault: false);

        private bool mIsDocked;

        private bool mIsFloating;

        private bool mIsLocked;

        private CommandEditor mCommandEditor;

        private PromptAndInput mPromptAndInput;

        private InputBuffer mInputBuffer;

        private HintListTimer mHintListTimer;

        private bool mNeedRefreshWithHistoryUpdate;

        private DispatcherTimer mNotifyTimer;

        private readonly List<string> mPropertiesToNotify = new List<string>();

        private readonly ObservableCollection<ICommand> mRecentCommands = new ObservableCollection<ICommand>();

        private string promptString;

        private readonly ObservableCollection<IPromptKeyword> mPromptKeywords = new ObservableCollection<IPromptKeyword>();

        private static readonly Dictionary<string, string> mKeywordCache = new Dictionary<string, string>();
        private HintViewerViewModel HintViewerVM => HintViewerViewModel.Instance;

        private bool mNeedRefreshHints;

        private string mUserInput;

        private bool mAllowAppend = true;

        private bool mStartAppend;

        private string mSelection = string.Empty;

        private readonly ICommand mCutCommand;

        private readonly ICommand mCopyCommand;

        private readonly ICommand mPasteCommand;

        private readonly ICommand mPasteToCommandLineCommand;

        public readonly bool mUsePlainKeywords;

        private bool mIsExpandedHistoryOpen;

        private static bool mIsOEMReadOnly = false;



        #endregion

        #region Public
        private HintListMode CurrentHintListMode
        {
            get;
            set;
        }
        private CommandLineSearcher Searcher
        {
            get;
            set;
        }
        public bool HintViewerVisible
        {
            get => mHintViewerVisible;
            set => this.SetProperty(ref mHintViewerVisible, value);
        }

        private StringCollection BOLCollectionsToInit
        {
            get;
            set;
        }
        public static CommandLineViewModel Instance
        {
            get
            {
                if (mInstance == null)
                {
                    try
                    {
                        mInstance = new CommandLineViewModel();
                    }
                    catch (Exception)
                    {
                        mInstance = null;
                    }
                }
                return mInstance;
            }
        }

        public bool IsOEMReadOnly => mIsOEMReadOnly;
        public bool AreKeytipsActive => ComponentManager.AreKeyTipsVisible;

        //public bool IsMDI => (short)Autodesk.AutoCAD.ApplicationServices.Core.Application.GetSystemVariable("SDI") == 0;
        public bool IsDocked
        {
            get => mIsDocked;
            set => this.SetProperty(ref this.mIsDocked, value);
        }

        public bool IsFloating
        {
            get => mIsFloating;
            set
            {
                this.SetProperty(ref mIsFloating, value);
                if (mIsFloating != value)
                {
                    //UpdateIsLocked();
                }

            }
        }

        public bool IsLocked
        {
            get => mIsLocked;
            private set => this.SetProperty(ref mIsLocked, value);
        }

        public bool NotifyImmediate
        {
            get;
            set;
        } = true;
        private bool IsValid => mInputBuffer != null;
        public bool DisableKoreanImeSpecificLogic
        {
            get;
            set;
        }
        public InputBuffer InputBuffer => this.mInputBuffer;

        public IList<ICommand> RecentCommands => mRecentCommands;

        
        public IList<CommandHistory> HistoryCollection 
        {
            get
            {
                if (!IsValid)
                {
                    return null;
                }
                return mCommandEditor.History;
            }
        }

        public IList<CommandHistory> TempPromptCollection 
        {
            get
            {
                if (!IsValid)
                {
                    return null;
                }
                return mCommandEditor.TemporaryHistory;
            }
        }
        /*
        public ImageSource CurrentCommandIcon
        {
            get
            {
                if (!IsValid)
                {
                    return null;
                }
                return CommandLineItem.ImageFromKey(mCommandEditor.CurrentCommand);
            }
        }*/

        public string CurrentCommand 
        {
            get
            {
            
                if (!IsValid)
                {
                    return string.Empty;
                }
                return mCommandEditor.CurrentCommand;
            }
           
        }

        public bool IsPromptingForInput 
        {
            get
            {
                if (IsValid && string.IsNullOrEmpty(mCommandEditor.CurrentCommand) && string.IsNullOrEmpty(mInputBuffer.Text) && string.IsNullOrEmpty(PromptPrefix) && string.IsNullOrEmpty(PromptPostfix))
                {
                    return mPromptKeywords.Count == 0;
                }
                return false;
            }
        }

        public string PromptPrefix
        {
            get;
            private set;
        }

        public string PromptPostfix
        {
            get;
            private set;
        }

        public IList<IPromptKeyword> PromptKeywords => mPromptKeywords;

        public string InputText
        {
            get
            {
                if (!IsValid)
                {
                    return string.Empty;
                }
                return mInputBuffer.Text;
            }
        }

        public TextSelectionRange SelectionRange
        {
            get
            {
                if (!IsValid)
                {
                    return new TextSelectionRange(0, 0);
                }

                return new TextSelectionRange(mInputBuffer.Position, mInputBuffer.SelectPosition);
                //return new TextSelectionRange(3, 3);
            }
        }
       
        public bool NeedRefreshHints
        {
            get
            {
                return mNeedRefreshHints;
            }
            set
            {
                mNeedRefreshHints = value;
                if (mNeedRefreshHints)
                {
                    mHintListTimer.Pause();
                }
            }
        }

        //private bool SupportsWordCompleteEffectively => GetEffectiveAutoCompleteMode();

        public ICommand Cut => mCutCommand;

        public ICommand Copy => mCopyCommand;

        //public ICommand CopyHistory => new DelegateCommand(DoCopyHistory);

        public ICommand Paste => mPasteCommand;

        public ICommand PasteToCommandLine => mPasteToCommandLineCommand;

        //public ICommand LinesOfPromptHistory => new DelegateCommand(SetLinesOfPromptHistory);

        //public ICommand Options => new Prism.Commands.DelegateCommand(InvokeOptionsDialog);

        //public ICommand SetInputDelayTime => new DelegateCommand(DoSetInputDelayTime);

        //public ICommand InputSearchOptions => new DelegateCommand(InvokeInputSearchOptionsDialog);

        public bool AutoComplete { get; set; }
        /*{
            get
            {
                //return CommandLineSettings.Instance.AutoComplete;
            }
            set
            {
                //CommandLineSettings.Instance.AutoComplete = value;
            }
        }*/

        public bool AutoCorrect { get; set; }
       /* {
            get
            {
                //return CommandLineSettings.Instance.AutoCorrect;
            }
            set
            {
                //CommandLineSettings.Instance.AutoCorrect = value;
            }
        }*/

        public bool SupportAutoCorrect => /*CommandLineSettings.Instance.SupportAutoCorrect*/true;

        public bool AutoAppend => /*CommandLineSettings.Instance.SupportAutoAppend*/true;

        public bool DisplaySysvar { get; set; }
       /* {
            get
            {
                //return CommandLineSettings.Instance.DisplaySysvar;
            }
            set
            {
                //CommandLineSettings.Instance.DisplaySysvar = value;
            }
        }*/

        public bool SearchContent { get; set; }
        /*{
            get
            {
                //return CommandLineSettings.Instance.SearchContent;
            }
            set
            {
                //CommandLineSettings.Instance.SearchContent = value;
            }
        }*/

        public bool MidString { get; set; }
       /* {
            get
            {
                //return CommandLineSettings.Instance.MidString;
            }
            set
            {
                //CommandLineSettings.Instance.MidString = value;
            }
        }*/

        public bool ImprovePerformance
        {
            get;
            private set;
        }

        public bool IsExpandedHistoryOpen
        {
            get
            {
                return mIsExpandedHistoryOpen;
            }
            set
            {
                if (mIsExpandedHistoryOpen != value)
                {
                    mIsExpandedHistoryOpen = value;
                    //ActuallyNotify("IsExpandedHistoryOpen");
                }
            }
        }

       

        #endregion

        #region Constructor
        public CommandLineViewModel()
        {
            this.InitSuggestionHints();
            this.SetupCommandLineHints();
            Application.Application.UiBindings.CommandEditorManager.PropertyChanged += this.CommandEditorManager_PropertyChanged;
            this.SwitchEditor();
            
        }

        #endregion

        #region Private Method
        private void SwitchEditor()
        {
            this.HintViewerVisible = false;
            if (this.mCommandEditor!= null)
            {
                this.mCommandEditor.PropertyChanged -= this.CommandEditor_PropertyChanged;
                this.mCommandEditor.History.CollectionChanged -= this.CommandEditor_HistoryChanged;
            }

            if (this.mPromptAndInput!= null)
            {
                this.mPromptAndInput.PropertyChanged -= this.PromptAndInput_PropertyChanged;
            }
            
            if (this.mInputBuffer != null)
            {
                this.mInputBuffer.PropertyChanged -= this.InputBuffer_PropertyChanged;
            }

            this.mCommandEditor = Application.Application.UiBindings.CommandEditorManager.ActiveEditor;
            this.mPromptAndInput = null;
            this.mInputBuffer = null;
            if (this.mCommandEditor!= null)
            {
                this.mCommandEditor.PropertyChanged += this.CommandEditor_PropertyChanged;
                this.mCommandEditor.History.CollectionChanged += this.CommandEditor_HistoryChanged;
                this.mPromptAndInput = this.mCommandEditor.PromptAndInput;
            }

            if (this.mPromptAndInput !=null)
            {
                this.mPromptAndInput.PropertyChanged += this.PromptAndInput_PropertyChanged;
                this.mInputBuffer = this.mPromptAndInput.InputBuffer;
            }
            if (this.mInputBuffer != null)
            {
                this.mInputBuffer.PropertyChanged += this.InputBuffer_PropertyChanged;
            }
            this.RefereshBindings();
        }
        
        private void InitSuggestionHints()
        {
            AcHintItem.CmdExecuteHintItem = new DelegateCommand<object>(this.ExecuteHintItem);
            this.Searcher = new CommandLineSearcher();
            this.CurrentHintListMode = HintListMode.None;
            this.HintViewerVM.CurrentHintChanged+= HintViewerVM_CurrentHintChanged;
            Application.Application.Idle += Application_Idle;
        }

        private void Application_Idle(object sender, System.EventArgs e)
        {
            
        }

        private void SetupCommandLineHints()
        {
            //CommandLineSettings.Instance.SettingsChnaged += OnInputSearchOptionsChanged;
            mHintListTimer = new HintListTimer();
            mHintListTimer.Tick += OnHintListTimer;
        }

        private void HintViewerVM_CurrentHintChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            this.UpdateInputBoxText();
        }

        private void OnIdleInitialize(object sender, System.EventArgs e)
        {

        }

        private void UpdateInputBoxText()
        {
            HintItem currentHint = this.HintViewerVM.CurrentHint;
            if (currentHint == null)
            {
                return;
            }
            /*if (!currentHint.IsCommandOrSysvarHint)
            {
                if (currentHint.Type == HintCategoryType.DwgContent && this.mInputBuffer.Text != this.mUserInput)
                {
                    this.mInputBuffer.ReplaceInputQueue(this.mUserInput);
                }
                return;
            }*/
            if (this.CurrentHintListMode != HintListMode.AutoComplete)
            {
                return;
            }
            /*if (!this.SupportsWordCompleteEffectively)
            {
                return;
            }*/
            if (!this.mStartAppend)
            {
                string text = currentHint.Value.ToLower();
                string value = this.mUserInput.ToLower();
                if (text.StartsWith(value))
                {
                    this.mStartAppend = true;
                }
            }
            if (this.mStartAppend)
            {
                this.mInputBuffer.ReplaceInputQueue(currentHint.Value,currentHint.Name);
            }
            this.mInputBuffer.OnSetSelectedTextQueue(this.mUserInput.Length, currentHint.Value.Length);
            this.RaisePropertyChanged(nameof(this.SelectionRange));
        }

        private void ShowSuggestionHints()
        {
           this.HintViewerVM.CleanUp();
            
           HintSearchContext hintSearchContext = new HintSearchContext(this.mUserInput);
           HintSearchResult hintSearchResult = this.Searcher.Search(hintSearchContext);
           IList<HintItem> hints = hintSearchResult.Hints;
           if (hints.Count <= 0)
           {
               this.HintViewerVisible = false;
               this.NeedRefreshHints = false;
               return;
           }
           
           this.CurrentHintListMode = this.GetCurrentHintListMode(hintSearchContext);
           this.mStartAppend = false;
           if (string.Compare(this.mUserInput, this.mHintListTimer.UserInput, true) != 0)
           {
               return;
           }
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
           this.NeedRefreshHints = false;
           this.mStartAppend = true;
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

        private IList<HintItem> GetSuggestionHints()
        {
            HintSearchContext context = new HintSearchContext(mUserInput);
            HintSearchResult hintSearchResult = Searcher.Search(context);
            CurrentHintListMode = GetCurrentHintListMode(context);
            return hintSearchResult.Hints;
        }

        private void ExecuteHintItem(object parameter)
        {

        }

        private void SearchHintItemHelp(object parameter)
        {

        }

        private void SearchHintItemOnInternet(object parameter)
        {
        }

        private void SearchStringOnInternet(string searchContent)
        {

        }

        private bool HandleKeyDownForHintList(ref MSG msg, ref bool handled)
        {
             InputBuffer inputBuffer=Application.Application.UiBindings.CommandEditorManager.ActiveEditor.PromptAndInput.InputBuffer;
            int num = msg.wParam.ToInt32();
            switch (num)
            {
                case 33:
                    handled = true;
                    if (!HintViewerVisible)
                    {
                        return false;
                    }
                    ChangeHintListSelectionByOffset(-1 * HintViewerViewModel.MaxItemsCount, wrappingSelection: false);
                    return true;
                case 34:
                    handled = true;
                    if (!HintViewerVisible)
                    {
                        return false;
                    }
                    ChangeHintListSelectionByOffset(HintViewerViewModel.MaxItemsCount, wrappingSelection: false);
                    return true;
                case 13:
                case 32:
                    HandleEnterAndSpaceKey(ref msg, ref handled);
                    return handled;
                case 27:
                    return true;
                case 38:
                case 40:
                    if (!HintViewerVisible)
                    {
                        return false;
                    }
                    mAllowAppend = true;
                    ChangeHintListSelectionByOffset((num == 40) ? 1 : (-1), wrappingSelection: true);
                    handled = true;
                    return true;
                case 9:
                    if (!HintViewerVisible)
                    {
                        return false;
                    }
                    handled = true;
                    mAllowAppend = true;
                    if (HintViewerVM.HintCategories.Count > 1)
                    {
                        ChangeHintCategorySelection((!Keyboard.Modifiers.HasFlag(ModifierKeys.Shift)) ? true : false);
                    }
                    else
                    {
                        ChangeHintListSelectionByOffset((!Keyboard.Modifiers.HasFlag(ModifierKeys.Shift)) ? 1 : (-1), wrappingSelection: true);
                    }
                    return true;
                case 8:
                case 46:
                    NeedRefreshHints = true;
                    mAllowAppend = false;
                    return false;
                default:
                    return false;
            }
           
        }

        private void HandleEnterAndSpaceKey(ref MSG msg, ref bool handled)
        {
            int num = msg.wParam.ToInt32();
            if (num != 13 && num != 32)
            {
                return;
            }
            if (!this.mInputBuffer.IsPromptingForCommandName())
            {
                return;
            }

            if (this.HintViewerVisible && !this.NeedRefreshHints)
            {
                HintItem currentHint = this.HintViewerVM.CurrentHint;
                if (currentHint == null)
                {
                    this.HintViewerVisible = false;
                    return;
                }
                if (currentHint.IsCommandOrSysVarHint)
                {
                    
                    if (this.CurrentHintListMode == HintListMode.AutoCorrect)
                    {
                        CommandSysvarHintItem commandSysvarHintItem = currentHint as CommandSysvarHintItem;
                        if (commandSysvarHintItem != null)
                        {
                            //AutoCorrectorService.UpdateErrInputCount(this.mInputBuffer.Text, commandSysvarHintItem.GlobalName, commandSysvarHintItem.LocalName);
                        }
                        //this.mInputBuffer.ReplaceInputQueue(commandSysvarHintItem.Value+'\n',commandSysvarHintItem.UnderlyingCommand);
                    }
                    this.mCommandEditor.RecentCommands.Add(currentHint.Name);
                    this.mInputBuffer.ReplaceInputQueue(currentHint.Value+'\n',currentHint.Name);
                    
                    this.mPromptAndInput.Prompt = this.mPromptAndInput.Text;
                    currentHint.Command.Execute(null);
                }
                else if (currentHint.Type == HintCategoryType.DwgContent && currentHint.Command != null)
                {
                    this.mInputBuffer.ReplaceInputQueue(currentHint.Value+'\n',currentHint.Name);
                    this.mPromptAndInput.Prompt = this.mPromptAndInput.Text;
                    currentHint.Command.Execute(currentHint);
                    handled = true;
                }
            }
            else if (!string.IsNullOrEmpty(this.mInputBuffer.Text))
            {
                
                HintSearchContext hintSearchContext = new HintSearchContext(this.mInputBuffer.Text);
                hintSearchContext.FastSearch = true;
                HintSearchResult hr = this.Searcher.Search(hintSearchContext);
                HintItem hintItem = null;
                if (this.GetCurrentHintListMode(hintSearchContext) == HintListMode.AutoComplete)
                {
                    hintItem = hintSearchContext.FirstStartWithItemInAutoComplete;
                }
                if (hintItem == null)
                {
                    hintItem = CommandSysvarSearcher.GetFirstHintItem(hr);
                }
                if (hintItem != null)
                {
                    this.mInputBuffer.ReplaceInputQueue(hintItem.Value+'\n',hintItem.Name);
                }
                else
                {
                    this.mInputBuffer.ReplaceInputQueue(this.mInputBuffer.Text+'\n',null);
                }
            }
            //AutoCorrectorService.IsInputCommand(true);
            this.HintViewerVisible = false;
        }

        private void HandleKeyUpForHintList(ref MSG msg, ref bool handled)
        {
            switch (msg.wParam.ToInt32())
            {
                case 37:
                case 39:
                    return;
                case 9:
                case 38:
                case 40:
                    mHintListTimer.Pause();
                    return;
            }
            if (NeedRefreshHints)
            {
                RestartHintSearchTimer(bForceRestart: false);
            }
        }

        private bool HandleCharForHintList(ref MSG msg, ref bool handled)
        {
            NeedRefreshHints = true;
            switch ((int)msg.wParam)
            {
                case 27:
                    NeedRefreshHints = false;
                    if (HintViewerVisible)
                    {
                        mInputBuffer.ReplaceInputQueue("",null);
                        HintViewerVM.CleanUp();
                        HintViewerVisible = false;
                        handled = true;
                        return true;
                    }
                    break;
                case 9:
                    if (HintViewerVisible)
                    {
                        NeedRefreshHints = false;
                        handled = true;
                        return true;
                    }
                    mAllowAppend = false;
                    break;
                case 8:
                case 46:
                    mAllowAppend = false;
                    break;
                default:
                    mAllowAppend = true;
                    break;
            }
            return false;
        }

        internal void NotifyIMEInput(string text)
        {

        }

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

        private void ChangeHintCategorySelection(bool bMoveForward)
        {
            if (bMoveForward)
            {
                this.HintViewerVM.SelectNextCategory();
                return;
            }
            this.HintViewerVM.SelectPreviousCategory();
        }

        public void UpdateIsLocked()
        {

        }

        private void LockUI_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void CommandEditorManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActiveEditor")
            {
                this.SwitchEditor();
            }
        }

        private void RefereshBindings()
        {
            this.RaisePropertyChanged("HistoryCollection");
            this.RaisePropertyChanged("TempPromptCollection");
            this.RaisePropertyChanged("IsPromptingForInput");
            this.RaisePropertyChanged("CurrentCommand");
            this.RaisePropertyChanged("CurrentCommandIcon");
            this.UpdatePrompt();
            this.RaisePropertyChanged("InputText");
            this.RaisePropertyChanged("SelectionRange");
        }

        private void InputBuffer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string propertyName = e.PropertyName;
            if (propertyName == "Text")
            {
                this.RaisePropertyChanged("IsPromptingForInput");
                this.RaisePropertyChanged("InputText");
                //return;
            }
            if (!(propertyName == "Position") && !(propertyName == "SelectPosition"))
            {
                return;
            }
            this.RaisePropertyChanged(nameof(SelectionRange));
        }

        private void PromptAndInput_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void CommandEditor_HistoryChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }

        private void CommandEditor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string propertyName = e.PropertyName;
            if (propertyName == nameof(this.CurrentCommand))
            {
                this.RaisePropertyChanged(nameof(this.CurrentCommand));
            }
        }

        public bool ProcessMessages(ref MSG msg, ref bool handled)
        {
            if (!this.IsValid)
            {
                return true;
            }
            if ((msg.message == Win32Consts.WM_SYSKEYDOWN || msg.message == Win32Consts.WM_KEYDOWN || msg.message == Win32Consts.WM_KEYUP || msg.message == Win32Consts.WM_CHAR))
            {
                //handled = Utils.PreRouteMessageToDwgView(msg.hwnd, msg.message, msg.wParam, msg.lParam);
                handled = false;
                if (!handled)
                {
                    this.ProcessMessagesForInput(ref msg, ref handled);
                }
                //Utils.PostRouteMessageToDwgView(msg.hwnd, msg.message, msg.wParam, msg.lParam);
            }
            return true;
        }

        private void ProcessMessagesForInput(ref MSG msg, ref bool handled)
        {
            
            switch (msg.message)
            {
                case 256:
                {

                    int num = msg.wParam.ToInt32();
                    if (!this.HandleKeyDownForHintList(ref msg, ref handled))
                    {
                        this.HandleKeyDownForCommandEdit(ref msg, ref handled);
                    }
                    break;
                }
                case 257:
                    HandleKeyUpForHintList(ref msg, ref handled);
                    break;
                case 258:
                {
                    if (!this.HandleCharForHintList(ref msg, ref handled))
                    {
                        this.HandleCharForCommandEdit(ref msg, ref handled);
                    }
                    if (msg.wParam.ToInt32() == 32)
                    {
                        this.mNeedRefreshWithHistoryUpdate = true;
                    }
                    this.IsExpandedHistoryOpen = false;
                    return;
                }
                case 259:
                    break;
                case 260:
                    HandleKeyDownForCommandEdit(ref msg, ref handled);
                    break;
                default:
                    return;
            }

        }

        private void HandleKeyDownForCommandEdit(ref MSG msg, ref bool handled)
        {
            
            uint inputChar = (uint)msg.wParam.ToInt32();
            long num = (long)msg.lParam;
            uint repCnt = (uint)(num & 65535L);
            uint flags = (uint)(num & 65535L);
            if (msg.message == 260)
            {
                handled = this.mInputBuffer.OnSysKeyDownQueue(inputChar, repCnt, flags);
                return;
            }
            if (msg.message == 256)
            {
                this.mInputBuffer.OnKeyDownQueue(inputChar, repCnt, flags);
            }

        }

        private void HandleCharForCommandEdit(ref MSG msg, ref bool handled)
        {
            uint num = (uint)msg.wParam.ToInt32();
            long num2 = (long)msg.lParam;
            uint repCnt = (uint)(num2 & 0xFFFF);
            uint flags = (uint)(num2 & 0xFFFFF);
            /*if (CommandLineService.IsInKoreanImeMode() && (num == 8u || (num >= 33u && num <= 126u)))
            {
                handled = false;
                return;
            }*/
            handled = this.mInputBuffer.OnCharQueue(num, repCnt, flags);
        }

         private void UpdatePrompt()
        {
            if (!this.IsValid)
            {
                return;
            }
            int keywordsStringStart = this.mPromptAndInput.KeywordsStringStart;
            if (keywordsStringStart != -1&& keywordsStringStart!=0)
            {
                
            }
            else
            {
                this.PromptPrefix = this.mPromptAndInput.Prompt;
                this.PromptPostfix = string.Empty;
            }
            
            if (string.IsNullOrEmpty(this.promptString))
            {
                this.promptString = Utils.GetCommandPromptString().TrimStart(new char[]
                {
                    '\n',
                    '\r'
                });

            }
            if (this.PromptPrefix.StartsWith(this.promptString))
            {
                this.PromptPrefix = this.PromptPrefix.Substring(this.promptString.Length);
            }
            this.mPromptKeywords.Clear();
            foreach (string keyword in this.mPromptAndInput.PromptKeywords)
            {
                CommandLineViewModel.PromptKeyword promptKeyword = new CommandLineViewModel.PromptKeyword();
                promptKeyword.Keyword = keyword;
                promptKeyword.Highlight = this.GetKeywordAbbreviation(keyword);
                this.mPromptKeywords.Add(promptKeyword);
            }
            this.RaisePropertyChanged("IsPromptingForInput");
            this.RaisePropertyChanged("PromptPrefix");
            this.RaisePropertyChanged("PromptPostfix");
            /*if (this.mCommandEditor.NeedShowClickableKeywords)
            {
                this.RaisePropertyChanged("PromptKeywords");
            }*/
        }

        private string GetKeywordAbbreviation(string keyword)
        {
            return "";
        }

        public bool IsDroppableExtension(string extension)
        {
            return true;
        }

        public void DropOpenFile(string filePath)
        {

        }

        public ToolTip ConvertToToolTip(string content)
        {
            return null;
        }

        public void Select(int start, int end)
        {
            if (this.IsValid)
            {
                this.mInputBuffer.OnSetSelectedTextQueue(start, end);
            }
        }

        public void PasteString(string stringToPaste)
        {

        }

        public void ExecuteKeyword(string keyword)
        {

        }

        private void OnInputSearchOptionsChanged(object sender, PropertyChangedEventArgs e)
        {
		
        }

        public void RestartHintSearchTimer(bool bForceRestart)
        {
            if ((CommandLineSettings.Instance.AutoComplete || CommandLineSettings.Instance.AutoCorrect || CommandLineSettings.Instance.SearchContent) && mInputBuffer.IsPromptingForCommandName())
            {
                mHintListTimer.Restart(HintViewerVisible, bForceRestart);
            }
        }

        private void OnHintListTimer(object sender, System.EventArgs e)
        {
            if (!this.IsValid)
            {
                return;
            }

            if (string.Compare(this.mUserInput,this.mHintListTimer.UserInput,true)!=0)
            {
                this.mUserInput = this.mHintListTimer.UserInput;
            }
            if (string.IsNullOrEmpty(this.mUserInput))
            {
                this.HintViewerVM.CleanUp();
                this.HintViewerVisible = false;
                return;
            }
            //Neu khong phai la Prompting cho Command Name thi return
            if (!this.mInputBuffer.IsPromptingForCommandName())
            {
                return;
            }
            if (CommandLineSettings.Instance.DisplaySuggestionList)
            {
                this.ShowSuggestionHints();
                return;
            }
            
            this.HintViewerVisible = false;
            //if (this.SupportsWordCompleteEffectively)
            //{
                this.mInputBuffer.OnCharQueue(9u, 0u, 0u);
                this.mInputBuffer.OnSetSelectedTextQueue(this.mUserInput.Length, 9999);
            /*}*/
            //this.mUserInput = this.InputBuffer.Text;
            //this.HintViewerVM.Text = this.InputBuffer.Text;
            /*
            if (String.IsNullOrEmpty(this.mUserInput))
            {
                this.HintViewerVM.CleanUp();
                this.HintViewerVisible = false;
                return;
            }*/
            //this.ShowSuggestionHints();
        }

        private bool GetEffectiveAutoCompleteMode()
        {
            return true;
        }

        private bool NextCharWillAppendedToEnd()
        {
            return true;
        }

        public void UpdateSelectionForMenu(string selection)
        {
		
        }

        public void UpdateContextMenu()
        {
		
        }

        public void CloseCommandLine()
        {
		
        }

        private void CopyTextSelected()
        {
		
        }

        public void DoCut()
        {
		
        }

        public void DoCopy()
        {
		
        }

        private void DoCopyHistory()
        {
		
        }

        public void DoPaste()
        {
		
        }

        private void DoPasteToCommandLine()
        {
		
        }

        private void SetLinesOfPromptHistory()
        {
		
        }

        private void InvokeOptionsDialog()
        {
		
        }

        private void DoSetInputDelayTime()
        {
		
        }

        private void InvokeInputSearchOptionsDialog()
        {
		
        }

        private static void ExecuteCommand(string command, bool echo)
        {
		
        }

        public void ClearTempPrompt(bool tillNextCommand)
        {
		
        }


        #endregion

        #region Public Method

        

        #endregion
    }
}
