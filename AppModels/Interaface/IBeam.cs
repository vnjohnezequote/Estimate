using System.Collections.ObjectModel;
using System.ComponentModel;
using AppModels.Enums;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Openings;
using AppModels.ResponsiveData.Support;

namespace AppModels.Interaface
{
    public interface IBeam : INotifyPropertyChanged
    {
        SupportType? PointSupportType { get; }
        EngineerMemberInfo SupportReference { get; }
        int SupportHeight { get; }
        int RealSupportHeight { get; }
        string Name { get; }
        int TotalSupportWidth { get; }
        ObservableCollection<SupportPoint> LoadPointSupports { get; }
    }
}