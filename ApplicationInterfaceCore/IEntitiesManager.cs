using System.Collections.ObjectModel;
using System.ComponentModel;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;

namespace ApplicationInterfaceCore
{
    public interface IEntitiesManager: INotifyPropertyChanged
    {
        EntityList Entities { get; }
        Entity SelectedEntity { get; }
        ObservableCollection<Entity> SelectedEntities { get; }
        void AddAndRefresh(Entity entity);
        void RemoveEntity(Entity entity);
        void Invalidate();
        void EntitiesRegen();

    }
}