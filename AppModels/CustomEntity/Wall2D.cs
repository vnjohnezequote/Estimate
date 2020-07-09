using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;

namespace AppModels.CustomEntity
{
    public class Wall2D: Line
    {
        #region Field
        //private string _wallLevelName;
        //private int _wallHeight;
        //private string _wallLength;
        #endregion

        #region Properties

        public string WallLevelName { get; set; }
        #endregion

        protected Wall2D(Line another) : base(another)
        {
            
        }

        
    }
}
