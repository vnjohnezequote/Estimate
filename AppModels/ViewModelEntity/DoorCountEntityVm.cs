using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Openings;
using devDept.Eyeshot.Entities;

namespace AppModels.ViewModelEntity
{
    public class DoorCountEntityVm: EntityVm
    {
        public string WallLevelName
        {
            get
            {
                if (this.Entity is DoorCountEntity door)
                {
                    return door.LevelName;
                }

                return string.Empty;
            }
            set
            {
                if (this.Entity is DoorCountEntity door)
                {
                    door.LevelName = value;
                    RaisePropertyChanged(nameof(WallLevelName));
                }
            }
        }

        public string OldLevel
        {
            get
            {
                if (Entity is DoorCountEntity door)
                {
                    return door.OldLevelName;
                }

                return string.Empty;
            }
        }

        public WallBase WallBelongTo
        {
            get
            {
                if (this.Entity is DoorCountEntity door)
                {
                    if (door.DoorReference!=null)
                    {
                        return door.DoorReference.WallReference;
                    }
                }

                return null;
            }
            set
            {
                if (this.Entity is DoorCountEntity door)
                {
                    if (door.DoorReference!=null)
                    {
                        door.DoorReference.WallReference = value;
                        door.DoorReference.WallReference.PropertyChanged += WallReference_PropertyChanged;
                        RaisePropertyChanged(nameof(WallBelongTo));
                        RaisePropertyChanged(nameof(LayerName));
                    }
                }

            }
        }

        public OpeningType? OpeningType
        {
            get
            {
                if (Entity is DoorCountEntity door)
                {
                    return door.DoorReference?.OpeningType;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (!(Entity is DoorCountEntity door)) return;
                if (door.DoorReference == null) return;
                if (value != null) door.DoorReference.OpeningType = (OpeningType) value;
                RaisePropertyChanged(nameof(OpeningType));
            }
            
        }

        public int DoorWidth
        {
            get
            {
                if(Entity is DoorCountEntity door)
                {
                    if (door.DoorReference!=null)
                    {
                        return door.DoorReference.Width;
                    }
                }

                return 0;
            }
            set
            {
                if (Entity is DoorCountEntity door)
                {
                    if (door.DoorReference != null)
                    {
                        door.DoorReference.Width = value;
                        RaisePropertyChanged(nameof(DoorWidth));
                    }
                }
            }
        }

        public int DoorHeight
        {
            get
            {
                if (Entity is DoorCountEntity door)
                {
                    if (door.DoorReference != null)
                    {
                        return door.DoorReference.Height;
                    }
                }

                return 0;
            }
            set
            {
                if (Entity is DoorCountEntity door)
                {
                    if (door.DoorReference != null)
                    {
                        door.DoorReference.Height = value;
                        RaisePropertyChanged(nameof(DoorHeight));
                    }
                }
            }
        }

        public int DoorSupportSpan
        {
            get
            {
                if (Entity is DoorCountEntity door)
                {
                    if (door.DoorReference != null)
                    {
                        return door.DoorReference.SupportSpan;
                    }
                }

                return 0;
            }
            set
            {
                if (Entity is DoorCountEntity door)
                {
                    if (door.DoorReference != null)
                    {
                        door.DoorReference.SupportSpan = value;
                        RaisePropertyChanged(nameof(DoorSupportSpan));
                    }
                }
            }
        }

        private void WallReference_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "WallColorLayer")
            {
                this.LayerName = WallBelongTo.WallColorLayer.Name;
            }
        }
        public override void NotifyPropertiesChanged()
        {
            base.NotifyPropertiesChanged();
            RaisePropertyChanged(nameof(WallLevelName));
        }

        public DoorCountEntityVm(Entity entity) : base(entity)
        {
        }

        
    }
}
