using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;

namespace AppModels.ViewModelEntity
{
    public class TextVm: EntityVm, ITextVm
    {
        public TextVm(Entity entity) : base(entity)
        {
        }

        public string TextString
        {
            get
            {
                if (Entity is Text textEntity)
                {
                    return textEntity.TextString;
                }

                return string.Empty;
            }
            set
            {
                if (Entity is Text textEntity)
                {
                    textEntity.TextString = value;
                    RaisePropertyChanged(nameof(TextString));
                }
            }
        }

        public override void NotifyPropertiesChanged()
        {
            base.NotifyPropertiesChanged();
            RaisePropertyChanged(nameof(TextString));
        }
    }
}
