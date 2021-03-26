using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AppModels.CustomEntity;
using AppModels.EntityCreator.Interface;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.EntityCreator
{
    public class OutTriggerCreator
    {
        private FramingSheet _framingSheet;
        private IFraming2D _framing2D;
        private IFraming _framingReference;
        public OutTriggerCreator(IFraming2DContaintHangerAndOutTrigger framing,Point3D startPoint,Point3D endPoint,bool isCreatorA = true, bool flipped = true, int thickness=35,int inSizeLength = 900,int outSizeLength=450)
        {
            _framingSheet = framing.FramingReference.FramingSheet;
            _framingReference = new OutTrigger(framing.FramingReference);
            _framing2D = new OutTrigger2D(startPoint, endPoint, (OutTrigger)_framingReference,framing, thickness, flipped,
                outSizeLength, inSizeLength);
            _framingReference.FullLength = (int)_framing2D.FullLength;
            _framing2D.Color = Color.FromArgb(255,127,0);
            _framing2D.ColorMethod = colorMethodType.byEntity;
            if (isCreatorA)
            {
                framing.OutTriggerAId = _framing2D.Id;
                framing.OutTriggerA = (OutTrigger2D)_framing2D;
                framing.IsOutTriggerA = true;
                ((IContaintOutTrigger)framing.FramingReference).OutTriggerA = (OutTrigger)_framingReference;
            }
            else
            {
                framing.OutTriggerBId = _framing2D.Id;
                framing.OutTriggerB = (OutTrigger2D)_framing2D;
                framing.IsOutTriggerB = true;
                ((IContaintOutTrigger)framing.FramingReference).OutTriggerB = (OutTrigger)_framingReference;
            }
            

        }

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
