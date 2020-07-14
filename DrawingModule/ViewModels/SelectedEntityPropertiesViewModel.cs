using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ApplicationCore.BaseModule;
using ApplicationInterfaceCore;
using AppModels.CustomEntity;
using AppModels.DynamicObject;
using AppModels.Interaface;
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
        //private ObservableCollection<string> hidePropertyItems = new ObservableCollection<string>();
        private IEntitiesManager _entitiesManger;
        private IEntityVm _selectedEntity;
        //public ObservableCollection<string> HidePropertyItems
        //{
        //    get { return hidePropertyItems; }
        //    set { hidePropertyItems = value; }
        //}
        public IEntitiesManager EntitiesManager
        {
            get => _entitiesManger;
        }

        public Visibility LayerVisibility => _selectedEntity==null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility ColorVisibility => _selectedEntity == null ? Visibility.Collapsed : Visibility.Visible;
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
            }
        }

        #endregion
        public SelectedEntityPropertiesViewModel()
        {

        }
        public SelectedEntityPropertiesViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager, IEntitiesManager entitiesManager)
            : base(unityContainer, regionManager, eventAggregator, layerManager)
        {
            //HidePropertyItems.Add("Entity");
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
                    var objectEntity = EntitiesManager.SelectedEntity;
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
