using AppModels.Interaface;

namespace AppModels.Undo
{
    public interface IUndoEngineering
    {
        void Undo();
        void SaveSnapshot(UndoList undoItem);
        void SetEntitiesManager(IEntitiesManager entitiesManager);
    }
}