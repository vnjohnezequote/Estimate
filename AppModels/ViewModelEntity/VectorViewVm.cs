using System;
using devDept.Eyeshot.Entities;
using MathExtension;


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

        public string Scale
        {
            get
            {
                if (Entity!=null && Entity is VectorView vectorView)
                {

                    var f = (Rational) vectorView.Scale;
                    return f.ToString().Replace('/',':');

                }

                return "";
            }
            set
            {
                if (Entity!=null && Entity is VectorView vectorView)
                {
                    //vectorView.Scale = value;
                    var inputScale = value.Replace(':', '/');
                    Helper.ConvertStringToDouble(inputScale, out var ouputScale);
                    vectorView.Scale = ouputScale;
                    RaisePropertyChanged(nameof(Scale));
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
            RaisePropertyChanged(nameof(Scale));
        }
    }
}
