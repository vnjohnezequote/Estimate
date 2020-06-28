using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
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
        public EntityList Entities { get; private set; }
        public Entity SelectedEntity { get=>_selectedEntity; set=>SetProperty(ref _selectedEntity,value); }
        public ObservableCollection<Entity> SelectedEntities { get=>_selectedEntities; set=>SetProperty(ref _selectedEntities,value); }

        public ICadDrawAble CanvasDrawing { get;private set; }
        //public ICadDrawAble CanvasDrawing { get; set; }

        public EntitiesManager()
        {
            SelectedEntities = new ObservableCollection<Entity>();
        }
        public void AddAndRefresh(Entity entity, string layerName)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {//this refer to form in WPF application 
                Entities.Add(entity, layerName);
                Entities.Regen();
                Invalidate();
            }));
        }

        public void RemoveEntity(Entity entity)
        {
           
        }

        public void Invalidate()
        {
            if (this.CanvasDrawing!=null)
            {
                this.CanvasDrawing.Invalidate();
                this.CanvasDrawing.Focus();
            }
            
        }

        public void EntitiesRegen()
        {
            //this.Entities.Regen();
            if (this.CanvasDrawing!=null)
            {
                this.CanvasDrawing.Entities.Regen();
                
            }
           
        }

        public void Refresh()
        {
            if (this.CanvasDrawing!=null)
            {
                this.CanvasDrawing.RefreshEntities();
            }
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

        public void SetCanvasDrawing(ICadDrawAble cadDraw)
        {
            this.CanvasDrawing = cadDraw;
        }
    }
}
