using AppModels.Enums;
using AppModels.ResponsiveData.Openings;

namespace AppModels.PocoDataModel.Openings
{
    public class OpeningInfoPoco
    {
        public int Id { get; set; }
        public string Name { get ; set ; }

        public int Width { get ; set ; }

        public int Height { get; set ; }
        public WallLocationTypes DoorTypeLocation { get ; set ; }

        public OpeningType DoorType { get ; set ; }

        public OpeningInfoPoco()
        {

        }
        public OpeningInfoPoco(OpeningInfo openingInfo)
        {
            Id = openingInfo.Id;
            Name = openingInfo.Name;
            Width = openingInfo.Width;
            Height = openingInfo.Height;
            DoorTypeLocation = openingInfo.DoorTypeLocation;
            DoorType = openingInfo.DoorType;
        }

    }
}
