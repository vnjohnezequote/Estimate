using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.ResponsiveData.Framings.Blocking;

namespace AppModels.PocoDataModel.Framings.Blocking
{
    public class TieDownPoco
    {
        public Guid Id { get; set; }
        public int TieDownMatId { get; set; }
        public int Qty { get; set; }

        public TieDownPoco()
        {

        }
        public TieDownPoco(TieDown tiewDown)
        {
            Id = tiewDown.Id;
            if (tiewDown.Material!=null)
            {
                TieDownMatId = tiewDown.Material.Id;
            }

            Qty = tiewDown.Qty;
        }
    }
}
