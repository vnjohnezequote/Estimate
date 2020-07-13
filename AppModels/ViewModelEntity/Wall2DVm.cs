using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using devDept.Eyeshot.Entities;

namespace AppModels.ViewModelEntity
{
    [Serializable]
    public class Wall2DVm : EntityVmBase
    {
        public string WallLevelName
        {
            get
            {
                if (Entity!=null &&Entity is Wall2D wall2D)
                {
                    return wall2D.WallLevelName;
                }

                return "";
            }
            set
            {
                if (Entity != null && Entity is Wall2D wall2D)
                {
                    wall2D.WallLevelName = value;
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
