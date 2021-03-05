using AppModels.CustomEntity;
using AppModels.EntityCreator.Interface;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Framings;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Beam;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using Prism;

namespace AppModels.EntityCreator
{
    
    public sealed class Beam2DCreator: Framing2DContaintHangerAndOutTriggerCreator
    {
        public Beam2DCreator(FramingSheet framingSheet, Point3D startPoint, Point3D endPoint,
            FramingTypes framingType = FramingTypes.FloorBeam, TimberBase framingInfo = null,
            bool centerCreator = false, int thickness = 45, bool isUnder = false) : base(framingSheet, startPoint,
            endPoint, framingType, framingInfo, centerCreator, thickness, isUnder)
        {

        }

        protected override IFraming2D CreateFraming2D(Point3D startPoint,Point3D endPoint,IFraming framingReference,int thickness, bool flipped,bool centerCretor = false,bool isUnder = false)
        {
            return new Beam2D(startPoint, endPoint, framingReference, thickness, false, centerCretor,isUnder);
        }

        protected override IFraming CreateFramingReference(FramingTypes framingType,Point3D startPoint,Point3D endPoint,TimberBase framingInfo = null)
        {
            var fullLength = (int) startPoint.DistanceTo(endPoint);
            var framingRef = new FBeam(_framingSheet);
            framingRef.Index = 1;
            framingRef.FramingType = framingType;
            framingRef.FullLength = fullLength;
            framingRef.FramingSpan = fullLength - 90;
            if (framingInfo != null)
            {
                framingRef.FramingInfo = framingInfo;
            }
            return framingRef;
        }
    }
}
