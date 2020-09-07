// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelWall.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the LevelWall type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AppModels.Enums;
using AppModels.Factories;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.PocoDataModel.Openings;
using AppModels.PocoDataModel.WallMemberData;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Openings;
using Newtonsoft.Json;
using Prism.Mvvm;
using ProtoBuf;

namespace AppModels.ResponsiveData
{
	/// <summary>
	/// The level wall.
	/// </summary>
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
		private double _lintelLm;

		/// <summary>
		/// The _total wall length.
		/// </summary>
		private int _totalWallLength;

		/// <summary>
		/// The _cost delivery.
		/// </summary>
		private int _costDelivery;

		/// <summary>
		/// The _roof beams.
		/// </summary>
		private ObservableCollection<Beam> _roofBeams;

		/// <summary>
		/// The wall layers.
		/// </summary>
		private ObservableCollection<WallBase> _wallLayers;

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
		//private ObservableCollection<WallTempLength> _tempLength;
		
		private ObservableCollection<GenericBracing> _generalBracingList;

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
			this.WallLayers = new ObservableCollection<WallBase>();
			GeneralBracings = new ObservableCollection<GenericBracing>();
			this.TimberWallBracings = new ObservableCollection<Bracing>();
			this.RoofBeams = new ObservableCollection<Beam>();
			this.Openings = new ObservableCollection<Opening>();
            //this.TempLengths = new ObservableCollection<WallTempLength>();
		}

        

        public void AddOpening(Opening opening)
        {
            opening.PropertyChanged += Opening_PropertyChanged;
            Openings.Add(opening);
        }

        private event EventHandler _lintelColletionChangedEvent;
        public event EventHandler LintelCollectionChangedEvent
        {
            add => _lintelColletionChangedEvent += value;
            remove => _lintelColletionChangedEvent -= value;
        }

		#endregion

		#region Property

		public string LevelNameInfo => LevelName + " Wall Infor";

		public string DoorNameInfo => LevelName + " Door Infor";

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
		public double LintelLm
		{
			get => this._lintelLm;
			set => this.SetProperty(ref this._lintelLm, value);
		}

		/// <summary>
		/// Gets or sets the layers.
		/// </summary>
		public ObservableCollection<WallBase> WallLayers
		{
			get => this._wallLayers;
			set => this.SetProperty(ref this._wallLayers, value);
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
			get => this._generalBracingList;
			set=>this.SetProperty(ref this._generalBracingList,value);
		}
		//public ObservableCollection<Opening> DoorAndWindowList { get; }
		/// <summary>
		/// Gets or sets the cost delivery.
		/// </summary>
		public int CostDelivery
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
        public ObservableCollection<LintelBeam> Lintels { get; private set; } = new ObservableCollection<LintelBeam>();

		#endregion

		#region Public Method

		public void LoadLevelInfo(LevelWallPoco level,Dictionary<string,List<TimberBase>> timberInforDict)
		{
            LevelName = level.LevelName;
			TotalWallLength = level.TotalWallLength;
			LintelLm = level.LintelLm;
			CostDelivery = level.CostDelivery;
			LevelInfo.LoadWallGlobalInfo(level.LevelInfo);
            this.InitializerWallLayers(level.WallLayers);
			InitializerRoofBeams(level.RoofBeams,timberInforDict);
			InitializerOpenings(level.Openings);
            this.GeneralBracings.AddRange(level.GeneralBracings);
            this.TimberWallBracings.AddRange(level.TimberWallBracings);
            //this.InitializerTimberBracings(level.TimberWallBracings.ToList());
            //this.InitializerGeneralBracings(level.GeneralBracings.ToList());
            //this.InitializerRoofBeams(level.RoofBeams.ToList());
            //this.InitializerOpenings(level.Openings.ToList());
        }
        private void Opening_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "IsDoorUnderLbw") return;
            _lintelColletionChangedEvent?.Invoke(sender, e);

            if (!(sender is Opening door)) return;
            if (door.IsDoorUnderLbw && !Lintels.Contains(door.Lintel))
            {
                Lintels.Add(door.Lintel);
                door.Lintel.Id = Lintels.IndexOf(door.Lintel) + 1;
            }
            else if (!door.IsDoorUnderLbw && Lintels.Contains(door.Lintel))
            {
                Lintels.Remove(door.Lintel);
                var i = 1;
                foreach (var lintelBeam in Lintels)
                {
                    door.Lintel.Id = i;
                    i++;
                }

            }
        }

		private void InitializerWallLayers(List<WallLayerPoco> wallLayers)
		{
			foreach (var wallLayerPoco in wallLayers)
			{
				var wallLayer = WallLayerFactory.CreateWallLayer(LevelInfo.GlobalInfo.Client.Name, wallLayerPoco.Id, LevelInfo, wallLayerPoco.WallType);
				wallLayer.LoadWallInfo(wallLayerPoco);
				WallLayers.Add(wallLayer);
			}
		}

		private void InitializerRoofBeams(List<BeamPoco> roofBeams,Dictionary<string,List<TimberBase>> timberInfoDict)
		{
			foreach (var roofBeam in roofBeams)
			{
				var rBeam = new Beam(BeamType.RoofBeam,this.LevelInfo);
				rBeam.LoadBeamInfo(roofBeam,LevelInfo.GlobalInfo.JobModel.EngineerMemberList.ToList(),this.WallLayers.ToList(),timberInfoDict);
				RoofBeams.Add(rBeam);
			}
		}

		private void InitializerOpenings(List<OpeningPoco> openings)
		{
			foreach (var openingPoco in openings)
			{
				var opening = new Opening(LevelInfo);
				opening.LoadOpeningInfo(openingPoco,LevelInfo.GlobalInfo.JobModel.DoorSchedules.ToList(),this.WallLayers.ToList(),LevelInfo.GlobalInfo.JobModel.EngineerMemberList.ToList());
				this.AddOpening(opening);
                if (!opening.IsDoorUnderLbw) continue;
                if (!Lintels.Contains(opening.Lintel))
                {
                    Lintels.Add(opening.Lintel);
                }
            }
			
		}


		#endregion
	}
}
