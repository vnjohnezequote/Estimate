using AppModels.Interaface;
using AppModels.PocoDataModel.WallMemberData;

namespace AppModels.PocoDataModel
{
    public class GlobalWallDetailPoco
    {
        public WallMemberBasePoco RibbonPlate { get; set; }
        public WallMemberBasePoco TopPlate { get; set; }
        public WallMemberBasePoco Stud { get; set; }
        public WallMemberBasePoco BottomPlate { get; set; }
        public WallMemberBasePoco Nogging { get; set; }
        public WallMemberBasePoco Trimmer { get; set; }
        public GlobalWallDetailPoco(){}
        public GlobalWallDetailPoco(IGlobalWallDetail globaWallDetail)
        {
            if (globaWallDetail.WallType.IsLoadBearingWall)
            {
                RibbonPlate = new WallMemberBasePoco(globaWallDetail.RibbonPlate);
            }
            TopPlate = new WallMemberBasePoco(globaWallDetail.TopPlate);
            Stud = new WallMemberBasePoco(globaWallDetail.Stud);
            BottomPlate = new WallMemberBasePoco(globaWallDetail.BottomPlate);
            Nogging = new WallMemberBasePoco(globaWallDetail.Nogging);
            Trimmer = new WallMemberBasePoco(globaWallDetail.Trimmer);
        }
    }
}
