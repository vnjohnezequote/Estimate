using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Interaface;
using AppModels.PocoDataModel;

namespace AppModels.ResponsiveData
{
    public class StickFrameWallLayer : WallBase
    {
        #region Properties
        public override int FinalWallHeight
        {
            get
            {
                if (WallType.IsRaked || IsWallUnderRakedArea || ForcedWallUnderRakedArea)
                {
                    return StudHeight;
                }

                return WallHeight;
            }
        }

        #endregion

        #region Constructor

        public StickFrameWallLayer(int id, IGlobalWallInfo globalWallInfo, WallTypePoco wallType,string levelName, int typeId = 1) : base(id, globalWallInfo, wallType,levelName, typeId)
        {
        }

        #endregion



    }
}
