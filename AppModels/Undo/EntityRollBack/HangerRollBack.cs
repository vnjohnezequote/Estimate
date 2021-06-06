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
    public class HangerRollBack: TextRollBack
    {
        private IFraming _framingReference;
        private Guid _framingSheetId;
        private IFraming2DContaintHangerAndOutTrigger _framing2D;
        private Guid _id;
        private Guid _levelId;
        private Guid _framingReferenceId;
        private double _fullLength;


        public HangerRollBack(Entity entity) : base(entity)
        {
            if (EntityRollBack is Hanger2D hanger)
            {
                _framingReference = hanger.FramingReference;
                _framingSheetId = hanger.FramingSheetId;
                _framing2D = hanger.Framing2D;
                _id = hanger.Id;
                _levelId = hanger.LevelId;
                _framingReferenceId = hanger.FramingReferenceId;
                _fullLength = hanger.FullLength;
            }
        }

        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is Hanger2D hanger)
            {
                 hanger.FramingReference = _framingReference ;
                 hanger.FramingSheetId = _framingSheetId ;
                 hanger.Framing2D= _framing2D;
                 hanger.Id = _id ;
                 hanger.LevelId = _levelId;
                 hanger.FramingReferenceId = _framingReferenceId ;
                 hanger.FullLength = _fullLength;
            }

        }
    }
}
