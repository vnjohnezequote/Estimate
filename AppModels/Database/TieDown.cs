using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Framings
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
