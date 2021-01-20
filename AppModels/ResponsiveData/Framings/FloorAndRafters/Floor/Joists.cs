using System.Collections.ObjectModel;

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
