using System;
using System.Windows;
using System.Windows.Input;
using DrawingModule.Interface;
using DrawingModule.ViewModels;
using TextBox = System.Windows.Controls.TextBox;

namespace DrawingModule.CommandLine
{
    public class CommandLineTextBox : TextBox
    {
        #region Private
        private CommandLineViewModel mCommandLine;
        private int mMouseDownPos;


        #endregion

        #region Public

        private CommandLineViewModel CommandLine => this.mCommandLine;
        private bool CanCommandLineAcceptInput
        {
            get
            {
                if (mCommandLine != null)
                {
                    return !mCommandLine.AreKeytipsActive;
                }
                return false;
            }
        }

        public bool IsInKoreanImeMode
        {
            get
            {
                /*
                try
                {
                    if (InputLanguage.CurrentInputLanguage != null && InputLanguage.CurrentInputLanguage.Culture.Name == "ko-KR")
                    {
                        if (!Util.IsVistaOrLaterOS())
                        {
                            return true;
                        }
                        HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
                        ImeMode imeMode = ImeContext.GetImeMode(hwndSource.Handle);
                        if (imeMode == ImeMode.AlphaFull || imeMode == ImeMode.Hangul || imeMode == ImeMode.HangulFull)
                        {
                            return true;
                        }
                    }
                }
                catch
                {
                }*/
                return false;
            }
        }

        private string PrevUserInput
        {
            get;
            set;
        }

      

        #endregion


        #region Method

        internal void SetCommandLine(ICommandLine commandLine)
        {
            if (commandLine is CommandLineViewModel commandLinewViewModel)
            {
                mCommandLine = commandLinewViewModel;
            }
        }

        internal void UpdateSelection()
        {
            if (this.mCommandLine == null)
            {
                return;
            }

            TextSelectionRange selectionRange = this.mCommandLine.SelectionRange;
            int num = Math.Min(selectionRange.Start, selectionRange.End);
            int num2 = Math.Abs(selectionRange.Start - selectionRange.End);
            if ((base.SelectionStart != num || base.SelectionLength != num2) &&
                (!this.IsInKoreanImeMode || num2 != 0 || base.SelectionLength != 1))
            {
                base.Select(num, num2);
                base.RaiseEvent(
                    new RoutedEventArgs(System.Windows.Controls.Primitives.TextBoxBase.SelectionChangedEvent, this));
            }

            if (num2 == 0 && base.CaretIndex != num)
            {
                base.CaretIndex = num;
            }

            if (base.CaretIndex == 0 && num2 == 0 && Keyboard.FocusedElement == this)
            {
                KeyboardFocusChangedEventArgs e =
                    new KeyboardFocusChangedEventArgs(Keyboard.PrimaryDevice, Environment.TickCount, null, this)
                    {
                        RoutedEvent = Keyboard.GotKeyboardFocusEvent,
                        Source = this
                    };
                base.RaiseEvent(e);
            }
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (this.mCommandLine != null)
            {
                this.mCommandLine.NeedRefreshHints = true;
            }
            base.OnGotKeyboardFocus(e);
        }

        private void UpdateInputBuffer(string text, int caretIndex)
        {
            InputBuffer mInputBuffer = this.CommandLine.InputBuffer;
            mInputBuffer.ReplaceInputQueue(text,null);
            mInputBuffer.OnSetSelectedTextQueue(caretIndex, caretIndex);
            this.CommandLine.NeedRefreshHints = true;
            this.CommandLine.RestartHintSearchTimer(false);
        }

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            try
            {
                if (!CanCommandLineAcceptInput) return;
                base.OnTextInput(e);
                if (!IsInKoreanImeMode)
                {
                    UpdateInputBuffer(base.Text, base.CaretIndex);
                }
            }
            catch (InvalidOperationException)
            {
            }
        }
        /*
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
         
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {

        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {

        }

        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {

        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {

        }*/

        private int GetInsertionIndexFromInputPoint(Point inputPoint)
        {
            int num = base.GetCharacterIndexFromPoint(inputPoint, true);
            if (num == base.Text.Length)
            {
                return num;
            }
            Rect rectFromCharacterIndex = base.GetRectFromCharacterIndex(num);
            Rect rectFromCharacterIndex2 = base.GetRectFromCharacterIndex(num, true);
            if (inputPoint.X > (rectFromCharacterIndex.Left + rectFromCharacterIndex2.Left) / 2.0)
            {
                num++;
            }
            return num;
        }




        #endregion
    }
}
