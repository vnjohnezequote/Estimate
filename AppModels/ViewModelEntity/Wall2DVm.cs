using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using devDept.Eyeshot.Entities;

namespace AppModels.ViewModelEntity
{
    public class Wall2DVm: EntityVmBase
    {
        public int WallThickness
        {
            get
            {
                if (this.Entity is Wall2D wall)
                {
                    return wall.WallThickness;
                }

                return 0;
            }
            set
            {
                if (this.Entity is Wall2D wall)
                {
                    wall.WallThickness = value;
                    RaisePropertyChanged(nameof(WallThickness));
                }
            }
        }

        public bool IsLoadBearingWall
        {
            get
            {
                if (this.Entity is Wall2D wall)
                {
                    return wall.IsLoadBearingWall;
                }

                return false;
            }
            set
            {
                if (this.Entity is Wall2D wall)
                {
                    wall.IsLoadBearingWall = value;
                    RaisePropertyChanged(nameof(IsLoadBearingWall));
                }
            }
        }

        public bool IsShowDimension
        {
            get
            {
                if (this.Entity is Wall2D wall)
                {
                    return wall.ShowWallDimension;
                }

                return false;
            }
            set
            {
                if (this.Entity is Wall2D wall)
                {
                    wall.ShowWallDimension = value;
                    RaisePropertyChanged(nameof(IsShowDimension));
                }
            }
        }
        public Wall2DVm(IEntity entity) : base(entity)
        {
        }
    }
}
