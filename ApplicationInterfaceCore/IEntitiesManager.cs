using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Documents;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;

namespace ApplicationInterfaceCore
{
    public interface IEntitiesManager: INotifyPropertyChanged
    {
        EntityList Entities { get; }
        Entity SelectedEntity { get; }
        ObservableCollection<Entity> SelectedEntities { get; }
        ICadDrawAble CanvasDrawing { get; }
        void AddAndRefresh(Entity entity, string layerName);
        void RemoveEntity(Entity entity);
        void Invalidate();
        void EntitiesRegen();
        void Refresh();
        void ChangeSelectedEntiesLayer(Layer layer);
        void ClearSelectedEntities();
        void SetEntitiesList(EntityList entities);
        void SetCanvasDrawing(ICadDrawAble cadDraw);
        void ResetSelection();
        Entity GetEntity(int index);
        List<Entity> GetSelectedEntities();
    }
}