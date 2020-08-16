using AppModels.Enums;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Openings
{
    public class OpeningInfo: BindableBase
    {
        private string _name;
        private int _width;
        private int _height;
        private OpeningType _doorType;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public int Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        public int Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        public OpeningType DoorType
        {
            get => _doorType;
            set => SetProperty(ref _doorType, value);
        }
    }
}