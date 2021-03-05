using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using AppModels.CustomEntity;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using devDept.Geometry;

namespace AppModels.EntityCreator
{
    public class OutTriggerController
    {
        private OutTrigger2D _outTriggerA2D;
        private OutTrigger2D _outTriggerB2D;
        private OutTrigger _outTriggerA;
        private OutTrigger _outTriggerB;
        private IEntitiesManager _entitiesManager;
        private IFraming2DContaintHangerAndOutTrigger _framing2D;
        private IContaintOutTrigger _framing;
        private FramingSheet _framingSheet;

        public OutTriggerController(IEntitiesManager entitiesManager, IFraming2DContaintHangerAndOutTrigger framing2D)
        {
            _entitiesManager = entitiesManager;
            _framing2D = framing2D;
            _framing = (IContaintOutTrigger)framing2D.FramingReference;
            _framingSheet = framing2D.FramingReference.FramingSheet;
            _outTriggerA2D = framing2D.OutTriggerA;
            _outTriggerB2D = framing2D.OutTriggerB;
            _outTriggerA = _framing.OutTriggerA;
            _outTriggerB = _framing.OutTriggerB;
        }

        public void RemoveOutTriggerA()
        {
            _framing2D.OutTriggerAFlipped = false;
            if (_outTriggerA!=null)
            {
                _entitiesManager.Entities.Remove(_outTriggerA2D);
            }
            _framing2D.OutTriggerAId = null;
            _framing2D.OutTriggerA = null;
            _framingSheet.OutTriggers.Remove(_outTriggerA);
            _framing.OutTriggerA = null;
            _framing2D.IsOutTriggerA = false;
        }

        public void RemoveOutTriggerB()
        {
            _framing2D.OutTriggerBFlipped = false;
            if (_outTriggerB != null)
            {
                _entitiesManager.Entities.Remove(_outTriggerB2D);
            }
            _framing2D.OutTriggerBId = null;
            _framing2D.OutTriggerB = null;
            _framingSheet.OutTriggers.Remove(_outTriggerB);
            _framing.OutTriggerB = null;
            _framing2D.IsOutTriggerB = false;
        }

        public void AddOutriggerA()
        {
            if (_outTriggerA2D!=null) return;
            Utility.OffsetPoint(_framing2D.OuterEndPoint, _framing2D.OuterStartPoint, 450, out var startPoint);
            Utility.OffsetPoint(startPoint, _framing2D.OuterStartPoint, 900, out var endPoint);
            var ouTriggerCreator = new OutTriggerCreator(_framing2D,startPoint,endPoint);
            _entitiesManager.AddAndRefresh((OutTrigger2D)ouTriggerCreator.GetFraming2D(),_framing2D.LayerName);
            _framing2D.IsOutTriggerA = true;
        }

        public void AddOutriggerB()
        {
            if (_outTriggerB2D != null) return;
            Utility.OffsetPoint(_framing2D.OuterStartPoint, _framing2D.OuterEndPoint, 450, out var startPoint);
            Utility.OffsetPoint(startPoint, _framing2D.OuterEndPoint, 900, out var endPoint);
            var ouTriggerCreator = new OutTriggerCreator(_framing2D,startPoint,endPoint,false,false);
            _entitiesManager.AddAndRefresh((OutTrigger2D)ouTriggerCreator.GetFraming2D(), _framing2D.LayerName);
            _framing2D.IsOutTriggerB = true;
        }
    }
}
