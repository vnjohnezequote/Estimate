using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.Undo.EntityRollBack
{
    public class BlockRefRollBack : RollBackEntity
    {
        private string _blockName;
        private Point3D _insertPoint;
        private Transformation _transFormation;

        public BlockRefRollBack(Entity entity) : base(entity)
        {
            if (entity is BlockReference blockRef)
            {
                _blockName = blockRef.BlockName;
                _insertPoint = blockRef.InsertionPoint;
                _transFormation = blockRef.Transformation;
            }
            
        }

        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is BlockReference blockRef)
            {
                blockRef.BlockName = _blockName;
                blockRef.InsertionPoint = _insertPoint;
                blockRef.Transformation = _transFormation;
            }
            
        }
    }
}
