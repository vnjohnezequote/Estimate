using ApplicationService;

namespace DrawingModule.UserInteractive
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
