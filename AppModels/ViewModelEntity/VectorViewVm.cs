using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;

namespace AppModels.ViewModelEntity
{
    [Serializable]
    public class VectorViewVm: EntityVmBase
    {
        public double X
        {
            get
            {
                if (Entity!= null && Entity is VectorView vectorView)
                {
                    return vectorView.X;
                }

                return 0;
            }
            set
            {
                if (Entity != null && Entity is VectorView vectorView)
                {
                    vectorView.X = value;
                    RaisePropertyChanged(nameof(X));
                }
            }
        }

        public double Y
        {
            get
            {
                if (Entity != null && Entity is VectorView vectorView)
                {
                    return vectorView.Y;
                }

                return 0;
            }
            set
            {
                if (Entity != null && Entity is VectorView vectorView)
                {
                    vectorView.Y = value;
                    RaisePropertyChanged(nameof(Y));
                }
            }
        }
        public VectorViewVm(IEntity entity) : base(entity)
        {
           
        }
        public override void NotifyPropertiesChanged()
        {
            base.NotifyPropertiesChanged();
            RaisePropertyChanged(nameof(X));
            RaisePropertyChanged(nameof(Y));
        }
    }
}
