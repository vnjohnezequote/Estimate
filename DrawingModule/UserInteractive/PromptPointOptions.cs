using ApplicationService;
using devDept.Geometry;
using DrawingModule.CustomControl;
using DrawingModule.ViewModels;
using CanvasDrawing = DrawingModule.CustomControl.CanvasControl.CanvasDrawing;

namespace DrawingModule.UserInteractive
{
    public class PromptPointOptions : PromptCornerOptions
    {
        #region Field

        #endregion

        #region Properties

        public bool UseBasePoint { get; set; }

        #endregion
        #region Constructor

        public PromptPointOptions(string message) :base(message,new Point3D(0,0,0))
        {
            this.UseBasePoint = false;

        }


        #endregion

        #region public method

        protected internal override PromptResult DoIt(CanvasDrawing canvasDrawing, DynamicInputViewModel dynamicInput)
        {
            string stringResult = null;
            base.InitGet(dynamicInput);
            PromptStatus retcode;
            Point3D point=null;
            if (UseBasePoint)
            {
                retcode = canvasDrawing.GetPoint(this.BasePoint,out stringResult, out point);
            }
            else
            {
                retcode = canvasDrawing.GetPoint(null, out stringResult, out point);
            }

            return new PromptPointResult(retcode, stringResult,point);
        }


        #endregion
        }
}
