using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationInterfaceCore;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using Prism.Mvvm;

namespace AppDataBase.DataBase
{
    public class EntitiesManager: BindableBase, IEntitiesManager
    {
        public EntityList Entities { get; }
        public Entity SelectedEntity { get; }
        public ObservableCollection<Entity> SelectedEntities { get; }
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
    }
}
