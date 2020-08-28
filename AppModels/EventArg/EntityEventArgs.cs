using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;

namespace AppModels.EventArg
{
    public class EntityEventArgs : EventArgs
    {
        public EntityEventArgs(Entity item)
        {
            Item = item;
        }

        public Entity Item { get; set; }
    }
}

