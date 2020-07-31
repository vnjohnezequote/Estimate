using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity.CustomEntitySurrogate;
using AppModels.Interaface;
using AppModels.ViewModelEntity;
using devDept.Eyeshot.Entities;
using devDept.Serialization;

namespace AppModels.CustomEntity
{
    [Serializable]
    public class Wall2D: Line,IEntityVmCreateAble,IWall2D
    {
        #region Field
        //private string _wallLevelName;
        //private int _wallHeight;
        //private string _wallLength;
        #endregion

        #region Properties

        public string WallLevelName { get; set; }

        #endregion

        public Wall2D(Line another) : base(another)
        {
            
        }
        public IEntityVm CreateEntityVm()
        {
            return new Wall2DVm(this);
        }
        public override EntitySurrogate ConvertToSurrogate()
        {
            return new Wall2DSurrogate(this);
        }
    }



}
