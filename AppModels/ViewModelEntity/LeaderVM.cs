using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;

namespace AppModels.ViewModelEntity
{
    public class LeaderVM : EntityVm
    {
        public LeaderVM(Entity entity) : base(entity)
        {
        }

        public double ArrowSize
        {
            get
            {
                if (Entity is Leader leader)
                {
                   return leader.ArrowheadSize;
                }

                return 0;
            }
            set
            {
                if (Entity is Leader leader)
                {
                    leader.ArrowheadSize = value;
                    RaisePropertyChanged(nameof(ArrowSize));
                }
            }
        }
        public override void NotifyPropertiesChanged()
        {
            base.NotifyPropertiesChanged();
            RaisePropertyChanged(nameof(ArrowSize));
        }

    }
}
