using AppModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;

namespace AppModels.Undo
{
    public class UndoList
    {
        public ActionTypes ActionType { get; set; }
        public List<Entity> NewAddedEntities = new List<Entity>();
        public List<Entity> RemovedEntities = new List<Entity>();
        public List<IRollBackEntity> EditedEntities = new List<IRollBackEntity>();
        public Dictionary<Entity,Entity> RemoveAndAddDictionary= new Dictionary<Entity, Entity>();
        public Dictionary<Entity, Entity> DependencyEntitiesDictionary = new Dictionary<Entity, Entity>();

    }
}
