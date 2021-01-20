using System;
using AppModels.CustomEntity;
using AppModels.Interaface;
using AppModels.ResponsiveData.EngineerMember;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.ViewModelEntity
{
    public class FramingVm: FramingVmBase,IFramingVm
    {
        public FramingNameEntity FramingName {
            get
            {
                if (this.Entity is FramingRectangle2D framing)
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
                if (Entity is FramingRectangle2D framing && framing.FramingReference!=null)
                {
                    return framing.IsShowFramingName;
                }

                return false;
            }
            set
            {
                if (Entity is FramingRectangle2D framing)
                {
                    framing.IsShowFramingName = value;
                    if (value)
                    {
                        if (framing.FramingReference!=null && this.FramingName==null)
                        {
                            var p0 = framing.OuterStartPoint;
                            var p1 = framing.OuterEndPoint;
                            if (p0.X>p1.X)
                            {
                                p0 = framing.InnerStartPoint;
                                p1 = framing.InnerEndPoint;
                                //swapp
                                Utility.Swap(ref p0, ref p1);
                            }
                            else if (Math.Abs(p0.X - p1.X) < 0.00001 &&p0.Y>p1.Y)
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
                            framingName.Rotate(radian,Vector3D.AxisZ,framingName.InsertionPoint);
                            framingName.Color = this.Entity.Color;
                                framingName.ColorMethod = colorMethodType.byEntity;
                            EntitiesManager.AddAndRefresh(framingName,this.LayerName);

                        }
                    }
                    else
                    {
                        if (this.FramingName!=null)
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
        public double Pitch
        {
            get
            {
                if (Entity is IFraming2D framing && framing.FramingReference != null)
                {
                    return framing.FramingReference.Pitch;
                }

                return 0;
            }
            set
            {
                if (Entity is IFraming2D framing && framing.FramingReference != null)
                {
                    framing.FramingReference.Pitch = value;
                    RaisePropertyChanged(nameof(Pitch));
                }
            }
        }
        public double ExtraLength
        {
            get
            {
                if (Entity is IFraming2D framing && framing.FramingReference != null)
                {
                    return framing.FramingReference.ExtraLength;
                }

                return 0;

            }
            set
            {
                if (Entity is IFraming2D framing && framing.FramingReference != null)
                {
                    framing.FramingReference.ExtraLength = value;
                    RaisePropertyChanged(nameof(ExtraLength));
                }
            }
        }
        public EngineerMemberInfo EngineerMember
        {
            get
            {
                if (Entity is BeamEntity beam)
                {
                    if (beam.FramingReference != null)
                    {
                        return beam.FramingReference.EngineerMember;
                    }
                }

                return null;
            }
            set
            {
                if (Entity is BeamEntity beam)
                {
                    if (beam.FramingReference != null)
                    {
                        beam.FramingReference.EngineerMember = value;
                        RaisePropertyChanged(nameof(EngineerMember));
                        RaisePropertyChanged(nameof(FramingInfo));
                        RaisePropertyChanged(nameof(BeamGrade));
                    }
                }
            }
        }
        public FramingVm(Entity entity, IEntitiesManager entityManager) : base(entity, entityManager)
        {
            
        }

        
    }
}
