using AppModels.ResponsiveData;

namespace AppModels.Interaface
{
    public interface IWallMember
    {
        IWallMemberInfo TimberMaterialInfo { get; }
        IWallInfo WallInfo { get; }
        int Thickness { get; }
    }
}