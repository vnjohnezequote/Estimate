using Prism.Mvvm;

namespace AppModels.Database
{
    public class TieDown: BindableBase
    {
        #region Field

        private int _id;

        #endregion

        #region Properties
        public int Id { get=>_id; set=>SetProperty(ref _id,value); }

        public string SupplierName { get; set; }
        public double Price { get; set; }


        #endregion
    }
}
