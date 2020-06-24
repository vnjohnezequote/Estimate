using devDept.Geometry;
using DrawingModule.ViewModels;
using CanvasDrawing = DrawingModule.CustomControl.CanvasControl.CanvasDrawing;

namespace DrawingModule.UserInteractive
{
    public class PromptCornerOptions : PromptEditorOptions
    {
        #region Field

        #endregion

        #region properties
        public Point3D BasePoint { get; set; }

        #endregion
        #region Constructor
        public PromptCornerOptions(string message, Point3D basePoint) :base(message)
        {
            this.BasePoint = basePoint;
            
        }


        #endregion

        #region public method

        protected internal override PromptResult DoIt(CanvasDrawing canvas,DynamicInputViewModel dynamicInputViewModel)
        {
            return null;
        }



        #endregion
        }
}
