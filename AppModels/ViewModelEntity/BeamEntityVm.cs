using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using AppModels.Enums;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Openings;
using devDept.Eyeshot.Entities;

namespace AppModels.ViewModelEntity
{
    public class BeamEntityVm: EntityVm
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

        public TimberBase TimberInfo
        {
            get
            {
                if (Entity is BeamEntity beam && beam.BeamReference!=null)
                {
                    beam.BeamReference.PropertyChanged += BeamReference_PropertyChanged;
                    return beam.BeamReference.TimberInfo;

                }

                return null;

            }
            set
            {
                if (!(Entity is BeamEntity beam)) return;
                beam.BeamReference.TimberInfo = value;
                RaisePropertyChanged(nameof(TimberInfo));
            }
        }

        public EngineerMemberInfo EngineerMember
        {
            get
            {
                if (Entity is BeamEntity beam)
                {
                    if (beam.BeamReference != null)
                    {
                        return beam.BeamReference.EngineerMemberInfo;
                    }
                }

                return null;
            }
            set
            {
                if (Entity is BeamEntity beam)
                {
                    if (beam.BeamReference != null)
                    {
                        beam.BeamReference.EngineerMemberInfo = value;
                        RaisePropertyChanged(nameof(EngineerMember));
                        RaisePropertyChanged(nameof(TimberInfo));
                        RaisePropertyChanged(nameof(BeamGrade));
                    }
                }
            }
        }

        public Beam BeamReference
        {
            get
            {
                if (Entity is BeamEntity beam)
                {
                    return beam.BeamReference;
                }
                return null;
            }
        }

        public string BeamGrade
        {
            get
            {
                if (Entity is BeamEntity beam)
                {
                    if (beam.BeamReference!=null)
                    {
                        return beam.BeamReference.TimberGrade;
                    }
                }

                return string.Empty;
            }
            set
            {
                if (Entity is BeamEntity beam)
                {
                    if (beam.BeamReference != null)
                    {
                        beam.BeamReference.TimberGrade=value;
                        RaisePropertyChanged(nameof(BeamGrade));
                    }
                }
            }
        }

        public Text.alignmentType BeamMarkedAlignmentType
        {
            get
            {
                if (Entity is BeamEntity beam)
                {
                    return beam.Attributes["Name"].Alignment;
                    //return beam.BeamName.Alignment;
                }

                return Text.alignmentType.MiddleCenter;
            }
            set
            {
                if (Entity is BeamEntity beam)
                {
                    beam.Attributes["Name"].Alignment = value;
                    beam.Attributes["Continues"].Alignment = value;
                    //beam.BeamName.Alignment = value;
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

        public string OldLevelName
        {
            get
            {
                if (Entity is BeamEntity beam)
                {
                    return beam.OldLevelName;
                }

                return string.Empty;
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
                    return BeamMarkedLocation.top;
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
                if (Entity is BeamEntity beam )
                {
                    return beam.BeamLocation;
                }

                return string.Empty;
            }
            set
            {
                if(Entity is BeamEntity beam )
                {
                    beam.BeamLocation = value;
                    RaisePropertyChanged(nameof(BeamLocation));
                }
            }

        }

        public string BeamName
        {
            get
            {
                if (Entity is BeamEntity beam)
                {
                    return beam.BeamNameString;
                }

                return string.Empty;
            }
            set
            {
                if (Entity is BeamEntity beam)
                {
                    beam.BeamNameString = value;
                    RaisePropertyChanged(nameof(BeamName));
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
                    RaisePropertyChanged(nameof(BeamName));
                }
            }
        }

        public BeamType BeamType {

            get
            {
                if (Entity is BeamEntity beam)
                {
                    return beam.BeamType;
                }

                return BeamType.RoofBeam;
            }
            set
            {
                if (Entity is BeamEntity beam)
                {
                    beam.BeamType = value;
                    RaisePropertyChanged(nameof(BeamType));
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

        public string BlockName
        {
            get
            {
                if (Entity is BeamEntity beam)
                {
                    return beam.BlockName;
                }

                return string.Empty;
            }
            set
            {
                if (Entity is BeamEntity beam)
                {
                    beam.BeamBlock.Name = value;
                    beam.BlockName = value;
                    //RaisePropertyChanged(nameof(CustomNotesBeam));
                }
            }
        }
        public BeamEntityVm(Entity entity) : base(entity)
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
            RaisePropertyChanged(nameof(TimberInfo));
            RaisePropertyChanged(nameof(BeamLocation));
            RaisePropertyChanged(nameof(EngineerMember));
            RaisePropertyChanged(nameof(ShowBeamNameOnly));
            RaisePropertyChanged(nameof(BeamName));
            RaisePropertyChanged(nameof(BeamType));
        }
        private void BeamReference_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TimberGrade")
            {
                RaisePropertyChanged(nameof(BeamGrade));
            }

            if (e.PropertyName == "TimberInfo")
            {
                RaisePropertyChanged(nameof(TimberInfo));
            }

            if (e.PropertyName == "Location")
            {
                RaisePropertyChanged(nameof(BeamLocation));
            }

            if (e.PropertyName == "Name")
            {
                RaisePropertyChanged(nameof(BeamName));
            }

            if (e.PropertyName == "Type")
            {
                RaisePropertyChanged(nameof(BeamType));
                RaisePropertyChanged(nameof(BeamName));
            }
        }
    }
}
