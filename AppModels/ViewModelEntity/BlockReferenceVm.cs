using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;

namespace AppModels.ViewModelEntity
{
    public class BlockReferenceVm: EntityVm
    {
        public bool IsBorderPage
        {
            get
            {
                if (!(this.Entity is BlockReference blockRef)) return false;
                return blockRef.Attributes.ContainsKey("Title");
            }
        }

        public string FloorName
        {
            get
            {
                if (!(this.Entity is BlockReference bockRef)) return string.Empty;
                return bockRef.Attributes.ContainsKey("Title") ? bockRef.Attributes["Title"].Value : string.Empty;
            }
            set
            {
                if (!(this.Entity is BlockReference bockRef)) return;
                if (!bockRef.Attributes.ContainsKey("Title")) return;
                bockRef.Attributes["Title"].Value = value;
                RaisePropertyChanged(nameof(FloorName));
            }
            
        }
        public BlockReferenceVm(Entity entity) : base(entity)
        {
        }
        public override void NotifyPropertiesChanged()
        {
            base.NotifyPropertiesChanged();
            RaisePropertyChanged(nameof(IsBorderPage));
            RaisePropertyChanged(nameof(FloorName));
        }
    }
}
