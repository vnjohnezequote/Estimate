using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using devDept.Eyeshot.Entities;

namespace AppModels.Undo.EntityRollBack
{
    public class WallLineRollBack: LineRollBack
    {
        private string _wallLevelName;
        public WallLineRollBack(Entity entity) : base(entity)
        {
            
            if (entity is WallLine2D wallLine)
            {
                _wallLevelName = wallLine.WallLevelName;
            }
        }

        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is WallLine2D wallLine)
            {
                wallLine.WallLevelName = _wallLevelName;
            }
        }
    }
}
