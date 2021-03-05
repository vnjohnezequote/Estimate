using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using AppModels;
using AppModels.AppData;
using AppModels.CustomEntity;
using AppModels.EntityCreator;
using AppModels.Enums;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using Prism.Mvvm;
using AppModels.Factories;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.ResponsiveData.Openings;
using devDept.Geometry;

namespace AppDataBase.DataBase
{
    public class EntitiesManager : BindableBase, IEntitiesManager
    {
        public event EventHandler EntitiesCollectionChanged;
        private IEntityVm _selectedEntity;
        private ObservableCollection<Entity> _selectedEntities;
        public EntityList Entities { get; set; }
        public BlockKeyedCollection Blocks { get; set; }
        public List<Wall2D> Walls { get; set; } = new List<Wall2D>();
        public IEntityVm SelectedEntity
        {
            get => _selectedEntity;
            set => SetProperty(ref _selectedEntity, value);
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
        public ObservableCollection<Entity> SelectedEntities { get => _selectedEntities; set => SetProperty(ref _selectedEntities, value); }
        public ICadDrawAble CanvasDrawing { get; private set; } 
        public EntitiesManager()
        {
            SelectedEntities = new ObservableCollection<Entity>();
            SelectedEntities.CollectionChanged += SelectedEntities_CollectionChanged;
        }
        private void SelectedEntities_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (SelectedEntities.Count > 0)
            {
                //if (SelectedEntities[0] is WallLine2D wall2D)
                //{
                //    SelectedEntity =wall2D.GetEntityVm();
                //    return;
                //}
                //SelectedEntity = new EntityVm(SelectedEntities[0]) ;
                var entityFactory = new EntitiyVmFactory();
                SelectedEntity = entityFactory.creatEntityVm(SelectedEntities[0], this);
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
                if (selectedEntity is WallLine2D)
                {
                    selectedEntity.Color = selectedLayer.Color;
                }
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
        /// <summary>
        /// Notify EntityList Changed to calculator againt wall length
        /// </summary>
        public void NotifyEntitiesListChanged()
        {
            EntitiesCollectionChanged?.Invoke(this, null);
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
            return Application.Current.Dispatcher.Invoke((Func<Entity>)(() => SelectedEntity?.Entity as Entity));
        }
        public void AddAndRefresh(Entity entity, string layerName, bool isAddExtension = true, bool isGeneralFramingName = true)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                entity.LineTypeScale = 10;
                if (entity is Picture)
                {
                    Entities.Add(entity, layerName);
                    Entities.Regen();
                }
                else if (entity is Wall2D wall)
                {
                    if (Walls.Count < 1)
                    {
                        Walls.Add(wall);
                        //var index = Entities.Count - 1;
                        //if (index < 0)
                        //{
                        //    index = 0;
                        //}
                        entity.LayerName = layerName;
                        //Entities.Insert(index, entity);
                        Entities.Add(entity);
                    }
                    else
                    {
                        var i = 0;
                        var totalWallInList = Walls.Count();
                        var intersetPointList = new List<Point3D>() { wall.StartPoint, wall.EndPoint };
                        var wallTobeRemove = new List<Wall2D>();
                        while (i < totalWallInList)
                        {
                            List<Wall2D> wallList = new List<Wall2D>();

                            var checkInterSection =
                                Walls[i].SplitWallAtAndNew(wall, wallList, out var intersectionType, intersetPointList);
                            if (checkInterSection && (intersectionType == segmentIntersectionType.Cross || intersectionType == segmentIntersectionType.Touch))
                            {
                                Entities.Remove(Walls[i]);
                                Entities.AddRange(wallList, layerName);
                                Walls.AddRange(wallList);
                                wallTobeRemove.Add(Walls[i]);
                                //Walls.Remove(Walls[i]);
                            }
                            i++;
                        }

                        foreach (var wallRegion in wallTobeRemove)
                        {
                            Walls.Remove(wallRegion);
                        }
                        // need to be sort list intersectpoint before draw new Line
                        if (intersetPointList.Count > 3)
                        {
                            intersetPointList = Helper.SortPointInLine(intersetPointList);
                        }
                        i = 0;
                        while (i < intersetPointList.Count - 1)
                        {
                            var newWall = new Wall2D(Plane.XY, intersetPointList[i], intersetPointList[i + 1], wall.WallThickness, wall.IsLoadBearingWall, true, wall.ShowWallDimension);
                            //newWall.Color = Color.FromArgb(127, Color.CornflowerBlue);
                            newWall.Color = Color.CornflowerBlue;
                            newWall.ColorMethod = colorMethodType.byEntity;
                            newWall.LineTypeName = "Dash Space";
                            newWall.LineTypeMethod = colorMethodType.byEntity;
                            Entities.Add(newWall);
                            Walls.Add(newWall);
                            i++;
                        };

                    }
                }
                else
                {
                    Entities.Insert(0, entity);
                    if (entity is IFraming2D framing)
                    {
                        AddFraming2D(framing, isAddExtension, isGeneralFramingName);
                    }
                }
                Entities.Regen();
                Invalidate();
                this.NotifyEntitiesListChanged();

            }));
        }
        public void Insert(int index, Entity entity)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                Entities.Insert(index, entity);
                Entities.Regen();
                Invalidate();
                this.NotifyEntitiesListChanged();

            }));
        }
        public void Union2dWall(string layerName, Wall2D wall)
        {
            foreach (var wallTest in Walls)
            {
                switch (wallTest.GetInterSection(wall, out var intersectionPoint, out var centerLinePatch))
                {
                    case WallIntersectionType.None:
                        var indexWall = Entities.Count - 1;
                        if (indexWall < 0)
                        {
                            indexWall = 0;
                        }
                        wall.LayerName = layerName;
                        Entities.Insert(indexWall, wall);
                        //this.Walls.Add(wall);
                        Entities.Regen();
                        break;
                    case WallIntersectionType.AtVertice:
                        ICurve ent = centerLinePatch;
                        var offsetWallLine1 = (LinearPath)ent.Offset(45, Vector3D.AxisZ, 0.01, true);
                        var offsetWallLine2 = (LinearPath)ent.Offset(-45, Vector3D.AxisZ, 0.01, true);
                        //Entities.Remove(wallTest);
                        if (intersectionPoint == wallTest.StartPoint)
                        {

                        }
                        else
                        {
                            wallTest.EndPoint1 = offsetWallLine1.Vertices[1];
                            wallTest.ShowEndWallLine = false;
                            wall.StartPoint1 = offsetWallLine1.Vertices[1];
                            wall.ShowStartWallLine = false;
                            var index = Entities.Count - 1;
                            if (index < 0)
                            {
                                index = 0;
                            }
                            wall.LayerName = layerName;
                            Entities.Insert(index, wall);
                            //this.Walls.Add(wall);
                        }
                        //this.Walls.Remove(wallTest);
                        //EntitiesRegen();
                        //this.Entities.Add(offsetWallLine1, layerName);
                        //this.Entities.Add(offsetWallLine2, layerName);
                        //this.Entities.Add(centerLinePatch);
                        break;
                    case WallIntersectionType.AtBetweenWall:
                        break;
                    case WallIntersectionType.Cross:
                        break;
                }
            }


        }
        public void RemoveEntity(Entity entity, bool isRemoveExtension = true)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                if (entity is IFraming2D framing)
                {
                    RemoveFraming2D(framing,isRemoveExtension);
                }
                else
                {
                    this.Entities.Remove(entity);
                }
                this.Entities.Regen();
                this.NotifyEntitiesListChanged();
            }));
        }
        private void RemoveFramingName(FramingRectangleContainHangerAndOutTrigger framing)
        {
            if (framing.FramingName != null)
            {
                Entities.Remove(framing.FramingName);
            }
        }
        private void RemoveHanger(FramingRectangleContainHangerAndOutTrigger framing)
        {
            var hangerController = new HangerControler(this, framing);
            hangerController.RemoveHangerA();
            hangerController.RemoveHangerB();
        }
        private void RemoveOutTrigger(FramingRectangleContainHangerAndOutTrigger framing)
        {
            var outTriggerController = new OutTriggerController(this, framing);
            outTriggerController.RemoveOutTriggerA();
            outTriggerController.RemoveOutTriggerB();
        }
        private void RemoveFraming2D(IFraming2D framing2D,bool isRemoveExtension = true)
        {
            if (isRemoveExtension)
            {
                if (framing2D is FramingRectangleContainHangerAndOutTrigger framingContainHanger)
                {
                    RemoveFramingName(framingContainHanger);
                    RemoveHanger(framingContainHanger);
                    RemoveOutTrigger(framingContainHanger);
                }
            }
            var framingSheet = framing2D.FramingReference.FramingSheet;
            if (framing2D is Beam2D beam)
            {
                framingSheet.Beams.Remove(beam.FramingReference);
            }
            if (framing2D is OutTrigger2D outrigger)
            {
                framingSheet.OutTriggers.Remove(outrigger.FramingReference);
            }
            if (framing2D is Joist2D joist)
            {
                framingSheet.Joists.Remove(joist.FramingReference);
            }

            if (framing2D is OutTrigger2D outTrigger)
            {
                framingSheet.OutTriggers.Remove(outTrigger.FramingReference);
            }

            if (framing2D is Blocking2D blocking)
            {
                framingSheet.Blockings.Remove(blocking.FramingReference);
            }
            this.Entities.Remove((Entity)framing2D);
        }
        private void AddFraming2D(IFraming2D framing2D, bool isAddExtension = true, bool isGeneralFramingName = true)
        {
            var framingSheet = framing2D.FramingReference.FramingSheet;
            if (framing2D is OutTrigger2D outtrigger)
            {
                framingSheet.OutTriggers.Add(outtrigger.FramingReference);
            }
            if (framing2D is Joist2D joist)
            {
                framingSheet.Joists.Add(joist.FramingReference);
            }
            if (framing2D is Beam2D beam)
            {
                framingSheet.Beams.Add(beam.FramingReference);
            }
            if (framing2D is Hanger2D hanger)
            {
                framingSheet.Hangers.Add((Hanger)hanger.FramingReference);
            }
            if (framing2D is Blocking2D blocking)
            {
                framingSheet.Blockings.Add(blocking.FramingReference);
            }
            if (isAddExtension)
            {
                if (framing2D is FramingRectangleContainHangerAndOutTrigger framing && framing.FramingReference != null && framing.FramingReference.FramingSheet != null)
                {
                    if (framing.FramingName != null)
                    {
                        framing.FramingName.LayerName = framing2D.LayerName;
                        Entities.Insert(0, framing.FramingName);
                    }
                    if (framing.HangerA != null)
                    {
                        Entities.Insert(0, framing.HangerA);
                        framingSheet.Hangers.Add((Hanger)framing.HangerA.FramingReference);
                    }
                    if (framing.HangerB != null)
                    {
                        Entities.Insert(0, framing.HangerB);
                        framingSheet.Hangers.Add((Hanger)framing.HangerB.FramingReference);
                    }
                }
            }
        }
        public Entity GetEntity(int index)
        {
            return Application.Current.Dispatcher.Invoke((Func<Entity>)(() => Entities[index]));
        }
        public void Invalidate()
        {
            if (this.CanvasDrawing != null)
            {
                this.CanvasDrawing.Invalidate();
                this.CanvasDrawing.Focus();
            }

        }
        public void EntitiesRegen()
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                CanvasDrawing?.Entities.Regen();
            }));
        }
        public void Refresh()
        {
            CanvasDrawing?.RefreshEntities();
        }
        public void SetEntitiesList(EntityList entities)
        {
            this.Entities = entities;
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
