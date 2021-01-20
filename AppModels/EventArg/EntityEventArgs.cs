using System;
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

