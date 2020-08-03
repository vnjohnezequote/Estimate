using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;

namespace AppModels.ResponsiveData.EngineerMember
{
    public class EngineerMemberInfo
    {
        public string LevelType { get; set; }

        public Suppliers Suplier { get; set; }
        public string EngineerName { get; set; }
        public WallMemberType MemberType { get; set; }
        public MaterialTypes MaterialType { get; set; }
        public int MinSize { get; set; }
        public int MaxSize { get; set; }
        public int Qty { get; set; }
        public int Depth { get; set; }
        public int Thickness { get; set; }
        public string TimberGrade { get; set; }
        public string Size { get; set; }
        public string SizeGrade { get; set; }
        public string RealGrade { get; set; }
        public string RealSizeGrade { get; set; }
    }
}
