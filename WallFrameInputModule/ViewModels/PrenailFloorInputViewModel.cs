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
using AppModels.Factories;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Openings;
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

        private PrenailWallLayer _selectedWall;
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
            this.WallThicknessList = new List<int> { 90, 70 };
            WallSpacingList = new List<int>(){300,350,400,450,600};
            //this.CreateLayers();
            //this.WallInputLoadedCommand = new DelegateCommand<FrameworkElement>(this.InputControlLoaded);
            this.NewWallRowInputCommand = new DelegateCommand(this.OnAddNewWallRow);
            this.DeleteWallRowCommand = new DelegateCommand<SfDataGrid>(this.OnDeleteWallRow);
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
        public PrenailWallLayer SelectedWall
        {
            get => _selectedWall;
            set => SetProperty(ref _selectedWall, value);
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
        public List<int> WallThicknessList { get; set; }
        public List<int> WallSpacingList { get; set; }

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
        public ICommand WallInputCellValueValidateChangeCommand { get; private set; }

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
            //if (this.Level.RoofBeams == null)
            //{
            //    this.Level.RoofBeams = new ObservableCollection<Beam>();
            //}
            //var newBeam = new Beam();
            //this.Level.RoofBeams.Add(newBeam);
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

            //this.LoadCsvLength();
            //this.LoadDataLength();
        }

        /// <summary>
        /// The load data length.
        /// </summary>
        //private void LoadCsvLength()
        //{
        //    using (var reader = new StreamReader(this._csvFilePath))
        //    using (var csvFile = new CsvReader(reader, CultureInfo.InvariantCulture))
        //    {
        //        var records = csvFile.GetRecords<WallTempLength>().ToList();
        //        //this.Level.TempLengths = new ObservableCollection<WallTempLength>(records);
        //    }
        //}

        /// <summary>
        /// The load data length.
        /// </summary>
        //private void LoadDataLength()
        //{
        //    //if (this.Level.WallLayers.Count <= 0 || this.Level.TempLengths.Count <= 0)
        //    //{
        //    //    return;
        //    //}
        //    //foreach (var wallLayer in this.Level.WallLayers)
        //    //{
        //    //    WallTempLength first = null;
        //    //    foreach (var x in this.Level.TempLengths)
        //    //    {
        //    //        if (x.Id != wallLayer.WallColorLayer.Name)
        //    //        {
        //    //            continue;
        //    //        }

        //    //        first = x;
        //    //        break;
        //    //    }

        //    //    if (first == null)
        //    //    {
        //    //        continue;
        //    //    }

        //    //    var length =
        //    //        first.Length;

        //    //    //wallLayer.TempLength = length / 1000;
        //    //}
        //}

        /// <summary>
        /// The on add new wall row.
        /// </summary>
        private void OnAddNewWallRow()
        {
            this._startItemId = this.Level.WallLayers.Count + 1;
            var data = WallLayerFactory.CreateWallLayer(JobModel.Info.ClientName, _startItemId, this.Level.LevelInfo,
                this.SelectedClient.WallTypes[0]);
            this.Level.WallLayers.Add(data);
            //SelectedWall = data;
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
                var tempData = new List<WallBase>(this.Level.WallLayers);
                tempData.Sort();
                this.Level.WallLayers = new ObservableCollection<WallBase>(tempData);

                // re Order ID
                var startId = 1;
                foreach (var wallLayer in this.Level.WallLayers)
                {
                    wallLayer.Id = startId;
                    startId++;
                }
            }
        }

        #endregion


    }
}
