using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.ResponsiveData;

namespace AppModels.Factories
{
    public class WallLayerFactory
    {
        public static WallBase CreateWallLayer(string clientName, int wallId, IGlobalWallInfo levelInfo, WallTypePoco wallType,string levelName)
        {
            switch (clientName)
            {
                case "StickFrame":
                    return new StickFrameWallLayer(wallId,levelInfo,wallType,levelName);
                case "Warnervale":
                    return new WarnervaleWallLayer(wallId,levelInfo,wallType,levelName);
                default:
                    return new PrenailWallLayer(wallId,levelInfo,wallType,levelName);
            }

        }
    }
}
