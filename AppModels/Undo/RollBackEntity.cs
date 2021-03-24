using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using devDept.Eyeshot.Entities;

namespace AppModels.Undo
{
    public abstract class RollBackEntity: IRollBackEntity
    {
        private colorMethodType _colorMethod;
        private Color _color;
        private colorMethodType _lineTypeMethod;
        private float _lineTypeScale;
        private string _layerName;
        private string _lineTypeName;
        private colorMethodType _lineWeightMethod;
        private bool _visible;

        public Entity EntityRollBack { get; set; }

        public RollBackEntity(Entity entity)
        {
            EntityRollBack = entity;
            _colorMethod = entity.ColorMethod;
            _color = entity.Color;
            _lineTypeMethod = entity.LineTypeMethod;
            _lineTypeScale = entity.LineTypeScale;
            _layerName = entity.LayerName;
            _lineWeightMethod = entity.LineWeightMethod;
            _visible = entity.Visible;
            _lineTypeName = entity.LineTypeName;
            _visible = entity.Visible;
        }

        public virtual void Undo()
        {
            EntityRollBack.ColorMethod = _colorMethod ;
            EntityRollBack.Color = _color;
            EntityRollBack.LineTypeMethod = _lineTypeMethod;
            EntityRollBack.LineTypeScale = _lineTypeScale;
            EntityRollBack.LayerName = _layerName;
            EntityRollBack.LineWeightMethod = _lineWeightMethod;
            EntityRollBack.LineTypeName = _lineTypeName;
            EntityRollBack.Visible = _visible;
        }
    }
}
