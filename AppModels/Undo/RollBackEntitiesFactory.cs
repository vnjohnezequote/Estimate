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
            if (entity is JoistArrowEntity joistArrow)
            {
                return new Joist2DRollBack(joistArrow);
            }

            if (entity is Leader leader)
            {
                return new LeaderRollBack(leader);
            }
            if (entity is LinearPath linePath)
            {
                return new LinearPathRollBack(linePath);
            }
            if (entity is Line line)
            {
                return new LineRollBack(line);
            }
            if (entity is AngularDim angularDim)
            {
                return new AngularDimRollBack(angularDim);
            }

            if (entity is Hanger2D hanger)
            {
                return new HangerRollBack(hanger);
            }

            if (entity is Blocking2D blocking)
            {
                return new Blocking2DRollBack(blocking);
            }

            if (entity is FramingNameEntity framingName)
            {
                return new FramingNameRollBack(framingName);
            }

            if (entity is MultilineText mText)
            {
                return new MultilineTextRollBack(mText);
            }
            if (entity is Text text)
            {
                return new TextRollBack(text);
            }

            if (entity is Joist2D joist2D)
            {
                return new Joist2DRollBack(joist2D);
            }

            if (entity is Beam2D beam2D)
            {
                return new Beam2DRollBack(beam2D);
            }

            if (entity is OutTrigger2D outTrigger)
            {
                return new OutTriggerRollBack(outTrigger);
            }

            
            return null;
        }
    }
}
