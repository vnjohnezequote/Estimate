using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot;
using Prism.Mvvm;

namespace AppModels.AppData
{
    public class LayerItem : BindableBase,IDisposable
    {
        private string _name;
        private string _lineTypeName;
        private float _lineWeight;
        private bool _isSeleted;
        private bool _isActivate;
        private bool _printAble;
        private bool _locked;
        private bool _visible;
        private bool _selectAble;
        private Color _color;

        public bool IsSelected
        {
            get=>_isSeleted;
            set=>SetProperty(ref _isSeleted,value);
        }
        public bool IsActivate { 
            get=>_isActivate;
            set=>SetProperty(ref _isActivate,value);

        }
        public bool PrintAble {
            get=>_printAble;
            set=>SetProperty(ref _printAble,value);

        }
        public bool Locked { 
            get=>_locked;
            set=>SetProperty(ref _locked, value);

        }

        public bool Visible
        {
            get=>_visible;
            set=>SetProperty(ref _visible,value);
        }

        public bool SelectAble
        {
            get=>_selectAble;
            set=>SetProperty(ref _selectAble,value);
        }

        public Color Color { 
            get=>_color;
            set => SetProperty(ref _color, value);
        }

        public string Name
        {
            get=>_name;
            set=>SetProperty(ref _name,value);
        }

        public string LineTypeName
        {
            get=>_lineTypeName;
            set=>SetProperty(ref _lineTypeName,value);
        }

        public float LineWeight
        {
            get=>_lineWeight;
            set=>SetProperty(ref _lineWeight,value);
        }

        public Layer Layer { get; private set; }

        #region Constructor

        public LayerItem()
        {
            Name = "New Layer";
            LineWeight = 0.5f;
            Color = Color.Red;
            this.Layer = new Layer(Name, Color);
            LineTypeName = "Continues";
            Locked = false;
            Visible = true;
            InitCompnent();

        }

        public LayerItem(string layerName)
        {
            Name = layerName;
            LineWeight = 0.5f;
            Color = Color.Red;
            this.Layer = new Layer(layerName,Color);
            LineTypeName = "Continues";
            Locked = false;
            Visible = true;
            InitCompnent();
        }

        public LayerItem (Layer layer)
        {
            this.Layer = layer;
            this.Name = layer.Name;
            if (string.IsNullOrEmpty(layer.LineTypeName))
            {
                this.LineTypeName = "Continues";
            }
            else
            {
                this.LineTypeName = layer.LineTypeName;
            }
            
            LineWeight = layer.LineWeight;
            Locked = layer.Locked;
            Visible = layer.Visible;
            Color = layer.Color;
           InitCompnent();
            
        }

        private void InitCompnent()
        {
            IsSelected = false;
            IsActivate = false;
            PrintAble = true;
            SelectAble = true;
            PropertyChanged += LayerItem_PropertyChanged;
        }
        private void LayerItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName==nameof(Name))
            {
                Layer.Name = Name;
            }

            if (e.PropertyName == nameof(LineWeight))
            {
                Layer.LineWeight = LineWeight;
            }

            if (e.PropertyName== nameof(LineTypeName))
            {
                if (LineTypeName == "Continues")
                {
                    Layer.LineTypeName = (string) null;
                }
                else
                {
                    Layer.LineTypeName = LineTypeName;
                }
                
            }

            if (e.PropertyName == nameof(Visible))
            {
                Layer.Visible = Visible;
            }

            if (e.PropertyName == nameof(Locked))
            {
                Layer.Locked = Locked;
            }

            if (e.PropertyName == nameof(Color))
            {
                Layer.Color = Color;
            }
        }

        

        #endregion

        public void Dispose()
        {
            Layer?.Dispose();
        }
    }
}
