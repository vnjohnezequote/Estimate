using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using AppModels.Factories;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.ResponsiveData.Openings;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.ViewModelEntity
{
    public class Joist2dVm: EntityVmBase,ITimberVm
    {
        private bool _isHangerA;
        private bool _isHangerB;
        public TimberBase TimberInfo
        {
            get
            {
                if (Entity is Joist2D joist && joist.JoistReference != null)
                {
                    return joist.JoistReference.FramingInfo;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (Entity is Joist2D joist && joist.JoistReference!=null)
                {
                    joist.JoistReference.FramingInfo = value;
                    RaisePropertyChanged(nameof(TimberInfo));
                }
            }
        }
        public IEntitiesManager EntitiesManager { get; set; }
        public string BeamGrade
        {
            get
            {
                if (Entity is Joist2D joist)
                {
                    if (joist.JoistReference !=null)
                    {
                        return joist.JoistReference.TimberGrade;
                    }
                }

                return string.Empty;
            }
            set
            {
                if (Entity is Joist2D joist)
                {
                    if (joist.JoistReference!=null)
                    {
                        joist.JoistReference.TimberGrade = value;
                        RaisePropertyChanged(nameof(BeamGrade));
                    }
                }
            }
        }
        public double Pitch
        {
            get
            {
                if (Entity is Joist2D joist && joist.JoistReference != null)
                {
                    return joist.JoistReference.Pitch;
                }

                return 0;
            }
            set
            {
                if (Entity is Joist2D joist && joist.JoistReference != null)
                {
                    joist.JoistReference.Pitch = value;
                    if (OutTriggerA!=null)
                    {
                        OutTriggerA.OutTriggerReference.Pitch = value;
                    }
                    if (OutTriggerB != null)
                    {
                        OutTriggerB.OutTriggerReference.Pitch = value;
                    }
                    RaisePropertyChanged(nameof(Pitch));
                }
            }
        }
        public double ExtraLength
        {
            get
            {
                if (Entity is Joist2D joist && joist.JoistReference != null)
                {
                    return joist.JoistReference.ExtraLength;
                }

                return 0;

            }
            set
            {
                if (Entity is Joist2D joist && joist.JoistReference != null)
                {
                    joist.JoistReference.ExtraLength = value;
                    RaisePropertyChanged(nameof(ExtraLength));
                }
            }
        }
        public Hanger2D HangerA
        {
            get
            {
                if (Entity is Joist2D joist)
                {
                    foreach (var entity in EntitiesManager.Entities)
                    {
                        if (entity is Hanger2D hanger)
                        {
                            if (hanger.Id.ToString().Equals(joist.HangerAId))
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
                if (Entity is Joist2D joist)
                {
                    foreach (var entity in EntitiesManager.Entities)
                    {
                        if (entity is Hanger2D hanger)
                        {
                            if (hanger.Id.ToString().Equals(joist.HangerBId))
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
                if (Entity is Joist2D joist)
                {
                    return joist.IsHangerA;
                }

                return false;
            }
            set
            {
                if (Entity is Joist2D joist)
                {
                    joist.IsHangerA = value;
                    if (value == true)
                    {
                        if (this.HangerA==null)
                        {
                            var hangerRef = new Hanger();
                            var hanger = new Hanger2D(joist.StartPoint, "H", 140,hangerRef);
                            hanger.Alignment = Text.alignmentType.MiddleCenter;
                            hanger.Color = System.Drawing.Color.CadetBlue;
                            hanger.ColorMethod = colorMethodType.byEntity;
                            joist.HangerAId = hanger.Id.ToString();
                            joist.HangerA = hanger;
                            joist.JoistReference.HangerA = hangerRef;
                            joist.JoistReference.FloorSheet.Hangers.Add(hangerRef);
                            this.EntitiesManager.AddAndRefresh(hanger, this.LayerName);
                            RaisePropertyChanged(nameof(HangerA));
                        }
                    }
                    else
                    {
                        if (this.HangerA!=null)
                        {
                            this.EntitiesManager.Entities.Remove(HangerA);
                        }
                        joist.HangerAId = string.Empty;
                        HangerAMat = null;
                        joist.JoistReference.FloorSheet.Hangers.Remove(joist.JoistReference.HangerA);
                        joist.JoistReference.HangerA = null;
                    }
                    RaisePropertyChanged(nameof(IsHangerA));
                }
            }
        }
        public bool IsHangerB
        {
            get
            {
                if (Entity is Joist2D joist)
                {
                    return joist.IsHangerB;
                }

                return false;
            }
            set
            {
                if (Entity is Joist2D joist)
                {
                    joist.IsHangerB = value;
                    if (value)
                    {
                        if (this.HangerB == null)
                        {
                            var hangerRef = new Hanger();
                            var hanger = new Hanger2D(joist.EndPoint, "H", 140,hangerRef);
                            hanger.Alignment = Text.alignmentType.MiddleCenter;
                            hanger.Color = System.Drawing.Color.CadetBlue;
                            hanger.ColorMethod = colorMethodType.byEntity;
                            joist.HangerBId = hanger.Id.ToString();
                            joist.HangerB = hanger;
                            joist.JoistReference.HangerB = hangerRef;
                            joist.JoistReference.FloorSheet.Hangers.Add(hangerRef);
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

                        joist.HangerBId = string.Empty;
                        HangerBMat = null;
                        joist.JoistReference.FloorSheet.Hangers.Remove(joist.JoistReference.HangerB);
                        joist.JoistReference.HangerB = null;

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
                if (Entity is Joist2D joist)
                {
                    if (joist.JoistReference!=null)
                    {
                        if (HangerA!=null)
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
                if (!(Entity is Joist2D joist)) return;
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
                if (Entity is Joist2D joist)
                {
                    foreach (var entity in EntitiesManager.Entities)
                    {
                        if (entity is OutTrigger2D outTrigger)
                        {
                            if (outTrigger.Id.ToString().Equals(joist.OutTriggerAId))
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
                if (Entity is Joist2D joist)
                {
                    foreach (var entity in EntitiesManager.Entities)
                    {
                        if (entity is OutTrigger2D outTrigger)
                        {
                            if (outTrigger.Id.ToString().Equals(joist.OutTriggerBId))
                            {
                                return outTrigger;
                            }
                        }
                    }
                }

                return null;
            }
        }
        public bool IsOutriggerA {
            get
            {
                if (Entity is Joist2D joist)
                {
                    return joist.IsOutTriggerA;
                }

                return false;
            }
            set
            {
                if (Entity is Joist2D joist)
                {
                    joist.IsOutTriggerA = value;
                    if (value)
                    {
                        if (this.OutTriggerA == null)
                        {
                            Utility.OffsetPoint(joist.OuterEndPoint,joist.OuterStartPoint,  450, out var startPoint);
                            Utility.OffsetPoint(startPoint,joist.OuterStartPoint,900,out var endPoint);
                            var outTrigger = new OutTrigger();
                            outTrigger.FloorSheet = joist.JoistReference.FloorSheet;
                            outTrigger.SheetName = joist.SheetName;
                            var outTriggerEntity = new OutTrigger2D(Plane.XY, startPoint, endPoint, outTrigger,false ,35);
                            outTrigger.FramingSpan = outTriggerEntity.FullLength;
                            outTriggerEntity.Color = System.Drawing.Color.Brown;
                            outTriggerEntity.ColorMethod = colorMethodType.byEntity;
                            joist.OutTriggerAId = outTriggerEntity.Id.ToString();
                            joist.OutTriggerA = outTriggerEntity;
                            joist.JoistReference.OutTriggerA = outTrigger;
                            joist.JoistReference.FloorSheet.OutTriggers.Add(outTrigger);
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

                        joist.OutTriggerAId = string.Empty;
                        joist.OutTriggerA = null;
                        joist.JoistReference.FloorSheet.OutTriggers.Remove(joist.JoistReference.OutTriggerA);
                        joist.JoistReference.OutTriggerA = null;

                    }
                    RaisePropertyChanged(nameof(IsOutriggerA));
                }
            }
        }
        public bool IsOutriggerB {
            get
            {
                if (Entity is Joist2D joist)
                {
                    return joist.IsOutTriggerB;
                }

                return false;
            }
            set
            {
                if (Entity is Joist2D joist)
                {
                    joist.IsOutTriggerB = value;
                    if (value)
                    {
                        if (this.OutTriggerB == null)
                        {
                            Utility.OffsetPoint(joist.OuterStartPoint, joist.OuterEndPoint, 450, out var startPoint);
                            Utility.OffsetPoint(startPoint, joist.OuterEndPoint, 900, out var endPoint);
                            var outTrigger = new OutTrigger();
                            outTrigger.FloorSheet = joist.JoistReference.FloorSheet;
                            outTrigger.SheetName = joist.SheetName;
                            var outTriggerEntity = new OutTrigger2D(Plane.XY, startPoint, endPoint, outTrigger, true,35);
                            outTrigger.FramingSpan = outTriggerEntity.FullLength;
                            outTriggerEntity.Color = System.Drawing.Color.Brown;
                            outTriggerEntity.ColorMethod = colorMethodType.byEntity;
                            joist.OutTriggerBId = outTriggerEntity.Id.ToString();
                            joist.OutTriggerB = outTriggerEntity;
                            joist.JoistReference.FloorSheet.OutTriggers.Add(outTrigger);
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

                        joist.OutTriggerBId = string.Empty;
                        joist.JoistReference.FloorSheet.OutTriggers.Remove(joist.JoistReference.OutTriggerB);
                        joist.OutTriggerB = null;

                    }
                    RaisePropertyChanged(nameof(IsOutriggerB));
                }
            }
        }
        public bool OutTriggerAFlipped
        {
            get
            {
                if (Entity is Joist2D joist)
                {
                    return joist.OutTriggerAFlipped;
                }

                return false;
            }
            set
            {
                if (Entity is Joist2D joist)
                {
                    joist.OutTriggerAFlipped = value;
                    RaisePropertyChanged(nameof(OutTriggerAFlipped));
                }
            }
        }
        public bool OutTriggerBFlipped
        {
            get
            {
                if (Entity is Joist2D joist)
                {
                    return joist.OutTriggerBFlipped;
                }

                return false;
            }
            set
            {
                if (Entity is Joist2D joist)
                {
                    joist.OutTriggerBFlipped = value;
                    RaisePropertyChanged(nameof(OutTriggerBFlipped));
                }
            }
        }
        public TimberBase OutTriggerAMat
        {
            get
            {
                if (Entity is Joist2D joist && OutTriggerA!=null)
                {
                   return OutTriggerA.OutTriggerReference.FramingInfo;
                }

                return null;
            }
            set
            {
                if (Entity is Joist2D joist && OutTriggerA != null)
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
                if (Entity is Joist2D joist && OutTriggerB != null)
                {
                    return OutTriggerB.OutTriggerReference.FramingInfo;
                }

                return null;
            }
            set
            {
                if (Entity is Joist2D joist && OutTriggerB != null)
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
                if (Entity is Joist2D joist && OutTriggerA!=null)
                {
                    return OutTriggerA.OutTriggerReference.TimberGrade;
                }

                return string.Empty;
            }
            set
            {
                if (Entity is Joist2D joist && OutTriggerA != null)
                {
                    OutTriggerA.OutTriggerReference.TimberGrade = value;
                    RaisePropertyChanged(nameof(OutTriggerAGrade));
                }
            }
        }

        public string OutTriggerBGrade {
            get
            {
                if (Entity is Joist2D joist && OutTriggerB != null)
                {
                    return OutTriggerB.OutTriggerReference.TimberGrade;
                }

                return string.Empty;
            }
            set
            {
                if (Entity is Joist2D joist && OutTriggerB != null)
                {
                    OutTriggerB.OutTriggerReference.TimberGrade = value;
                    RaisePropertyChanged(nameof(OutTriggerBGrade));
                }
            }
        }
        public int OutTriggerAOutSize
        {
            get
            {
                if (Entity is Joist2D joist)
                {
                    if (OutTriggerA!=null)
                    {
                        return OutTriggerA.OutSizeLength;
                    }
                }

                return 0;
            }
            set
            {
                if (Entity is Joist2D joist)
                {
                    if (OutTriggerA!=null)
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
                if (Entity is Joist2D joist)
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
                if (Entity is Joist2D joist)
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
                if (Entity is Joist2D joist)
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
                if (Entity is Joist2D joist)
                {
                    if (OutTriggerA != null)
                    {
                        OutTriggerA.InSizeLength = value;
                        RaisePropertyChanged(nameof(OutTriggerAInSize));
                    }
                }
            }
        }
        public int OutTriggerBInSize {
            get
            {
                if (Entity is Joist2D joist)
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
                if (Entity is Joist2D joist)
                {
                    if (OutTriggerB != null)
                    {
                        OutTriggerB.InSizeLength = value;
                        RaisePropertyChanged(nameof(OutTriggerBInSize));
                    }
                }
            }
        }

        public double QuoteLength
        {
            get
            {
                if (Entity is Joist2D joist)
                {
                    return joist.JoistReference.QuoteLength;
                }

                return 0;
            }
        }

        public Joist2dVm(IEntity entity,IEntitiesManager entitiesManager) : base(entity)
        {
            EntitiesManager = entitiesManager;
        }
    }
}
