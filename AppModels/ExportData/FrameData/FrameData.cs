using System;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.Openings;
using Prism.Mvvm;

namespace AppModels.ExportData.FrameData
{
    public class FrameData : BindableBase,IComparable<FrameData>
    {
        #region MyRegion

        private int _quantity;
        private double _length;
        private string _name;
        #endregion

        #region Properties
        public string Name { get=>_name; set=>SetProperty(ref _name,value); }
        public string Material { get; set; } = string.Empty;
        public FramingTypes FramingType { get; set; }
        public bool IsDouble { get; set; }
        public int Quantity
        {
            get => _quantity;
            set
            {
                SetProperty(ref _quantity, value);
                RaisePropertyChanged(nameof(TotalLength));
            }
        }
        public string FullName { get; set; }
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
                IsDouble = framing.FramingInfo.NoItem > 1;
            }
            Length = framing.QuoteLength;
            Quantity = framing.Quantity;
            this.FramingType = framing.FramingType;
            
            CreateFullName(framing);
        }

        private void CreateFullName(IFraming framing)
        {
            string preName = string.Empty;
            switch (FramingType)
            {
                case FramingTypes.FloorJoist:
                    preName = IsDouble ? "Double Joist" : "Floor Joist";
                    break;
                case FramingTypes.RafterBeam:
                    preName = "Roof Beam";
                    break;
                case FramingTypes.FloorBeam:
                    preName = "Floor Beam";
                    break;
                case FramingTypes.RafterJoist:
                    preName = IsDouble ? "Double Rafter" : "Rafter";
                    break;
                case FramingTypes.BoundaryJoist:
                    preName = "Boundary Joist";
                    break;
                case FramingTypes.CeilingJoist:
                    preName = "Ceiling Joist";
                    break;
                case FramingTypes.Purlin:
                    preName = "Purlin";
                    break;
                case FramingTypes.PolePlate:
                    preName = "Pole plate";
                    break;
                case FramingTypes.Fascia:
                    preName = "Fascia";
                    break;
                case FramingTypes.TieDown:
                    preName = "TieDown";
                    break;
                case FramingTypes.Hanger:
                    preName = "Hanger";
                    break;
                case FramingTypes.Trimmer:
                    preName = "Trimmer";
                    break;
                case FramingTypes.Rimboard:
                    preName = "Rimbard";
                    break;
                case FramingTypes.HipRafter:
                    preName = "HiprRafter";
                    break;
                case FramingTypes.TrussBeam:
                    preName = "Roof Beam";
                    break;
                case FramingTypes.Blocking:
                    preName = "Blocking";
                    break;
                case FramingTypes.RidgeBeam:
                    preName = "Ridge Beam";
                    break;
                case FramingTypes.OutTrigger:
                    preName = "Outrigger";
                    break;
                case FramingTypes.RafterOutTrigger:
                    preName = "OutTrigger";
                    break;
                case FramingTypes.DeckJoist:
                    preName = IsDouble ? "Double Deck Joist" : "Deck Joist";
                    
                    break;
                case FramingTypes.LintelBeam:
                    preName = "Lintel Beam";
                    break;
                case FramingTypes.SteelBeam:
                    preName = "Steel Beam";
                    break;
                case FramingTypes.DeckBeam:
                    preName = "Deck Beam";
                    break;
                case FramingTypes.DeckRoofBeam:
                    preName = "Deck Roof Beam";
                    break;
                default:
                    preName = "";
                    break;
                    
            }

            FullName = preName + " (" + Name + ")";
        }
        public FrameData(Hanger hanger, string name)
        {
            Name = name;
            if (hanger.HangerMaterial!=null)
            {
                Material = hanger.HangerMaterial.Supplier + " " + hanger.HangerMaterial.Size;
            }

            Quantity = hanger.Quantity;
            this.FramingType = hanger.FramingType;
            this.CreateFullName(hanger);

        }
        public bool IsSameWith(FrameData another)
        {
            return this.Material == another.Material && this.Name == another.Name && Math.Abs(this.Length - another.Length) < 0.001;
            
        }

        public int CompareTo(FrameData other)
        {
            return this.FramingType.CompareTo(other.FramingType);
        }
    }
}
