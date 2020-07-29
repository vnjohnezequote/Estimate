using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.ResponsiveData;

namespace AppModels.Factories
{
    public class WallLayerFactory
    {
        public static WallBase CreateWallLayer(string clientName, int wallId, IGlobalWallInfo levelInfo, WallTypePoco wallType)
        {
            switch (clientName)
            {
                case "StickFrame":
                    return new StickFrameWallLayer(wallId,levelInfo,wallType);
                default:
                    return new PrenailWallLayer(wallId,levelInfo,wallType);
            }

        }
    }
}
