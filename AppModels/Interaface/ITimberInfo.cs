using System.ComponentModel;
using AppModels.Enums;
using AppModels.PocoDataModel;

namespace AppModels.Interaface
{
    public interface ITimberInfo : INotifyPropertyChanged
    {
        int NoItem { get; }
        int Thickness { get; }
        int Depth { get; }
        string TimberGrade { get; }
        string Size { get; }
        string SizeGrade { get; }
    }
}