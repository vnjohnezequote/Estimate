using AppModels.Enums;

namespace AppModels.Interaface
{
    public interface ISupportMember
    {
        string Id { get; }
        SupportType PointSupportType { get; }
        ISupportInfo SupportInfo { get; }
    }
}