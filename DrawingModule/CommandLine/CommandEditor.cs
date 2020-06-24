// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandEditor.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the <see cref="CommandEditor" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DrawingModule.Application;
using DrawingModule.ViewModels;
using Prism.Mvvm;

namespace DrawingModule.CommandLine
{
    /// <summary>
    /// Defines the <see cref="CommandEditor" />
    /// </summary>
    public sealed class CommandEditor : BindableBase
    {
        #region Private Fields

        private bool mIsBusy;
        private bool mbEnableTempPrompt;

        private string mCurrentCommand;

        private string mCurrentCommandLocalName;

        private IntPtr mCurrentCommandIcon;

        private int mTempPromptCount;

        private int mCurrentCommandHistoryCount;

        private bool mbMissedCommandHistoryItems;

        private PromptAndInput mPromptAndInput;

        private List<string> mRecentCommands;

        private ObservableCollection<CommandHistory> mHistory;

        private ObservableCollection<CommandHistory> mTempHistory;

        private int mVisisbleHistoryItemsCount;

        private int mCLIPromptLines;

        private int mHighTrafficUpdatedPromptAndInputCounts;

        private int mHighTrafficUpdatedHistoryItems;

        private int mBufferedHistoryItemsFromLastTimeUpdating;

        private int mActualDisplayedLinesCount;

        #endregion

        internal  bool IsHighTraffic { get; set; }
        internal int CLIPromptLines
        {
            get => mCLIPromptLines;
            set => mCLIPromptLines = value;
        }

        public int ActualDisplayedLinesCount
        {
            get => mActualDisplayedLinesCount;
            set => mActualDisplayedLinesCount = value;
        }

        public bool NeedShowClickableKeywords { get; set; }


       
        public int VisibleHistoryItemsCount
        {
            get => this.mVisisbleHistoryItemsCount;
            set => this.mVisisbleHistoryItemsCount = value;
        }

        public bool IsBusy
        {
            get => mIsBusy;
            private set => this.SetProperty(ref mIsBusy, value);
        }
        public IntPtr CurrentCommandIcon => mCurrentCommandIcon;

        public string CurrentCommand => mCurrentCommandLocalName;

        public ObservableCollection<CommandHistory> TemporaryHistory => mTempHistory;

        public ObservableCollection<CommandHistory> History => mHistory;

        public List<string> RecentCommands
        {
            get;set;
            
        }

       /// <summary>
        /// Gets or sets the PromptAndInput
        /// </summary>
        public PromptAndInput PromptAndInput {
            get
            {
                if (this.mPromptAndInput == null)
                {
                    return this.mPromptAndInput = new PromptAndInput();
                }

                return this.mPromptAndInput;
            } 
        }

        public CommandEditor()
        {
            this.RecentCommands = new List<string>();
            this.mHistory = new ObservableCollection<CommandHistory>();
            this.mTempHistory = new ObservableCollection<CommandHistory>();
            this.mCurrentCommand = null;
            this.mCurrentCommandIcon = IntPtr.Zero;
            this.mTempPromptCount = 0;
            this.mCurrentCommandHistoryCount = 0;
            this.mbMissedCommandHistoryItems = false;
            this.mIsBusy = false;
            this.mbEnableTempPrompt = false;
            this.mVisisbleHistoryItemsCount = 0;
            /*SystemVariable systemVariable = Application.UiBindings.SystemVariables["CLIPROMPTLINES"];
            int num;
            if (systemVariable == null)
            {
                num = 3;
            }
            else
            {
                num = (int)((short)systemVariable.Value);
            }*/
            //this.mCLIPromptLines = num;
            this.mCLIPromptLines = 3;
            this.mHighTrafficUpdatedPromptAndInputCounts = 0;
            this.mHighTrafficUpdatedHistoryItems = 0;
            this.mBufferedHistoryItemsFromLastTimeUpdating = 0;
            this.mActualDisplayedLinesCount = 0;
        }

        public void Flush(char echoChar, bool isUpdateHistory)
        {
            this.FireCommandLineChanged(echoChar, isUpdateHistory);
        }
        public void CopyHistoryToClipBoard()
        {

        }


        public void SyncHistoryViewList()
        {
           
        }

        public void ClearTempHistory()
        {
            //mTempPromptCount = 0;
            //mTempHistory.Clear();
        }

        internal void FireCommandLineChanged(char echoChar, bool isUpdateHistory)
        {
            this.PromptAndInput.InputBuffer.FireContentChanged();
            this.PromptAndInput.FireContentChanged(false);
            this.PromptAndInput.InputBuffer.FirePositionChanged();
            this.PromptAndInput.InputBuffer.FireSelectionChanged();
            this.UpdateCurrentCommand();
            if (isUpdateHistory)
            {
                this.UpdateHistory();
                //this.UpdateTempHistory();    
            }
            

        }

        internal void AddReactor()
        {

        }

        internal void RemoveReactor()
        {

        }

        internal void AddDocumentEvents(Document doc)
        {
            Application.Application.Idle += this.OnIdle;
            
        }

        

        internal void RemoveDocumentEvents(Document doc)
        {

        }

        internal void IncreaseTempHistoryCount(int count)
        {
            mTempPromptCount += count;
            mCurrentCommandHistoryCount += count;
        }

        internal void CheckScriptStatus()
        {

        }

        private void UpdateCurrentCommand()
        {
            string a = mCurrentCommand;
            
            int index = this.RecentCommands.Count;
            if (index>0)
            {
                string b = this.RecentCommands[this.RecentCommands.Count - 1];    
                this.mCurrentCommand = b;
                this.mCurrentCommandLocalName = this.mCurrentCommand;
                this.UpdateTempHistory();
                this.RaisePropertyChanged(nameof(CurrentCommand));
            }
            
            //string b = Module.GetCommandNameForDisplay();
            
        }

        public void UpdateHistory(/*int linesAdded*/)
        {
            int maxLine = this.PromptAndInput.commandLineHistory.CommandHistoryList.Count;
            int maxLineOfHistory = this.History.Count;
            int line = maxLineOfHistory;
            while (maxLine!= maxLineOfHistory)
            {
                string CommandLine = this.PromptAndInput.commandLineHistory.CommandHistoryList[line];
                    this.History.Add(new CommandHistory(CommandLine));
                    line++;
                    maxLine = this.PromptAndInput.commandLineHistory.CommandHistoryList.Count;
                    maxLineOfHistory = this.History.Count;

            }
            /*if (line>=0)
            {
                
            }*/
            
        }

        public void UpdateTempHistory()
        {
            if (this.mbEnableTempPrompt)
            {
                
            }
            
        }

        private void UpdateRecentCommand()
        {

        }

        private void SyncUpAllCommandLineState()
        {

        }

        private void OnIdle(object sender, System.EventArgs e)
        {
            this.clearHighTrafficFlags();
            
            this.UpdateCurrentCommand();
        }

        private void OnModal(object sender, System.EventArgs e)
        {

        }

        private void clearHighTrafficFlags()
        {

        }

    }
}
