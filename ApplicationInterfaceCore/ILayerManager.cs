using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AppModels.AppData;
using devDept.Eyeshot;

namespace ApplicationInterfaceCore
{
    public interface ILayerManager: INotifyPropertyChanged
    {
        event EventHandler SelectedPropertiesChanged;
        ObservableCollection<LayerItem> Layers { get; }
        LayerItem SelectedLayer { get; set; }
        LayerKeyedCollection CanvasLayers { get; }
        //string ActiveLayer { get; }
        ObservableCollection<LayerItem> SelectedLayers { get; }
        void SetLayerList(LayerKeyedCollection layers);
        
        void Add(LayerItem addLayerItem);
        void RemoveAt(int id);
        void Remove(LayerItem removeLayer);

    }

}
