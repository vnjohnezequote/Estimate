using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Openings;
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
        private List<TimberBase> _timberList;
        public ObservableCollection<EngineerMemberInfo> EngineerList { get; }
        public IEntitiesManager EntitiesManager
        {
            get => _entitiesManger;
        }

        public Visibility FloorNameVisibility
        {
            get
            {
                if (SelectedEntity is BlockReferenceVm blockRef && blockRef.IsBorderPage)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }
        public Visibility LayerVisibility => _selectedEntity==null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility ColorVisibility => _selectedEntity == null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility ColorMethodVisibility => _selectedEntity == null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility LeaderVisibility => SelectedEntity is LeaderVM ? Visibility.Visible : Visibility.Collapsed;
        public List<string> ScaleList { get; } = new List<string>(){"1:50","1:75","1:100","1:125","1:150","1:175","1:200","1:225","1:250","1:275","1:300","1:325","1:350"};
        public Visibility BeamVisibility =>
            SelectedEntity is BeamEntityVm ? Visibility.Visible : Visibility.Collapsed;
        public Visibility LevelVisibility
        {
            get
            {
                switch (_selectedEntity)
                {
                    case null:
                        return Visibility.Collapsed;
                    case WallLine2DVm _:
                    case BeamEntityVm _:
                        return Visibility.Visible;
                    case LinearPathWall2DVm _:
                        return Visibility.Visible;
                    default:
                        return Visibility.Collapsed;
                }
            }
        }

        public Visibility VectorViewVisibility =>
            SelectedEntity is VectorViewVm ? Visibility.Visible : Visibility.Collapsed;
        public Visibility TextContentVisibility
        {
            get
            {
                switch (_selectedEntity)
                {
                    case ITextVm _:
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
                RaisePropertyChanged(nameof(TextContentVisibility));
                RaisePropertyChanged(nameof(LeaderVisibility));
                RaisePropertyChanged(nameof(BeamVisibility));
                RaisePropertyChanged(nameof(VectorViewVisibility));
                RaisePropertyChanged(nameof(FloorNameVisibility));
                RaisePropertyChanged(nameof(BeamGradeList));
            }
        }
        public ObservableCollection<LevelWall> Levels { get; private set; }

        #endregion
        public List<string> BeamGradeList
        {
            get
            {
                if (this.JobModel.Info.Client != null && this.JobModel.Info.Client.Beams != null)
                    return new List<string>(this.JobModel.Info.Client.Beams.Keys);
                else return null;
            }
        }
        public List<TimberBase> TimberList { get=>_timberList; set=>SetProperty(ref _timberList,value); } 
        public SelectedEntityPropertiesViewModel(): base()
        {

        }
        public SelectedEntityPropertiesViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager, IEntitiesManager entitiesManager,IJob jobModel)
            : base(unityContainer, regionManager, eventAggregator, layerManager,jobModel)
        {
            //HidePropertyItems.Add("Entity");
            EngineerList = new ObservableCollection<EngineerMemberInfo>();
            _entitiesManger = entitiesManager;
            this.RaisePropertyChanged(nameof(EntitiesManager));
            EntitiesManager.PropertyChanged += EntitiesManager_PropertyChanged;
            if (jobModel!=null)
            {
                this.Levels = jobModel.Levels;
            }

            this.EventAggregator.GetEvent<EntityService>().Subscribe(OnPaperSpaceSelectedChanged);

        }

        private void OnPaperSpaceSelectedChanged(IEntityVm selEntity)
        {
            SelectedEntity = selEntity;
        }

        private void EntitiesManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "SelectedEntity") return;
            if (EntitiesManager.SelectedEntity != null)
            {
                var objectEntity = EntitiesManager.SelectedEntity;
                this.SelectedEntity = objectEntity;
                this.SelectedEntity.PropertyChanged += SelectedEntity_PropertyChanged;
                GeneralTimberList();
                GeneralEngineerList();
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
                if (EntitiesManager!=null)
                {
                    if (EntitiesManager.SelectedEntity is IWall2D)
                    {
                        EntitiesManager.SyncLevelSelectedsEntityPropertyChanged(((IWall2D)this.SelectedEntity).WallLevelName);
                    }

                    if (SelectedEntity is BeamEntityVm beam)
                    {
                        if (JobModel!=null)
                        {
                            LevelWall level = null;
                            LevelWall newLevel = null;
                            foreach (var levelWall in Levels)
                            {
                                if (levelWall.LevelName == beam.OldLevelName)
                                {
                                    level = levelWall;
                                }

                                if (levelWall.LevelName == beam.WallLevelName)
                                {
                                    newLevel = levelWall;
                                }
                            }
                            if (level != null)
                            {
                                if (level.RoofBeams.Contains(beam.BeamReference))
                                {
                                    level.RoofBeams.Remove(beam.BeamReference);
                                }

                                var i = 1;
                                foreach (var levelRoofBeam in level.RoofBeams)
                                {
                                    levelRoofBeam.Id = i;
                                    i++;
                                }
                            }

                            if (newLevel!=null)
                            {
                                beam.BeamReference.InitGlobalInfor(newLevel.LevelInfo);
                                beam.BeamReference.Id = newLevel.RoofBeams.Count + 1;
                                beam.BeamName = beam.BeamReference.Name;
                                //beam.BlockName = beam.BeamReference.Name+beam.WallLevelName;
                                beam.EngineerMember = null;
                                newLevel.RoofBeams.Add(beam.BeamReference);
                            }
                        }
                    }
                }
            }

            if (e.PropertyName=="BeamGrade")
            {
             GeneralTimberList();   
            }
            EntitiesManager?.Refresh();
        }

        private void GeneralTimberList()
        {
            if (SelectedEntity is BeamEntityVm beam)
            {
                if (JobModel.Info.Client != null && JobModel.Info.Client.Beams != null)
                {
                    if(string.IsNullOrEmpty(beam.BeamGrade))
                        return;
                    if (JobModel.Info.Client.Beams.ContainsKey(beam.BeamGrade))
                    {
                        JobModel.Info.Client.Beams.TryGetValue(beam.BeamGrade, out var timberList);
                        TimberList = timberList;
                    }
                }
            }
        }

        private void GeneralEngineerList()
        {
            if (SelectedEntity is BeamEntityVm beam)
            {
                EngineerList.Clear();
                if (JobModel != null && JobModel.EngineerMemberList != null)
                {
                    foreach (var engineerMemberInfo in JobModel.EngineerMemberList)
                    {
                        if (engineerMemberInfo.LevelType == beam.WallLevelName || engineerMemberInfo.LevelType =="Global")
                        {
                            EngineerList.Add(engineerMemberInfo);
                        }
                    }
                }
            }
            
        }
    }
}
