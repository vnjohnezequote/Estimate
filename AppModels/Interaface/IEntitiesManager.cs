using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AppModels.AppData;
using AppModels.CustomEntity;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;

namespace AppModels.Interaface
{
    public interface IEntitiesManager: INotifyPropertyChanged
    {
        event EventHandler EntitiesCollectionChanged;
        EntityList Entities { get; set; }
        BlockKeyedCollection Blocks { get; set; }
        IEntityVm SelectedEntity { get; set; }
        ObservableCollection<Entity> SelectedEntities { get; }
        List<Wall2D> Walls { get; } 
        ICadDrawAble CanvasDrawing { get; }
        void AddAndRefresh(Entity entity, string layerName,bool isAddExtension = true,bool isGeneralFramingName = true);
        void RemoveEntity(Entity entity,bool isRemoveExtension=true);
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
        void Insert(int index, Entity entity);
        Entity GetEntity(int index);
        List<Entity> GetSelectedEntities();
        void ChangLayerForSelectedEntities(LayerItem layer);
        void SyncLevelSelectedsEntityPropertyChanged(string selectedLevel);
    }
}