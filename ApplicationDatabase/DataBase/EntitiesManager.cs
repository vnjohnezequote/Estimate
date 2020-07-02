using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
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
            SelectedEntities.CollectionChanged += SelectedEntities_CollectionChanged;
        }

        private void SelectedEntities_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (SelectedEntities.Count>0)
            {
                SelectedEntity = SelectedEntities[0];
                return;
            }

            SelectedEntity = null;
        }

        public List<Entity> GetSelectedEntities()
        {
            return Application.Current.Dispatcher.Invoke((Func<List<Entity>>)(() =>
            {//this refer to form in WPF application 
                return this.SelectedEntities.ToList();
            }));

        }

        public void ClearSelectedEntities()
        {
            foreach (var entity in Entities)
            {
                entity.Selected = false;
            }
            this.SelectedEntities.Clear();
        }
        public Entity GetSelectionEntity()
        {
            return Application.Current.Dispatcher.Invoke((Func<Entity>) (() =>
            {
                //this refer to form in WPF application 
                return this.SelectedEntity;
            }));
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
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {//this refer to form in WPF application 
                this.Entities.Remove(entity);
                this.Entities.Regen();
            }));
        }

        public Entity GetEntity(int index)
        {
            return Application.Current.Dispatcher.Invoke((Func<Entity>)(() => Entities[index]));
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
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {//this refer to form in WPF application 
                if (this.CanvasDrawing != null)
                {
                    this.CanvasDrawing.Entities.Regen();

                }
            }));
            //this.Entities.Regen();
            
           
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
