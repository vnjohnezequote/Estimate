using devDept.Eyeshot.Entities;

namespace AppModels.Undo
{
    public interface IRollBackEntity
    {
        Entity EntityRollBack { get; set; }
        void Undo();
    }
}