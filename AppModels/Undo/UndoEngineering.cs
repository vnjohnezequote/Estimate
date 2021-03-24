using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using AppModels.Enums;
using AppModels.Interaface;

namespace AppModels.Undo
{
    public class UndoEngineering: IUndoEngineering
    {
        private int _stackMemory = 30;
        private List<UndoList> _undoList = new List<UndoList>();
        private IEntitiesManager _entitiesMangager;

        public void SetEntitiesManager(IEntitiesManager entitiesManager)
        {
            _entitiesMangager = entitiesManager;
        }
        public void Undo()
        {
            if (_undoList.Count==0)
            {
                return;
            }
            var undoItem = _undoList[_undoList.Count - 1];
            switch (undoItem.ActionType)
            {
                case ActionTypes.Add:
                    foreach (var entity in undoItem.NewAddedEntities)
                    {
                        _entitiesMangager.RemoveEntity(entity);
                    }
                    break;
                case ActionTypes.Edit:
                    foreach (var editedEntities in undoItem.EditedEntities)
                    {
                        editedEntities.Undo();
                    }
                    _entitiesMangager.EntitiesRegen();
                    break;
                case ActionTypes.AddAndRemove:
                    foreach (var newAddEntity in undoItem.NewAddedEntities)
                    {
                        _entitiesMangager.RemoveEntity(newAddEntity);
                    }
                    foreach (var removeEntity in undoItem.RemovedEntities)
                    {
                        _entitiesMangager.AddAndRefresh(removeEntity,removeEntity.LayerName);
                    }
                    break;
                case ActionTypes.Remove:
                    foreach (var removeEntity in undoItem.RemovedEntities)
                    {
                        removeEntity.Selected = false;
                        _entitiesMangager.AddAndRefresh(removeEntity, removeEntity.LayerName);
                        if (removeEntity is FramingNameEntity framingName)
                        {
                            if (undoItem.DependencyEntitiesDictionary.TryGetValue(framingName,
                                out var dependencyEntity))
                            {
                                if (dependencyEntity is FramingRectangleContainHangerAndOutTrigger framing)
                                {
                                    framing.FramingName = framingName;
                                    framing.FramingNameId = framingName.Id;
                                    framing.IsShowFramingName = true;
                                }

                                if (dependencyEntity is JoistArrowEntity joistArrow)
                                {
                                    _entitiesMangager.AddAndRefresh(joistArrow,joistArrow.LayerName);
                                }
                            }

                        }
                    }
                    break;
            }
            _undoList.Remove(undoItem);
        }

        public void SaveSnapshot(UndoList undoItem)
        {
            if (_undoList.Count>_stackMemory)
            {
                _undoList.RemoveAt(0);
                _undoList.Add(undoItem);
            }
            else
            {
                _undoList.Add(undoItem);
            }
        }
    }
}
