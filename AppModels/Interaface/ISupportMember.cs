using AppModels.Enums;
using AppModels.ResponsiveData.EngineerMember;

namespace AppModels.Interaface
{
    public interface ISupportMember
    {
        string Id { get; set; }
        SupportType PointSupportType { get; set; }
        EngineerMemberInfo SupportInfo { get; set; }
    }
}