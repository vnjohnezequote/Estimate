using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Database;
using AppModels.PocoDataModel.Framings.Blocking;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Framings.Blocking
{
    public class TieDown: BindableBase
    {
        private TieDownMat _material;
        private int _qty;
        public Guid Id { get; set; }
        public TieDownMat Material { get=>_material; set=>SetProperty(ref _material,value); }
        public int Qty { get=>_qty; set=>SetProperty(ref _qty,value); }
        public TieDown()
        {
            Id = Guid.NewGuid();
        }
        public TieDown (TieDownPoco tiewDownPoco, TieDownMat timberMat)
        {
            Id = tiewDownPoco.Id;
            Qty = tiewDownPoco.Qty;
            Material = timberMat;
        }

    }
}
