// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelWall.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the LevelWall type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AppModels
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;

    using AppModels.PocoDataModel;

    using Prism.Mvvm;

    /// <summary>
    /// The level wall.
    /// </summary>
    public class LevelWall : BindableBase
    {
        #region private field

        /// <summary>
        /// The _level name.
        /// </summary>
        private string levelName;

        /// <summary>
        /// The lintel lm.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private double? lintelLm;

        /// <summary>
        /// The _total wall length.
        /// </summary>
        private int totalWallLength;

        /// <summary>
        /// The _cost delivery.
        /// </summary>
        private int? costDelivery;

        /// <summary>
        /// The _roof beams.
        /// </summary>
        private ObservableCollection<Beam> roofBeams;

        /// <summary>
        /// The wall layers.
        /// </summary>
        private ObservableCollection<WallLayer> wallLayers;

        /// <summary>
        /// The _wall bracings.
        /// </summary>
        private ObservableCollection<Bracing> timberBracings;

        /// <summary>
        /// The openings.
        /// </summary>
        private ObservableCollection<Opening> openings;

        /// <summary>
        /// The level info.
        /// </summary>
        private LevelWallDefaultInfo levelInfo;

        /// <summary>
        /// The wall temp length.
        /// </summary>
        private ObservableCollection<WallTempLength> tempLength;
        
		private ObservableCollection<GenericBracing> generalBracing;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelWall"/> class.
        /// </summary>
        /// <param name="jobInfo">
        /// The job Info.
        /// </param>
        public LevelWall(JobWallDefaultInfo jobInfo)
        {
            this.LevelInfo = new LevelWallDefaultInfo(jobInfo);
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
            get => this.levelName;
            set => this.SetProperty(ref this.levelName, value);
        }

        /// <summary>
        /// Gets or sets the total wall length.
        /// </summary>
        public int TotalWallLength
        {
            get => this.totalWallLength;
            set => this.SetProperty(ref this.totalWallLength, value);
        }

        /// <summary>
        /// Gets or sets the lintel lm.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public double? LintelLm
        {
            get => this.lintelLm;
            set => this.SetProperty(ref this.lintelLm, value);
        }

        /// <summary>
        /// Gets or sets the layers.
        /// </summary>
        public ObservableCollection<WallLayer> WallLayers
        {
            get => this.wallLayers;
            set => this.SetProperty(ref this.wallLayers, value);
        }

        /// <summary>
        /// Gets or sets the temp length.
        /// </summary>
        public ObservableCollection<WallTempLength> TempLengths
        {
            get => this.tempLength;
            set => this.SetProperty(ref this.tempLength, value);
        }

        /// <summary>
        /// Gets or sets the wall bracings.
        /// </summary>
        public ObservableCollection<Bracing> TimberWallBracings
        {
            get => this.timberBracings;
            set => this.SetProperty(ref this.timberBracings, value);
        }
		
		public ObservableCollection<GenericBracing> GeneralBracings 
		{
			get => this.generalBracing;
			set=>this.SetProperty(ref this.generalBracing,value);
		}

        /// <summary>
        /// Gets or sets the cost delivery.
        /// </summary>
        public int? CostDelivery
        {
            get => this.costDelivery;
            set => this.SetProperty(ref this.costDelivery, value);
        }

        /// <summary>
        /// Gets or sets the roof beams.
        /// </summary>
        public ObservableCollection<Beam> RoofBeams
        {
            get => this.roofBeams;
            set => this.SetProperty(ref this.roofBeams, value);
        }

        /// <summary>
        /// Gets or sets the opening.
        /// </summary>
        public ObservableCollection<Opening> Openings
        {
            get => this.openings;
            set => this.SetProperty(ref this.openings, value);
        }

        /// <summary>
        /// Gets or sets the level info.
        /// </summary>
        public LevelWallDefaultInfo LevelInfo
        {
            get => this.levelInfo;
            set => this.SetProperty(ref this.levelInfo, value);
        }

        #endregion
    }
}
