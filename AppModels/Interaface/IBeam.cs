using System.ComponentModel;
using AppModels.Enums;

namespace AppModels.Interaface
{
    public interface IBeam : INotifyPropertyChanged
    {
        SupportType PointSupportType { get; }
        int NumberOfSupport { get; }
    }
}