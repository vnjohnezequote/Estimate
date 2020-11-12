using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.ResponsiveData.Openings;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Framings
{
    public class FramingSheet: BindableBase
    {
        private int _id;
        private string _floorName;
        private bool _showSheetId;
        public IGlobalWallInfo GloaWallInfo { get; }
        public int Id
        {
            get => _id;
            set
            {
                SetProperty(ref _id, value);
                RaisePropertyChanged(nameof(Name));
            } 
        }

        public bool ShowSheetId
        {
            get => _showSheetId;
            set
            {
                SetProperty(ref _showSheetId, value);
                RaisePropertyChanged(nameof(Name));
            }
        }

        public string Name
        {
            get
            {
                if (ShowSheetId)
                {
                    return FloorName + " " + Id;
                }

                return FloorName;
            }
        }

        public string FloorName
        {
            get => _floorName;
            set
            {
                SetProperty(ref _floorName, value);
                RaisePropertyChanged(nameof(Name));
            }
        }
            
        public ObservableCollection<Joist> Joists { get; set; } = new ObservableCollection<Joist>();
        public ObservableCollection<Beam> Beams { get; set; } = new ObservableCollection<Beam>();
        public ObservableCollection<OutTrigger> OutTriggers { get; set; } = new ObservableCollection<OutTrigger>();
        public ObservableCollection<Hanger> Hangers { get; set; } = new ObservableCollection<Hanger>();
        public FramingSheet(IGlobalWallInfo globalInfo)
        {
            GloaWallInfo = globalInfo;
            this.PropertyChanged+=OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Name))
            {
                foreach (var joist in Joists)
                {
                    joist.SheetName = this.Name;
                }

                foreach (var beam in Beams )
                {
                    beam.SheetName = this.Name;
                }

                foreach (var outTrigger in OutTriggers)
                {
                    outTrigger.SheetName = this.Name;
                }
            }
        }
    }
}
