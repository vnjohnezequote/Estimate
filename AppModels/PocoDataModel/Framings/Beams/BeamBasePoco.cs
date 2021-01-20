using System;
using System.Collections.Generic;
using AppModels.Enums;
using AppModels.PocoDataModel.Framings.FloorAndRafter;
using AppModels.ResponsiveData.Framings.Beams;

namespace AppModels.PocoDataModel.Framings.Beams
{
    public class BeamBasePoco: FramingBasePoco
    {
        public string Location { get; set; }
        public List<SupportPointPoco> LoadPointSupports { get; }
            = new List<SupportPointPoco>();
        public SupportType PointSupportType { get; set; }
        public Suppliers? Suplier { get; set; }
        public Guid SupportReferenceId { get; set; }
        public int SupportHeight { get; set; }
        public int WallReferenceId { get; set; }
        public int NoItem { get; set; }
        public int Thickness { get; set; }
        public int Depth { get; set; }
        public bool IsQuoteToExtraExcel { get; set; }

        public BeamBasePoco()
        {

        }

        public BeamBasePoco(BeamBase beamInfo): base(beamInfo)
        {
            Location = beamInfo.Location;
            InitializerSupports(beamInfo);
            if (beamInfo.PointSupportType != null) PointSupportType = (SupportType)beamInfo.PointSupportType;
            Suplier = beamInfo.Supplier;
            SupportHeight = beamInfo.SupportHeight;
            if (beamInfo.WallReference != null)
            {
                WallReferenceId = beamInfo.WallReference.Id;
            }
            NoItem = beamInfo.NoItem;
            Thickness = beamInfo.NoItem;
            Depth = beamInfo.Depth;
            IsQuoteToExtraExcel = beamInfo.IsQuoteToExtraExcel;
        }

        private void InitializerSupports(BeamBase beamInfo)
        {
            foreach (var beamInfoLoadPointSupport in beamInfo.LoadPointSupports)
            {
                var support = new SupportPointPoco(beamInfoLoadPointSupport);
                LoadPointSupports.Add(support);
            }
        }

    }
}
