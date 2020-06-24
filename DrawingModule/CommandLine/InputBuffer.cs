// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputBuffer.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The input buffer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Prism.Mvvm;

namespace DrawingModule.CommandLine
{
    /// <summary>
    /// The input buffer.
    /// </summary>
    public class InputBuffer : BindableBase
    {
        private string text;
        private int position=0;
        private int selectPosition=0;
        /// <summary>
        /// Initializes a new instance of the <see cref="InputBuffer"/> class.
        /// </summary>
        public InputBuffer()
        {
            this.Text = "";
        }

        /// <summary>
        /// Gets or sets a value indicating whether HasSelection
        /// </summary>
        public bool HasSelection { get; set; }

        /// <summary>
        /// Gets or sets the Position
        /// </summary>
        public int Position {  get=>this.position;  }

        /// <summary>
        /// Gets or sets the SelectionPosition
        /// </summary>
        public int SelectPosition { get=>this.selectPosition;  }

        /// <summary>
        /// Gets or sets the Text
        /// </summary>
        public string Text { get=>this.text; set=>this.SetProperty(ref text,value); }
        
        public bool IsPromptingForCommandName()
        {
            return true;
        }

        public void ReplaceInputQueue(string newInput,string command)
        {
            //CCmdThroat cmdThroat = this.GetCcmdThroat();
            //if (cmdThroat != null)
            //{
            //    /*

            //    if (!String.IsNullOrEmpty(newInput))
            //    {
            //        char c = newInput[newInput.Length - 1];
            //        bool check = c == '\n';
            //        if (!check)
            //        {
            //            this.Text = newInput;
            //            cmdThroat.ConOut(newInput,true, null);
                            
            //        }
            //        else
            //        {
            //            this.Text = newInput;
            //            cmdThroat.ConOut(newInput,true, command);
            //            this.Text = null;
            //        }
                    
            //    }
            //    */
                
            //    //0xCu == "\f"
            //    cmdThroat.StuffKDB('\f', 0,true,3);

            //}
            this.Text = newInput;


        }

        private void stuffKeyBoard(CCmdThroat cmdThroat, string newInput, bool param1, bool param2)
        {
           

        }

        

        //public bool OnSetSelectedTextQueue(int start, int end)
        //{

        //}

        public bool OnCharQueue(uint inputChar, uint repCnt, uint flags)
        {
            bool result = false;
            
                switch (inputChar)
                {
                    case 0x03u:
                        break;
                    case 0x08u: // Back button
                        result = true;
                        break;
                    case 0x9u: // Tab button
                        result = true;
                        break;
                    case 0x16u: // underfine key
                        result = false;
                        break;
                    case 0x1B: //escape case
                        result = true;
                        break;
                }

                return result;
        }

        public bool OnKeyDownQueue(uint inputChar, uint repCnt, uint flags)
        {
            return true;
        }

        public bool HandleShiftKeyQueue()
        {
            return true;
        }
       

        public bool OnSysKeyDownQueue(uint inputChar, uint repCnt, uint flags)
        {
            
            

            // 79 = F10, 77 = F11, F11
              
            if ((inputChar != 0x79) && (((flags & 0x2000) == 0 || ((inputChar != 0x77 && (inputChar !=0x7a))))))
            {
                return false;
            }
            return this.OnKeyDownQueue(inputChar,repCnt,flags);
            

        }

        /*public bool StuffNonPrintableKey(uint inputChar,uint repCnt, uint flags)
        {
            
        }*/

        public bool HandleDeleteKeyQueue(uint inputChar, uint repCnt, uint flags)
        {
            return true;
        }

        public bool HandleFunctionKeyQueue(uint inputChar, uint repCnt, uint flags)
        {
            return true;
        }

        public void FireContentChanged()
        {
            this.RaisePropertyChanged(nameof(Text));
        }

        public void FirePositionChanged()
        {
            this.RaisePropertyChanged("Position");
        }

        
        public void FireSelectionChanged()
        {
            this.RaisePropertyChanged("SelectPosition");
        }
        public bool OnSetSelectedTextQueue(int start, int end)
        {
            this.position = start;
            this.selectPosition = end;
            return this.StuffNonPrintableKey(0x29U, start,end << 0x10 | start & 0xffffU);
        }

        private bool StuffNonPrintableKey(uint a, int start, long end)
        {
            //CCmdThroat cmdThroat = this.CmdThroat();
            return true;
        }
        /*
        private CCmdThroat CmdThroat()
        {
          
        }*/
    }
}
