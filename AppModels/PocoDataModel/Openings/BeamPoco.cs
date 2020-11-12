using System.Collections.Generic;
using AppModels.Enums;
using AppModels.ResponsiveData.Openings;

namespace AppModels.PocoDataModel.Openings
{
    public class BeamPoco
    {
        public string Location { get; set; }
        //public IWallMemberInfo GLobalSupportInfo { get; set; }
        public List<SupportPointPoco> LoadPointSupports { get; }
            = new List<SupportPointPoco>();
        public SupportType PointSupportType { get; set; }
        public BeamType Type { get; set; }
        public Suppliers Suplier { get; set; }
        public int Id { get ; set; }
        public string Name { get; set; }
        public int SpanLength { get; set; }
        public double ExtraLength { get; set; }
        public int Quantity { get; set; }
        public int EngineerMemberId { get; set; }
        public int SupportReferenceId { get; set; }
        public int SupportHeight { get; set; }
        public int WallReferenceID { get ; set; }
        public int NoItem { get; set; }
        public int Thickness { get; set; }
        public int Depth  { get; set; }
        public string TimberGrade { get; set; }
        public MaterialTypes? MaterialType { get; set; }
        public int TimberInfoId { get; set; }
        public double BeamPitch { get; set; }
        public bool IsBeamExportToExcel { get; set; }

        public BeamPoco()
        {

        }

        public BeamPoco(Beam beamInfo)
        {
            Location = beamInfo.Location;
            InitializerSupports(beamInfo);
            BeamPitch = beamInfo.BeamPitch;
            PointSupportType = (SupportType)beamInfo.PointSupportType;
            Type = beamInfo.Type;
            Suplier = (Suppliers)beamInfo.Supplier;
            Id = beamInfo.Id;
            Name = beamInfo.Name;
            SpanLength = beamInfo.SpanLength;
            ExtraLength = beamInfo.ExtraLength;
            Quantity = beamInfo.Quantity;
            if (beamInfo.EngineerMemberInfo!=null)
            {
                EngineerMemberId = beamInfo.EngineerMemberInfo.Id;
            }

            if (beamInfo.SupportReference!=null)
            {
                SupportReferenceId = beamInfo.SupportReference.Id;
            }

            SupportHeight = beamInfo.SupportHeight;
            if (beamInfo.WallReference != null)
            {
                WallReferenceID = beamInfo.WallReference.Id;
            }

            if (beamInfo.TimberInfo!=null)
            {
                TimberInfoId = beamInfo.TimberInfo.Id;
            }
            NoItem = beamInfo.NoItem;
            Thickness = beamInfo.NoItem;
            Depth = beamInfo.Depth;
            TimberGrade = beamInfo.TimberGrade;
            MaterialType = beamInfo.MaterialType;
            IsBeamExportToExcel = beamInfo.IsBeamExportToExcel;
            //GLobalSupportInfo = beamInfo.GLobalSupportInfo;

        }

        private void InitializerSupports(Beam beamInfo)
        {
            foreach (var beamInfoLoadPointSupport in beamInfo.LoadPointSupports)
            {
                var support = new SupportPointPoco(beamInfoLoadPointSupport);
                LoadPointSupports.Add(support);
            }
        }
    }
}
