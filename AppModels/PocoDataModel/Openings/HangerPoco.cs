using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.ResponsiveData.Openings;

namespace AppModels.PocoDataModel.Openings
{
    public class HangerPoco
    {
        public Guid Id { get; set; }
        public Guid LevelId { get; set; }
        public Guid FramingSheetId { get; set; }
        public int Quantity { get; set; }
        public bool IsExisting { get; set; }
        public int Index { get; set; }
        public int SubFixIndex { get; set; }

        public int HangerMatId { get; set; } 
        public string Name { get; set; }

        public HangerPoco()
        {

        }
        public HangerPoco(Hanger hanger)
        {
            Id = hanger.Id;
            LevelId = hanger.LevelId;
            FramingSheetId = hanger.FramingSheetId;
            Quantity = hanger.Quantity;
            IsExisting = hanger.IsExisting;
            Index = hanger.Index;
            SubFixIndex = hanger.SubFixIndex;
            if (hanger.HangerMaterial!=null)
            {
                HangerMatId = hanger.HangerMaterial.ID;
            }

            Name = hanger.Name;
        }
    }
}
