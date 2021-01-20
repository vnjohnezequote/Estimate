using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Framings.Blocking
{
    public class BlockingList: BindableBase
    {
        #region Field

        private string _name;

        #endregion

        #region Properties
        public ObservableCollection<Blocking> Blockings { get; set; }
        public string Name { get=>_name; set=>SetProperty(ref _name,value); }
        
        public int Quantities => Blockings.Count;
        public int QuoteLength { get; set; }


        #endregion

        #region Constructor

        public BlockingList()
        {
            Blockings = new ObservableCollection<Blocking>();
            Blockings.CollectionChanged += Blockings_CollectionChanged;
        }

        private void Blockings_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Quantities));
        }
        #endregion

    }
}
