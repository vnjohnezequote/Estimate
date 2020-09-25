using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Documents;
using AppModels.AppData;
using AppModels.DynamicObject;
using AppModels.Interaface;
using AppModels.ViewModelEntity;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;

namespace ApplicationInterfaceCore
{
    public interface IEntitiesManager: INotifyPropertyChanged
    {
        event EventHandler EntitiesCollectionChanged;
        EntityList Entities { get; }
        BlockKeyedCollection Blocks { get; }
        IEntityVm SelectedEntity { get; }
        ObservableCollection<Entity> SelectedEntities { get; }
        ICadDrawAble CanvasDrawing { get; }
        void AddAndRefresh(Entity entity, string layerName);
        void RemoveEntity(Entity entity);
        void Invalidate();
        void EntitiesRegen();
        void Refresh();
        //void ChangeSelectedEntiesLayer(Layer layer);
        void ClearSelectedEntities();
        void SetEntitiesList(EntityList entities);
        void SetBlocks(BlockKeyedCollection blocks);
        void SetCanvasDrawing(ICadDrawAble cadDraw);
        void NotifyEntitiesListChanged();
        void ResetSelection();
        Entity GetEntity(int index);
        List<Entity> GetSelectedEntities();
        void ChangLayerForSelectedEntities(LayerItem layer);
        void SyncLevelSelectedsEntityPropertyChanged(string selectedLevel);
    }
}