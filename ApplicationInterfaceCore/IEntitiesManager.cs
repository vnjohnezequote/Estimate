using System.Collections.ObjectModel;
using System.ComponentModel;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using DrawingModule.Interface;

namespace ApplicationInterfaceCore
{
    public interface IEntitiesManager: INotifyPropertyChanged
    {
        EntityList Entities { get; }
        Entity SelectedEntity { get; }
        ObservableCollection<Entity> SelectedEntities { get; }
        ICadDrawAble CanvasDrawing { get; set; }
        void AddAndRefresh(Entity entity);
        void RemoveEntity(Entity entity);
        void Invalidate();
        void EntitiesRegen();
        void ChangeSelectedEntiesLayer(Layer layer);
        void SetEntitiesList(EntityList entities);

    }
}