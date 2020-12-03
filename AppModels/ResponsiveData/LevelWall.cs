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
using AppModels.PocoDataModel.Framings;
using AppModels.PocoDataModel.Openings;
using AppModels.ResponsiveData.Framings;
using AppModels.ResponsiveData.Openings;
using Prism.Mvvm;

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
		private double _totalWallLength;

		/// <summary>
		/// The _cost delivery.
		/// </summary>
		private int _costDelivery;

		/// <summary>
		/// The _roof beams.
		/// </summary>
		private ObservableCollection<IFraming> _roofBeams;

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

		#region Property
		public Guid Id { get; set; }
		public string LevelNameInfo => LevelName + " Wall2D Infor";
		public string DoorNameInfo => LevelName + " Door Infor";
		public string LevelName
		{
			get => this._levelName;
			set => this.SetProperty(ref this._levelName, value);
		}
		public double TotalWallLength
		{
			get => this._totalWallLength;
			set => this.SetProperty(ref this._totalWallLength, value);
		}
        public double LintelLm
		{
			get => this._lintelLm;
			set => this.SetProperty(ref this._lintelLm, value);
		}
        public ObservableCollection<WallBase> WallLayers
		{
			get => this._wallLayers;
			set => this.SetProperty(ref this._wallLayers, value);
		}
        public ObservableCollection<Bracing> TimberWallBracings
		{
			get => this._timberBracings;
			set => this.SetProperty(ref this._timberBracings, value);
		}
		public ObservableCollection<GenericBracing> GeneralBracings
		{
			get => this._generalBracingList;
			set => this.SetProperty(ref this._generalBracingList, value);
		}
		public ObservableCollection<FramingSheet> FramingSheets { get; set; } = new ObservableCollection<FramingSheet>();
        public int CostDelivery
		{
			get => this._costDelivery;
			set => this.SetProperty(ref this._costDelivery, value);
		}
        public ObservableCollection<IFraming> RoofBeams
		{
			get => this._roofBeams;
			set => this.SetProperty(ref this._roofBeams, value);
		}
		public ObservableCollection<Opening> Openings
		{
			get => this._openings;
			set => this.SetProperty(ref this._openings, value);
		}
		public LevelGlobalWallInfo LevelInfo
		{
			get => this._levelInfo;
			set => this.SetProperty(ref this._levelInfo, value);
		}
		public ObservableCollection<LintelBeam> Lintels { get; private set; } = new ObservableCollection<LintelBeam>();

		#endregion

		#region Constructor
        public LevelWall(IGlobalWallInfo jobInfo)
		{
			Id = Guid.NewGuid();
			this.LevelInfo = new LevelGlobalWallInfo(jobInfo);
			this.WallLayers = new ObservableCollection<WallBase>();
			GeneralBracings = new ObservableCollection<GenericBracing>();
			this.TimberWallBracings = new ObservableCollection<Bracing>();
			this.RoofBeams = new ObservableCollection<IFraming>();
			this.Openings = new ObservableCollection<Opening>();
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

		

		#region Public Method

		public void LoadLevelInfo(LevelWallPoco level,ClientPoco client)
		{
            LevelName = level.LevelName;
			TotalWallLength = level.TotalWallLength;
			LintelLm = level.LintelLm;
			CostDelivery = level.CostDelivery;
			LevelInfo.LoadWallGlobalInfo(level.LevelInfo);
            this.InitializerWallLayers(level.WallLayers);
			InitializerRoofBeams(level.RoofBeams,client);
			//InitializerOpenings(level.Openings);
            this.GeneralBracings.AddRange(level.GeneralBracings);
            this.TimberWallBracings.AddRange(level.TimberWallBracings);
            LoadFramingSheets(level.FramingSheets, client);
        }
        private void Opening_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "IsDoorUnderLbw") return;
            _lintelColletionChangedEvent?.Invoke(sender, e);

            if (!(sender is Opening door)) return;
            if (door.IsDoorUnderLbw && !Lintels.Contains(door.Lintel))
            {
                Lintels.Add(door.Lintel);
                door.Lintel.Index = Lintels.IndexOf(door.Lintel) + 1;
            }
            else if (!door.IsDoorUnderLbw && Lintels.Contains(door.Lintel))
            {
                Lintels.Remove(door.Lintel);
                var i = 1;
                foreach (var lintelBeam in Lintels)
                {
                    door.Lintel.Index = i;
                    i++;
                }

            }
        }

		private void InitializerWallLayers(List<WallLayerPoco> wallLayers)
		{
			foreach (var wallLayerPoco in wallLayers)
			{
                var wallLayer = WallLayerFactory.CreateWallLayer(LevelInfo.GlobalInfo.Client.Name, wallLayerPoco.Id, LevelInfo, wallLayerPoco.WallType,LevelName);
				wallLayer.LoadWallInfo(wallLayerPoco);
				WallLayers.Add(wallLayer);
			}
		}

        private void LoadFramingSheets(List<FramingSheetPoco> framingSheets,ClientPoco client)
        {
            foreach (var framingSheetPoco in framingSheets)
            {
                var framingSheet = new FramingSheet(framingSheetPoco,this,client);
                //framingSheet.LoadFramingSheet(framingSheetPoco,client);
				FramingSheets.Add(framingSheet);
            }
        }
		private void InitializerRoofBeams(List<BeamPoco> roofBeams,ClientPoco client)
		{
			foreach (var roofBeam in roofBeams)
			{
                client.Beams.TryGetValue(roofBeam.TimberGrade, out var timberList);
                var rBeam = new Beam(roofBeam, this, timberList,
                    LevelInfo.GlobalInfo.JobModel.EngineerMemberList.ToList());
                rBeam.LoadWallSupportBeam(this.WallLayers.ToList(),roofBeam);
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
