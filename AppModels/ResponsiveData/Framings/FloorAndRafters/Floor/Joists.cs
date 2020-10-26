using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppModels.ResponsiveData.Framings.FloorAndRafters.Floor
{
    public class Joists
    {
        public string Name { get; set; }
        public int Quantities { get; set; }
        public int QuoteLength { get; set; }

        public ObservableCollection<Joist> JoistList { get; set; }

    }
}
