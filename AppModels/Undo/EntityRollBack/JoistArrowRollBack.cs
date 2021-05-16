using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppModels.CustomEntity;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.Undo.EntityRollBack
{
    public class JoistArrowRollBack: LineRollBack
    {
        private FramingNameEntity _framingName;
        private Guid _framingNameid;
        private Point3D _startArrow;
        private Point3D _endArrow;
        public JoistArrowRollBack(Entity entity) : base(entity)
        {
            if (EntityRollBack is JoistArrowEntity joistArrow)
            {
                _framingName = joistArrow.FramingName;
                _framingNameid = joistArrow.FramingNameId;
                _startArrow = joistArrow.StartArrow;
                _endArrow = joistArrow.EndArrow;

            }
        }

        public override void Undo()
        {
            base.Undo();
            if (EntityRollBack is JoistArrowEntity joistArrow)
            {
                joistArrow.FramingName = _framingName;
                joistArrow.FramingNameId = _framingNameid;
                joistArrow.StartArrow = _startArrow;
                joistArrow.EndArrow = _endArrow;

            }
        }
    }
}
