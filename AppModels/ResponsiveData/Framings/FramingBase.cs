using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Framings
{
    public abstract class FramingBase: BindableBase
    {
        #region Field\

        private int _id;
        private double _pitch;
        private bool _isExisting;
        private TimberBase _framingInfo;
        private int _framingSpan;
        private FramingSheet _floorSheet;
        private string _sheetName;
        private string _timberGrade;
        private int _fullLength;
        private double _extraLength;
        private string _sizeGradeTreatment { get; set; }
        

        #endregion

        #region Properties
        public int Id { get=>_id; set=>SetProperty(ref _id,value); }
        public bool IsExisting { get => _isExisting; set => SetProperty(ref _isExisting, value); }
        public abstract double QuoteLength { get; }
        public int FramingSpan { get=>_framingSpan; set=>SetProperty(ref _framingSpan,value); }
        public int FullLength { 
            get=>_fullLength; 
            set
        {
            SetProperty(ref _fullLength, value);
            RaisePropertyChanged(nameof(QuoteLength));
            }
        }

        public double ExtraLength
        {
            get => _extraLength;
            set
            {
                SetProperty(ref _extraLength, value);
                RaisePropertyChanged(nameof(QuoteLength));
            }
        }

        public double Pitch { get=>_pitch; set=>SetProperty(ref _pitch,value); }
        public string SheetName { get=>_sheetName; set=>SetProperty(ref _sheetName,value); }
        public FramingSheet FloorSheet { get; set; }
        public TimberBase FramingInfo
        {
            get => _framingInfo;
            set
            {
                SetProperty(ref _framingInfo, value);
                TimberGrade = value != null ? value.TimberGrade : string.Empty;
            }
        }

        public string TimberGrade
        {
            get => _timberGrade;
            set => SetProperty(ref _timberGrade, value);
        }


        #endregion

        #region Constructor

        protected FramingBase()
        {
            PropertyChanged += FramingBasePropertyChanged;
        }
        #endregion

        #region Private Method
        private void FramingBasePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(FramingSpan):
                case nameof(Pitch):
                    NotifyPropertiesChanged();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Protected Method
        protected virtual void NotifyPropertiesChanged()
        {
            RaisePropertyChanged(nameof(QuoteLength));
        }
        #endregion
        #region Public Method
        #endregion

    }
}
