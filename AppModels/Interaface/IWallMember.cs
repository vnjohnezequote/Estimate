using AppModels.ResponsiveData;

namespace AppModels.Interaface
{
    public interface IWallMember
    {
        IWallMemberInfo TimberMaterialInfo { get; }
        IWallInfo WallInfo { get; }
        string Thickness { get; }
    }
}