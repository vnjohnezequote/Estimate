using System.ComponentModel;
using System.Drawing;
using devDept.Eyeshot.Entities;

namespace AppModels.Interaface
{
    public interface IEntityVm: INotifyPropertyChanged
    {
        IEntity Entity { get; }
        System.Windows.Media.Color Color { get; }
        string LayerName { get; }

        void NotifyPropertiesChanged();

    }
}