using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.BaseModule;
using ApplicationInterfaceCore;
using AppModels.ViewModelEntity;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using GeometryGym.Ifc;
using Prism.Events;
using Prism.Regions;
using Syncfusion.UI.Xaml.Grid;
using Unity;

namespace DrawingModule.ViewModels
{
    public class SelectedEntityPropertiesViewModel : BaseViewModel
    {
        #region Private Field

        private IEntitiesManager _entitiesManger;
        private EntityVm _selectedEntity;

        public IEntitiesManager EntitiesManager
        {
            get => _entitiesManger;
        }

        public EntityVm SelectedEntity
        {
            get => _selectedEntity;
            set=> SetProperty( ref _selectedEntity , value);
        }
        #endregion
        public SelectedEntityPropertiesViewModel()
        {

        }
        public SelectedEntityPropertiesViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager, IEntitiesManager entitiesManager)
            : base(unityContainer, regionManager, eventAggregator, layerManager)
        {
            _entitiesManger = entitiesManager;
            this.RaisePropertyChanged(nameof(EntitiesManager));
            EntitiesManager.PropertyChanged += EntitiesManager_PropertyChanged;
        }

        private void EntitiesManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName=="SelectedEntity")
            {
                if (EntitiesManager.SelectedEntity != null)
                {
                    //this.SelectedEntity = EntitiesManager.SelectedEntity;
                    var objectEntity = EntitiesManager.SelectedEntity;
                    if (objectEntity is Wall2DVm wall2DVm)
                    {
                        this.SelectedEntity = wall2DVm;
                        return;
                    }
                    this.SelectedEntity = objectEntity;
                }
                else
                {
                    this.SelectedEntity = null;
                }

            }
        }
    }
}
