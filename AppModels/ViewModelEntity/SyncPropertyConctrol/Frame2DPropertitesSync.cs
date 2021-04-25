using AppModels.Interaface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppModels.ViewModelEntity.SyncPropertyConctrol
{
    public class Frame2DPropertitesSync : EyeShotEntityPropertiesSync
    {
        private bool disposedValue;

        public Frame2DPropertitesSync(IEntitiesManager entitiesManager, IEntityVm selectedEntity) 
            : base(entitiesManager, selectedEntity)
        {
        }

        protected override void SyncPropertiesChanged(string propertiesName)
        {
            base.SyncPropertiesChanged(propertiesName);


        }

       
    }
}
