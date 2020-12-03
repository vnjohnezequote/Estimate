using System;
using AppModels.CustomEntity;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;

namespace AppModels.ViewModelEntity
{
    [Serializable]
    public class WallLine2DVm : EntityVmBase,IWall2D
    {
        public string WallLevelName
        {
            get
            {
                if (Entity!=null &&Entity is WallLine2D wall2D)
                {
                    return wall2D.WallLevelName;
                }

                return "";
            }
            set
            {
                if (Entity != null && Entity is WallLine2D)
                {
                    ((WallLine2D) Entity).WallLevelName = value;
                    RaisePropertyChanged(nameof(WallLevelName));
                }
                
            }
        }

        public double Length()
        {
            if (this.Entity != null && this.Entity is IWall2D wall2D)
            {
                return wall2D.Length();
            }

            return 0;
        }

        public WallLine2DVm(Entity entity) : base(entity)
        {
        }

        public override void NotifyPropertiesChanged()
        {
            base.NotifyPropertiesChanged();
            RaisePropertyChanged(nameof(WallLevelName));
        }
    }
}
