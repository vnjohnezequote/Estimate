using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using ApplicationInterfaceCore;
using AppModels.AppData;
using devDept.Eyeshot;
using Prism.Mvvm;

namespace AppDataBase.DataBase
{
    public class LayerManager : BindableBase, ILayerManager
    {
        private ObservableCollection<LayerItem> _layers;
        private ObservableCollection<LayerItem> _selectedLayers;
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
            get => _selectedLayers;
            set => SetProperty(ref _selectedLayers, value);
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
            if (CanvasLayers.Count == 1 && CanvasLayers[0].Name == "Default")
            {
                PrepairLayer();
                CanvasLayers.RemoveAt(0);

            }
            else
            {
                foreach (var layer in CanvasLayers)
                {
                    var checkName = Layers.Any(layerCheck => layerCheck.Name == layer.Name);
                    if (checkName)
                    {
                        continue;
                    }
                    var layerItem =
                        new LayerItem(layer)
                        {
                            IsSelected = false,
                            PrintAble = true,
                        };
                    this.Layers.Add(layerItem);
                    //if (layerItem.Name == "Default")
                    //{
                    //    layerItem.Color = Color.Red;
                    //}
                }

            }
        }

        private void PrepairLayer()
        {
            for (int i = 0; i < 16; i++)
            {
                var newLayer = new LayerItem(){Name ="Wall2D"+ i,LineWeight = 5};
                Add(newLayer);
            }
            Layers[0].Color = Color.Blue;
            Layers[1].Color = Color.Red;
            Layers[2].Color = Color.Orange;
            Layers[3].Color = Color.LimeGreen;
            Layers[4].Color = Color.Magenta;
            Layers[5].Color=Color.Aqua;
            Layers[6].Color = Color.Maroon;
            Layers[7].Color = Color.CornflowerBlue;
            Layers[8].Color = Color.DarkOliveGreen;
            Layers[9].Color = Color.DarkGoldenrod;
            Layers[10].Color = Color.RoyalBlue;
            Layers[11].Color = Color.DodgerBlue;
            Layers[12].Color = Color.MediumVioletRed;
            Layers[13].Color = Color.SaddleBrown;
            Layers[14].Color = Color.MidnightBlue;
            Layers[15].Color = Color.MediumSpringGreen;
            var beamLayer = new LayerItem();
            beamLayer.Color = Color.Blue;
            beamLayer.Name = "Beam";
            beamLayer.LineWeight = 5;
            Layers.Add(beamLayer);
            var beamLayer2 = new LayerItem();
            beamLayer2.Color = Color.Aqua;
            beamLayer2.Name = "Beam Stick Frame";
            beamLayer2.LineTypeName = "Dash Space";
            beamLayer2.LineWeight = 5;
            Layers.Add(beamLayer2);
            var planLayer = new LayerItem();
            planLayer.Name = "Plan";
            planLayer.Color = Color.Red;
            planLayer.PrintAble = false;
            Layers.Add(planLayer);
            var beamMarkedLayer = new LayerItem();
            beamMarkedLayer.Name = "BeamMarked";
            beamMarkedLayer.Color = Color.Blue;
            beamMarkedLayer.LineWeight = 3;
            Layers.Add(beamMarkedLayer);
        
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

            if (Layers[id].Name == "Plan" || Layers[id].Name == "BeamMarked" || Layers[id].Name .Contains("Beam"))
            {
                MessageBox.Show("This layer can not be deleted");
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
                        //var removeItem = e.NewItems[0] as LayerItem;
                        //RemoveCanvasLayer(removeItem);
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
