using AppModels.Enums;
using AppModels.ResponsiveData.Openings;
using Prism.Mvvm;

namespace AppModels.ExportData
{
    public class ExportDoorData : BindableBase
    {
        #region private field
        private int _height;
        private int _width;
        private OpeningType _doorTypes;
        private int _qty;
        private int _trussSpan;
        #endregion

        #region Properties
        public int Height { get => _height; set => SetProperty(ref _height, value); }
        public int Width { get => _width; set => SetProperty(ref _width, value); }

        public OpeningType DoorType { get => _doorTypes; set => SetProperty(ref _doorTypes, value); }
        public int Qty { get => _qty; set => SetProperty(ref _qty, value); }
        public int TrussSpan { get=>_trussSpan; set=>SetProperty(ref _trussSpan,value); }
        #endregion
        public ExportDoorData(Opening opening)
        {
            if (opening.OpeningType != null) DoorType = (OpeningType) opening.OpeningType;
            Height = opening.Height;
            Width = opening.Width;
            TrussSpan = opening.SupportSpan;
        }
    }
}
