using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AppModels.Database;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.PocoDataModel.Framings;
using AppModels.PocoDataModel.Framings.Beams;
using AppModels.PocoDataModel.Framings.Blocking;
using AppModels.PocoDataModel.Framings.FloorAndRafter;
using AppModels.PocoDataModel.Openings;
using AppModels.ResponsiveData.Framings.Blocking;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Beam;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.ResponsiveData.Openings;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Framings
{
    public class FramingSheet : BindableBase
    {
        private int _index;
        private string _floorName;
        private bool _showSheetId;
        private int _framingSpacing;
        //private string _name;
        private FramingSheetTypes _framingSheetType;
        public LevelWall Level { get; }
        public Guid Id { get; set; }
        public Guid LevelId { get; set; }
        public int Index
        {
            get => _index;
            set
            {
                SetProperty(ref _index, value);
                if (Index == 0)
                {
                    this.ShowSheetId = false;
                }
                else
                {
                    ShowSheetId = true;
                }
                RaisePropertyChanged(nameof(Name));
            }
        }
        public int FramingSpacing
        {
            get => _framingSpacing;
            set => SetProperty(ref _framingSpacing, value);
        }
        public bool ShowSheetId
        {
            get => _showSheetId;
            set
            {
                SetProperty(ref _showSheetId, value);
                RaisePropertyChanged(nameof(Name));
            }
        }
        public string Name
        {
            get
            {
                //if (!string.IsNullOrEmpty(_name))
                //    return _name;
                if (ShowSheetId)
                {
                    switch (FramingSheetType)
                    {
                        case FramingSheetTypes.FloorFraming:
                            return "Floor - " + FloorName + " " + Index;
                        case FramingSheetTypes.RafterFraming:
                            return "Rafter - " + FloorName + " " + Index;
                        case FramingSheetTypes.PurlinFraming:
                            return "Purlin - " + FloorName + " " + Index;
                        case FramingSheetTypes.CeilingJoistFraming:
                            return "Ceiling Joist - " + FloorName + " " + Index;
                        default:
                            return FloorName + Index;
                    }

                }
                else
                {
                    switch (FramingSheetType)
                    {
                        case FramingSheetTypes.FloorFraming:
                            return "Floor - " + FloorName;
                        case FramingSheetTypes.RafterFraming:
                            return "Rafter - " + FloorName;
                        case FramingSheetTypes.PurlinFraming:
                            return "Purlin - " + FloorName;
                        case FramingSheetTypes.CeilingJoistFraming:
                            return "Ceiling Joist - " + FloorName;
                        default:
                            return FloorName;
                    }
                }
            }
            //set=>  SetProperty(ref _name, value);
        }
        public string FloorName
        {
            get => _floorName;
            set
            {
                SetProperty(ref _floorName, value);
                RaisePropertyChanged(nameof(Name));
            }
        }
        public FramingSheetTypes FramingSheetType
        {
            get => _framingSheetType;
            set

            {
                SetProperty(ref _framingSheetType, value);
                RaisePropertyChanged(nameof(Name));

            }
        }
        public ObservableCollection<IFraming> Joists { get; set; } = new ObservableCollection<IFraming>();
        public ObservableCollection<IFraming> Beams { get; set; } = new ObservableCollection<IFraming>();
        public ObservableCollection<IFraming> OutTriggers { get; set; } = new ObservableCollection<IFraming>();
        public ObservableCollection<Hanger> Hangers { get; set; } = new ObservableCollection<Hanger>();
        public ObservableCollection<IFraming> Blockings { get; set; } = new ObservableCollection<IFraming>();
        public ObservableCollection<TieDown> TieDowns { get; set; } = new ObservableCollection<TieDown>();
        public FramingSheet(LevelWall level)
        {
            Id = Guid.NewGuid();
            Level = level;
        }

        public FramingSheet(FramingSheetPoco framingPoco, LevelWall level, ClientPoco client)
        {
            Id = framingPoco.Id;
            Level = level;
            this.LoadFramingSheet(framingPoco, client);
        }

        public void LoadFramingSheet(FramingSheetPoco framingSheet, ClientPoco client)
        {
            Index = framingSheet.Index;
            Id = framingSheet.Id;
            LevelId = framingSheet.LevelId;
            FramingSpacing = framingSheet.FramingSpacing;
            FloorName = framingSheet.FloorName;
            FramingSheetType = framingSheet.FramingSheetType;
            ShowSheetId = framingSheet.ShowSheetId;
            this.LoadHangers(framingSheet.Hangers, client.Hangers);
            this.LoadOutTrigger(framingSheet.OutTriggers, client.TimberMaterialList);
            this.LoadJoists(framingSheet.Joists, client.TimberMaterialList);
            this.LoadBeams(framingSheet.Beams, client.TimberMaterialList);
            this.LoadBlockings(framingSheet.Blockings, client.TimberMaterialList);
            this.LoadTieDowns(framingSheet.TieDowns, client.TiewDownList);

        }
        private void LoadHangers(List<HangerPoco> hangers, List<HangerMat> hangerMats)
        {
            foreach (var hangerPoco in hangers)
            {
                HangerMat hangerMat = null;
                foreach (var mat in hangerMats)
                {
                    if (mat.ID == hangerPoco.HangerMatId)
                    {
                        hangerMat = mat;
                    }
                }
                var hanger = new Hanger(hangerPoco, this, hangerMat);
                this.Hangers.Add(hanger);
            }

        }
        private void LoadOutTrigger(List<OutTriggerPoco> outTriggers, List<TimberBase> timberList)
        {
            foreach (var outTriggerPoco in outTriggers)
            {
                var outTrigger = new OutTrigger(outTriggerPoco, this, timberList,
                    Level.LevelInfo.GlobalInfo.JobModel.EngineerMemberList.ToList());
                this.OutTriggers.Add(outTrigger);
            }
        }

        private void LoadJoists(List<JoistPoco> joists, List<TimberBase> timbersList)
        {
            foreach (var joistPoco in joists)
            {
                var joist = new Joist(joistPoco, this, timbersList, Level.LevelInfo.GlobalInfo.JobModel.EngineerMemberList.ToList());
                joist.LoadHangers(Hangers.ToList(), joistPoco);
                joist.LoadOutTriggers(OutTriggers.ToList(), joistPoco);
                Joists.Add(joist);
            }
        }
        private void LoadBeams(List<FBeamPoco> beams, List<TimberBase> timbersList)

        {
            foreach (var beamPoco in beams)
            {
                var fBeam = new FBeam(beamPoco, this, timbersList, Level.LevelInfo.GlobalInfo.JobModel.EngineerMemberList.ToList());
                fBeam.LoadHangers(Hangers.ToList(), beamPoco);
                fBeam.LoadOutTriggers(OutTriggers.ToList(), beamPoco);
                Beams.Add(fBeam);
            }
        }
        private void LoadBlockings(List<BlockingPoco> blockings, List<TimberBase> timberList)
        {
            foreach (var blockingPoco in blockings)
            {
                var blocking = new Blocking.Blocking(blockingPoco, this, timberList, Level.LevelInfo.GlobalInfo.JobModel.EngineerMemberList.ToList());
                Blockings.Add(blocking);
            }
        }
        private void LoadTieDowns(List<TieDownPoco> tiewDowns, List<TieDownMat> tiewDownList)
        {
            foreach (var tieDownPoco in tiewDowns)
            {
                TieDownMat tieDonwMat = null;
                foreach (var tieDown in tiewDownList)
                {
                    if (tieDown.Id == tieDownPoco.TieDownMatId)
                    {
                        tieDonwMat = tieDown;
                    }
                }

                var tieDownMember = new TieDown(tieDownPoco, tieDonwMat);
                TieDowns.Add(tieDownMember);
            }
        }
        public void OrderJoistList()
        {
            var orderedEnumerable = this.Joists.OrderBy(framing => framing.Name);
            var sortedList = orderedEnumerable.ToList();
            this.Joists.Clear();
            this.Joists.AddRange(sortedList);

        }
        public void OrderBeamList()
        {
            var orderedEnumerable = this.Beams.OrderBy(framing => framing.Name);
            var sortedList = orderedEnumerable.ToList();
            this.Beams.Clear();
            this.Beams.AddRange(sortedList);

        }
        public void OrderHangerList()
        {
            var orderedEnumerable = this.Hangers.OrderBy(framing => framing.Name);
            var sortedList = orderedEnumerable.ToList();
            this.Hangers.Clear();
            this.Hangers.AddRange(sortedList);

        }
        public void OrderBlockingList()
        {
            var orderedEnumerable = this.Blockings.OrderBy(framing => framing.Name);
            var sortedList = orderedEnumerable.ToList();
            this.Blockings.Clear();
            this.Blockings.AddRange(sortedList);

        }
        public void OrderOuttriggerList()
        {
            var orderedEnumerable = this.OutTriggers.OrderBy(framing => framing.Name);
            var sortedList = orderedEnumerable.ToList();
            this.OutTriggers.Clear();
            this.OutTriggers.AddRange(sortedList);

        }
        public void GeneralJoistName(Joist joist)
        {
            if (joist.FramingSheet.Joists.Count <= 0) return;
            var maximumSubFixIndex = 0;
            IFraming matchJoist = null;
            var matchingJoistLength = false;
            IFraming maxIndexJoist = null;
            var maximumIndex = 0;
            foreach (var existJoist in joist.FramingSheet.Joists)
            {
                if (existJoist.FramingType != joist.FramingType)
                {
                    continue;
                }
                if (existJoist.FramingInfo != null && joist.FramingInfo != null && existJoist.FramingInfo == joist.FramingInfo)
                {
                    matchJoist = existJoist;
                    // nếu giống vật liệu nhưng khác chiều dài thì tìm sub index lớn nhất
                    if (Math.Abs(existJoist.QuoteLength - joist.QuoteLength) > 0.001)
                    {
                        if (maximumIndex < existJoist.SubFixIndex)
                        {
                            maximumSubFixIndex = existJoist.SubFixIndex;
                        }
                    }
                    // nếu tìm thấy joist cùng vật liệu cùng chiều dài thì lập matchingJoistLength = true, thoát vòng lặp ko kiểm tra nữa.
                    else
                    {
                        matchingJoistLength = true;
                        break;
                    }
                }
                else
                {
                    if (existJoist.NamePrefix != joist.NamePrefix) continue;
                    maxIndexJoist = existJoist;

                    if (maximumIndex < existJoist.Index)
                    {
                        maximumIndex = existJoist.Index;
                    }

                }
            }

            //Nếu có joist giống vật liệu thì
            if (matchJoist != null)
            {
                if (matchingJoistLength)
                {
                    joist.Index = matchJoist.Index;
                    joist.SubFixIndex = matchJoist.SubFixIndex;
                }
                else
                {
                    joist.Index = matchJoist.Index;
                    joist.SubFixIndex = maximumSubFixIndex + 1;
                }
            }
            //Nếu không có joist giống vật liệu thì đăng joist index lên;
            else if (maxIndexJoist != null)
            {
                joist.Index = maximumIndex + 1;
            }

        }

    }
}
