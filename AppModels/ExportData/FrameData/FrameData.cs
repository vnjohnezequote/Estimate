using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings.Blocking;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.ResponsiveData.Openings;
using Prism.Mvvm;

namespace AppModels.ExportData.FrameData
{
    public class FrameData : BindableBase
    {
        #region MyRegion

        private int _quantity;
        private double _length;
        private string _name;
        #endregion

        #region Properties
        public string Name { get=>_name; set=>SetProperty(ref _name,value); }
        public string Material { get; set; }

        public int Quantity
        {
            get => _quantity;
            set
            {
                SetProperty(ref _quantity, value);
                RaisePropertyChanged(nameof(TotalLength));
            }
        }

        public double Length
        {
            get => _length;
            set
            {
                SetProperty(ref _length, value);
                RaisePropertyChanged(nameof(TotalLength));
            }
        }
        public double TotalLength => Length * Quantity;

        #endregion

        public FrameData(IFraming framing, string name)
        {
            Name = name;
            if (framing.FramingInfo != null)
            {
                Material = framing.FramingInfo.SizeGradeTreatment;
            }
            Length = framing.QuoteLength;
            Quantity = framing.Quantity;
        }

        public FrameData(Hanger hanger, string name)
        {
            Name = name;
            if (hanger.HangerMaterial!=null)
            {
                Material = hanger.HangerMaterial.Supplier + " " + hanger.HangerMaterial.Size;
            }

            Quantity = hanger.Quantity;
        }
        public bool IsSameWith(FrameData another)
        {
            return this.Material == another.Material && this.Name == another.Name && Math.Abs(this.Length - another.Length) < 0.001;
        }
    }
}
