using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Openings;
using AppModels.ResponsiveData.Support;

namespace AppModels.PocoDataModel.Openings
{
    public class LintelBeamPoco
    {
        public Suppliers Supplier { get; set; }
        public SupportType PointSupportType { get; set; }
        public Guid SupportReferenceId { get; set; }
        public Guid EngineerMemberInfoId { get ; set; }
        public List<SupportPointPoco> LoadPointSupports { get; }
            = new List<SupportPointPoco>();
        public int SupportHeight { get; set; }
        public FramingTypes FramingType { get; set; }
        public int Id { get; set ; }
        public string Name { get; set ; }
        public string StandardDoorJambSupport { get; set ; }
        public int NoItem { get; set; }
        public int Thickness { get; set; }
        public int Depth { get; set; }
        public MaterialTypes? MaterialType { get; set; }
        public string TimberGrade { get; set; }
        public double ExtraLength { get; set; }

        public LintelBeamPoco()
        {

        }
        public LintelBeamPoco(LintelBeam lintelInfo)
        {
            if (lintelInfo.Supplier != null) Supplier = (Suppliers) lintelInfo.Supplier;
            if (lintelInfo.PointSupportType != null) PointSupportType = (SupportType) lintelInfo.PointSupportType;
            if (lintelInfo.SupportReference!=null)
            {
                SupportReferenceId = lintelInfo.SupportReference.Id;
            }

            if (lintelInfo.EngineerMember!=null)
            {
                EngineerMemberInfoId = lintelInfo.EngineerMember.Id;
            }
            IninitializerLoadPointSupport(lintelInfo.LoadPointSupports.ToList());
            SupportHeight = lintelInfo.SupportHeight;
            FramingType = lintelInfo.FramingType;
            Id = lintelInfo.Index;
            Name = lintelInfo.Name;
            StandardDoorJambSupport = lintelInfo.StandardDoorJambSupport;
            NoItem = lintelInfo.NoItem;
            Thickness = lintelInfo.Thickness;
            Depth = lintelInfo.Depth;
            TimberGrade = lintelInfo.TimberGrade;
            ExtraLength = lintelInfo.ExtraLength;

        }

        private void IninitializerLoadPointSupport(List<SupportPoint> supports)
        {
            foreach (var supportPoint in supports)
            {
                var supportPoco = new SupportPointPoco(supportPoint);
                LoadPointSupports.Add(supportPoco);
            }
        }
    }
}
