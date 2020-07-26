// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrenailFloorInputViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the PrenailFloorInputViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System.Globalization;
using ApplicationInterfaceCore;
using AppModels.CustomEntity;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.WallMemberData;
using devDept.Eyeshot.Entities;

namespace WallFrameInputModule.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    //using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    using ApplicationCore.BaseModule;

    using AppModels;
    using CsvHelper;

    using MaterialDesignExtensions.Controls;

    using Prism.Commands;
    using Prism.Events;
    using Prism.Regions;

    using Syncfusion.UI.Xaml.Grid;
    using Syncfusion.UI.Xaml.Grid.Helpers;

    using Unity;

    

    /// <summary>
    /// The Prenail floor input view model.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class PrenailFloorInputViewModel : BaseFloorViewModelAware
    {
        #region private Field
        private IEntitiesManager _entitiesManager;
        /// <summary>
        /// The title.
        /// </summary>
        private string _title;

        //private string _levelName;
        /// <summary>
        /// The start item id.
        /// </summary>
        private int _startItemId;

        /// <summary>
        /// The studs.
        /// </summary>
        private Dictionary<string, List<TimberBase>> _studs;

        /// <summary>
        /// The bottomPlates.
        /// </summary>
        private Dictionary<string, List<TimberBase>> _ribbonPlates;

        /// <summary>
        /// The topPlates.
        /// </summary>
        private Dictionary<string, List<TimberBase>> _topPlates;

        /// <summary>
        /// The topPlates.
        /// </summary>
        private Dictionary<string, List<TimberBase>> _bottomPlates;

        /// <summary>
        /// The csv file path.
        /// </summary>
        private string _csvFilePath;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PrenailFloorInputViewModel"/> class.
        /// </summary>
        public PrenailFloorInputViewModel()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrenailFloorInputViewModel"/> class.
        /// </summary>
        /// <param name="unityContainer">
        /// The unity container.
        /// </param>
        /// <param name="regionManager">
        /// The region manager.
        /// </param>
        /// <param name="eventAggregator">
        /// The event aggregator.
        /// </param>
        public PrenailFloorInputViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator,ILayerManager layerManager, IEntitiesManager entitiesManager,IJob jobModel )
            : base(unityContainer, regionManager, eventAggregator,layerManager,jobModel)
        {
            this.EntitiesManager = entitiesManager;
            EntitiesManager.EntitiesCollectionChanged += EntitiesManager_EntitiesCollectionChanged;
            this.PropertyChanged += PrenailFloorInputViewModelPropertyChanged;
            this.WallThicknessList = new List<string> { "90", "70" };
            //this.CreateLayers();
            //this.WallInputLoadedCommand = new DelegateCommand<FrameworkElement>(this.InputControlLoaded);
            this.NewWallRowInputCommand = new DelegateCommand(this.OnAddNewWallRow);
            this.DeleteWallRowCommand = new DelegateCommand<SfDataGrid>(this.OnDeleteWallRow);
            this.WallInputCellValueEndChangeCommand = new DelegateCommand<SfDataGrid>(this.OnWallInputCellValueEndChanged);
            this.WallInputSortCommand = new DelegateCommand(this.OnWallInputSort);
            this.LoadCSVFileCommand = new DelegateCommand(this.OnLoadCSVFile);
            this.AddBeamCommand = new DelegateCommand(this.OnAddNewBeam);
            this.ReFreshWallCommand = new DelegateCommand(this.CalculatorWallLength);
        }



        #endregion

        #region public
        //public string LevelName
        //{
        //    get => _levelName;
        //    set => SetProperty(ref _levelName, value);
        //}
        public IEntitiesManager EntitiesManager
        {
            get => _entitiesManager;
            set => SetProperty(ref _entitiesManager, value);
        }
        /// <summary>
        /// Gets or sets the beam grade list.
        /// </summary>
        public List<string> BeamGradeList
        {
            get
            {
                if (this.SelectedClient != null && this.SelectedClient.Beams != null)
                    return new List<string>(this.SelectedClient.Beams.Keys);
                else return null;
            }
        }

        /// <summary>
        /// Gets or sets the wall input data grid.
        /// </summary>
        public SfDataGrid WallInputDataGrid { get; set; }

        /// <summary>
        /// Gets or sets the studs.
        /// </summary>
        public Dictionary<string, List<TimberBase>> Studs
        {
            get => this._studs;
            set => this.SetProperty(ref this._studs, value);
        }

        /// <summary>
        /// Gets or sets the ribbonplates.
        /// </summary>
        public Dictionary<string, List<TimberBase>> RibbonPlates
        {
            get => this._ribbonPlates;
            set => this.SetProperty(ref this._ribbonPlates, value);
        }

        /// <summary>
        /// Gets or sets the ribbonplates.
        /// </summary>
        public Dictionary<string, List<TimberBase>> TopPlates
        {
            get => this._topPlates;
            set => this.SetProperty(ref this._topPlates, value);
        }

        /// <summary>
        /// Gets or sets the ribbonplates.
        /// </summary>
        public Dictionary<string, List<TimberBase>> BottomPlates
        {
            get => this._bottomPlates;
            set => this.SetProperty(ref this._bottomPlates, value);
        }

        /// <summary>
        /// Gets or sets the wall thickness.
        /// </summary>
        public List<string> WallThicknessList { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get => this._title;
            set => this.SetProperty(ref this._title, value);
        }

        /// <summary>
        /// Gets or sets the layers.
        /// </summary>
        //public List<Layer> Layers { get; set; }

        /// <summary>
        /// Gets the new wall row input command.
        /// </summary>
        public ICommand NewWallRowInputCommand { get; private set; }

        /// <summary>
        /// Gets the delete wall row command.
        /// </summary>
        public ICommand DeleteWallRowCommand { get; private set; }

        /// <summary>
        /// Gets the wall info cell value change command.
        /// </summary>
        public ICommand WallInputCellValueEndChangeCommand { get; private set; }

        /// <summary>
        /// Gets the wall input sort.
        /// </summary>
        public ICommand WallInputSortCommand { get; private set; }

        /// <summary>
        /// Gets the wall input loaded command.
        /// </summary>
        public ICommand WallInputLoadedCommand { get; private set; }

        /// <summary>
        /// Gets the load csv file command.
        /// </summary>
        public ICommand LoadCSVFileCommand { get; private set; }

        /// <summary>
        /// Gets the add beam command.
        /// </summary>
        public ICommand AddBeamCommand { get; private set; }

        public ICommand ReFreshWallCommand { get; private set; }

        #endregion

        #region Public Method

        /// <summary>
        /// The on navigated to.
        /// </summary>
        /// <param name="navigationContext">
        /// The navigation context.
        /// </param>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            this.Title = "Prenai - " + this.Level.LevelName;
            //this.LevelName = this.Level.LevelName;
            // this.Studs = this.SelectedClient.Studs;
            // this.RibbonPlates = this.SelectedClient.RibbonPlates;
            // this.TopPlates = this.SelectedClient.TopPlates;
            // this.BottomPlates = this.SelectedClient.BottomPlates;
            // this.BeamGradeList = new List<string>(this.SelectedClient.Beams.Keys);
            this.Level.TimberWallBracings = new ObservableCollection<Bracing>();
            this.Level.GeneralBracings = new ObservableCollection<GenericBracing>();
            var genericBracing = new GenericBracing { BracingInfo = new GenericBracingBase { Name = "MetalBrace" } };
            this.Level.GeneralBracings.Add(genericBracing);
            this.Level.LevelInfo.PropertyChanged += this.LevelChangedInfo;
            //.WallLayers[1].WallColorLayer.Color

        }





        #endregion

        #region Private method

        private void EntitiesManager_EntitiesCollectionChanged(object sender, System.EventArgs e)
        {
            CalculatorWallLength();
        }

        public void CalculatorWallLength()
        {
            if (EntitiesManager.Entities != null && this.Level != null && this.Level.WallLayers != null && this.Level.WallLayers.Count != 0)
            {
                var wallayers = this.Level.WallLayers;
                var entities = EntitiesManager.Entities;
                foreach (var levelWallLayer in wallayers)
                {
                    var tempLength = 0.0;
                    if (levelWallLayer.WallColorLayer==null)
                    {
                        continue;
                    }
                    foreach (var entity in entities)
                    {
                        if (entity is IWall2D iWall2D && iWall2D.WallLevelName == this.Level.LevelName)
                        {
                            if (entity.LayerName == levelWallLayer.WallColorLayer.Name)
                            {
                                tempLength += iWall2D.Length();
                            }
                        }
                       
                    }

                    //levelWallLayer.TempLength = tempLength / 1000;
                }

            }
        }
        private void PrenailFloorInputViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(this.SelectedClient):
                    this.RaisePropertyChanged(nameof(this.BeamGradeList));
                    break;
            }

        }

        /// <summary>
        /// The on add new beam.
        /// </summary>
        private void OnAddNewBeam()
        {
            if (this.Level.RoofBeams == null)
            {
                this.Level.RoofBeams = new ObservableCollection<Beam>();
            }
            var newBeam = new Beam();
            this.Level.RoofBeams.Add(newBeam);
        }

        /// <summary>
        /// The on load csv file.
        /// </summary>
        private async void OnLoadCSVFile()
        {
            OpenFileDialogArguments dialogArgs = new OpenFileDialogArguments()
            {
                Width = 600,
                Height = 1600,
                Filters = "All files|*.*|CSv files|*.csv|Text files|*.txt"
            };
            OpenFileDialogResult result = await OpenFileDialog.ShowDialogAsync("openFileDialogHost", dialogArgs);

            if (result.File == null)
            {
                return;
            }
            this._csvFilePath = result.File;

            this.LoadCsvLength();
            this.LoadDataLength();
        }

        /// <summary>
        /// The load data length.
        /// </summary>
        private void LoadCsvLength()
        {
            using (var reader = new StreamReader(this._csvFilePath))
            using (var csvFile = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csvFile.GetRecords<WallTempLength>().ToList();
                this.Level.TempLengths = new ObservableCollection<WallTempLength>(records);
            }
        }

        /// <summary>
        /// The load data length.
        /// </summary>
        private void LoadDataLength()
        {
            if (this.Level.WallLayers.Count <= 0 || this.Level.TempLengths.Count <= 0)
            {
                return;
            }
            foreach (var wallLayer in this.Level.WallLayers)
            {
                WallTempLength first = null;
                foreach (var x in this.Level.TempLengths)
                {
                    if (x.Id != wallLayer.WallColorLayer.Name)
                    {
                        continue;
                    }

                    first = x;
                    break;
                }

                if (first == null)
                {
                    continue;
                }

                var length =
                    first.Length;

                //wallLayer.TempLength = length / 1000;
            }
        }

        /// <summary>
        /// The input control loaded.
        /// </summary>
        /// <param name="param">
        /// The param.
        /// </param>
        //private void InputControlLoaded(FrameworkElement param)
        //{
        //    this.WallInputDataGrid = param.FindName("WallInput") as SfDataGrid;
        //    //this.WallInputDataGrid.CurrentCellEndEdit += this.WallInput_CurrentCellEndEdit;
        //}

        /// <summary>
        /// The level changed info.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void LevelChangedInfo(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            //if (e.PropertyName == "ExternalWallThickness" || e.PropertyName == "InternalWallThickness")
            //{
            //    foreach (var wallLayer in this.Level.WallLayers)
            //    {
            //        wallLayer.ChangeWallThickness();


            //        if (wallLayer.TimberWallTypePoco.IsLoadBearingWall && e.PropertyName == "ExternalWallThickness")
            //        {
            //            if (wallLayer.WallThickness.Size == this.Level.LevelInfo.ExternalWallThickness)
            //            {
            //                wallLayer.WallThickness.IsDefaultValue = true;
            //                // goi ham thay doi stud, ribbon, topplate bottom plate thickness o day
            //                this.ChangedWallThickness(wallLayer);
            //            }
            //        }
            //        else if (!wallLayer.TimberWallTypePoco.IsLoadBearingWall && e.PropertyName == "InternalWallThickness")
            //        {
            //            if (wallLayer.WallThickness.Size == this.Level.LevelInfo.InternalWallThickness)
            //            {
            //                wallLayer.WallThickness.IsDefaultValue = true;
            //                // goi ham thay doi stud, ribbon, topplate bottom plate thickness o day
            //                this.ChangedWallThickness(wallLayer);
            //            }
            //        }
            //    }
            //}
        }

        /// <summary>
        /// The on add new wall row.
        /// </summary>
        private void OnAddNewWallRow()
        {
            this._startItemId = this.Level.WallLayers.Count + 1;

            var data = new WallLayer(this._startItemId, this.Level.LevelInfo,this.SelectedClient.WallTypes[0]);
            this.Level.WallLayers.Add(data);
        }

        /// <summary>
        /// The on wall input cell value changed.
        /// </summary>
        /// <param name="wallInput">
        /// The wall input.
        /// </param>
        private void OnWallInputCellValueEndChanged(SfDataGrid wallInput)
        {

            // wallInput.CurrentCellEndEdit += WallInput_CurrentCellEndEdit;
            //var currentRow = wallInput.SelectedIndex;
            //var currentWallLayer = this.Level.WallLayers[currentRow];

            //if (wallInput.CurrentColumn.MappingName == "TimberWallTypePoco")
            //{
            //    this.ChangedWallThickness(currentWallLayer);
            //}
            //if (wallInput.CurrentColumn.MappingName == "WallThickness.Size")
            //{
            //    this.ChangedWallThickness(currentWallLayer);
            //}
            //if (wallInput.CurrentColumn.MappingName == "Stud.TimberMaterialInfo")
            //{
            //    this.ChangeWallMemberByDefault(wallInput);
            //}

            //if (wallInput.CurrentColumn.MappingName == "RibbonPlate.TimberMaterialInfo"
            //    || wallInput.CurrentColumn.MappingName == "TopPlate.TimberMaterialInfo"
            //    || wallInput.CurrentColumn.MappingName == "BottomPlate.TimberMaterialInfo")
            //{
            //    this.ChangeDefaultMemberIs(wallInput);
            //}
        }

        private void WallInput_CurrentCellEndEdit(object sender, CurrentCellEndEditEventArgs e)
        {
            //var sfDataGrid = sender as SfDataGrid;

            //var currentRow = e.RowColumnIndex.RowIndex;
            //var rowData = sfDataGrid.GetRecordAtRowIndex(currentRow);
            //var currentWallLayer = (WallLayer)rowData;

            //if (sfDataGrid.CurrentColumn.MappingName == "TimberWallTypePoco")
            //{
            //    this.ChangedWallThickness(currentWallLayer);

            //}
            //if (sfDataGrid.CurrentColumn.MappingName == "WallThickness.Size")
            //{
            //    this.ChangedWallThickness(currentWallLayer);
            //}
            //if (sfDataGrid.CurrentColumn.MappingName == "Stud.TimberMaterialInfo")
            //{
            //    this.ChangeWallMemberByDefault(sfDataGrid);
            //}

            //if (sfDataGrid.CurrentColumn.MappingName == "RibbonPlate.TimberMaterialInfo"
            //    || sfDataGrid.CurrentColumn.MappingName == "TopPlate.TimberMaterialInfo"
            //    || sfDataGrid.CurrentColumn.MappingName == "BottomPlate.TimberMaterialInfo")
            //{
            //    this.ChangeDefaultMemberIs(sfDataGrid);
            //}

            // this.OnWallInputSort();

        }

        /// <summary>
        /// The change default member is.
        /// </summary>
        /// <param name="wallInput">
        /// The wall input.
        /// </param>
        private void ChangeDefaultMemberIs(SfDataGrid wallInput)
        {
            //var currentRow = wallInput.SelectedIndex;
            //var currenLayer = this.Level.WallLayers[currentRow];
            //var timberRibbon = currenLayer.RibbonPlate;
            //var timberTopplate = currenLayer.TopPlate;
            //var timberBottomPlate = currenLayer.BottomPlate;
            //var timberStud = currenLayer.Stud;
            //if (timberRibbon.TimberMaterialInfo != null)
            //{
            //    //timberRibbon.IsDefault = this.ChangeDefaultMemberInfo(timberRibbon, timberStud);
            //}

            //if (timberTopplate.TimberMaterialInfo != null)
            //{
            //    //timberTopplate.IsDefault = this.ChangeDefaultMemberInfo(timberTopplate, timberStud);
            //}

            //if (timberBottomPlate.TimberMaterialInfo != null)
            //{
            //    //timberBottomPlate.IsDefault = this.ChangeDefaultMemberInfo(timberBottomPlate, timberStud);
            //}


        }

        /// <summary>
        /// The change default member info.
        /// </summary>
        /// <param name="currentTimberbase">
        /// The current timberbase.
        /// </param>
        /// <param name="currentStud">
        /// The current stud.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        //private bool ChangeDefaultMemberInfo(WallMember currentTimberbase, WallStud currentStud)
        //{
        //    //if (currentStud.TimberMaterialInfo == null)
        //    //{
        //    //    return false;
        //    //}

        //    //return currentTimberbase.TimberMaterialInfo.Thickness == currentStud.TimberMaterialInfo.Thickness && currentTimberbase.TimberMaterialInfo.Depth == currentStud.TimberMaterialInfo.Depth
        //    //       && currentTimberbase.TimberMaterialInfo.TimberGrade == currentStud.TimberMaterialInfo.TimberGrade;
        //}

        /// <summary>
        /// The change wall member by default.
        /// </summary>
        /// <param name="wallInput">
        /// The wall input.
        /// </param>
        private void ChangeWallMemberByDefault(SfDataGrid wallInput)
        {
            //var currentRow = wallInput.SelectedIndex;
            //var currenLayer = this.Level.WallLayers[currentRow];
            //var currentStud = currenLayer.Stud;

            //var currentRibbonPlate = currenLayer.RibbonPlate;

            //if (currentStud.TimberMaterialInfo == null)
            //{
            //    return;
            //}

            //if (!currenLayer.TimberWallTypePoco.IsLoadBearingWall)
            //{
            //    currenLayer.RibbonPlate.TimberMaterialInfo = this.ChangedTimberPlateThickness("RibbonPlate", currenLayer, currenLayer.RibbonPlate, this.RibbonPlates);
            //}
            //else if (currentRibbonPlate.IsDefault)
            //{
            //    currentRibbonPlate.TimberMaterialInfo = this.ChangeTimberInfoByDefault(
            //        currentStud,
            //        currenLayer,
            //        this.RibbonPlates);
            //}

            //var currentTopPlate = currenLayer.TopPlate;
            //if (currentTopPlate.IsDefault)
            //{
            //    currentTopPlate.TimberMaterialInfo = this.ChangeTimberInfoByDefault(currentStud, currenLayer, this.TopPlates);
            //}

            //var currentBottomPlate = currenLayer.BottomPlate;
            //if (currentBottomPlate.IsDefault)
            //{
            //    currentBottomPlate.TimberMaterialInfo = this.ChangeTimberInfoByDefault(
            //        currentStud,
            //        currenLayer,
            //        this.BottomPlates);
            //}

        }

        /// <summary>
        /// The change timber info by default.
        /// </summary>
        /// <param name="currentStud">
        /// The current stud.
        /// </param>
        /// <param name="currenLayer">
        /// The curren layer.
        /// </param>
        /// <param name="listTimberBases">
        /// The list timber bases.
        /// </param>
        /// <returns>
        /// The <see cref="TimberBase"/>.
        /// </returns>
        private TimberBase ChangeTimberInfoByDefault(
            WallStud currentStud,
            WallLayer currenLayer,
            Dictionary<string, List<TimberBase>> listTimberBases)
        {

            //var filterTimbers = listTimberBases.SelectMany(x => x.Value).Where(x => x.Thickness == currenLayer.WallThickness.Size);

            //var selectedTimber = filterTimbers.FirstOrDefault(
            //x => x.Depth == currentStud.TimberMaterialInfo.Depth && x.TimberGrade == currentStud.TimberMaterialInfo.TimberGrade);
            //if (selectedTimber == null)
            //{
            //    return null;
            //}

            //return selectedTimber;

            //Fix sau
            return null;

        }

        /// <summary>
        /// The changed wall thickness.
        /// </summary>
        /// <param name="wallInput">
        /// The wall input.
        /// </param>
        //private void ChangedWallThickness(WallLayer currentLayer)
        //{
        //    this.ChangedStudThickness(currentLayer);

        //    if (currentLayer.RibbonPlate != null)
        //    {
        //        //currentLayer.RibbonPlate.TimberMaterialInfo = this.ChangedTimberPlateThickness("RibbonPlate", currentLayer, currentLayer.RibbonPlate, this.RibbonPlates);
        //    }

        //    if (currentLayer.TopPlate != null)
        //    {
        //        //currentLayer.TopPlate.TimberMaterialInfo = this.ChangedTimberPlateThickness("TopPlate", currentLayer, currentLayer.TopPlate, this.TopPlates);
        //    }

        //    if (currentLayer.BottomPlate != null)
        //    {
        //        //currentLayer.BottomPlate.TimberMaterialInfo = this.ChangedTimberPlateThickness("BottomPlate", currentLayer, currentLayer.BottomPlate, this.BottomPlates);
        //    }
        //}

        /// <summary>
        /// The changed stud thickness.
        /// </summary>
        /// <param name="wallInput">
        /// The wall input.
        /// </param>
        /// <param name="currenLayer">
        /// The curren layer.
        /// </param>
        //private void ChangedStudThickness(WallLayer currenLayer)
        //{
        //    if (currenLayer.Stud.TimberMaterialInfo == null)
        //    {
        //        return;
        //    }

        //    var currentTimber = currenLayer.Stud;
        //    //var filterStuds = this.Studs.SelectMany(x => x.Value).Where(x => x.Thickness == currenLayer.WallThickness.Size);

        //    //var selectedStud = filterStuds.FirstOrDefault(
        //    //    x => x.Depth == currentTimber.TimberMaterialInfo.Depth && x.TimberGrade == currentTimber.TimberMaterialInfo.TimberGrade);
        //    //if (selectedStud == null)
        //    //{
        //    //    return;
        //    //}

        //    //currenLayer.Stud.TimberMaterialInfo = selectedStud;
        //}

        /// <summary>
        /// The changed timber plate thickness.
        /// </summary>
        /// <param name="memberNameChange">
        /// The member Name Change.
        /// </param>
        /// <param name="currentLayer">
        /// The current layer.
        /// </param>
        /// <param name="currentWallMember">
        /// The current wall member.
        /// </param>
        /// <param name="listTimbers">
        /// The list timbers.
        /// </param>
        /// <returns>
        /// The <see cref="TimberBase"/>.
        /// </returns>
        private TimberBase ChangedTimberPlateThickness(
            string memberNameChange,
            WallLayer currentLayer,
            WallMember currentWallMember,
            Dictionary<string, List<TimberBase>> listTimbers)
        {
            if (listTimbers==null)
            {
                return null;
            }
            //var filterTimbers = listTimbers.SelectMany(x => x.Value).Where(x => x.Thickness == currentLayer.WallThickness.Size);
            if (memberNameChange == "RibbonPlate")
            {
                // return nil if wall is None LBW and member change is ribbon plate
                //if (!currentLayer.TimberWallTypePoco.IsLoadBearingWall)
                //{
                //    var rbInfor = filterTimbers.FirstOrDefault(
                //    x => x.SizeGrade == "Nil");
                //    return rbInfor;
                //}
            }

            // return null if current wall member is nothing / not available
            //if (currentWallMember.TimberMaterialInfo == null)
            //{
            //    return null;
            //}

            // if change from non LBW to LBW 
            //if (currentWallMember.TimberMaterialInfo.SizeGrade == "Nil")
            //{
            //    // return null if topplate info is null
            //    if (currentLayer.TopPlate.TimberMaterialInfo == null)
            //    {
            //        return null;
            //    }

            //    // else select ribbon plate like top plate
            //    var rbInfo = filterTimbers.FirstOrDefault(
            //    x => x.Depth == currentLayer.TopPlate.TimberMaterialInfo.Depth && x.TimberGrade == currentLayer.TopPlate.TimberMaterialInfo.TimberGrade);
            //    return rbInfo;
            //}

            // else change member thickness with iformation as current member
            //var selectedTimberInfo = filterTimbers.FirstOrDefault(
            //    x => x.Depth == currentWallMember.TimberMaterialInfo.Depth && x.TimberGrade == currentWallMember.TimberMaterialInfo.TimberGrade);
            //if (selectedTimberInfo == null)
            //{
            //    // them dialog new ko chuyen doi duoc o day
            //    return currentWallMember.TimberMaterialInfo;
            //}

            //return selectedTimberInfo;
            //fixed sau
            return null;

        }

        /// <summary>
        /// The on delete wall row.
        /// </summary>
        /// <param name="wallGrid">
        /// The wall grid.
        /// </param>
        private void OnDeleteWallRow(SfDataGrid wallGrid)
        {
            var recordId = wallGrid.SelectedIndex;

            if (recordId < 0)
            {
                return;
            }

            this.Level.WallLayers.RemoveAt(recordId);

            if (this.Level.WallLayers.Count == 0)
            {
                return;
            }

            var startCount = 0;

            if (recordId == 0)
            {
                startCount = 1;
            }
            else
            {
                startCount = this.Level.WallLayers[recordId - 1].Id + 1;
            }


            var itemIndex = recordId;

            for (var i = recordId; i <= this.Level.WallLayers.Count - 1; i++)
            {
                this.Level.WallLayers[itemIndex].Id = startCount;
                startCount++;
                itemIndex++;
            }
        }

        #endregion

        #region Create method sample

        /// <summary>
        /// The on wall input sort.
        /// </summary>
        private void OnWallInputSort()
        {
            if (this.Level.WallLayers.Count > 0)
            {
                var tempData = new List<WallLayer>(this.Level.WallLayers);
                tempData.Sort();
                this.Level.WallLayers = new ObservableCollection<WallLayer>(tempData);

                // re Order ID
                var startId = 1;
                foreach (var wallLayer in this.Level.WallLayers)
                {
                    wallLayer.Id = startId;
                    startId++;
                }
            }
        }

        /// <summary>
        /// The create layers.
        /// </summary>
        private void CreateLayers()
        {
            //this.Layers = new List<Layer>();
            //var layer = new Layer("1", System.Drawing.Color.Red);
            
            //this.Layers.Add(layer);
            //var layer2 = new Layer("2", System.Drawing.Color.LawnGreen);
            
            //this.Layers.Add(layer2);
        }

        #endregion


    }
}
