using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel.Openings;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Framings;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Openings
{
    public class Hanger : BindableBase, IFraming
    {
        private HangerMat _hangerMat;
        private int _quantity;
        private string _name;
        private FramingTypes _framingType;
        private int _index;
        private int _subFixIndex;
        private bool _isExisting;
        public Guid Id { get; set; }
        public Guid LevelId { get; }
        public Guid FramingSheetId { get; }
        public LevelWall Level { get; set; }
        public HangerMat HangerMaterial { get => _hangerMat; set => SetProperty(ref _hangerMat, value); }
        public string Name
        {
            get
            {
                if (!string.IsNullOrEmpty(_name))
                {
                    return _name;
                }

                if (SubFixIndex !=0)
                {
                    return NamePrefix + Index + "." + SubFixIndex;
                }

                if (Index!=0)
                {
                    return NamePrefix + Index;
                }

                return NamePrefix;

            }
            set
            {
                if (value == Name)
                {
                    value = string.Empty;
                }

                SetProperty(ref _name, value);
            }
        }

        public string NamePrefix => "H";
        public int SubFixIndex
        {
            get => _subFixIndex;
            set
            {
                SetProperty(ref _subFixIndex, value);
                RaisePropertyChanged(nameof(Name));
            }
        }

        public bool IsExisting
        {
            get=>_isExisting; 
            set=>SetProperty(ref _isExisting,value);
        }
        /*** not Implement**/
        public double QuoteLength { get; }
        public int FramingSpan { get; }
        public int FullLength { get; set; }
        public double ExtraLength { get; set; }
        public double Pitch { get; set; }

       
        public FramingSheet FramingSheet { get; }


        /*** Not Implement ***/
        public TimberBase FramingInfo { get; set; }
        public string TimberGrade { get; set; }

        public FramingTypes FramingType
        {
            get => FramingTypes.Hanger;
            set {/***Not Implement***/ }
        }
        public int Quantity { get=>_quantity; set=>SetProperty(ref _quantity,value); }
        public int Index { get => _index; set => SetProperty(ref _index, value); }
        public EngineerMemberInfo EngineerMember { get; set; }
        
        
        /*** Not Implement***/
        public bool IsLongerStockList { get; }

       

        public Hanger()
        {

        }

        public Hanger(FramingSheet framingSheet)
        {
            Id = Guid.NewGuid();
            LevelId = framingSheet.LevelId;
            FramingSheetId = framingSheet.Id;
            FramingSheet = framingSheet;
            Level = framingSheet.Level;
            Quantity = 1;
        }

        public Hanger(HangerPoco hanger, LevelWall level,HangerMat hangerMat)
        {
            Id = hanger.Id;
            LevelId = hanger.LevelId;
            FramingSheetId = hanger.FramingSheetId;
            HangerMaterial = hangerMat;
            IsExisting = hanger.IsExisting;
            SubFixIndex = hanger.SubFixIndex;
            Index = hanger.Index;
            Level = level;
            Name = hanger.Name;
            Quantity = hanger.Quantity;
        }

        public Hanger(Hanger another)
        {
            Id = Guid.NewGuid();
            LevelId = another.LevelId;
            FramingSheetId = another.FramingSheetId;
            FramingSheet = another.FramingSheet;
            HangerMaterial = another.HangerMaterial;
            IsExisting = another.IsExisting;
            SubFixIndex = another.SubFixIndex;
            Level = another.Level;
            Name = another.Name;
            Quantity = another.Quantity;

        }

        public object Clone()
        {
            return new Hanger(this);
        }
    }
}
