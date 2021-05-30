using AppModels.Interaface;
using devDept.Eyeshot.Entities;

namespace AppModels.Undo
{
    public interface IDependencyUndoEntity
    {
        //Entity Clone();
        //Entity CloneToUndo();
        void RollBackDependency(UndoList undoItem, IEntitiesManager entitiesManager);
    }
}