using System.Drawing;

namespace AppModels.AppData
{
    public class ColorItem 
    {
        //private Color _color;
        public string Name => Color.ToString();

        public Color Color
        {
            get;
            private set;
        }

        public ColorItem(Color color)
        {
            Color = color;
        }
    }
}
