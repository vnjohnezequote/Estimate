using AppModels.Enums;
using AppModels.PocoDataModel.Openings;
using AppModels.ResponsiveData.Openings;

namespace AppModels.PocoDataModel
{
    
    public class OpeningPoco
    {
        public int WallReferenceId { get; set; }
        public int DoorWindowInfoId { get; set; }
        public int NoDoor { get; set; }
        public bool IsGarageDoor { get; set; }
        public DoorTypes DoorType { get; set; }
        public NumberOfDoors DoorNumberType { get; set; }
        public WallLocationTypes? DoorTypeLocation { get; set; }
        public int Id { get; set; }
        public OpeningType OpeningType { get; set; }
        public string Location { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int DoorHeaderHeight { get; set; }
        public LintelBeamPoco Lintel { get ; set; }
        public int SupportSpan { get; set; }

        public OpeningPoco()
        {

        }
        public OpeningPoco(Opening openingInfo)
        {
            if (openingInfo.WallReference!=null)
            {
                WallReferenceId = openingInfo.WallReference.Id;
            }

            if (openingInfo.DoorWindowInfo!=null)
            {
                DoorWindowInfoId = openingInfo.DoorWindowInfo.Id;
            }

            NoDoor = openingInfo.NoDoor;
            IsGarageDoor = openingInfo.IsGarageDoor;
            DoorType = openingInfo.DoorType;
            DoorNumberType = openingInfo.DoorNumberType;
            DoorTypeLocation = openingInfo.DoorTypeLocation;
            Id = openingInfo.Id;
            if (openingInfo.OpeningType != null) OpeningType = (OpeningType) openingInfo.OpeningType;
            Location = openingInfo.Location;
            Height = openingInfo.Height;
            Width = openingInfo.Width;
            DoorHeaderHeight = openingInfo.DoorHeaderHeight;
            SupportSpan = openingInfo.SupportSpan;
            Lintel = new LintelBeamPoco(openingInfo.Lintel);
        }

    }
}
