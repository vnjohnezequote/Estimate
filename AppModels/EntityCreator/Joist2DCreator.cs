using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using AppModels.EntityCreator.Interface;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Framings;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.ResponsiveData.Openings;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.EntityCreator
{
    public class Joist2DCreator: Framing2DContaintHangerAndOutTriggerCreator
    {
        // private FramingSheet _framingSheet;
        // private IFraming2D _framing2D;
        // private IFraming _framingReference;
        // private Hanger2D _hanger2DA;
        // private Hanger2D _hanger2DB;

        public Joist2DCreator(IFraming2D other, Point3D startPoint, Point3D endPoint, bool centerCreator = false) : base(other,startPoint,endPoint,centerCreator)
        {
           
        }

        public Joist2DCreator(FramingSheet framingSheet, Point3D startPoint, Point3D endPoint,
            FramingTypes framingType = FramingTypes.FloorJoist, TimberBase framingInfo = null,
            bool centerCreator = false, int thickness = 45): base(framingSheet,startPoint,endPoint,framingType,framingInfo,centerCreator,thickness,false)
        {
            
        }

        protected override IFraming CreateFramingReference(FramingTypes framingType,Point3D startPoint, Point3D endPoint,TimberBase framingInfo = null)
        {
            var framingSpan = (int)startPoint.DistanceTo(endPoint)-90;
            var framingRef = new Joist(_framingSheet)
            {
                Index = 1,
                FramingType = framingType
            };

            if (framingInfo!=null)
            {
                framingRef.FramingInfo = framingInfo;
            }
            framingRef.FramingSpan = framingSpan;
            framingRef.FullLength = (int) startPoint.DistanceTo(endPoint);
            return framingRef;
        }
         protected override IFraming2D CreateFraming2D(Point3D startPoint,Point3D endPoint,IFraming framingReference,int thickness=45, bool flipped = false,bool centerCreator = false,bool isUnder = false)
        {
            return new Joist2D((Point3D)startPoint.Clone(),(Point3D)endPoint.Clone(),framingReference,thickness,false,centerCreator);

        }



        // // public IFraming2D GetFraming2D() => _framing2D;
        // public IFraming GetFramingReference() => _framingReference;
        // public Hanger2D GetHangerA()
        // {
        //     return _hanger2DA;
        // }
        // public Hanger2D GetHangerB()
        // {
        //     return _hanger2DB;
        // }


    }
}
