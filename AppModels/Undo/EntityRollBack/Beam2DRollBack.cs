using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using devDept.Eyeshot.Entities;

namespace AppModels.Undo.EntityRollBack
{
    public class Beam2DRollBack: FramingRectangleContainHangerAndOutTriggerRollBack
    {
        private bool _isBeamUnder;
        private bool _showDimension;
        public Beam2DRollBack(Entity entity) : base(entity)
        {
            if (entity is Beam2D beam)
            {
                _isBeamUnder = beam.IsBeamUnder;
                _showDimension = beam.ShowDimension;
            }
        }

        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is Beam2D beam)
            {
                beam.IsBeamUnder = _isBeamUnder;
                beam.ShowDimension = _showDimension;
            }
        }
    }
}
