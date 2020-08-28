using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ApplicationCore.BaseModule;
using ApplicationInterfaceCore;
using ApplicationService;
using AppModels;
using AppModels.CustomEntity;
using AppModels.DynamicObject;
using AppModels.Factories;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ViewModelEntity;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using GeometryGym.Ifc;
using Prism.Events;
using Prism.Regions;
using Unity;

namespace DrawingModule.ViewModels
{
    public class SelectedEntityPropertiesViewModel : BaseViewModel
    {
        #region Private Field
        private IEntitiesManager _entitiesManger;
        private IEntityVm _selectedEntity;
        public IEntitiesManager EntitiesManager
        {
            get => _entitiesManger;
        }

        public Visibility LayerVisibility => _selectedEntity==null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility ColorVisibility => _selectedEntity == null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility ColorMethodVisibility => _selectedEntity == null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility LevelVisibility
        {
            get
            {
                switch (_selectedEntity)
                {
                    case null:
                        return Visibility.Collapsed;
                    case Wall2DVm _:
                        return Visibility.Visible;
                    case LinearPathWall2DVm _:
                        return Visibility.Visible;
                    default:
                        return Visibility.Collapsed;
                }
            }
        } 

        public IEntityVm SelectedEntity
        {
            get => _selectedEntity;
            set
            {
                SetProperty(ref _selectedEntity, value);
                RaisePropertyChanged(nameof(LayerVisibility));
                RaisePropertyChanged(nameof(ColorVisibility));
                RaisePropertyChanged(nameof(LevelVisibility));
                RaisePropertyChanged(nameof(ColorMethodVisibility));
            }
        }
        public ObservableCollection<LevelWall> Levels { get; private set; }

        #endregion
        public SelectedEntityPropertiesViewModel(): base()
        {

        }
        public SelectedEntityPropertiesViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager, IEntitiesManager entitiesManager,IJob jobModel)
            : base(unityContainer, regionManager, eventAggregator, layerManager,jobModel)
        {
            //HidePropertyItems.Add("Entity");
            _entitiesManger = entitiesManager;
            this.RaisePropertyChanged(nameof(EntitiesManager));
            EntitiesManager.PropertyChanged += EntitiesManager_PropertyChanged;
            if (jobModel!=null)
            {
                this.Levels = jobModel.Levels;
            }

            this.EventAggregator.GetEvent<EntityService>().Subscribe(OnPaperSpaceSelectedChanged);

        }

        private void OnPaperSpaceSelectedChanged(Entity selEntity)
        {
            var entityFactory = new EntitiyVmFactory();
            SelectedEntity = entityFactory.creatEntityVm(selEntity);
        }

        private void EntitiesManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "SelectedEntity") return;
            if (EntitiesManager.SelectedEntity != null)
            {
                var objectEntity = EntitiesManager.SelectedEntity;
                this.SelectedEntity = objectEntity;
                this.SelectedEntity.PropertyChanged += SelectedEntity_PropertyChanged;
            }
            else
            {
                this.SelectedEntity = null;
            }
        }

        private void SelectedEntity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "WallLevelName")
            {
                EntitiesManager?.SyncLevelSelectedsEntityPropertyChanged(((IWall2D)this.SelectedEntity).WallLevelName);
            }
        }
    }
}
