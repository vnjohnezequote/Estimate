using devDept.Eyeshot.Entities;
using AppModels.CustomEntity;
using AppModels.Interaface;

namespace AppModels.ViewModelEntity
{
    public class LinearPathWall2DVm: EntityVmBase, IWall2D
    {
        public string WallLevelName
        {
            get
            {
                if (Entity != null && Entity is LinearPathWall2D wall2D)
                {
                    return wall2D.WallLevelName;
                }

                return "";
            }
            set
            {
                if (Entity != null && Entity is LinearPathWall2D wall2D)
                {
                    wall2D.WallLevelName = value;
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

        public LinearPathWall2DVm(IEntity entity) : base(entity)
        {
        }
        public override void NotifyPropertiesChanged()
        {
            base.NotifyPropertiesChanged();
            RaisePropertyChanged(nameof(WallLevelName));
        }

    }
}
