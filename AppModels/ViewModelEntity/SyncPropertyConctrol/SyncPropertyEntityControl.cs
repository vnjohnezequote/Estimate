using AppModels.Interaface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppModels.ViewModelEntity.SyncPropertyConctrol
{
    public abstract class SyncPropertyEntityControl
    {
        public IEntitiesManager EntitiesManager { get; }
        public IEntityVm SelectedEntity { get; }
        public SyncPropertyEntityControl(IEntitiesManager entitiesManager, IEntityVm selectedEntity)
        {
            EntitiesManager = entitiesManager;
            SelectedEntity = selectedEntity;
            if(SelectedEntity!=null)
            {
                SelectedEntity.PropertyChanged += _selectedEntity_PropertyChanged;
            }
        }

        private void _selectedEntity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SyncPropertiesChanged(e.PropertyName);
        }
        protected abstract void SyncPropertiesChanged(string propertiesName);
    }
}
