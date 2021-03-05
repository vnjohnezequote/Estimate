using AppModels.CustomEntity;
using AppModels.EntityCreator.Interface;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Framings;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.EntityCreator
{
    public abstract class Framing2DContaintHangerAndOutTriggerCreator:IFramingRectangle2DCreator
    {
        protected FramingSheet _framingSheet;
        private IFraming2D _framing2D;
        private IFraming _framingReference;
        private Hanger2D _hanger2DA;
        private Hanger2D _hanger2DB;


        public Framing2DContaintHangerAndOutTriggerCreator(IFraming2D other,Point3D startPoint,Point3D endPoint,bool centerCreator = false)
        {
             _framingSheet = other.FramingReference.FramingSheet;
            //_framingReference = CreateFramingReference(other.FramingReference.FramingType,startPoint,endPoint,other.FramingReference.FramingInfo);
            _framingReference = other.FramingReference.Clone() as IFraming;
            if (_framingReference != null)
            {
                ((IFraming) _framingReference).FullLength = (int) startPoint.DistanceTo(endPoint);
                var thickness = ((FramingRectangle2D) other).Thickness;
                _framing2D = CreateFraming2D(startPoint,endPoint,_framingReference,thickness,false,centerCreator);
                if (other is IFraming2DContaintHangerAndOutTrigger framing)
                {
                    if (framing.IsHangerA)
                    {
                        var hangerCreator = new HangerCreator(((IFraming2DContaintHangerAndOutTrigger)_framing2D).HangerACenterPoint, (IFraming2DContaintHangerAndOutTrigger)_framing2D);
                        _hanger2DA = hangerCreator.GetHanger2D();
                    }

                    if (framing.IsHangerB)
                    {
                        var hangerCreator = new HangerCreator(((IFraming2DContaintHangerAndOutTrigger)_framing2D).HangerBCenterPoint,(IFraming2DContaintHangerAndOutTrigger)_framing2D, false);
                        _hanger2DB = hangerCreator.GetHanger2D();
                    }
                }
            }

            _framing2D.ColorMethod = colorMethodType.byEntity;

        }
        public Framing2DContaintHangerAndOutTriggerCreator(FramingSheet framingSheet, Point3D startPoint, Point3D endPoint,
            FramingTypes framingType = FramingTypes.FloorBeam, TimberBase framingInfo = null,
            bool centerCreator = false, int thickness = 45,bool isUnder = false)
        {
            _framingSheet = framingSheet;
            _framingReference = CreateFramingReference(framingType,startPoint,endPoint,framingInfo);
            if (framingInfo!=null)
            {
                thickness = framingInfo.NoItem * framingInfo.Depth;
            }

            _framing2D = CreateFraming2D(startPoint,endPoint,_framingReference,thickness,false,centerCreator,isUnder);
            _framing2D.ColorMethod = colorMethodType.byEntity;
        }

        protected abstract IFraming2D CreateFraming2D(Point3D startPoint, Point3D endPoint, IFraming framingReference,int thickness,bool flipped, bool centerCreator, bool isUnder = false);
        protected abstract IFraming CreateFramingReference(FramingTypes framingType,Point3D startPoint,Point3D endPoint,TimberBase framingInfo = null);
        public IFraming2D GetFraming2D()
        {
            return _framing2D;
        }
        public IFraming GetFramingReference()
        {
            return _framingReference;
        }

    }
}
