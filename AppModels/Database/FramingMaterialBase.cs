using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;

namespace AppModels.Database
{
    public class FramingMaterialBase
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public double Price { get; set; }
        public string Grade { get; set; }
        public int Thickness { get; set; }
        public int Depth { get; set; }
        public int Treatment { get; set; }
        public int NoItem { get; set; }
        public int StockLength { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitCostPrice { get; set; }
        public decimal UnitCostSell { get; set; }
        public MaterialTypes FramingType { get; set; }
    }
}

