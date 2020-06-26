using System.Collections.ObjectModel;
using System.ComponentModel;
using AppModels.AppData;
using devDept.Eyeshot;

namespace ApplicationInterfaceCore
{
    public interface ILayerManager: INotifyPropertyChanged
    {
        ObservableCollection<LayerItem> Layers { get; }
        LayerItem SelectedLayer { get; }
        LayerKeyedCollection CanvasLayers { get; }
        //string ActiveLayer { get; }
        ObservableCollection<LayerItem> SelectedLayers { get; }
        void SetLayer(LayerKeyedCollection layers);
        
        void Add(LayerItem addLayerItem);
        void RemoveAt(int id);
        void Remove(LayerItem removeLayer);

    }

}
