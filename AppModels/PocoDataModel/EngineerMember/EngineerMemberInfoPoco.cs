using AppModels.Enums;
using AppModels.ResponsiveData.EngineerMember;

namespace AppModels.PocoDataModel.EngineerMember
{
    public class EngineerMemberInfoPoco
    {
        public int Id { get; set; }
        public Suppliers Supplier { get; set; }
        public string LevelType { get; set; }
        public string EngineerName { get; set; }
        public WallMemberType MemberType { get; set; }
        public MaterialTypes MaterialType { get;set; }
        public int MinSpan { get; set; }
        public int MaxSpan { get; set; }
        public int NoItem { get; set; }
        public int Depth { get; set; }
        public int Thickness { get; set; }
        public string TimberGrade { get; set; }
        public int TimberInfoId { get; set; }
        public EngineerMemberInfoPoco()
        {

        }
        public EngineerMemberInfoPoco(EngineerMemberInfo memberInfo)
        {
            Id = memberInfo.Id;
            Supplier = (Suppliers)memberInfo.Supplier;
            LevelType = memberInfo.LevelType;
            EngineerName = memberInfo.EngineerName;
            MemberType = memberInfo.MemberType;
            MaterialType = memberInfo.MaterialType;
            MinSpan = memberInfo.MinSpan;
            MaxSpan = memberInfo.MaxSpan;
            NoItem = memberInfo.NoItem;
            Depth = memberInfo.Depth;
            Thickness = memberInfo.Thickness;
            TimberGrade = memberInfo.TimberGrade;
            if (memberInfo.TimberInfo != null)
            {
                TimberInfoId = memberInfo.TimberInfo.Id;
            }
        }

    }
}
