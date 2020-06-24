using System.Windows.Media;
using devDept.Eyeshot;
using devDept.Graphics;

namespace DrawingModule.CommandLine
{
    public class ListViewModelItem
    {
        public ListViewModelItem(Layer layer)
        {
            Layer = layer;
            IsChecked = layer.Visible;
            ForeColor = RenderContextUtility.ConvertColor(Layer.Color);
        }

        public Layer Layer { get; set; }

        public string LayerName => Layer.Name;

        public float LayerLineWeight => Layer.LineWeight;

        public Brush ForeColor { get; set; }

        public bool IsChecked { get; set; }
    }
}
