// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelWall.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the LevelWall type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using AppModels.Interaface;
using Prism.Mvvm;
using ProtoBuf;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The level wall.
    /// </summary>
    [ProtoContract]
    public class LevelWall : BindableBase
    {
        #region private field

        /// <summary>
        /// The _level name.
        /// </summary>
        private string _levelName;

        /// <summary>
        /// The lintel lm.
        /// </summary>
        private double? _lintelLm;

        /// <summary>
        /// The _total wall length.
        /// </summary>
        private int _totalWallLength;

        /// <summary>
        /// The _cost delivery.
        /// </summary>
        private int? _costDelivery;

        /// <summary>
        /// The _roof beams.
        /// </summary>
        private ObservableCollection<Beam> _roofBeams;

        /// <summary>
        /// The wall layers.
        /// </summary>
        private ObservableCollection<WallLayer> _wallLayers;

        /// <summary>
        /// The _wall bracings.
        /// </summary>
        private ObservableCollection<Bracing> _timberBracings;

        /// <summary>
        /// The openings.
        /// </summary>
        private ObservableCollection<Opening> _openings;

        /// <summary>
        /// The level info.
        /// </summary>
        private LevelGlobalWallInfo _levelInfo;

        /// <summary>
        /// The wall temp length.
        /// </summary>
        private ObservableCollection<WallTempLength> _tempLength;
        
		private ObservableCollection<GenericBracing> _generalBracing;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelWall"/> class.
        /// </summary>
        /// <param name="jobInfo">
        /// The job Info.
        /// </param>
        public LevelWall(IGlobalWallInfo jobInfo)
        {
            this.LevelInfo = new LevelGlobalWallInfo(jobInfo);
            this.WallLayers = new ObservableCollection<WallLayer>();
            this.TimberWallBracings = new ObservableCollection<Bracing>();
            this.RoofBeams = new ObservableCollection<Beam>();
            this.Openings = new ObservableCollection<Opening>();
            this.TempLengths = new ObservableCollection<WallTempLength>();
        }

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the level name.
        /// </summary>
        public string LevelName
        {
            get => this._levelName;
            set => this.SetProperty(ref this._levelName, value);
        }

        /// <summary>
        /// Gets or sets the total wall length.
        /// </summary>
        public int TotalWallLength
        {
            get => this._totalWallLength;
            set => this.SetProperty(ref this._totalWallLength, value);
        }

        /// <summary>
        /// Gets or sets the lintel lm.
        /// </summary>
        public double? LintelLm
        {
            get => this._lintelLm;
            set => this.SetProperty(ref this._lintelLm, value);
        }

        /// <summary>
        /// Gets or sets the layers.
        /// </summary>
        public ObservableCollection<WallLayer> WallLayers
        {
            get => this._wallLayers;
            set => this.SetProperty(ref this._wallLayers, value);
        }

        /// <summary>
        /// Gets or sets the temp length.
        /// </summary>
        public ObservableCollection<WallTempLength> TempLengths
        {
            get => this._tempLength;
            set => this.SetProperty(ref this._tempLength, value);
        }

        /// <summary>
        /// Gets or sets the wall bracings.
        /// </summary>
        public ObservableCollection<Bracing> TimberWallBracings
        {
            get => this._timberBracings;
            set => this.SetProperty(ref this._timberBracings, value);
        }
        public ObservableCollection<GenericBracing> GeneralBracings 
		{
			get => this._generalBracing;
			set=>this.SetProperty(ref this._generalBracing,value);
		}

        /// <summary>
        /// Gets or sets the cost delivery.
        /// </summary>
        public int? CostDelivery
        {
            get => this._costDelivery;
            set => this.SetProperty(ref this._costDelivery, value);
        }

        /// <summary>
        /// Gets or sets the roof beams.
        /// </summary>
        public ObservableCollection<Beam> RoofBeams
        {
            get => this._roofBeams;
            set => this.SetProperty(ref this._roofBeams, value);
        }

        /// <summary>
        /// Gets or sets the opening.
        /// </summary>
        public ObservableCollection<Opening> Openings
        {
            get => this._openings;
            set => this.SetProperty(ref this._openings, value);
        }

        /// <summary>
        /// Gets or sets the level info.
        /// </summary>
        public LevelGlobalWallInfo LevelInfo
        {
            get => this._levelInfo;
            set => this.SetProperty(ref this._levelInfo, value);
        }

        #endregion
    }
}
