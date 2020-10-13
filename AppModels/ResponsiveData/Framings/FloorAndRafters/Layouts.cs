using System.Collections.ObjectModel;
using AppModels.ResponsiveData.Framings.Blocking;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.ResponsiveData.Openings;

namespace AppModels.ResponsiveData.Framings.FloorAndRafters
{
    public class Layout
    {
        public ObservableCollection<Beam> RafterBeams { get; set; }
        public ObservableCollection<Joist> Rafters { get; set; } = new ObservableCollection<Joist>();
        public ObservableCollection<BlockingList> BlockingList { get; set; } = new ObservableCollection<BlockingList>();
        public ObservableCollection<TimberBase> PolePlates { get; set; } = new ObservableCollection<TimberBase>();
        //public ObservableCollection<BoundaryJoits> BoundaryFraming { get; set; } = new ObservableCollection<BoundaryJoits>();
        //public ObservableCollection<Fascia> Facias { get; set; } = new ObservableCollection<Fascia>();
        //public ObservableCollection<TieDown> TieDowns { get; set; } = new ObservableCollection<TieDown>();
        //public ObservableCollection<Joist> Purlins { get; set; } = new ObservableCollection<Joist>();
        //public ObservableCollection<Joist> CeilingJoists { get; set; } = new ObservableCollection<Joist>();
    }
}
