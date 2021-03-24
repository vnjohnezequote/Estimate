using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;

namespace AppModels.Undo.EntityRollBack
{
    public class LeaderRollBack: RollBackEntity
    {
        public LeaderRollBack(Entity entity) : base(entity)
        {
        }
    }
}
