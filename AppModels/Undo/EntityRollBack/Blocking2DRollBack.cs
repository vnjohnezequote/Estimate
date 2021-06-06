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
    public class Blocking2DRollBack: TextRollBack
    {
        private Guid _id;
        private Guid _levelId;
        private Guid _framingSheetId;
        private bool _isRotate;
        private IFraming _framingReference;
        private Guid _framingReferenceId;
        private double _fullLength;


        public Blocking2DRollBack(Entity entity) : base(entity)
        {
            if (EntityRollBack is Blocking2D blocking)
            {
                _id = blocking.Id;
                _levelId =blocking.LevelId;
                _framingSheetId = blocking.FramingSheetId;
                _isRotate = blocking.IsRotate;
                _framingReference = blocking.FramingReference;
                _framingReferenceId = blocking.FramingReferenceId;
                _fullLength= blocking.FullLength;
            }
        }

        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is Blocking2D blocking)
            {
                blocking.Id = _id;
                blocking.LevelId = _levelId;
                blocking.FramingSheetId = _framingSheetId;
                blocking.IsRotate = _isRotate;
                blocking.FramingReference = _framingReference;
                blocking.FramingReferenceId = _framingReferenceId;
                blocking.FullLength = _fullLength;
            }
        }
    }
}
