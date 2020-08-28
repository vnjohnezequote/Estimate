using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;
using Prism.Mvvm;
using Syncfusion.Windows.PropertyGrid;
using Color = System.Drawing.Color;

namespace AppModels.ViewModelEntity
{
    [Serializable]
    public abstract class EntityVmBase: BindableBase, IEntityVm
    {
        public IEntity Entity { get; }
        [PropertyGrid(NestedPropertyDisplayMode = NestedPropertyDisplayMode.None)]
        public Color? Color
        {
            get => Entity?.Color;
            set
            {
                if (this.Entity == null) return;
                if (value != null) this.Entity.Color = (Color) value;
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

        public colorMethodType ColorMethod
        {
            get => Entity?.ColorMethod ?? colorMethodType.byEntity;
            set
            {
                if (Entity ==null) return;
                Entity.ColorMethod = value;
            }
        }

        protected EntityVmBase(IEntity entity)
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
