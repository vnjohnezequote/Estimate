﻿using System.Collections.ObjectModel;
using ApplicationInterfaceCore;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using Prism.Mvvm;

namespace AppDataBase.DataBase
{
    public class EntitiesManager: BindableBase, IEntitiesManager
    {
        private Entity _selectedEntity;
        private ObservableCollection<Entity> _selectedEntities;
        public EntityList Entities { get; set; }
        public Entity SelectedEntity { get=>_selectedEntity; set=>SetProperty(ref _selectedEntity,value); }
        public ObservableCollection<Entity> SelectedEntities { get=>_selectedEntities; set=>SetProperty(ref _selectedEntities,value); }
        public ICadDrawAble CanvasDrawing { get; set; }

        public EntitiesManager()
        {

        }
        public void AddAndRefresh(Entity entity)
        {
            
        }

        public void RemoveEntity(Entity entity)
        {
           
        }

        public void Invalidate()
        {
            
        }

        public void EntitiesRegen()
        {
            
        }

        public void ChangeSelectedEntiesLayer(Layer layer)
        {
            
        }

        public void SetEntitiesList(EntityList entities)
        {
            if (Entities == null)
            {
                this.Entities=entities;
            }


        }
        
    }
}
