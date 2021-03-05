using System;
using System.Linq;
using AppModels.CustomEntity;
using AppModels.EntityCreator;
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
                if (value)
                {
                    if (this.HangerA == null)
                    {
                        var hangerController = new HangerControler(EntitiesManager, framing);
                        hangerController.AddHangerA();
                        RaisePropertyChanged(nameof(HangerA));
                    }
                }
                else
                {
                    var hangerControler = new HangerControler(EntitiesManager, framing);
                    hangerControler.RemoveHangerA();
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
                if (value)
                {
                    if (this.HangerB == null)
                    {
                        var hangerController = new HangerControler(EntitiesManager, framing);
                        hangerController.AddHangerB();
                        RaisePropertyChanged(nameof(HangerB));
                    }
                }
                else
                {
                    var hangerController = new HangerControler(EntitiesManager, framing);
                    hangerController.RemoveHangerB();
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
                    
                    if (value)
                    {
                        if (this.OutTriggerA == null)
                        {
                            var outTriggerController = new OutTriggerController(EntitiesManager, framing);
                            outTriggerController.AddOutriggerA();
                            RaisePropertyChanged(nameof(OutTriggerA));
                        }
                    }
                    else
                    {
                        var outTriggerController = new OutTriggerController(EntitiesManager, framing);
                        outTriggerController.RemoveOutTriggerA();
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
                    if (value)
                    {
                        if (this.OutTriggerB == null)
                        {
                            var outrTriggerController = new OutTriggerController(EntitiesManager, framing);
                            outrTriggerController.AddOutriggerB();
                            RaisePropertyChanged(nameof(OutTriggerB));
                        }
                    }
                    else
                    {
                        var outriggerController = new OutTriggerController(EntitiesManager, framing);
                        outriggerController.RemoveOutTriggerB();
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
        public FramingNameEntity FramingName
        {
            get
            {
                if (this.Entity is FramingRectangleContainHangerAndOutTrigger framing)
                {
                    return framing.FramingName;
                }

                return null;
            }
        }
        public bool IsShowFramingName
        {
            get
            {
                if (Entity is FramingRectangleContainHangerAndOutTrigger framing && framing.FramingReference != null)
                {
                    return framing.IsShowFramingName;
                }

                return false;
            }
            set
            {
                if (Entity is FramingRectangleContainHangerAndOutTrigger framing)
                {
                    framing.IsShowFramingName = value;
                    if (value)
                    {
                        if (framing.FramingReference != null && this.FramingName == null)
                        {
                            var p0 = framing.OuterStartPoint;
                            var p1 = framing.OuterEndPoint;
                            if (p0.X > p1.X)
                            {
                                p0 = framing.InnerStartPoint;
                                p1 = framing.InnerEndPoint;
                                //swapp
                                Utility.Swap(ref p0, ref p1);
                            }
                            else if (Math.Abs(p0.X - p1.X) < 0.00001 && p0.Y > p1.Y)
                            {
                                //swapp
                                p0 = framing.InnerStartPoint;
                                p1 = framing.InnerEndPoint;
                                Utility.Swap(ref p0, ref p1);
                            }

                            var framingBaseLine = new Segment2D(p0, p1);
                            framingBaseLine = framingBaseLine.Offset(-100);
                            var initPoint = framingBaseLine.MidPoint.ConvertPoint2DtoPoint3D();
                            var framingName = new FramingNameEntity(initPoint, framing.FramingReference.Name, 200,
                                Text.alignmentType.BaselineCenter, framing.FramingReference);
                            framing.FramingName = framingName;
                            framing.FramingNameId = framingName.Id;
                            Vector2D v = new Vector2D(p0, p1);
                            var radian = v.Angle;
                            framingName.Rotate(radian, Vector3D.AxisZ, framingName.InsertionPoint);
                            framingName.Color = this.Entity.Color;
                            framingName.ColorMethod = colorMethodType.byEntity;
                            EntitiesManager.AddAndRefresh(framingName, this.LayerName);

                        }
                    }
                    else
                    {
                        if (this.FramingName != null)
                        {
                            this.EntitiesManager.Entities.Remove(FramingName);
                        }

                        framing.FramingNameId = null;
                        framing.FramingName = null;
                    }
                    RaisePropertyChanged(nameof(IsShowFramingName));
                }
            }
        }
        public EntityContainhangerAndOutTriggerVm(Entity entity, IEntitiesManager entityManager) : base(entity, entityManager)
        {
        }
    }
}
