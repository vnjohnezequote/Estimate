using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.Undo.EntityRollBack
{
    public class Framing2DRollBack: PlannarRollBack
    {
        private Guid _id;
        private Guid _levelId;
        private Guid _framingSheetId;

        private IFraming _framingReference;
        private Guid _framingReferenceId;
        private double _fullLength;
        public Framing2DRollBack(Entity entity) : base(entity)
        {
            if(EntityRollBack is IFraming2D framing)
            {
                _id = framing.Id;
                _levelId = framing.LevelId;
                _framingSheetId = framing.FramingSheetId;
                _framingReference = framing.FramingReference;
                _framingReferenceId = framing.FramingReferenceId;
                _fullLength = framing.FullLength;
            }
        }

        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is IFraming2D framing)
            {
                framing.Id = _id;
                framing.LevelId = _levelId;
                framing.FramingSheetId = _framingSheetId;
                framing.FramingReference = _framingReference;
                framing.FramingReferenceId = _framingReferenceId;
                framing.FullLength = _fullLength;
            }
        }
    }
}
