using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Framings;

namespace AppModels.PocoDataModel.Framings.FloorAndRafter
{
    public class FramingBasePoco
    {
        public Guid Id { get; set; }
        public Guid LevelId { get; set; }
        public Guid FramingSheetId { get; set; }
        public int Index { get; set; }
        public int SubfixIndex { get; set; }
        public FramingTypes FramingType { get; set; }
        public bool IsExisting { get; set; }
        public string Name { get; set; }
        public int FramingSpan { get; set; }
        public int FullLength { get; set; }
        public double ExtraLength { get;set; }
        public double Pitch { get; set; }
        public int Quantity { get; set; }
        public Guid EngineerMemberId { get; set; }
        public string TimberGrade { get; set; }
        public int FramingInfoId { get; set; }
        
        public FramingBasePoco()
        {

        }
        public FramingBasePoco(FramingBase framing)
        {
            Id = framing.Id;
            LevelId = framing.LevelId;
            FramingSheetId = framing.FramingSheetId;
            Index = framing.Index;
            SubfixIndex = framing.SubFixIndex;
            FramingType = framing.FramingType;
            IsExisting = framing.IsExisting;
            Name = framing.Name;
            FramingSpan = framing.FramingSpan;
            FullLength = framing.FullLength;
            ExtraLength = framing.ExtraLength;
            Pitch = framing.Pitch;
            Quantity = framing.Quantity;
            if (framing.EngineerMember!=null)
            {
                EngineerMemberId = framing.EngineerMember.Id;
            }
            TimberGrade = framing.TimberGrade;
            if (framing.FramingInfo!=null)
            {
                FramingInfoId = framing.FramingInfo.Id;
            }
        }
    }
}
