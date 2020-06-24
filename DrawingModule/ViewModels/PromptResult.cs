using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationService;
using Prism.Mvvm;

namespace DrawingModule.ViewModels
{
    public class PromptResult
    {
        #region Field

        #endregion

        #region Properties
        public PromptStatus Status { get; set; }
        public string StringResult { get; set; }
        #endregion

        #region Constructor

        internal PromptResult(PromptStatus promptStatus, string stringResult)
        {
            this.Status = promptStatus;
            this.StringResult = stringResult;
        }


        #endregion

        }
}
