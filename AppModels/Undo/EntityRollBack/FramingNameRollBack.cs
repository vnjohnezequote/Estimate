using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;

namespace AppModels.Undo.EntityRollBack
{
    public class FramingNameRollBack: TextRollBack
    {
        private IFraming _framingReference;
        private Guid _id;
        private Guid _levelId;
        private Guid _framingSheetId;
        private Guid _framingReferenceId;
        public FramingNameRollBack(Entity entity) : base(entity)
        {
            if (EntityRollBack is FramingNameEntity framingName)
            {
                _framingReference = framingName.FramingReference;
                _id = framingName.Id;
                _levelId = framingName.LevelId;
                _framingSheetId = framingName.FramingSheetId;
                _framingReferenceId = framingName.FramingReferenceId;
            }
        }

        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is FramingNameEntity framingName)
            {
                framingName.FramingReference = _framingReference;
                framingName.Id = _id;
                framingName.LevelId = _levelId;
                framingName.FramingSheetId = _framingSheetId;
                framingName.FramingReferenceId = _framingReferenceId;
            }

        }
    }
}
