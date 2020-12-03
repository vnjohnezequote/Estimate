using AppModels.CustomEntity;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.ResponsiveData.Openings;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.ViewModelEntity
{
    public class EntityContainhangerAndOutTriggerVm: FramingVm,IFraming2DContainHangerAndOutTriggerVm
    {
        public Hanger2D HangerA
        {
            get
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger framing)
                {
                    if (framing.HangerAId == null)
                    {
                        return null;
                    }
                    foreach (var entity in EntitiesManager.Entities)
                    {
                        if (entity is Hanger2D hanger)
                        {
                            if (hanger.Id == framing.HangerA.Id)
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

                if (Entity is IFraming2DContaintHangerAndOutTrigger framing)
                {
                    if (framing.HangerBId == null)
                    {
                        return null;
                    }
                    foreach (var entity in EntitiesManager.Entities)
                    {
                        if (entity is Hanger2D hanger)
                        {
                            if (hanger.Id == framing.HangerBId)
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
                if (Entity is IFraming2DContaintHangerAndOutTrigger framing)
                {
                    return framing.IsHangerA;
                }

                return false;
            }
            set
            {
                if (!(Entity is IFraming2DContaintHangerAndOutTrigger framing)) return;
                framing.IsHangerA = value;
                if (value)
                {
                    if (this.HangerA == null)
                    {
                        var hangerRef = new Hanger(framing.FramingReference.FramingSheet);
                        var hanger = new Hanger2D(framing.StartPoint, "H", 140, hangerRef)
                        {
                            Alignment = Text.alignmentType.MiddleCenter,
                            Color = System.Drawing.Color.CadetBlue,
                            ColorMethod = colorMethodType.byEntity
                        };
                        framing.HangerAId = hanger.Id;
                        framing.HangerA = hanger;
                        ((IContaintHanger)framing.FramingReference).HangerA = hangerRef;
                        framing.FramingReference.FramingSheet.Hangers.Add(hangerRef);
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
                    framing.HangerAId = null;
                    framing.HangerA = null;
                    framing.FramingReference.FramingSheet.Hangers.Remove(((IContaintHanger)(framing.FramingReference)).HangerA);
                    ((IContaintHanger)(framing.FramingReference)).HangerA = null;
                }
                RaisePropertyChanged(nameof(IsHangerA));
            }
        }
        public bool IsHangerB
        {
            get
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger framing)
                {
                    return framing.IsHangerB;
                }

                return false;
            }
            set
            {
                if (!(Entity is IFraming2DContaintHangerAndOutTrigger framing)) return;
                framing.IsHangerB = value;
                if (value)
                {
                    if (this.HangerB == null)
                    {
                        var hangerRef = new Hanger(framing.FramingReference.FramingSheet);
                        var hanger = new Hanger2D(framing.EndPoint, "H", 140, hangerRef)
                        {
                            Alignment = Text.alignmentType.MiddleCenter,
                            Color = System.Drawing.Color.CadetBlue,
                            ColorMethod = colorMethodType.byEntity
                        };
                        framing.HangerBId = hanger.Id;
                        framing.HangerB = hanger;
                        ((IContaintHanger)framing.FramingReference).HangerB = hangerRef;
                        framing.FramingReference.FramingSheet.Hangers.Add(hangerRef);
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
                    framing.HangerBId = null;
                    framing.HangerB = null;
                    framing.FramingReference.FramingSheet.Hangers.Remove(((IContaintHanger)(framing.FramingReference)).HangerB);
                    ((IContaintHanger)(framing.FramingReference)).HangerB = null;
                }
                RaisePropertyChanged(nameof(IsHangerB));
            }
        }
        public HangerMat HangerAMat
        {
            get
            {
                if (HangerA==null)
                {
                    return null;
                }
                return ((Hanger) HangerA?.FramingReference)?.HangerMaterial;
            }
            set
            {
                if (!(Entity is IFraming2DContaintHangerAndOutTrigger framing)) return;
                if (framing.FramingReference == null) return;
                if (HangerA != null)
                {
                   ((Hanger) HangerA.FramingReference).HangerMaterial = value;
                }
                RaisePropertyChanged(nameof(HangerAMat));
            }
        }
        public HangerMat HangerBMat
        {
            get
            {
                if (HangerB== null)
                {
                    return null;
                }
                return ((Hanger) HangerB.FramingReference)?.HangerMaterial;
            }
            set
            {
                if (!(Entity is IFraming2DContaintHangerAndOutTrigger framing)) return;
                if (framing.FramingReference == null) return;
                if (HangerA != null)
                {
                    ((Hanger)HangerB.FramingReference).HangerMaterial = value;
                }
                RaisePropertyChanged(nameof(HangerAMat));
            }
        }
        public OutTrigger2D OutTriggerA
        {
            get
            {
                switch (Entity)
                {
                    case IFraming2DContaintHangerAndOutTrigger framing:
                    {
                        foreach (var entity in EntitiesManager.Entities)
                        {
                            if (!(entity is OutTrigger2D outTrigger)) continue;
                            if (outTrigger.Id==framing.OutTriggerAId)
                            {
                                return outTrigger;
                            }
                        }

                        break;
                    }
                }

                return null;
            }
        }
        public OutTrigger2D OutTriggerB
        {
            get
            {
                switch (Entity)
                {
                    case IFraming2DContaintHangerAndOutTrigger framing:
                    {
                        foreach (var entity in EntitiesManager.Entities)
                        {
                            if (!(entity is OutTrigger2D outTrigger)) continue;
                            if (outTrigger.Id==framing.OutTriggerBId)
                            {
                                return outTrigger;
                            }
                        }

                        break;
                    }
                }

                return null;
            }
        }
        public bool IsOutriggerA
        {
            get
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger framing)
                {
                    return framing.IsOutTriggerA;
                }

                return false;
            }
            set
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger framing)
                {
                    framing.IsOutTriggerA = value;
                    if (value)
                    {
                        if (this.OutTriggerA == null)
                        {
                            Utility.OffsetPoint(framing.OuterEndPoint, framing.OuterStartPoint, 450, out var startPoint);
                            Utility.OffsetPoint(startPoint, framing.OuterStartPoint, 900, out var endPoint);
                            var outTrigger = new OutTrigger(framing.FramingReference);
                            var outTriggerEntity = new OutTrigger2D(startPoint, endPoint, outTrigger, 35);
                            outTrigger.FullLength = (int)outTriggerEntity.FullLength;
                            outTriggerEntity.Color = System.Drawing.Color.Brown;
                            outTriggerEntity.ColorMethod = colorMethodType.byEntity;
                            framing.OutTriggerAId = outTriggerEntity.Id;
                            framing.OutTriggerA = outTriggerEntity;
                            ((IContaintOutTrigger)framing.FramingReference).OutTriggerA = outTrigger;
                            framing.FramingReference.FramingSheet.OutTriggers.Add(outTrigger);
                            this.EntitiesManager.AddAndRefresh(outTriggerEntity, this.LayerName);
                            RaisePropertyChanged(nameof(OutTriggerA));
                        }
                    }
                    else
                    {
                        this.OutTriggerAFlipped = false;
                        if (this.OutTriggerA != null)
                        {
                            this.EntitiesManager.Entities.Remove(OutTriggerA);
                        }
                        framing.OutTriggerAId = null;
                        framing.OutTriggerA = null;
                        framing.FramingReference.FramingSheet.OutTriggers.Remove(((IContaintOutTrigger)framing.FramingReference).OutTriggerA);
                        ((IContaintOutTrigger)framing.FramingReference).OutTriggerA = null;
                        

                    }
                    RaisePropertyChanged(nameof(IsOutriggerA));
                }
            }
        }
        public bool IsOutriggerB
        {
            get
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger framing)
                {
                    return framing.IsOutTriggerB;
                }

                return false;
            }
            set
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger framing)
                {
                    framing.IsOutTriggerB = value;
                    if (value)
                    {
                        if (this.OutTriggerB == null)
                        {
                            Utility.OffsetPoint(framing.OuterStartPoint, framing.OuterEndPoint, 450, out var startPoint);
                            Utility.OffsetPoint(startPoint, framing.OuterEndPoint, 900, out var endPoint);
                            var outTrigger = new OutTrigger(framing.FramingReference);
                            var outTriggerEntity = new OutTrigger2D(startPoint, endPoint, outTrigger,  35,false);
                            outTrigger.FullLength = (int)outTriggerEntity.FullLength;
                            outTriggerEntity.Color = System.Drawing.Color.Brown;
                            outTriggerEntity.ColorMethod = colorMethodType.byEntity;
                            framing.OutTriggerBId = outTriggerEntity.Id;
                            framing.OutTriggerB = outTriggerEntity;
                            ((IContaintOutTrigger)framing.FramingReference).OutTriggerB = outTrigger;
                            framing.FramingReference.FramingSheet.OutTriggers.Add(outTrigger);
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

                        OutTriggerBFlipped = false;
                        framing.OutTriggerBId = null;
                        framing.OutTriggerB = null;
                        framing.FramingReference.FramingSheet.OutTriggers.Remove(((IContaintOutTrigger)framing.FramingReference).OutTriggerB);
                        ((IContaintOutTrigger)framing.FramingReference).OutTriggerB = null;
                        
                    }
                    RaisePropertyChanged(nameof(IsOutriggerB));
                }
            }
        }
        public bool OutTriggerAFlipped
        {
            get
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger framing)
                {
                    return framing.OutTriggerAFlipped;
                }

                return false;
            }
            set
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger framing)
                {
                    framing.OutTriggerAFlipped = value;
                    RaisePropertyChanged(nameof(OutTriggerAFlipped));
                }
            }
        }
        public bool OutTriggerBFlipped
        {
            get
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger framing)
                {
                    return framing.OutTriggerBFlipped;
                }

                return false;
            }
            set
            {
                if (!(Entity is IFraming2DContaintHangerAndOutTrigger framing)) return;
                framing.OutTriggerBFlipped = value;
                RaisePropertyChanged(nameof(OutTriggerBFlipped));
            }
        }
        public TimberBase OutTriggerAMat
        {
            get
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger && OutTriggerA != null)
                {
                    return OutTriggerA.FramingReference.FramingInfo;
                }

                return null;
            }
            set
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger && OutTriggerA != null)
                {
                    OutTriggerA.FramingReference.FramingInfo = value;
                    RaisePropertyChanged(nameof(OutTriggerAMat));
                }
            }

        }
        public TimberBase OutTriggerBMat
        {
            get
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger && OutTriggerB != null)
                {
                    return OutTriggerB.FramingReference.FramingInfo;
                }

                return null;
            }
            set
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger && OutTriggerB != null)
                {
                    OutTriggerB.FramingReference.FramingInfo = value;
                    RaisePropertyChanged(nameof(OutTriggerBMat));
                }
            }
        }
        public string OutTriggerAGrade
        {
            get
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger && OutTriggerA != null)
                {
                    return OutTriggerA.FramingReference.TimberGrade;
                }

                return string.Empty;
            }
            set
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger && OutTriggerA != null)
                {
                    OutTriggerA.FramingReference.TimberGrade = value;
                    RaisePropertyChanged(nameof(OutTriggerAGrade));
                }
            }
        }
        public string OutTriggerBGrade
        {
            get
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger && OutTriggerB != null)
                {
                    return OutTriggerB.FramingReference.TimberGrade;
                }

                return string.Empty;
            }
            set
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger && OutTriggerB != null)
                {
                    OutTriggerB.FramingReference.TimberGrade = value;
                    RaisePropertyChanged(nameof(OutTriggerBGrade));
                }
            }
        }
        public int OutTriggerAOutSize
        {
            get
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger)
                {
                    if (OutTriggerA != null)
                    {
                        return OutTriggerA.OutSizeLength;
                    }
                }

                return 0;
            }
            set
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger)
                {
                    if (OutTriggerA != null)
                    {
                        OutTriggerA.OutSizeLength = value;
                        RaisePropertyChanged(nameof(OutTriggerAOutSize));
                    }
                }
            }
        }
        public int OutTriggerBOutSize
        {
            get
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger)
                {
                    if (OutTriggerB != null)
                    {
                        return OutTriggerB.OutSizeLength;
                    }
                }

                return 0;
            }
            set
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger)
                {
                    if (OutTriggerB != null)
                    {
                        OutTriggerB.OutSizeLength = value;
                        RaisePropertyChanged(nameof(OutTriggerBOutSize));
                    }
                }
            }
        }
        public int OutTriggerAInSize
        {
            get
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger)
                {
                    if (OutTriggerA != null)
                    {
                        return OutTriggerA.InSizeLength;
                    }
                }

                return 0;
            }
            set
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger)
                {
                    if (OutTriggerA != null)
                    {
                        OutTriggerA.InSizeLength = value;
                        RaisePropertyChanged(nameof(OutTriggerAInSize));
                    }
                }
            }
        }
        public int OutTriggerBInSize
        {
            get
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger)
                {
                    if (OutTriggerB != null)
                    {
                        return OutTriggerB.InSizeLength;
                    }
                }
                return 0;
            }
            set
            {
                if (Entity is IFraming2DContaintHangerAndOutTrigger)
                {
                    if (OutTriggerB != null)
                    {
                        OutTriggerB.InSizeLength = value;
                        RaisePropertyChanged(nameof(OutTriggerBInSize));
                    }
                }
            }
        }
        public EntityContainhangerAndOutTriggerVm(Entity entity, IEntitiesManager entityManager) : base(entity, entityManager)
        {
        }
    }
}
