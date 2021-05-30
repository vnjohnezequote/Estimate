using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
using ApplicationCore.BaseModule;
using ApplicationInterfaceCore;
using ApplicationService;
using AppModels.CustomEntity;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Openings;
using AppModels.ViewModelEntity;
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
        private List<TimberBase> _outTriggerATimberList;
        private List<TimberBase> _outTriggerBTimberList;

        private List<HangerMat> _hangerList;
        private ObservableCollection<WallBase> _wallList;
        private List<int> _doorWidthList;
        public ObservableCollection<EngineerMemberInfo> EngineerList { get; }
        public List<int> WallThicknessList { get; set; } = new List<int>() { 70, 90, 140, 200, 230 };
        public List<int> DoorWidthList { get => _doorWidthList; set => SetProperty(ref _doorWidthList, value); }
        public List<int> ExtDoorListHeights { get; set; } = new List<int> { 600, 900, 1200, 1500, 1800, 2100, 2400, 2700, 3000 };
        public List<int> DoorSupportSpanList { get; set; } = new List<int>() { 3000, 6000, 9000, 12000, 15000 };
        public List<TimberBase> OutTriggerATimberList { get => _outTriggerATimberList; set => SetProperty(ref _outTriggerATimberList, value); }
        public List<TimberBase> OutTriggerBTimberList { get => _outTriggerBTimberList; set => SetProperty(ref _outTriggerBTimberList, value); }
        public List<string> ScaleList { get; } = new List<string>() { "1:50", "1:75", "1:100", "1:125", "1:150", "1:175", "1:200", "1:225", "1:250", "1:275", "1:300", "1:325", "1:350" };
        public Visibility WallVisibility => SelectedEntity is Wall2DVm ? Visibility.Visible : Visibility.Collapsed;
        public Visibility OutTriggerVisibility => SelectedEntity is OutTrigger2dVm ? Visibility.Visible : Visibility.Collapsed;

        public Visibility ShowFramingNameVisibility =>
            SelectedEntity is FramingVm ? Visibility.Visible : Visibility.Collapsed;
        
        public bool FramingTypeCanChange
        {
            get
            {
                if (SelectedEntity is BlockingVm || SelectedEntity is Hanger2DVm)
                {
                    return false;
                }

                return true;
            }
        }
        public Visibility FramingVisibility =>
            SelectedEntity is IFramingVmBase ? Visibility.Visible : Visibility.Collapsed;
        public Visibility FBeamVisibility => SelectedEntity is Beam2DVm ? Visibility.Visible : Visibility.Collapsed;
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
        public Visibility LayerVisibility => _selectedEntity == null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility ColorVisibility => _selectedEntity == null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility ColorMethodVisibility => _selectedEntity == null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility LeaderVisibility => SelectedEntity is LeaderVM ? Visibility.Visible : Visibility.Collapsed;
        public Visibility DoorVisibility =>
            SelectedEntity is DoorCountEntityVm ? Visibility.Visible : Visibility.Collapsed;
        public Visibility BeamVisibility
        {
            get
            {
                if (SelectedEntity is BeamEntityVm)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }
        public Visibility HangerVisibility
        {
            get
            {
                if (SelectedEntity is Hanger2DVm)
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
        public Visibility BlockingVisibility
        {
            get
            {
                if (SelectedEntity is BlockingVm)
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
        }
        public Visibility HangerOutTriggerVisibility
        {
            get
            {
                if (SelectedEntity is EntityContainhangerAndOutTriggerVm)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public Visibility FramingNameVisibility
        {
            get
            {
                if (SelectedEntity is IFraming2DVmContainName)
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
        }
        public Visibility FramingBaseVisibility
        {
            get
            {
                if (SelectedEntity is IFramingVmBase || SelectedEntity is Hanger2DVm)
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
        }

        public bool IsBlockingVm
        {
            get
            {
                if (SelectedEntity is BlockingVm)
                {
                    return true;
                }

                return false;
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
                RaisePropertyChanged(nameof(OutTriggerVisibility));
                RaisePropertyChanged(nameof(FramingVisibility));
                RaisePropertyChanged(nameof(BlockingVisibility));
                RaisePropertyChanged(nameof(HangerVisibility));
                RaisePropertyChanged(nameof(FBeamVisibility));
                RaisePropertyChanged(nameof(HangerOutTriggerVisibility));
                RaisePropertyChanged(nameof(FramingBaseVisibility));
                RaisePropertyChanged(nameof(IsBlockingVm));
                RaisePropertyChanged(nameof(FramingTypeCanChange));
                RaisePropertyChanged(nameof(ShowFramingNameVisibility));
                RaisePropertyChanged(nameof(FramingNameVisibility));
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
                else if (SelectedEntity is IFramingVmBase)
                {
                    if (this.JobModel.Info.Client != null && this.JobModel.Info.Client.TimberMaterialList != null)
                    {
                        var materialGradeList = new List<string>();
                        foreach (var timberBase in JobModel.Info.Client.TimberMaterialList)
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
            get => _wallList;
            set => SetProperty(ref _wallList, value);
        }
        public List<TimberBase> TimberList { get => _timberList; set => SetProperty(ref _timberList, value); }
        public List<HangerMat> HangerList
        {
            get => _hangerList;
            set => SetProperty(ref _hangerList, value);
        }
        public SelectedEntityPropertiesViewModel() : base()
        {

        }
        public SelectedEntityPropertiesViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager, IEntitiesManager entitiesManager, IJob jobModel)
            : base(unityContainer, regionManager, eventAggregator, layerManager, jobModel)
        {
            EngineerList = new ObservableCollection<EngineerMemberInfo>();
            _entitiesManger = entitiesManager;
            this.RaisePropertyChanged(nameof(EntitiesManager));
            EntitiesManager.PropertyChanged += EntitiesManager_PropertyChanged;
            if (jobModel != null)
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
                this.SelectedEntity = EntitiesManager.SelectedEntity;
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
                if (EntitiesManager != null)
                {
                    if (EntitiesManager.SelectedEntity is IWall2D)
                    {
                        EntitiesManager.SyncLevelSelectedsEntityPropertyChanged(((IWall2D)this.SelectedEntity).WallLevelName);
                    }

                    if (SelectedEntity is BeamEntityVm beam)
                    {
                        var currentLevel = beam.FramingReference.Level;
                        if (currentLevel!=null)
                        {
                            if (currentLevel.RoofBeams.Contains(beam.FramingReference))
                            {
                                var index = currentLevel.RoofBeams.IndexOf(beam.FramingReference);
                                currentLevel.RoofBeams.Remove(beam.FramingReference);

                                for (int i = index; i < currentLevel.RoofBeams.Count; i++)
                                {
                                    currentLevel.RoofBeams[i].Index = i+1;
                                }
                            }
                        }
                        LevelWall newLevel = null;
                            foreach (var levelWall in Levels)
                            {
                                if (levelWall.LevelName == beam.WallLevelName)
                                {
                                    newLevel = levelWall;
                                }
                            }
                            if (newLevel != null)
                            {
                                ((Beam)beam.FramingReference).InitGlobalInfor(newLevel.LevelInfo);
                                beam.FramingReference.Level = newLevel;
                                beam.FramingReference.Index = newLevel.RoofBeams.Count + 1;
                                beam.Name = beam.FramingReference.Name;
                                beam.EngineerMember = null;
                                newLevel.RoofBeams.Add(beam.FramingReference);
                            }
                        
                    }
                }
            }
            if (e.PropertyName == "LayerName")
            {
                foreach(var entity in EntitiesManager.SelectedEntities)
                {
                    entity.LayerName = SelectedEntity.LayerName;
                }
            }
            if (e.PropertyName== "ColorMethod")
            {
                foreach(var entity in EntitiesManager.SelectedEntities)
                {
                    entity.ColorMethod = SelectedEntity.ColorMethod;
                }
            }
            if (e.PropertyName == "Color")
            {
                foreach (var entity in EntitiesManager.SelectedEntities)
                {
                    entity.Color = (Color)SelectedEntity.Color;
                }
            }
            if (e.PropertyName == "BeamGrade" || e.PropertyName == "OutTriggerBGrade" || e.PropertyName == "OutTriggerAGrade")
            {
                GeneralTimberList();
            }
            if (e.PropertyName == "WallBelongTo")
            {
                GeneralDoorWidthList();
            }
            if (e.PropertyName == "IsLoadBearingWall")
            {
                if (SelectedEntity is Wall2DVm wallvm)
                {
                    foreach (var selectedEntity in EntitiesManager.SelectedEntities)
                    {
                        if (selectedEntity is Wall2D wall)
                        {
                            wall.IsLoadBearingWall = wallvm.IsLoadBearingWall;

                        }
                    }
                }
            }
            if (e.PropertyName == "IsShowDimension")
            {
                if (SelectedEntity is Wall2DVm wallvm)
                {
                    foreach (var selectedEntity in EntitiesManager.SelectedEntities)
                    {
                        if (selectedEntity is Wall2D wall)
                        {
                            wall.ShowWallDimension = wallvm.IsShowDimension;

                        }
                    }
                }
            }
            if (e.PropertyName == "IsHangerA")
            {
                if (SelectedEntity is IFraming2DContainHangerAndOutTriggerVm framingVm)
                {
                    if (EntitiesManager != null)
                        foreach (var selectedEntity in EntitiesManager.SelectedEntities)
                        {
                            if (selectedEntity == EntitiesManager.SelectedEntities[0])
                            {
                                continue;
                            }
                            if (selectedEntity is I2DContaintOutTrigger &&
                                selectedEntity is IEntityVmCreateAble entityVmCreateAble)
                            {
                                var entityVm = entityVmCreateAble.CreateEntityVm(EntitiesManager);
                                if (entityVm is EntityContainhangerAndOutTriggerVm framing)
                                {
                                    framing.IsHangerA = framingVm.IsHangerA;
                                }
                            }
                        }
                }
            }
            if (e.PropertyName == "IsHangerB")
            {
                if (SelectedEntity is IFraming2DContainHangerAndOutTriggerVm framingVm)
                {
                    if (EntitiesManager != null)
                        foreach (var selectedEntity in EntitiesManager.SelectedEntities)
                        {
                            if (selectedEntity == EntitiesManager.SelectedEntities[0])
                            {
                                continue;
                            }
                            if (selectedEntity is I2DContaintOutTrigger &&
                                selectedEntity is IEntityVmCreateAble entityVmCreateAble)
                            {
                                var entityVm = entityVmCreateAble.CreateEntityVm(EntitiesManager);
                                if (entityVm is EntityContainhangerAndOutTriggerVm framing)
                                {
                                    framing.IsHangerB = framingVm.IsHangerB;
                                }
                            }
                        }
                }
            }
            if (e.PropertyName == "IsOutriggerA")
            {
                if (SelectedEntity is IFraming2DContainHangerAndOutTriggerVm framingVm)
                {
                    foreach (var selectedEntity in EntitiesManager.SelectedEntities)
                    {
                        if (selectedEntity == EntitiesManager.SelectedEntities[0])
                        {
                            continue;
                        }
                        if (selectedEntity is I2DContaintOutTrigger && selectedEntity is IEntityVmCreateAble entityVmCreateAble)
                        {
                            var entityVm = entityVmCreateAble.CreateEntityVm(EntitiesManager);
                            if (entityVm is EntityContainhangerAndOutTriggerVm framing)
                            {
                                framing.IsOutriggerA = framingVm.IsOutriggerA;
                            }
                        }
                    }
                }
            }
            if (e.PropertyName == "IsOutriggerB")
            {
                if (SelectedEntity is IFraming2DContainHangerAndOutTriggerVm framingVm)
                {
                    if (EntitiesManager != null)
                        foreach (var selectedEntity in EntitiesManager.SelectedEntities)
                        {
                            if (selectedEntity == EntitiesManager.SelectedEntities[0])
                            {
                                continue;
                            }
                            if (selectedEntity is I2DContaintOutTrigger && selectedEntity is IEntityVmCreateAble entityVmCreateAble)
                            {
                                var entityVm = entityVmCreateAble.CreateEntityVm(EntitiesManager);
                                if (entityVm is EntityContainhangerAndOutTriggerVm framing)
                                {
                                    framing.IsOutriggerB = framingVm.IsOutriggerB;
                                }
                            }
                            //if (selectedEntity is Joist2D joist)
                            //{
                            //    var joistVM = (Joist2dVm) joist.CreateEntityVm(EntitiesManager);
                            //    joistVM.IsOutriggerB = joist2d.IsOutriggerB;
                            //}
                        }
                }
            }
            if (e.PropertyName == "Material")
            {
                if (SelectedEntity is Hanger2DVm hanger2D)
                {
                    if (EntitiesManager != null)
                        foreach (var selectedEntity in EntitiesManager.SelectedEntities)
                        {
                            if (selectedEntity is Hanger2D hanger)
                            {
                                ((Hanger) hanger.FramingReference).HangerMaterial = hanger2D.Material;
                            }
                        }
                }
            }
            if (e.PropertyName == "BeamGrade")
            {
                if (SelectedEntity is FramingVmBase timberVm)
                {
                    if (EntitiesManager != null)
                        foreach (var selectedEntity in EntitiesManager.SelectedEntities)
                        {
                            if (selectedEntity is IFraming2D && selectedEntity is IEntityVmCreateAble entityCreateAble)
                            {
                                var entityVm = entityCreateAble.CreateEntityVm(EntitiesManager);
                                if (entityVm is IFramingVmBase framingVm)
                                {
                                    framingVm.BeamGrade = timberVm.BeamGrade;
                                }
                            }
                        }
                }
            }
            if (e.PropertyName == "FramingInfo")
            {
                if (SelectedEntity is FramingVmBase timberVm)
                {
                    foreach (var selectedEntity in EntitiesManager.SelectedEntities)
                    {
                        if (selectedEntity is IFraming2D && selectedEntity is IEntityVmCreateAble entityCreateAble)
                        {
                            var entityVm = entityCreateAble.CreateEntityVm(EntitiesManager);
                            if (entityVm is IFramingVmBase framingVm)
                            {
                                framingVm.BeamGrade = timberVm.BeamGrade;
                                framingVm.FramingInfo = timberVm.FramingInfo;
                            }
                        }
                       
                    }
                }
            }
            if (e.PropertyName == "BlockingType")
            {
                if (SelectedEntity is BlockingVm blocking)
                {
                    foreach (var selectedEntity in EntitiesManager.SelectedEntities)
                    {
                        if (selectedEntity is Blocking2D blocking2D)
                        {
                            var blockingVm = blocking2D.CreateEntityVm(EntitiesManager) as BlockingVm;
                            blockingVm.BlockingType = blocking.BlockingType;
                        }
                    }
                }
            }
            if (e.PropertyName =="Pitch")
            {
                 if (SelectedEntity is FramingVm timberVm)
                 {
                     foreach (var selectedEntity in EntitiesManager.SelectedEntities)
                     {
                         if (selectedEntity is IFraming2D && selectedEntity is IEntityVmCreateAble entityCreateAble)
                         {
                             var entityVm = entityCreateAble.CreateEntityVm(EntitiesManager);
                             if (entityVm is IFramingVm framingVm)
                             {
                                 framingVm.Pitch = timberVm.Pitch;
                             }
                         }
                     }
                 }

            }
            if (e.PropertyName== "FramingType")
            {
                 if (SelectedEntity is FramingVm timberVm)
                 {
                     foreach (var selectedEntity in EntitiesManager.SelectedEntities)
                     {
                         if (selectedEntity is IFraming2D && selectedEntity is IEntityVmCreateAble entityCreateAble)
                         {
                             var entityVm = entityCreateAble.CreateEntityVm(EntitiesManager);
                             if (entityVm is IFramingVm framingVm)
                             {
                                 framingVm.FramingType= timberVm.FramingType;
                             }
                         }
                       
                     }
                 }

            }
            if (e.PropertyName == "HangerAMat")
            {
                if(SelectedEntity is EntityContainhangerAndOutTriggerVm framingVm)
                {
                    foreach(var selecttedEntity in EntitiesManager.SelectedEntities)
                    {
                        if(selecttedEntity is IFraming2DContaintHangerAndOutTrigger && selecttedEntity is IEntityVmCreateAble vmCreateAble)
                        {
                            var entityVM = vmCreateAble.CreateEntityVm(EntitiesManager);
                            if (entityVM is IFraming2DContainHangerAndOutTriggerVm frameVM)
                            {
                                frameVM.HangerAMat = framingVm.HangerAMat;
                            }
                        }
                    }
                }
            }
            if (e.PropertyName == "HangerBMat")
            {
                if (SelectedEntity is EntityContainhangerAndOutTriggerVm framingVm)
                {
                    foreach (var selecttedEntity in EntitiesManager.SelectedEntities)
                    {
                        if (selecttedEntity is IFraming2DContaintHangerAndOutTrigger && selecttedEntity is IEntityVmCreateAble vmCreateAble)
                        {
                            var entityVM = vmCreateAble.CreateEntityVm(EntitiesManager);
                            if (entityVM is IFraming2DContainHangerAndOutTriggerVm frameVM)
                            {
                                frameVM.HangerBMat = framingVm.HangerBMat;
                            }
                        }
                    }
                }
            }
            if (e.PropertyName == "OutTriggerAMat")
            {
                if (SelectedEntity is EntityContainhangerAndOutTriggerVm framingVm)
                {
                    foreach (var selecttedEntity in EntitiesManager.SelectedEntities)
                    {
                        if (selecttedEntity is IFraming2DContaintHangerAndOutTrigger && selecttedEntity is IEntityVmCreateAble vmCreateAble)
                        {
                            var entityVM = vmCreateAble.CreateEntityVm(EntitiesManager);
                            if (entityVM is IFraming2DContainHangerAndOutTriggerVm frameVM)
                            {
                                frameVM.OutTriggerAMat = framingVm.OutTriggerAMat;
                            }
                        }
                    }
                }
            }
            if (e.PropertyName == "OutTriggerBMat")
            {
                if (SelectedEntity is EntityContainhangerAndOutTriggerVm framingVm)
                {
                    foreach (var selecttedEntity in EntitiesManager.SelectedEntities)
                    {
                        if (selecttedEntity is IFraming2DContaintHangerAndOutTrigger && selecttedEntity is IEntityVmCreateAble vmCreateAble)
                        {
                            var entityVM = vmCreateAble.CreateEntityVm(EntitiesManager);
                            if (entityVM is IFraming2DContainHangerAndOutTriggerVm frameVM)
                            {
                                frameVM.OutTriggerBMat = framingVm.OutTriggerBMat;
                            }
                        }
                    }
                }
            }
            if (e.PropertyName == "Index")
            {
                if(SelectedEntity is IFraming2DVmContainName framing2D)
                {
                    foreach(var selectedEntity in EntitiesManager.SelectedEntities)
                    {
                        if(selectedEntity is IFraming2D selectedFraming2D && selectedEntity is IEntityVmCreateAble entityVMCreate)
                        {
                            var entVm = entityVMCreate.CreateEntityVm(EntitiesManager);
                            if (entVm is IFraming2DVmContainName entVmBase)
                                entVmBase.Index = framing2D.Index;
                        }
                    }
                }
            }    
            EntitiesManager?.Refresh();
        }

        private void GeneralTimberList()
        {
            if (SelectedEntity is IFramingVmBase framingVm && !(SelectedEntity is BeamEntityVm) )
            {
                if (JobModel.Info.Client != null)
                {
                    if (JobModel.Info.Client.TimberMaterialList != null)
                    {
                        if (!string.IsNullOrEmpty(framingVm.BeamGrade))
                        {
                            TimberList = GeneralTimberListForFraming(framingVm.BeamGrade, JobModel.Info.Client.TimberMaterialList);
                        }
                    }
                }
            }
            //TimberList.Clear();
            else if (SelectedEntity is BeamEntityVm beam)
            {
                if (JobModel.Info.Client != null && JobModel.Info.Client.Beams != null)
                {
                    if (string.IsNullOrEmpty(beam.BeamGrade))
                        return;
                    if (JobModel.Info.Client.Beams.ContainsKey(beam.BeamGrade))
                    {
                        JobModel.Info.Client.Beams.TryGetValue(beam.BeamGrade, out var timberList);
                        TimberList = timberList;
                    }
                }
            }
            if (SelectedEntity is EntityContainhangerAndOutTriggerVm framing)
            {
                if (JobModel.Info.Client != null)
                {
                    if (JobModel.Info.Client.TimberMaterialList != null)
                    {
                        if (!string.IsNullOrEmpty((framing.OutTriggerAGrade)))
                        {
                            OutTriggerATimberList = GeneralTimberListForFraming(framing.OutTriggerAGrade,
                                JobModel.Info.Client.TimberMaterialList);
                        }
                        if (!string.IsNullOrEmpty((framing.OutTriggerBGrade)))
                        {
                            OutTriggerBTimberList = GeneralTimberListForFraming(framing.OutTriggerBGrade,
                                JobModel.Info.Client.TimberMaterialList);
                        }
                    }

                    HangerList = JobModel.Info.Client.Hangers;
                }

            }
            //else if (SelectedEntity is OutTrigger2dVm outTrigger)
            //{
            //    if (JobModel.Info.Client != null)
            //    {
            //        if (JobModel.Info.Client.TimberMaterialList != null)
            //        {
            //            if (!string.IsNullOrEmpty((outTrigger.BeamGrade)))
            //            {
            //                var tempList = new List<TimberBase>();
            //                foreach (var timber in JobModel.Info.Client.TimberMaterialList)
            //                {
            //                    if (timber.TimberGrade == outTrigger.BeamGrade)
            //                    {
            //                        tempList.Add(timber);
            //                    }
            //                }

            //                TimberList = tempList;
            //            }


            //        }
            //    }
            //}
        }

        public List<TimberBase> GeneralTimberListForFraming(string timberGrade,List<TimberBase> timberList)
        {
            List<TimberBase> timbers = new List<TimberBase>();
            foreach (var timber in timberList)    
            {
                if (timber.TimberGrade == timberGrade)
                {
                    timbers.Add(timber);
                }
            }
            return timbers;
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
                        if (engineerMemberInfo.LevelType == beam.WallLevelName || engineerMemberInfo.LevelType == "Global")
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
                if (JobModel != null && JobModel.Levels != null)
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
                if (door.WallBelongTo != null)
                {
                    if (door.WallBelongTo.WallType != null)
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
