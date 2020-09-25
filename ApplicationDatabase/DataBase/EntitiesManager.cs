﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;
using ApplicationInterfaceCore;
using AppModels;
using AppModels.AppData;
using AppModels.CustomEntity;
using AppModels.DynamicObject;
using AppModels.ViewModelEntity;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using Prism.Mvvm;
using AppModels.Factories;
using AppModels.Interaface;

namespace AppDataBase.DataBase
{
    public class EntitiesManager: BindableBase, IEntitiesManager
    {
        public event EventHandler EntitiesCollectionChanged ;
        private IEntityVm _selectedEntity;
        private ObservableCollection<Entity> _selectedEntities;
        public EntityList Entities { get; set; }
        public BlockKeyedCollection Blocks { get; set; }

        public IEntityVm SelectedEntity
        {
            get=>_selectedEntity;
            set => SetProperty(ref _selectedEntity,value);
        }

        public void SyncLevelSelectedsEntityPropertyChanged(string wallLevel)
        {
            foreach (var selectedEntity in SelectedEntities)
            {
                if (selectedEntity is IWall2D wall2D)
                {
                    wall2D.WallLevelName = wallLevel;
                }
            }

            this.NotifyEntitiesListChanged();
        }

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
                //if (SelectedEntities[0] is Wall2D wall2D)
                //{
                //    SelectedEntity =wall2D.GetEntityVm();
                //    return;
                //}
                //SelectedEntity = new EntityVm(SelectedEntities[0]) ;
                var entityFactory = new EntitiyVmFactory();
                SelectedEntity = entityFactory.creatEntityVm(SelectedEntities[0]);
                return;
            }

            SelectedEntity = null;
        }

        public List<Entity> GetSelectedEntities()
        {
            return Application.Current.Dispatcher.Invoke((Func<List<Entity>>)(() => this.SelectedEntities.ToList()));

        }

        public void ChangLayerForSelectedEntities(LayerItem selectedLayer)
        {
            if (SelectedEntities == null || SelectedEntities.Count <= 0) return;
            if (selectedLayer == null) return;
            foreach (var selectedEntity in SelectedEntities)
            {
                selectedEntity.LayerName = selectedLayer.Name;
                if (selectedEntity.LineTypeMethod != colorMethodType.byLayer) continue;
                if (selectedLayer.LineTypeName == "Continues")
                {
                    selectedEntity.LineTypeName = (string)null;
                }
                else
                {
                    selectedEntity.LineTypeName = selectedLayer.LineTypeName;
                }

                selectedEntity.RegenMode = regenType.CompileOnly;
            }

            SelectedEntity?.NotifyPropertiesChanged();
            NotifyEntitiesListChanged();
        }

        //public void SyncSelectedsPropertyChanged()
        //{
        //    if (this.SelectedEntity is IWall2D selWall2D)
        //    {
        //        foreach (var selectedEntity in SelectedEntities)
        //        {
        //            if (selectedEntity is IWall2D wall)
        //            {
        //                wall.WallLevelName = selWall2D.WallLevelName;
        //            }
        //        }
        //    }
            
        //}

        /// <summary>
        /// Notify EntityList Changed to calculator againt wall length
        /// </summary>
        public void NotifyEntitiesListChanged()
        {
            EntitiesCollectionChanged?.Invoke(this,null);
        }

        public void ResetSelection()
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {//this refer to form in WPF application 
                foreach (var selectedEntity in SelectedEntities)
                {
                    selectedEntity.Selected = false;
                }
                this.SelectedEntities.Clear();
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
            return Application.Current.Dispatcher.Invoke((Func<Entity>) (() => SelectedEntity?.Entity as Entity));
        }

        public void AddAndRefresh(Entity entity, string layerName)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {//this refer to form in WPF application 
                if (entity is Picture)
                {
                    Entities.Add(entity, layerName);
                    Entities.Regen();
                }
                else
                {
                    var index = Entities.Count - 1;
                    if (index<0)
                    {
                        index = 0;
                    }
                    entity.LayerName = layerName;
                    Entities.Insert(index,entity);
                    Entities.Regen();
                }
                
                Invalidate();
                this.NotifyEntitiesListChanged();
            }));
        }

        public void RemoveEntity(Entity entity)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {//this refer to form in WPF application 
                this.Entities.Remove(entity);
                this.Entities.Regen();
                this.NotifyEntitiesListChanged();
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
            {
                //this refer to form in WPF application 
                CanvasDrawing?.Entities.Regen();
            }));
            //this.Entities.Regen();
            
           
        }

        public void Refresh()
        {
            CanvasDrawing?.RefreshEntities();
        }

        public void SetEntitiesList(EntityList entities)
        {
            this.Entities = entities;
            //if (Entities == null)
            //{
            //    this.Entities=entities;
            //}
        }

        public void SetBlocks(BlockKeyedCollection blocks)
        {
            if (Blocks == null)
            {
                Blocks = blocks;
            }
        }

        public void SetCanvasDrawing(ICadDrawAble cadDraw)
        {
            this.CanvasDrawing = cadDraw;
        }
    }
}
