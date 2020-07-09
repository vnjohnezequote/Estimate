using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using devDept.Eyeshot.Entities;

namespace AppModels.ViewModelEntity
{
    public class Wall2DVm : EntityVm
    {
        public string WallLevelName
        {
            get
            {
                if (_entity is Wall2D wall2D)
                {
                    return wall2D.WallLevelName;
                }

                return "";
            }
            set
            {
                if (_entity != null && _entity is Wall2D wall2D)
                {
                    wall2D.LayerName = value;
                    RaisePropertyChanged(nameof(WallLevelName));
                }
                
            }
        }

        public Wall2DVm(Entity entity) : base(entity)
        {
        }

        public override void NotifyPropertiesChanged()
        {
            base.NotifyPropertiesChanged();
            RaisePropertyChanged(nameof(WallLevelName));
        }
    }
}
