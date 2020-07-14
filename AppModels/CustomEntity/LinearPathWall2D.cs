using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Interaface;
using AppModels.ViewModelEntity;
using devDept.Eyeshot.Entities;

namespace AppModels.CustomEntity
{
    [Serializable]
    public class LinearPathWall2D: LinearPath, IWall2D,IEntityVmCreateAble
    {
        public string WallLevelName { get; set; }

        public LinearPathWall2D(LinearPath another) : base(another)
        {

        }
        public IEntityVm CreateEntityVm()
        {
            return new LinearPathWall2DVm(this);
        }
    }
}
