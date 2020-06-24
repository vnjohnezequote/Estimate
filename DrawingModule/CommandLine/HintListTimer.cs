using System;
using System.Windows.Threading;
using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public class HintListTimer
    {

        internal HintListTimer()
        {
            this.mAutoCompleteTimer = new DispatcherTimer();
            this.mAutoCompleteTimer.Tick += this.OnHintListTimer;
        }
        internal event EventHandler Tick;
        private DispatcherTimer mAutoCompleteTimer;
        private bool mForceRestart;
        private DateTime mStartTime = DateTime.Now;
        private double mInterval = 0.3;
        private double TimerTimeout = 10.0;
        private string mPreviousInput = string.Empty;


        private void OnHintListTimer(object sender, System.EventArgs e)
        {
            if (DateTime.Now.Subtract(this.mStartTime).TotalSeconds > this.TimerTimeout)
            {
                this.Pause();
            }

            string userInput = this.GetUserInput();
            
            //if (!this.mForceRestart && userInput !=string.Empty&& string.Compare(userInput, this.mPreviousInput, true)==0)
            //{
            //    return;
            //}
            if (this.Tick != null)
            {
                this.Tick(sender, e);
            }
            this.Pause();
          
        }

        

        internal void Restart(bool bShortInterval, bool bForceRestart)
        {
            this.mForceRestart = bForceRestart;
            this.mAutoCompleteTimer.Interval = TimeSpan.FromSeconds(bShortInterval ? 0.1 : this.mInterval);
            this.mAutoCompleteTimer.Start();
            this.mStartTime = DateTime.Now;
        }
        internal void Pause()
        {
            this.mAutoCompleteTimer.Stop();
            this.mPreviousInput = this.GetUserInput();
        }

        private string GetUserInput()
        {
            CommandEditor activeEditor = Application.Application.UiBindings.CommandEditorManager.ActiveEditor;
            if (activeEditor == null)
            {
                return string.Empty;
            }

            InputBuffer inputBuffer = activeEditor.PromptAndInput.InputBuffer;
            /*if (inputBuffer.Text !=null)
            {*/
                int length = inputBuffer.Text.Length;
                if (inputBuffer.Position == inputBuffer.SelectPosition || (inputBuffer.Position!=length && inputBuffer.SelectPosition != length))
                {
                    return inputBuffer.Text;
                }

                int length2 = Math.Min(inputBuffer.Position, inputBuffer.SelectPosition);
                return inputBuffer.Text.Substring(0, length2);    
           /*}*/

            //return null;

        }
        internal string UserInput => this.GetUserInput();
    }
}