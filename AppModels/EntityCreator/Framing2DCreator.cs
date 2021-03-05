using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.EntityCreator.Interface;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings;

namespace AppModels.EntityCreator
{
    public abstract class FramingRectangleCreator: IFramingRectangle2DCreator
    {
        protected FramingSheet _framingSheet;
        protected IFraming2D _framing2D;
        protected IFraming _framingReference;

        public FramingRectangleCreator(FramingSheet framingSheet)
        {
            _framingSheet = framingSheet;
        }

        public IFraming2D GetFraming2D()
        {
            return this._framing2D;
        }

        public IFraming GetFramingReference()
        {
            return this._framingReference;
        }

    }
}
