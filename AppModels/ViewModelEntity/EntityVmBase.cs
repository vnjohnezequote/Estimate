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
        //[Browsable(false)]
        [Display(AutoGenerateField = false)]
        public IEntity Entity { get; }
        [PropertyGrid(NestedPropertyDisplayMode = NestedPropertyDisplayMode.None)]
        public System.Windows.Media.Color Color
        {
            get
            {
                if (this.Entity != null)
                {
                    System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(
                        this.Entity.Color.A,
                        this.Entity.Color.R,
                        this.Entity.Color.G,
                        this.Entity.Color.B);
                    
                    return newColor;
                }
                return Colors.White;
            }
            set
            {
                if(this.Entity!= null)
                {
                    this.Entity.Color = System.Drawing.Color.FromArgb(value.A, value.R, value.G, value.B);
                }
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
