using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;
using Prism.Mvvm;

namespace AppModels.ViewModelEntity
{
    [Serializable]
    public abstract class EntityVmBase: BindableBase, IEntityVm
    {
        public IEntity Entity { get; }
        public Color? Color
        {
            get => Entity?.Color;
            set
            {
                if (Entity == null) return;
                if (value != null) Entity.Color = (Color)value;
                this.RaisePropertyChanged(nameof(Color));
            }
        }

        public string LayerName
        {
            get => Entity == null ? "" : Entity.LayerName;
            set
            {
                if (Entity == null) return;
                Entity.LayerName = value;
                RaisePropertyChanged(nameof(LayerName));
            }
        }

        public EntityVmBase(Entity entity)
        {
            this.Entity = entity;
        }

        public virtual void NotifyPropertiesChanged()
        {
            this.RaisePropertyChanged(nameof(LayerName));
            this.RaisePropertyChanged(nameof(Color));
        }
    }
}
