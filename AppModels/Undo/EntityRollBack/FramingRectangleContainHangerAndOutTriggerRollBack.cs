using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.ResponsiveData.Openings;
using AppModels.Undo.Backup;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.Undo.EntityRollBack
{
    public class FramingRectangleContainHangerAndOutTriggerRollBack: FramingRectangle2DRollBack
    {
        private Guid? _hangerAId;
        private Guid? _hangerBId;
        private Hanger2D _hangerA;
        private Hanger2D _hangerB;
        private bool _isHangerA;
        private bool _isHangerB;
        private Guid? _outTriggerAId;
        private Guid? _outTriggerBId;
        private OutTrigger2D _outTriggerA;
        private OutTrigger2D _outTriggerB;
        private bool _isOutTriggerA;
        private bool _isOutriggerB;
        private bool _outTriggerAFlipped;
        private bool _outTriggerBFlipped;
        private Guid? _framingNameId;
        private bool _isShowFramingName;
        private OutTrigger _outTriggerARef;
        private OutTrigger _outTriggerBRef;
        private Hanger _hangerARef;
        private Hanger _hangerBRef;
        

        public FramingRectangleContainHangerAndOutTriggerRollBack(Entity entity,UndoList undoItem) : base(entity)
        {
            if(EntityRollBack is FramingRectangleContainHangerAndOutTrigger framing)
            {
                _hangerAId = framing.HangerAId;
                _hangerBId = framing.HangerBId;
                _hangerA = framing.HangerA;
                _hangerB = framing.HangerB;
                _isHangerA = framing.IsHangerA;
                _isHangerB = framing.IsHangerB;
                _outTriggerAId = framing.OutTriggerAId;
                _outTriggerBId = framing.OutTriggerBId;
                _outTriggerA = framing.OutTriggerA;
                _outTriggerB = framing.OutTriggerB;
                _isOutTriggerA = framing.IsOutTriggerA;
                _isOutriggerB = framing.IsOutTriggerB;
                _outTriggerAFlipped = framing.OutTriggerAFlipped;
                _outTriggerBFlipped = framing.OutTriggerBFlipped;
                _framingNameId = framing.FramingNameId;
                _isShowFramingName = framing.IsShowFramingName;
                if(_hangerA!=null)
                {
                    var iRollBack = EditBackupFactory.CreateBackup(_hangerA, undoItem, null);
                    iRollBack?.Backup();
                    _hangerARef =((IContaintHanger)(framing.FramingReference)).HangerA;
                }
                if (_hangerB != null)
                {
                    var iRollBack = EditBackupFactory.CreateBackup(_hangerB, undoItem, null);
                    iRollBack?.Backup();
                    _hangerBRef = ((IContaintHanger)(framing.FramingReference)).HangerB;
                }
                if(_outTriggerA!=null)
                {
                    var iRollBack = EditBackupFactory.CreateBackup(_outTriggerA, undoItem, null);
                    iRollBack?.Backup();
                    _outTriggerARef = ((IContaintOutTrigger)(framing.FramingReference)).OutTriggerA;
                }
                if (_outTriggerB != null)
                {
                    var iRollBack = EditBackupFactory.CreateBackup(_outTriggerB, undoItem, null);
                    iRollBack?.Backup();
                    _outTriggerBRef = ((IContaintOutTrigger)(framing.FramingReference)).OutTriggerB;
                }
            }

        }
        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is FramingRectangleContainHangerAndOutTrigger framing)
            {
                framing.HangerAId = _hangerAId;
                framing.HangerBId = _hangerBId;
                framing.HangerA = _hangerA;
                framing.HangerB = _hangerB;
                framing.IsHangerA = _isHangerA;
                framing.IsHangerB = _isHangerB;
                framing.OutTriggerAId = _outTriggerAId;
                framing.OutTriggerBId = _outTriggerBId;
                framing.OutTriggerA = _outTriggerA;
                framing.OutTriggerB = _outTriggerB;
                framing.IsOutTriggerA = _isOutTriggerA;
                framing.IsOutTriggerB = _isOutriggerB;
                framing.SetFlippedOutriggerA(_outTriggerAFlipped);
                framing.SetFlippedOutriggerB(_outTriggerBFlipped);
                framing.FramingNameId = _framingNameId;
                framing.IsShowFramingName = _isShowFramingName;
                if (_hangerA != null)
                {
                    ((IContaintHanger)(framing.FramingReference)).HangerA = _hangerARef;
                }
                if (_hangerB != null)
                {
                    ((IContaintHanger)(framing.FramingReference)).HangerB = _hangerBRef;

                }
                if (_outTriggerA != null)
                {
                     ((IContaintOutTrigger)(framing.FramingReference)).OutTriggerA= _outTriggerARef ;
                }
                if (_outTriggerA != null)
                {
                    ((IContaintOutTrigger)(framing.FramingReference)).OutTriggerB = _outTriggerBRef ;
                }
            }
        }
    }
}
