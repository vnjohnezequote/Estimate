using System.ComponentModel;
using AppModels.Enums;
using AppModels.ResponsiveData.EngineerMember;

namespace AppModels.Interaface
{
    public interface IBeam : INotifyPropertyChanged
    {
        SupportType PointSupportType { get; }
        EngineerMemberInfo SupportReference { get; }
    }
}