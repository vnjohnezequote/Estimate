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
using AppModels.NewReposiveData;
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
        private ObservableCollection<WallBase> _wallList;
        private List<int> _doorWidthList;
        public ObservableCollection<EngineerMemberInfo> EngineerList { get; }
        public List<int> WallThicknessList { get; set; } = new List<int>() {70, 90, 140, 200,230};
        public List<int> DoorWidthList { get=>_doorWidthList; set=>SetProperty(ref _doorWidthList,value); }  
        public List<int> ExtDoorListHeights { get; set; } = new List<int> { 600, 900, 1200, 1500, 1800, 2100, 2400, 2700, 3000 };
        public List<int> DoorSupportSpanList { get; set; } = new List<int>(){  3000, 6000, 9000, 12000, 15000 };

        public Visibility WallVisibility => SelectedEntity is Wall2DVm ? Visibility.Visible : Visibility.Collapsed;

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
        public Visibility DoorVisibility =>
            SelectedEntity is DoorCountEntityVm ? Visibility.Visible : Visibility.Collapsed;
        public List<string> ScaleList { get; } = new List<string>(){"1:50","1:75","1:100","1:125","1:150","1:175","1:200","1:225","1:250","1:275","1:300","1:325","1:350"};
        public Visibility BeamVisibility
        {
            get
            {
                if (SelectedEntity is BeamEntityVm || SelectedEntity is Beam2DVm || SelectedEntity is Joist2dVm)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }
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
                RaisePropertyChanged(nameof(DoorVisibility));
                RaisePropertyChanged(nameof(WallVisibility));
            }
        }
        public ObservableCollection<LevelWall> Levels { get; private set; }

        #endregion
        public List<string> BeamGradeList
        {
            get
            {
                if (SelectedEntity is BeamEntityVm)
                {
                    if (this.JobModel.Info.Client != null && this.JobModel.Info.Client.Beams != null)
                        return new List<string>(this.JobModel.Info.Client.Beams.Keys);
                    else return null;
                }
                else if(SelectedEntity is Beam2DVm || SelectedEntity is Joist2dVm)
                {
                    if (this.JobModel.Info.Client!=null && this.JobModel.Info.Client.TimberMaterialList!=null)
                    {
                        var materialGradeList = new List<string>();
                        foreach ( var timberBase in JobModel.Info.Client.TimberMaterialList)
                        {
                            if (!materialGradeList.Contains(timberBase.TimberGrade))
                            {
                                materialGradeList.Add(timberBase.TimberGrade);
                            }
                        }

                        return materialGradeList;
                    }
                }

                return null;

            }
        }
        public ObservableCollection<WallBase> WallList
        {
            get=>_wallList;
            set=>SetProperty(ref _wallList,value);
        }
        public List<TimberBase> TimberList { get=>_timberList; set=>SetProperty(ref _timberList,value); } 
        public SelectedEntityPropertiesViewModel(): base()
        {

        }
        public SelectedEntityPropertiesViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager, IEntitiesManager entitiesManager,IJob jobModel)
            : base(unityContainer, regionManager, eventAggregator, layerManager,jobModel)
        {
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
                GeneralWallList();
                GeneralDoorWidthList();
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

            if (e.PropertyName == "WallBelongTo")
            {
                GeneralDoorWidthList();
            }
            EntitiesManager?.Refresh();
        }

        private void GeneralTimberList()
        {
            //TimberList.Clear();
            if (SelectedEntity is BeamEntityVm beam)
            {
                if (JobModel.Info.Client != null && JobModel.Info.Client.Beams != null)
                {
                    if(string.IsNullOrEmpty(beam.BeamGrade))
                        return;
                    if (JobModel.Info.Client.Beams.ContainsKey(beam.BeamGrade))
                    {
                        JobModel.Info.Client.Beams.TryGetValue(beam.BeamGrade, out var timberList);
                        TimberList=timberList;
                    }
                }
            }
            else if(SelectedEntity is Beam2DVm beam2D)
            {
                if (JobModel.Info.Client!=null && JobModel.Info.Client.TimberMaterialList!=null)
                {
                    if (string.IsNullOrEmpty((beam2D.BeamGrade)))
                    {
                        return;
                    }

                    var tempList = new List<TimberBase>();
                    foreach (var timber in JobModel.Info.Client.TimberMaterialList)
                    {
                        if (timber.TimberGrade == beam2D.BeamGrade)
                        {
                            tempList.Add(timber);
                        }
                    }

                    TimberList = tempList;
                }
            }
            else if(SelectedEntity is Joist2dVm joist)
            {
                if (JobModel.Info.Client!=null && JobModel.Info.Client.TimberMaterialList != null)
                {
                    if (string.IsNullOrEmpty((joist.BeamGrade)))
                    {
                        return;
                    }

                    var tempList = new List<TimberBase>();
                    foreach (var timber in JobModel.Info.Client.TimberMaterialList)
                    {
                        if (timber.TimberGrade == joist.BeamGrade)
                        {
                            tempList.Add(timber);
                        }
                    }

                    TimberList = tempList;
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

        private void GeneralWallList()
        {
            if (SelectedEntity is DoorCountEntityVm door)
            {
                //WallList.Clear();
                if (JobModel!=null && JobModel.Levels!=null)
                {
                    foreach (var jobModelLevel in JobModel.Levels)
                    {
                        if (jobModelLevel.LevelName == door.WallLevelName)
                        {
                            WallList = jobModelLevel.WallLayers;
                        }
                    }
                }
            }
        }

        private void GeneralDoorWidthList()
        {
            if (SelectedEntity is DoorCountEntityVm door)
            {
                if (door.WallBelongTo!=null)
                {
                    if (door.WallBelongTo.WallType!=null )
                    {
                        if (door.WallBelongTo.WallType.IsLoadBearingWall)
                        {
                            this.DoorWidthList = new List<int>() { 660, 960, 1260, 1560, 1860, 2160, 2460, 2760, 3060, 3360, 3660 };
                        }
                        else
                        {
                            this.DoorWidthList = new List<int>() { 875, 930, 1201, 1501, 1801, 2101, 2401, 2701, 3001, 3301, 3601, 4201 };
                        }
                    }
                }
            }
        }
    }
}
