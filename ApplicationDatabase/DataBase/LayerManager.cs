using System;
using System.Collections.ObjectModel;
using System.Linq;
using ApplicationInterfaceCore;
using AppModels.AppData;
using devDept.Eyeshot;
using devDept.Serialization;
using Prism.Mvvm;

namespace AppModels
{
    public class LayerManager : BindableBase, ILayerManager
    {
        private ObservableCollection<LayerItem> _layers;
        private ObservableCollection<LayerItem> _seletedLayers;
        private LayerItem _selectedLayer;
        public event EventHandler SelectedPropertiesChanged;
        public LayerItem SelectedLayer
        {
            get
            {
                if (this._selectedLayer != null)
                {
                    return _selectedLayer;
                }
                if (this.Layers!= null & this.Layers.Count!=0)
                {
                    //Layers[0].PropertyChanged += SelectedLayer_PropertyChanged;
                    this.SelectedLayer = Layers[0];
                    return _selectedLayer;
                }

                return null;

            }
            set
            {
                if (_selectedLayer == value)
                {
                    return;
                }

                if (_selectedLayer==null)
                {
                    SetProperty(ref _selectedLayer, value);
                    _selectedLayer.PropertyChanged += SelectedLayer_PropertyChanged;
                }
                else
                {
                    _selectedLayer.PropertyChanged -= SelectedLayer_PropertyChanged;
                    SetProperty(ref _selectedLayer, value);
                    if (_selectedLayer!=null)
                    {
                        _selectedLayer.PropertyChanged += SelectedLayer_PropertyChanged;
                    }
                    
                }
            } 
            //RaisePropertyChanged(nameof(ActiveLayer));
        }

       

        public ObservableCollection<LayerItem> Layers
        {
            get => _layers;
            set => SetProperty(ref _layers, value);
        }
        public ObservableCollection<LayerItem> SelectedLayers
        {
            get => _seletedLayers;
            set => SetProperty(ref _seletedLayers, value);
        }
        public LayerKeyedCollection CanvasLayers { get; private set; }
        //public string ActiveLayer {
        //    get
        //    {
        //        if (SelectedLayer == null)
        //        {
        //            return "Default";
        //        }
        //        else
        //        {
        //            return SelectedLayer.Name;
        //        }
        //    }
        //    } 

        public LayerManager()
        {
            Layers= new ObservableCollection<LayerItem>();
            SelectedLayers = new ObservableCollection<LayerItem>();
            Layers.CollectionChanged += Layers_CollectionChanged;
        }

        private void SelectedLayer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SelectedPropertiesChanged?.Invoke(this,e);
        }

        public void SetLayerList(LayerKeyedCollection layers)
        {
            if (CanvasLayers== null)
            {
                CanvasLayers = layers;
            }
            SyncLayerListAndLayers();
            
        }

        private void SyncLayerListAndLayers()
        {
            foreach (var layer in CanvasLayers)
            {
                var checkName = Layers.Any(layerCheck => layerCheck.Name == layer.Name);
                if (checkName)
                {
                    return;
                }
                var layerItem =
                    new LayerItem(layer)
                    {
                        IsSelected = false,
                        PrintAble = true,
                    };
                this.Layers.Add(layerItem);
            }
        }

        public void Add(LayerItem addLayerItem)
        {
            this.Layers.Add(addLayerItem);
            if (this.CanvasLayers == null)
            {
                return;
            }
            //Layer
            AddCanvasLayer(addLayerItem);
        }

        public void RemoveAt(int id)
        {
            if (this.CanvasLayers == null)
            {
                return;
            }
            RemoveCanvasLayer(Layers[id]);
            this.Layers.RemoveAt(id);
            
        }

        public void Remove(LayerItem removeLayer)
        {
            this.Layers.Remove(removeLayer);
            RemoveCanvasLayer(removeLayer);

        }

        private void RemoveCanvasLayer(string removeLayerName)
        {
            foreach (var canvasLayer in CanvasLayers)
            {
                if (canvasLayer.Name != removeLayerName) continue;
                CanvasLayers.Remove(canvasLayer);
                return;
            }

        }

        private void RemoveCanvasLayer(LayerItem removeLayer)
        {
            if (CanvasLayers.Contains(removeLayer.Layer))
            {
                CanvasLayers.Remove(removeLayer.Layer);
            }
        }

        private void AddCanvasLayer(LayerItem addLayerItem)
        {
            //var checkName = CanvasLayers.Any(layerItem => layerItem.Name == addLayerItem.Name);
            //if (checkName)
            //{
            //    return;
            //}
            //var layer = new Layer(addLayerItem.Name, addLayerItem.Color, addLayerItem.LineTypeName, addLayerItem.LineWeight, addLayerItem.Visible, addLayerItem.Locked);
            if (CanvasLayers.Contains(addLayerItem.Layer))
            {
                return;
            }
            this.CanvasLayers.Add(addLayerItem.Layer);
        }
        private void Layers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action.ToString())
            {
                case "Add":
                {
                    if (e.NewItems!=null)
                    {
                        var addLayerItem = e.NewItems[0] as LayerItem;
                        this.AddCanvasLayer(addLayerItem);
                    }

                    break;
                }
                case "Remove":
                {
                    if (e.OldItems!=null)
                    {
                        var removeItem = e.NewItems[0] as LayerItem;
                        RemoveCanvasLayer(removeItem);
                    }

                    break;
                }
            }

            //if (e.OldItems.Count <= 0) return;
            //var oldItem = e.OldItems[0];
            //if (oldItem is LayerItem oldLayer)
            //{

            //}
            this.RaisePropertyChanged(nameof(SelectedLayer));
            //RaisePropertyChanged(nameof(ActiveLayer));
        }
    }
}
