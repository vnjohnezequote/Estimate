﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.PocoDataModel.Framings.FloorAndRafter;
using AppModels.ResponsiveData.EngineerMember;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Framings
{
    public abstract class FramingBase : BindableBase, IFraming
    {
        #region Field
        private int _index;
        private string _name;
        private double _pitch;
        private bool _isExisting;
        private TimberBase _framingInfo;
        private int _framingSpan;
        private double _extraLength;
        private int _fullLength;
        private string _timberGrade;
        private FramingTypes _framingType;
        private int _quantitity;
        private EngineerMemberInfo _engineerMember;
        private int _subFixIndex;

        #endregion

        #region Properties
        public Guid Id { get; set; }
        public Guid LevelId { get; set; }
        public Guid FramingSheetId { get; set; }
        public int Index
        {
            get => _index;
            set
            {
                SetProperty(ref _index, value);
                RaisePropertyChanged(nameof(Name));
            } 
        }
        public FramingTypes FramingType
        {
            get => _framingType;
            set
            {
                SetProperty(ref _framingType, value);
                RaisePropertyChanged(nameof(Name));
            }
        }
        public virtual string NamePrefix
        {
            get
            {
                switch (FramingType)
                {
                    case FramingTypes.RafterBeam:
                        return "RB";
                    case FramingTypes.FloorBeam:
                        return "FB";
                    case FramingTypes.TrussBeam:
                        return "B";
                    case FramingTypes.RafterJoist:
                        if (FramingInfo != null && FramingInfo.NoItem == 2)
                        {
                            return "DR";
                        }
                        else
                            return "R";
                    case FramingTypes.FloorJoist:
                        if (FramingInfo != null && FramingInfo.NoItem == 2)
                        {
                            return "DJ";
                        }
                        else
                            return "FJ";
                    case FramingTypes.BoundaryJoist:
                        return "BJ";
                    case FramingTypes.CeilingJoist:
                        return "CJ";
                    case FramingTypes.Purlin:
                        return "PJ";
                    case FramingTypes.PolePlate:
                        return "PP";
                    case FramingTypes.Fascia:
                        return "FS";
                    case FramingTypes.TieDown:
                        return string.Empty;
                    case FramingTypes.Hanger:
                        return "H";
                    case FramingTypes.Trimmer:
                        return "TR";
                    case FramingTypes.Rimboard:
                        return "RJ";
                    case FramingTypes.HipRafter:
                        return "HR";
                    case FramingTypes.LintelBeam:
                        return "L";
                    case FramingTypes.Blocking:
                        return "B";
                    case FramingTypes.RidgeBeam:
                        return "RG";
                    case FramingTypes.RafterOutTrigger:
                        return "OR";
                    case FramingTypes.OutTrigger:
                        return "OJ";
                    case FramingTypes.DeckJoist:
                        return "DJ";
                    default:
                        return string.Empty;
                }
            }
        }
        public int SubFixIndex
        {
            get => _subFixIndex;
            set
            {
                SetProperty(ref _subFixIndex, value);
                RaisePropertyChanged(nameof(Name));
            }
        }
        public bool IsExisting { get => _isExisting; set => SetProperty(ref _isExisting, value); }
        public string Name
        {
            get
            {
                if (!string.IsNullOrEmpty(_name))
                {
                    return _name;
                }
                else
                {
                    if (SubFixIndex != 0)
                    {
                        return NamePrefix + Index + "." + SubFixIndex;
                    }

                    if (Index !=0)
                    {
                        return NamePrefix + Index;
                    }

                    return NamePrefix;

                }
            }
            set
            {
                if (value == Name)
                {
                    value = string.Empty;
                }
                SetProperty(ref _name, value);
            }
        }
        public int FramingSpan
        {
            get => _framingSpan;
            set
            {
                
                SetProperty(ref _framingSpan, value);
                RaisePropertyChanged(nameof(QuoteLength));
            }
        }
        public int FullLength
        {
            get => _fullLength;
            set
            {
                SetProperty(ref _fullLength, value);
                RaisePropertyChanged(nameof(QuoteLength));
            }
        }
        public double ExtraLength
        {
            get => _extraLength;
            set
            {
                SetProperty(ref _extraLength, value);
                RaisePropertyChanged(nameof(QuoteLength));
            }
        }
        public double Pitch
        {
            get => _pitch;
            set
            {
                SetProperty(ref _pitch, value);
                RaisePropertyChanged(nameof(QuoteLength));
            }
        }
        public abstract double QuoteLength { get; }
        public int Quantity { get => _quantitity; set => SetProperty(ref _quantitity, value); }
        public EngineerMemberInfo EngineerMember
        {
            get => _engineerMember;
            set
            {
                SetProperty(ref _engineerMember, value);
                if (value!=null && value.TimberInfo!=null)
                {
                    if (FramingInfo!=null)
                    {
                        FramingInfo = value.TimberInfo;
                    }
                    value.PropertyChanged += EngineerMemberPropertiesChanged;
                }
                
                RaisePropertyChanged(nameof(FramingInfo));
            }
        }
        public bool IsLongerStockList
        {
            get
            {
                if (FramingInfo != null && FramingInfo.MaximumLength < QuoteLength)
                {
                    return true;
                }

                return false;
            }
        }
        public string TimberGrade
        {
            get => _timberGrade;
            set
            {
                SetProperty(ref _timberGrade, value);
                RaisePropertyChanged(nameof(IsLongerStockList));
            }
        }
        public TimberBase FramingInfo
        {
            get => _framingInfo ?? EngineerMember?.TimberInfo;
            set
            {
                if (EngineerMember != null && value == EngineerMember.TimberInfo)
                {
                    value = null;
                }
                SetProperty(ref _framingInfo, value);
                RaisePropertyChanged(nameof(IsLongerStockList));
            }
        }
        public FramingSheet FramingSheet { get; set; }
        public LevelWall Level { get; set; }
        

        #endregion

        #region Constructor

        public FramingBase()
        {
            PropertyChanged += FramingBasePropertyChanged;
        }
        public FramingBase(FramingSheet framingSheet)
        {
            PropertyChanged += FramingBasePropertyChanged;
            Id = Guid.NewGuid();
            LevelId = framingSheet.LevelId;
            FramingSheetId = framingSheet.Id;
            FramingSheet = framingSheet;
            Level = framingSheet.Level;
            Quantity = 1;
            
        }
        protected FramingBase(FramingBase another)
        {
            PropertyChanged += FramingBasePropertyChanged;
            Id = Guid.NewGuid();
            LevelId = another.LevelId;
            FramingSheetId = another.FramingSheetId;
            FramingSheet = another.FramingSheet;
            Name = another.Name;
            IsExisting = another.IsExisting;
            FramingSpan = another.FramingSpan;
            FullLength = another.FullLength;
            ExtraLength = another.ExtraLength;
            Pitch = another.Pitch;
            FramingInfo = another.FramingInfo;
            TimberGrade = another.TimberGrade;
            FramingType = another.FramingType;
            Level = another.Level;
        }

        public FramingBase(FramingBasePoco framingPoco,LevelWall level, List<TimberBase> timberList,List<EngineerMemberInfo> engineerMembers)
        {
            PropertyChanged += FramingBasePropertyChanged;
            Id = framingPoco.Id;
            LevelId = framingPoco.LevelId;
            FramingSheetId = framingPoco.FramingSheetId;
            Index = framingPoco.Index;
            SubFixIndex = framingPoco.SubfixIndex;
            FramingType = framingPoco.FramingType;
            IsExisting = framingPoco.IsExisting;
            Name = framingPoco.Name;
            FramingSpan = framingPoco.FramingSpan;
            FullLength = framingPoco.FullLength;
            ExtraLength = framingPoco.ExtraLength;
            Pitch = framingPoco.Pitch;
            TimberGrade = framingPoco.TimberGrade;
            Level = level;
            EngineerMember = InitEngineerInfo(framingPoco,engineerMembers);
            FramingInfo = InitFramingInfo(framingPoco,timberList);
        }
        #endregion

        #region protected Method

       

       
        #endregion

        #region Public Method
        #endregion

        #region Private Methiod
        private void EngineerMemberPropertiesChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EngineerMember.TimberInfo))
            {
                RaisePropertyChanged(nameof(FramingInfo));
            }
        }
        private EngineerMemberInfo InitEngineerInfo(FramingBasePoco framingPoco,
            List<EngineerMemberInfo> engineerMembers)
        {
            if (engineerMembers==null)
            {
                return null;
            }
            foreach (var engineerMemberInfo in engineerMembers)
            {
                LoadExtraEngineerInfo(framingPoco, engineerMemberInfo);
                if (framingPoco.EngineerMemberId == engineerMemberInfo.Id)
                {
                    return EngineerMember = engineerMemberInfo;
                }
                
            }

            return null;
        }

        private TimberBase InitFramingInfo(FramingBasePoco framingPoco, List<TimberBase> timbersList)
        {
            if (timbersList==null)
            {
                return null;
            }
            foreach (var timberBase in timbersList)
            {
                if (framingPoco.FramingInfoId == timberBase.Id)
                {
                    return timberBase;
                }
            }

            return null;
        }

        protected virtual void LoadExtraEngineerInfo(FramingBasePoco framingPoco, EngineerMemberInfo engineerMemberInfo)
        {

        }
        protected virtual void FramingBasePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(QuoteLength):
                    RaisePropertyChanged(nameof(IsLongerStockList));
                    break;
            }

        }

        #endregion
        public abstract object Clone();
    }
}
