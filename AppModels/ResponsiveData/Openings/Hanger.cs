using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Openings
{
    public class Hanger : BindableBase
    {
        private int _id;
        private string _size;
        private string _supplier;
        private double _unitPrice;
        public int ID { get=>_id; set=>SetProperty(ref _id,value); }
        public string Size
        {
            get => _size;
            set => SetProperty(ref _size, value);
        }

        public string Supplier
        {
            get => _supplier;
            set => SetProperty(ref _supplier, value);
        }
        public double UnitPrice { get=>_unitPrice; set=>SetProperty(ref _unitPrice,value); }
    }
}
