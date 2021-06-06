using AppModels.CustomEntity;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;

namespace AppModels.Undo.EntityRollBack
{
    public class OutTriggerRollBack: FramingRectangle2DRollBack
    {
        private int _insizeLength;
        private int _outSizeLength;
        private IFraming2DContaintHangerAndOutTrigger _framing2D;
        
        public OutTriggerRollBack(Entity entity) : base(entity)
        {
            if (EntityRollBack is OutTrigger2D outTrigger)
            {
                _insizeLength = outTrigger.InSizeLength;
                _outSizeLength = outTrigger.OutSizeLength;
                _framing2D = outTrigger.Framing2D;
            }
        }

        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is OutTrigger2D outTrigger)
            {
                outTrigger.SetInsizeLength(_insizeLength);
                outTrigger.SetOutSizeLength(_outSizeLength);
                outTrigger.Framing2D = _framing2D;
            }
        }
    }
}
