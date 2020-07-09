using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;
using Prism.Mvvm;

namespace AppModels.ViewModelEntity
{
    [Serializable]
    public class EntityVm: BindableBase
    {
        protected readonly Entity _entity;
        [ReadOnly(true)]
        public Color? Color
        {
            get => _entity?.Color;
            set
            {
                if (_entity == null) return;
                if (value != null) _entity.Color = (Color) value;
                this.RaisePropertyChanged(nameof(Color));
            }
        }

        public string LayerName
        {
            get => _entity==null ? "" : _entity.LayerName;
            set
            {
                if (_entity == null) return;
                _entity.LayerName = value;
                RaisePropertyChanged(nameof(LayerName));
            }
        }

        public EntityVm(Entity entity)
        {
            this._entity = entity;
        }

        public virtual void NotifyPropertiesChanged()
        {
            this.RaisePropertyChanged(nameof(LayerName));
            this.RaisePropertyChanged(nameof(Color));
        }

        public Entity GetEntity()
        {
            return _entity;
        }
       
    }
}
