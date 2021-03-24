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
        //public EngineerMemberInfo EngineerMember
        //{
        //    get
        //    {
        //        if (Entity is BeamEntity beam)
        //        {
        //            if (beam.FramingReference != null)
        //            {
        //                return beam.FramingReference.EngineerMember;
        //            }
        //        }

        //        return null;
        //    }
        //    set
        //    {
        //        if (Entity is BeamEntity beam)
        //        {
        //            if (beam.FramingReference != null)
        //            {
        //                beam.FramingReference.EngineerMember = value;
        //                RaisePropertyChanged(nameof(EngineerMember));
        //                RaisePropertyChanged(nameof(FramingInfo));
        //                RaisePropertyChanged(nameof(BeamGrade));
        //            }
        //        }
        //    }
        //}
        public FramingVm(Entity entity, IEntitiesManager entityManager) : base(entity, entityManager)
        {
            
        }

        
    }
}
