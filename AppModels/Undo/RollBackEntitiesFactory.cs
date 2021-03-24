using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using AppModels.Undo.EntityRollBack;
using devDept.Eyeshot.Entities;

namespace AppModels.Undo
{
    public static class RollBackEntitiesFactory
    {
        public static IRollBackEntity CreateRollBackEntity(Entity entity)
        {
            if (entity is WallLine2D wallLine)
            {
                return new WallLineRollBack(wallLine);
            }
            if (entity is Line line)
            {
                return new LineRollBack(line);
            }
            if (entity is AngularDim angularDim)
            {
                return new AngularDimRollBack(angularDim);
            }
            if (entity is Text text)
            {
                return new TextRollBack(text);
            }

            
            return null;
        }
    }
}
