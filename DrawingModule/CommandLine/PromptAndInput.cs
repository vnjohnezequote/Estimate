// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PromptAndInput.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the PromptAndInput type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using DrawingModule.ViewModels;
using Prism.Mvvm;

namespace DrawingModule.CommandLine
{
    /// <summary>
    /// The prompt and input.
    /// </summary>
    public sealed class PromptAndInput : BindableBase
    {
        #region Fields

        private string prompt;
        private string promptDefault = "";
        private PromptAndInput pInput;
        private EventHandler m_acceptedEvent;
        private EventHandler m_canceledEvent;
        private string mPreviousPrompt = string.Empty;
        private InputBuffer mInputBuffer;
        private ObservableCollection<string> mPromptKeywords;
        internal string PreviousPrompt => mPreviousPrompt;
        public int SelectPosition { get; set; }
        public int Position { get; set; }

        public PromptAndInput PInput
        {
            get
            {
                if (this.pInput == null)
                {
                    return pInput = new PromptAndInput();
                }

                return pInput;
            }
        }

        public CommandLineHistory commandLineHistory;
        public string PromptDefault => this.PInput == null ? string.Empty : this.promptDefault;
        public int KeywordsStringLength { get; set; }
        public int KeywordsStringStart { get; set; }

        public string Prompt
        {
            get => PInput == null ? string.Empty : this.prompt;
            set => this.SetProperty(ref prompt, value);
        }

        private string text;

        public string Text
        {
            get
            {
                if (this.InputBuffer ==  null)
                {
                    return string.Empty;
                }

                return this.UpdateCacheText();
            }
        }

        public event EventHandler Canceled
        {
            add
            {
                m_canceledEvent = (EventHandler)Delegate.Combine(m_canceledEvent, value);
            }
            remove
            {
                m_canceledEvent = (EventHandler)Delegate.Remove(m_canceledEvent, value);
            }
        }

        public event EventHandler Accepted
        {
            add
            {
                m_acceptedEvent = (EventHandler)Delegate.Combine(m_acceptedEvent, value);
            }
            remove
            {
                m_acceptedEvent = (EventHandler)Delegate.Remove(m_acceptedEvent, value);
            }
        }

        public InputBuffer InputBuffer
        {
            get
            {
                if (this.mInputBuffer == null)
                {
                    return this.mInputBuffer = new InputBuffer();
                }

                return this.mInputBuffer;
            }
        }

        public string UpdateCacheText()
        {
            string resultString;
            resultString = "";
            const string COMMAND = "Command: ";
            resultString = string.Concat(resultString,COMMAND);
            resultString = string.Concat(resultString, this.InputBuffer.Text);
            return resultString;
        }

        public string GetCommandPromptString()
        {
            string result;
            this.ParsePrompt();
            if (String.IsNullOrEmpty(this.Prompt))
            {
                result = "\nCommand: ";
            }
            else
            {
                result = this.Prompt;
            }
            return result;
        }
        public void ParsePrompt()
        {
            if (this.prompt == null)
            {
                this.prompt = "";
                this.KeywordsStringStart = -1;
            }
        }

        public int GetPromptKeywordsStringStart()
        {
            
            this.ParsePrompt();
            return this.KeywordsStringStart;
        }
        public ObservableCollection<string> PromptKeywords { get; set; }

        #endregion
        /// <summary>
        /// Initializes a new instance of the <see cref="PromptAndInput"/> class.
        /// </summary>
        public PromptAndInput()
        {
            this.mInputBuffer = null;
            this.PromptKeywords = new ObservableCollection<string>();
            this.KeywordsStringStart = -1;
            this.prompt = "";
            this.InputBuffer.PropertyChanged += InputBuffer_PropertyChanged;
            this.commandLineHistory = new CommandLineHistory();
        }

        private void InputBuffer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName=="Text")
            {
                this.RaisePropertyChanged(nameof(this.Text));
            }
        }

        public void FireContentChanged(bool forceupdate)
        {
            if (forceupdate || string.Compare(this.mPreviousPrompt, this.Prompt) != 0)
            {
                this.mPreviousPrompt = this.Prompt;
                this.UpdatePromptKeywords();
                this.RaisePropertyChanged("Prompt");
            }
        }
        internal void FireAccepted()
        {
            if (m_acceptedEvent != null)
            {
                m_acceptedEvent(this, new System.EventArgs());
            }
        }
        internal void FireCanceled()
        {
            if (m_canceledEvent != null)
            {
                m_canceledEvent(this, new System.EventArgs());
            }
        }

        private void UpdatePromptKeywords()
        {
            //mPromptKeywords.Clear();
            if (this.pInput !=null)
            {
                
            }
        }
        private void AddLineToHistory(string commandPrompt)
        {
            this.commandLineHistory.AddLine(commandPrompt);
        }

        private void EchoAddCharCoreNewLine (char echoChar, string command)
        {
            this.Prompt = this.Text;
            this.AddLineToHistory(this.Text);
            this.Prompt = string.Empty;
            if (command != null)
            {
                this.AddLineToHistory(command);
            }
            
            /*
            this.Prompt = this.Text;
            if (this.Prompt==null)
            {
                this.UpdateCacheText();
                this.AddLineToHistory(this.Text);
            }
            else
            {
                this.AddLineToHistory(this.Prompt);
                this.Prompt = string.Empty;
            }*/
            this.Flush(echoChar, true);
            
        }

        private void EchoAddCharCore(char echoChar)
        {
            if (this.InputBuffer != null)
            {
                this.prompt = "Command: ";
            }
            this.Flush(echoChar,false);

        }

        private void Flush(char echoChar, bool isUpdateHistory)
        {
            Application.Application.UiBindings.CommandEditorManager.ActiveEditor.Flush(echoChar, isUpdateHistory);
        }
        public void Echo(char echoChar,string command)
        {
            if (!String.IsNullOrEmpty(this.Prompt) && echoChar!= '\n' )
            {
                this.Prompt = string.Empty;
            }
            switch (echoChar)
            {
                case '\b':
                    break;
                case '\t':
                    break;
                case '\n':
                    this.EchoAddCharCoreNewLine(echoChar, command);
                    break;
                case '\r':
                    break;
                default:
                    this.EchoAddCharCore(echoChar);
                    break;
            }
            
        }

        public string InputBufferVisibleText()
        {
            return this.InputBuffer.Text;
        }

        public void Echo(string commandString, int line)
        {

        }
    }
}
