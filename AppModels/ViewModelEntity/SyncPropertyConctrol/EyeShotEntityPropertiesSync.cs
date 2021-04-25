using AppModels.Interaface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppModels.ViewModelEntity.SyncPropertyConctrol
{
    public class EyeShotEntityPropertiesSync : SyncPropertyEntityControl
    {
        public EyeShotEntityPropertiesSync(IEntitiesManager entitiesManager, IEntityVm selectedEntity)
            : base(entitiesManager, selectedEntity)
        {

        }
        protected override void SyncPropertiesChanged(string propertiesName)
        {
            if (propertiesName == "LayerName")
            {
                foreach (var entity in EntitiesManager.SelectedEntities)
                {
                    entity.LayerName = SelectedEntity.LayerName;
                }
            }
            if (propertiesName == "ColorMethod")
            {
                foreach (var entity in EntitiesManager.SelectedEntities)
                {
                    entity.ColorMethod = SelectedEntity.ColorMethod;
                }
            }
            if (propertiesName == "Color")
            {
                foreach (var entity in EntitiesManager.SelectedEntities)
                {
                    entity.Color = (Color)SelectedEntity.Color;
                }
            }
        }
    }
}
