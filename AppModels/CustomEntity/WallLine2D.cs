using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Media.Media3D;
using AppModels.CustomEntity.CustomEntitySurrogate;
using AppModels.Interaface;
using AppModels.ViewModelEntity;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Serialization;

namespace AppModels.CustomEntity
{
    [Serializable]
    public class WallLine2D: Line,IEntityVmCreateAble,IWall2D
    {
        #region Field
        //private string _wallLevelName;
        //private int _wallHeight;
        //private string _wallLength;
        #endregion

        #region Properties

        public string WallLevelName { get; set; }

        #endregion

        public WallLine2D(Line another) : base(another)
        {
        }

        protected WallLine2D(WallLine2D another):base(another)
        {
            WallLevelName = another.WallLevelName;
        }
        public WallLine2D(Point3D start, Point3D end) : base(start, end)
        {

        }

        public override object Clone()
        {
            return new WallLine2D(this);
        }

        public IEntityVm CreateEntityVm(IEntitiesManager entitiesManager)
        {
            return new WallLine2DVm(this);
        }
        public override EntitySurrogate ConvertToSurrogate()
        {
            return new Wall2DSurrogate(this);
        }
    }



}
