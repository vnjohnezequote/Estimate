using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Geometry;
using DrawingModule.Function;

namespace DrawingModule.ViewModels
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
