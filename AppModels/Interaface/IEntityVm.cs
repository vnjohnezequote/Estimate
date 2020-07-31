using System.ComponentModel;
using System.Drawing;
using devDept.Eyeshot.Entities;

namespace AppModels.Interaface
{
    public interface IEntityVm: INotifyPropertyChanged
    {
        IEntity Entity { get; }
        Color? Color { get; }
        string LayerName { get; }
        colorMethodType ColorMethod { get; }

        void NotifyPropertiesChanged();

    }
}