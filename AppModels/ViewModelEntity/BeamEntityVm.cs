using AppModels.CustomEntity;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using devDept.Eyeshot.Entities;

namespace AppModels.ViewModelEntity
{
    public class BeamEntityVm : FramingVm
    {
        public string ClientName
        {
            get
            {
                if (Entity is BeamEntity beam)
                {
                    return beam.ClientName;
                }

                return "";
            }
            set
            {
                if (!(Entity is BeamEntity beam)) return;
                beam.ClientName = value;
                RaisePropertyChanged(nameof(ClientName));
            }
        }

        public override TimberBase FramingInfo
        {
            get
            {
                if (Entity is BeamEntity beam && beam.FramingReference != null)
                {
                    beam.FramingReference.PropertyChanged += BeamReference_PropertyChanged;
                    return beam.FramingReference.FramingInfo;

                }

                return null;

            }
            set
            {
                if (!(Entity is BeamEntity beam)) return;
                beam.FramingReference.FramingInfo = value;
                RaisePropertyChanged(nameof(FramingInfo));
            }
        }
        public IFraming FramingReference
        {
            get
            {
                if (Entity is BeamEntity beam)
                {
                    return beam.FramingReference;
                }
                return null;
            }
        }
        public Text.alignmentType BeamMarkedAlignmentType
        {
            get
            {
                if (Entity is BeamEntity beam)
                {
                    return beam.Attributes["Name"].Alignment;
                    //return beam.Name.Alignment;
                }

                return Text.alignmentType.MiddleCenter;
            }
            set
            {
                if (Entity is BeamEntity beam)
                {
                    beam.Attributes["Name"].Alignment = value;
                    beam.BeamNameAttribute.Alignment = value;
                    beam.Attributes["Continues"].Alignment = value;
                    beam.Attributes["Support"].Alignment = value;
                    beam.Attributes["Treatment"].Alignment = value;
                    beam.Attributes["Custom"].Alignment = value;
                    beam.Attributes["Lintel"].Alignment = value;
                    //beam.Name.Alignment = value;
                    RaisePropertyChanged(nameof(BeamMarkedAlignmentType));
                }
            }
        }
        public string WallLevelName
        {
            get
            {
                if (Entity is BeamEntity beam)
                {
                    return beam.LevelName;
                }

                return string.Empty;
            }
            set
            {
                if (Entity is BeamEntity beam)
                {
                    beam.LevelName = value;
                    RaisePropertyChanged(nameof(WallLevelName));
                }
            }
        }
        public BeamMarkedLocation BeamMarkLocation
        {
            get
            {
                if (Entity is BeamEntity beam)
                {
                    return beam.BeamMarkedLocation;
                }
                else
                {
                    return BeamMarkedLocation.Top;
                }
            }
            set
            {
                if (Entity is BeamEntity beam)
                {
                    beam.BeamMarkedLocation = value;
                    RaisePropertyChanged(nameof(BeamMarkLocation));
                }
            }
        }
        public string BeamLocation
        {
            get
            {
                if (Entity is BeamEntity beam)
                {
                    return beam.BeamLocation;
                }

                return string.Empty;
            }
            set
            {
                if (Entity is BeamEntity beam)
                {
                    beam.BeamLocation = value;
                    RaisePropertyChanged(nameof(BeamLocation));
                }
            }

        }
        public bool ShowBeamNameOnly
        {
            get
            {
                if (Entity is BeamEntity beam)
                {
                    return beam.ShowBeamNameOnly;
                }

                return false;
            }
            set
            {
                if (Entity is BeamEntity beam)
                {
                    beam.ShowBeamNameOnly = value;
                    RaisePropertyChanged(nameof(Name));
                }
            }
        }
        public bool ContinuesBeam
        {
            get
            {
                if (Entity is BeamEntity beam)
                {
                    return beam.ContinuesBeam;
                }
                return false;
            }
            set
            {
                if (Entity is BeamEntity beam)
                {
                    beam.ContinuesBeam = value;
                    RaisePropertyChanged(nameof(ContinuesBeam));
                }
            }
        }
        public bool SupportWallOver
        {
            get
            {
                if (Entity is BeamEntity beam)
                {
                    return beam.SupportWallOver;
                }
                return false;
            }
            set
            {
                if (Entity is BeamEntity beam)
                {
                    beam.SupportWallOver = value;
                    RaisePropertyChanged(nameof(SupportWallOver));
                }
            }
        }
        public string CustomNotesBeam
        {
            get
            {
                if (Entity is BeamEntity beam)
                {
                    return beam.CustomAtrributeString;
                }

                return string.Empty;
            }
            set
            {
                if (Entity is BeamEntity beam)
                {
                    beam.CustomAtrributeString = value;
                    RaisePropertyChanged(nameof(CustomNotesBeam));
                }
            }
        }
        public BeamEntityVm(Entity entity,IEntitiesManager entitiesManager) : base(entity,entitiesManager)
        {
            
        }
        public override void NotifyPropertiesChanged()
        {
            base.NotifyPropertiesChanged();
            RaisePropertyChanged(nameof(ClientName));
            RaisePropertyChanged(nameof(BeamMarkedAlignmentType));
            RaisePropertyChanged(nameof(BeamMarkLocation));
            RaisePropertyChanged(nameof(BeamGrade));
            RaisePropertyChanged(nameof(WallLevelName));
            RaisePropertyChanged(nameof(FramingInfo));
            RaisePropertyChanged(nameof(BeamLocation));
            RaisePropertyChanged(nameof(EngineerMember));
            RaisePropertyChanged(nameof(ShowBeamNameOnly));
            RaisePropertyChanged(nameof(Name));
            RaisePropertyChanged(nameof(FramingType));
        }
        private void BeamReference_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TimberGrade")
            {
                RaisePropertyChanged(nameof(BeamGrade));
            }

            if (e.PropertyName == "FramingInfo")
            {
                RaisePropertyChanged(nameof(FramingInfo));
            }

            if (e.PropertyName == "Location")
            {
                RaisePropertyChanged(nameof(BeamLocation));
            }

            if (e.PropertyName == "Name")
            {
                RaisePropertyChanged(nameof(Name));
            }

            if (e.PropertyName == "FramingType")
            {
                RaisePropertyChanged(nameof(FramingType));
                RaisePropertyChanged(nameof(Name));
            }
        }

        
    }
}
