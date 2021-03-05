using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using AppModels.CustomEntity;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings;
using AppModels.ResponsiveData.Openings;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.EntityCreator
{
    public class HangerCreator
    {
        private FramingSheet _framingSheet;
        private IFraming2D _framing2D;
        private IFraming _framingReference;

        public HangerCreator(Point3D initPoint,IFraming2DContaintHangerAndOutTrigger framing,bool isCreateA = true)
        {
            _framingSheet = framing.FramingReference.FramingSheet;
            _framingReference = new Hanger(_framingSheet);
            _framing2D = new Hanger2D(initPoint, "H", 140,(Hanger) _framingReference,framing)
            {
                Alignment = Text.alignmentType.MiddleCenter,
                Color = Color.CadetBlue,
                ColorMethod =  colorMethodType.byEntity
            };
            if (isCreateA)
            {
                 framing.HangerAId =_framing2D.Id;
                 framing.HangerA = (Hanger2D)_framing2D;
                 ((IContaintHanger)framing.FramingReference).HangerA = (Hanger)_framingReference;
                 framing.IsHangerA = true;

            }
            else
            {
                 framing.HangerBId =_framing2D.Id;
                 framing.HangerB = (Hanger2D)_framing2D;
                 ((IContaintHanger)framing.FramingReference).HangerB = (Hanger)_framingReference;
                 framing.IsHangerB = true;
            }
        }

        public Hanger GetHanger()
        {
            return (Hanger)_framingReference;
        }

        public Hanger2D GetHanger2D()
        {
            return (Hanger2D)_framing2D;
        }
    }
}
