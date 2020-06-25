using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

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
