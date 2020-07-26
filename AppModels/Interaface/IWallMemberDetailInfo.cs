using AppModels.Enums;

namespace AppModels.Interaface
{
    public interface IWallMemberDetailInfo:IWallMemberInfo
    {
       IWallInfo WallInfo { get; }
    }
}