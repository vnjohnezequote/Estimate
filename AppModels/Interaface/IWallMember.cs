using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppModels.Interaface
{
    public interface IWallMember:INotifyPropertyChanged
    {
        IGlobalWallInfo GlobalWallInfo { get; }
        string NoItem { get; }
        string Thickness { get; }
        string Depth { get; }
        string TimberGrade { get; }
    }
}
