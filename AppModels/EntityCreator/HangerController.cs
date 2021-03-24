using System.Linq;
using AppModels.CustomEntity;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings;
using AppModels.ResponsiveData.Openings;

namespace AppModels.EntityCreator
{
    public class HangerControler
    {
        private Hanger2D _hangerA2D;
        private Hanger2D _hangerB2D;
        private Hanger _hangerA;
        private Hanger _hangerB;
        private IEntitiesManager _entitiesManager;
        private IFraming2DContaintHangerAndOutTrigger _framing2D;
        private IContaintHanger _framing;
        private FramingSheet _framingSheet;
        public HangerControler(IEntitiesManager entitiesManager,IFraming2DContaintHangerAndOutTrigger framing2D)
        {
            _entitiesManager = entitiesManager;
            _framing2D = framing2D;
            if (framing2D.FramingReference == null) return;
            _framingSheet = framing2D.FramingReference.FramingSheet;
            _framing = (IContaintHanger)framing2D.FramingReference;
            _hangerA2D = framing2D.HangerA;
            _hangerB2D = framing2D.HangerB;
            _hangerA = ((IContaintHanger) (_framing2D.FramingReference))
                .HangerA;
             _hangerB = ((IContaintHanger) (_framing2D.FramingReference))
                .HangerB;
        }
        public void RemoveHangerA(bool isRegerationHangerName = true)
        {
            if (_hangerA2D!=null)
            {
                _entitiesManager.Entities.Remove(_hangerA2D);
            }
            _framing2D.HangerAId = null;
            _framing2D.HangerA = null;
            if (_hangerA!=null)
            {
               _framingSheet.Hangers.Remove(_hangerA);

            }
            if (_framing == null) return;
            _framing.HangerA = null;
            _framing2D.IsHangerA = false;
        }
        public void RemoveHangerB()
        {
            if (_hangerB2D!=null)
            {
                _entitiesManager.Entities.Remove(_hangerB2D);
            }
            _framing2D.HangerBId = null;
            _framing2D.HangerB = null;
            if (_hangerB!=null)
            {
               _framingSheet.Hangers.Remove(_hangerB);

            }
            if (_framing == null) return;
            _framing.HangerB = null;
            _framing2D.IsHangerB = false;
        }
        public void AddHangerA()
        {
            if (_hangerA2D != null) return;
            var hangerCreator = new HangerCreator(_framing2D.HangerACenterPoint, _framing2D);
            _entitiesManager.AddAndRefresh(hangerCreator.GetHanger2D(), _framing2D.LayerName,false,false);
            _framing2D.IsHangerA = true;
        }
        public void AddHangerB()
        {
            if (_hangerB2D != null) return;
            var hangerCreator = new HangerCreator(_framing2D.HangerBCenterPoint, _framing2D,false);
            _entitiesManager.AddAndRefresh(hangerCreator.GetHanger2D(), _framing2D.LayerName,false,false);
            _framing2D.IsHangerB = true;

        }
        
    }
}
