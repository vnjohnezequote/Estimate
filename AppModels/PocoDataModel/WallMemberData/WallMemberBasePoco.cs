using AppModels.Enums;
using AppModels.Interaface;

namespace AppModels.PocoDataModel.WallMemberData
{
    public class WallMemberBasePoco
    {
        public WallMemberType MemberType { get; set; }
        public int NoItem { get; set; }
        public int Thickness { get; set; }
        public int Depth { get; set; }
        public string TimberGrade { get; set; }

        public WallMemberBasePoco()
        {

        }
        public WallMemberBasePoco(IWallMemberInfo memberInfo)
        {
            NoItem = memberInfo.NoItem;
            Thickness = memberInfo.Thickness;
            Depth = memberInfo.Depth;
            TimberGrade = memberInfo.TimberGrade;
        }
    }
}
