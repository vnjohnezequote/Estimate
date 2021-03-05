using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings;

namespace AppModels.EntityCreator
{
    public class Joist2DController
    {
        private FramingSheet _framingSheet;
        private Joist2D _framing2D;
        private IFraming _framing;
        private IEntitiesManager _entitiesManager;
        public Joist2DController(IEntitiesManager entitiesManager, Joist2D framing2D)
        {
            _entitiesManager = entitiesManager;
            _framing2D = framing2D;
            _framing = framing2D.FramingReference;
            _framingSheet = framing2D.FramingReference.FramingSheet;
        }

    }
}
