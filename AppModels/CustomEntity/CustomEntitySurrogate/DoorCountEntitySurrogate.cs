using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Serialization;

namespace AppModels.CustomEntity.CustomEntitySurrogate
{
    public class DoorCountEntitySurrogate: TextSurrogate
    {
        public string LevelName
        {
            get;
            set;
        }

        public int DoorReferenceId
        {
            get;
            set;
        }
        public DoorCountEntitySurrogate(DoorCountEntity door): base(door)
        {

        }
        protected override Entity ConvertToObject()
        {
            Entity ent;
            ent = new DoorCountEntity(new Text(this.Plane,this.TextString,this.Height));
            CopyDataToObject(ent);
            return ent;

        }
        protected override void CopyDataToObject(Entity entity)
        {
            if (entity is DoorCountEntity door)
            {
                door.LevelName = LevelName;
                door.DoorReferenceId = DoorReferenceId;
            }
            base.CopyDataToObject(entity);
        }

        protected override void CopyDataFromObject(Entity entity)
        {
            if (entity is DoorCountEntity door)
            {
                LevelName = door.LevelName;
                DoorReferenceId = door.DoorReferenceId;
            }
            base.CopyDataFromObject(entity);
        }
    }
}
