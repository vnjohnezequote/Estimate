using AppModels.CustomEntity;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.ResponsiveData.Openings;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.ViewModelEntity
{
    public class Beam2DVm: EntityVmBase,ITimberVm
    {
        #region Field



        #endregion
        #region Properties

        public TimberBase TimberInfo
        {
            get
            {
                if (Entity is Beam2D beam && beam.BeamReference != null)
                {
                    return beam.BeamReference.TimberInfo;
                }

                return null;
            }
            set
            {
                if (!(Entity is Beam2D beam)) return;
                beam.BeamReference.TimberInfo = value;
                RaisePropertyChanged(nameof(TimberInfo));
            }
        }

        public string BeamGrade
        {
            get
            {
                if (Entity is Beam2D beam)
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
                if (Entity is Beam2D beam)
                {
                    if (beam.BeamReference!=null)
                    {
                        beam.BeamReference.TimberGrade = value;
                        RaisePropertyChanged(nameof(BeamGrade));
                    }
                }
            }
        }

        public bool BeamUnder
        {
            get
            {
                if (Entity is Beam2D beam)
                {
                    return beam.IsBeamUnder;
                }

                return false;
            }
            set
            {
                if (Entity is Beam2D beam)
                {
                    beam.IsBeamUnder = value;
                    RaisePropertyChanged(nameof(BeamUnder));
                    RaisePropertyChanged(nameof(Color));
                }
            }
        }

        public double Pitch
        {
            get
            {
                if (Entity is Beam2D beam && beam.BeamReference !=null)
                {
                    return beam.BeamReference.BeamPitch;
                }

                return 0;
            }
            set
            {
                if (Entity is Beam2D beam && beam.BeamReference!=null)
                {
                    beam.BeamReference.BeamPitch = value;
                    RaisePropertyChanged(nameof(Pitch));
                }
            }
        }

        public double ExtraLength
        {
            get
            {
                if (Entity is Beam2D beam && beam.BeamReference !=null)
                {
                    return beam.BeamReference.ExtraLength;
                }

                return 0;

            }
            set
            {
                if (Entity is Beam2D beam && beam.BeamReference!=null)
                {
                    beam.BeamReference.ExtraLength = value;
                    RaisePropertyChanged(nameof(ExtraLength));
                }
            }
        }
        public IEntitiesManager EntitiesManager { get; set; }
        public Hanger2D HangerA
        {
            get
            {
                if (Entity is Beam2D beam)
                {
                    foreach (var entity in EntitiesManager.Entities)
                    {
                        if (entity is Hanger2D hanger)
                        {
                            if (hanger.Id.ToString().Equals(beam.HangerAId))
                            {
                                return hanger;
                            }
                        }
                    }
                }
                return null;
            }
        }
        public Hanger2D HangerB
        {
            get
            {
                if (Entity is Beam2D beam)
                {
                    foreach (var entity in EntitiesManager.Entities)
                    {
                        if (entity is Hanger2D hanger)
                        {
                            if (hanger.Id.ToString().Equals(beam.HangerBId))
                            {
                                return hanger;
                            }
                        }
                    }
                }
                return null;
            }
        }
        public bool IsHangerA
        {
            get
            {
                if (Entity is Beam2D beam)
                {
                    return beam.IsHangerA;
                }

                return false;
            }
            set
            {
                if (Entity is Beam2D beam)
                {
                    beam.IsHangerA = value;
                    if (value == true)
                    {
                        if (this.HangerA == null)
                        {
                            var hangerRef = new Hanger();
                            var hanger = new Hanger2D(beam.StartPoint, "H", 140, hangerRef);
                            hanger.Alignment = Text.alignmentType.MiddleCenter;
                            hanger.Color = System.Drawing.Color.CadetBlue;
                            hanger.ColorMethod = colorMethodType.byEntity;
                            beam.HangerAId = hanger.Id.ToString();
                            beam.HangerA = hanger;
                            beam.BeamReference.HangerA = hangerRef;
                            beam.BeamReference.FloorSheet.Hangers.Add(hangerRef);
                            this.EntitiesManager.AddAndRefresh(hanger, this.LayerName);
                            RaisePropertyChanged(nameof(HangerA));
                        }
                    }
                    else
                    {
                        if (this.HangerA != null)
                        {
                            this.EntitiesManager.Entities.Remove(HangerA);
                        }
                        beam.HangerAId = string.Empty;
                        HangerAMat = null;
                        beam.BeamReference.FloorSheet.Hangers.Remove(beam.BeamReference.HangerA);
                        beam.BeamReference.HangerA = null;
                    }
                    RaisePropertyChanged(nameof(IsHangerA));
                }
            }
        }
        public bool IsHangerB
        {
            get
            {
                if (Entity is Beam2D beam)
                {
                    return beam.IsHangerB;
                }

                return false;
            }
            set
            {
                if (Entity is Beam2D beam)
                {
                    beam.IsHangerB = value;
                    if (value)
                    {
                        if (this.HangerB == null)
                        {
                            var hangerRef = new Hanger();
                            var hanger = new Hanger2D(beam.EndPoint, "H", 140, hangerRef);
                            hanger.Alignment = Text.alignmentType.MiddleCenter;
                            hanger.Color = System.Drawing.Color.CadetBlue;
                            hanger.ColorMethod = colorMethodType.byEntity;
                            beam.HangerBId = hanger.Id.ToString();
                            beam.HangerB = hanger;
                            beam.BeamReference.HangerB = hangerRef;
                            beam.BeamReference.FloorSheet.Hangers.Add(hangerRef);
                            this.EntitiesManager.AddAndRefresh(hanger, this.LayerName);
                            RaisePropertyChanged(nameof(HangerB));
                        }
                    }
                    else
                    {
                        if (this.HangerB != null)
                        {
                            this.EntitiesManager.Entities.Remove(HangerB);
                        }

                        beam.HangerBId = string.Empty;
                        HangerBMat = null;
                        beam.BeamReference.FloorSheet.Hangers.Remove(beam.BeamReference.HangerB);
                        beam.BeamReference.HangerB = null;

                    }
                    RaisePropertyChanged(nameof(IsHangerB));
                }
            }
        }
        public HangerMat HangerAMat
        {
            get => HangerA?.HangerReference.HangerMaterial;
            set
            {
                if (Entity is Beam2D beam)
                {
                    if (beam.BeamReference != null)
                    {
                        if (HangerA != null)
                        {
                            HangerA.HangerReference.HangerMaterial = value;
                        }
                        RaisePropertyChanged(nameof(HangerAMat));
                    }
                }
            }
        }
        public HangerMat HangerBMat
        {
            get => HangerB?.HangerReference.HangerMaterial;
            set
            {
                if (!(Entity is Beam2D beam)) return;
                if (HangerB != null)
                {
                    HangerB.HangerReference.HangerMaterial = value;
                }
                RaisePropertyChanged(nameof(HangerBMat));
            }
        }
        public OutTrigger2D OutTriggerA
        {
            get
            {
                if (Entity is Beam2D beam)
                {
                    foreach (var entity in EntitiesManager.Entities)
                    {
                        if (entity is OutTrigger2D outTrigger)
                        {
                            if (outTrigger.Id.ToString().Equals(beam.OutTriggerAId))
                            {
                                return outTrigger;
                            }
                        }
                    }
                }

                return null;
            }
        }
        public OutTrigger2D OutTriggerB
        {
            get
            {
                if (Entity is Beam2D beam)
                {
                    foreach (var entity in EntitiesManager.Entities)
                    {
                        if (entity is OutTrigger2D outTrigger)
                        {
                            if (outTrigger.Id.ToString().Equals(beam.OutTriggerBId))
                            {
                                return outTrigger;
                            }
                        }
                    }
                }

                return null;
            }
        }
        public bool IsOutriggerA
        {
            get
            {
                if (Entity is Beam2D beam)
                {
                    return beam.IsOutTriggerA;
                }

                return false;
            }
            set
            {
                if (Entity is Beam2D beam)
                {
                    beam.IsOutTriggerA = value;
                    if (value)
                    {
                        if (this.OutTriggerA == null)
                        {
                            Utility.OffsetPoint(beam.OuterEndPoint, beam.OuterStartPoint, 450, out var startPoint);
                            Utility.OffsetPoint(startPoint, beam.OuterStartPoint, 900, out var endPoint);
                            var outTrigger = new OutTrigger();
                            outTrigger.FloorSheet = beam.BeamReference.FloorSheet;
                            outTrigger.SheetName = beam.SheetName;
                            var outTriggerEntity = new OutTrigger2D(Plane.XY, startPoint, endPoint, outTrigger, false, 35);
                            outTrigger.FramingSpan = outTriggerEntity.FullLength;
                            outTriggerEntity.Color = System.Drawing.Color.Brown;
                            outTriggerEntity.ColorMethod = colorMethodType.byEntity;
                            beam.OutTriggerAId = outTriggerEntity.Id.ToString();
                            beam.OutTriggerA = outTriggerEntity;
                            beam.BeamReference.OutTriggerA = outTrigger;
                            beam.BeamReference.FloorSheet.OutTriggers.Add(outTrigger);
                            this.EntitiesManager.AddAndRefresh(outTriggerEntity, this.LayerName);
                            RaisePropertyChanged(nameof(OutTriggerA));
                        }
                    }
                    else
                    {
                        if (this.OutTriggerA != null)
                        {
                            this.EntitiesManager.Entities.Remove(OutTriggerA);
                        }

                        beam.OutTriggerAId = string.Empty;
                        beam.OutTriggerA = null;
                        beam.BeamReference.FloorSheet.OutTriggers.Remove(beam.BeamReference.OutTriggerA);
                        beam.BeamReference.OutTriggerA = null;

                    }
                    RaisePropertyChanged(nameof(IsOutriggerA));
                }
            }
        }
        public bool IsOutriggerB
        {
            get
            {
                if (Entity is Beam2D beam)
                {
                    return beam.IsOutTriggerB;
                }

                return false;
            }
            set
            {
                if (Entity is Beam2D beam)
                {
                    beam.IsOutTriggerB = value;
                    if (value)
                    {
                        if (this.OutTriggerB == null)
                        {
                            Utility.OffsetPoint(beam.OuterStartPoint, beam.OuterEndPoint, 450, out var startPoint);
                            Utility.OffsetPoint(startPoint, beam.OuterEndPoint, 900, out var endPoint);
                            var outTrigger = new OutTrigger();
                            outTrigger.FloorSheet = beam.BeamReference.FloorSheet;
                            outTrigger.SheetName = beam.SheetName;
                            var outTriggerEntity = new OutTrigger2D(Plane.XY, startPoint, endPoint, outTrigger, true, 35);
                            outTrigger.FramingSpan = outTriggerEntity.FullLength;
                            outTriggerEntity.Color = System.Drawing.Color.Brown;
                            outTriggerEntity.ColorMethod = colorMethodType.byEntity;
                            beam.OutTriggerBId = outTriggerEntity.Id.ToString();
                            beam.OutTriggerB = outTriggerEntity;
                            beam.BeamReference.FloorSheet.OutTriggers.Add(outTrigger);
                            this.EntitiesManager.AddAndRefresh(outTriggerEntity, this.LayerName);
                            RaisePropertyChanged(nameof(OutTriggerB));
                        }
                    }
                    else
                    {
                        if (this.OutTriggerB != null)
                        {
                            this.EntitiesManager.Entities.Remove(OutTriggerB);
                        }

                        beam.OutTriggerBId = string.Empty;
                        beam.BeamReference.FloorSheet.OutTriggers.Remove(beam.BeamReference.OutTriggerB);
                        beam.OutTriggerB = null;

                    }
                    RaisePropertyChanged(nameof(IsOutriggerB));
                }
            }
        }
        public bool OutTriggerAFlipped
        {
            get
            {
                if (Entity is Beam2D beam)
                {
                    return beam.OutTriggerAFlipped;
                }

                return false;
            }
            set
            {
                if (Entity is Beam2D beam)
                {
                    beam.OutTriggerAFlipped = value;
                    RaisePropertyChanged(nameof(OutTriggerAFlipped));
                }
            }
        }
        public bool OutTriggerBFlipped
        {
            get
            {
                if (Entity is Beam2D beam)
                {
                    return beam.OutTriggerBFlipped;
                }

                return false;
            }
            set
            {
                if (Entity is Beam2D beam)
                {
                    beam.OutTriggerBFlipped = value;
                    RaisePropertyChanged(nameof(OutTriggerBFlipped));
                }
            }
        }
        public TimberBase OutTriggerAMat
        {
            get
            {
                if (Entity is Beam2D && OutTriggerA != null)
                {
                    return OutTriggerA.OutTriggerReference.FramingInfo;
                }

                return null;
            }
            set
            {
                if (Entity is Beam2D && OutTriggerA != null)
                {
                    OutTriggerA.OutTriggerReference.FramingInfo = value;
                    RaisePropertyChanged(nameof(OutTriggerAMat));
                }
            }

        }
        public TimberBase OutTriggerBMat
        {
            get
            {
                if (Entity is Beam2D && OutTriggerB != null)
                {
                    return OutTriggerB.OutTriggerReference.FramingInfo;
                }

                return null;
            }
            set
            {
                if (Entity is Beam2D && OutTriggerB != null)
                {
                    OutTriggerB.OutTriggerReference.FramingInfo = value;
                    RaisePropertyChanged(nameof(OutTriggerBMat));
                }
            }
        }

        public string OutTriggerAGrade
        {
            get
            {
                if (Entity is Beam2D && OutTriggerA != null)
                {
                    return OutTriggerA.OutTriggerReference.TimberGrade;
                }

                return string.Empty;
            }
            set
            {
                if (Entity is Beam2D && OutTriggerA != null)
                {
                    OutTriggerA.OutTriggerReference.TimberGrade = value;
                    RaisePropertyChanged(nameof(OutTriggerAGrade));
                }
            }
        }

        public string OutTriggerBGrade
        {
            get
            {
                if (Entity is Beam2D && OutTriggerB != null)
                {
                    return OutTriggerB.OutTriggerReference.TimberGrade;
                }

                return string.Empty;
            }
            set
            {
                if (Entity is Beam2D && OutTriggerB != null)
                {
                    OutTriggerB.OutTriggerReference.TimberGrade = value;
                    RaisePropertyChanged(nameof(OutTriggerBGrade));
                }
            }
        }
        #endregion


        public Beam2DVm(Entity entity,IEntitiesManager entityManager) : base(entity)
        {
            this.EntitiesManager = entityManager;
        }

        public override void NotifyPropertiesChanged()
        {
            base.NotifyPropertiesChanged();
        }
    }
}