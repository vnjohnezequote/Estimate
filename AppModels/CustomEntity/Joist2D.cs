using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using AppModels.CustomEntity.CustomEntitySurrogate;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using AppModels.ViewModelEntity;
using devDept.Geometry;
using devDept.Serialization;

namespace AppModels.CustomEntity
{
    public class Joist2D : FramingRectangleContainHangerAndOutTrigger,IEntityVmCreateAble
    {
        //private bool _outTriggerAFlipped;
        //private bool _outTriggerBFlipped;

        public Joist2D(Point3D outerStartPoint, Point3D outerEndPoint, IFraming joistReference,
            int thickness = 90,bool flipped = false,bool centerCreater = false) :
            base(outerStartPoint,outerEndPoint,joistReference,thickness,flipped,centerCreater)
        {
            
        }

        public Joist2D(Point3D outerStartPoint, Point3D outerEndPoint, int thickness,bool flipped,bool centerCreator = false) : base(outerStartPoint,
            outerEndPoint, thickness,flipped,centerCreator)
        {

        }

        public Joist2D(Joist2D another) : base(another)
        {
            
        }

        
        //public override void SetFlippedOutriggerA(bool outTriggerAFlipped)
        //{
        //    _outTriggerAFlipped = outTriggerAFlipped;
        //}
        //public override void SetFlippedOutriggerB(bool outTriggerBFlipped)
        //{
        //    _outTriggerBFlipped = outTriggerBFlipped;
        //}
       
        public IEntityVm CreateEntityVm(IEntitiesManager entitiesManager)
        {
            return new Joist2dVm(this, entitiesManager);
        }

        public override EntitySurrogate ConvertToSurrogate()
        {
            return new Joist2DSurrogate(this);
        }

        public override object Clone()
        { 
            return new Joist2D(this);
        }

        protected override void FramingPropertiesChanged(object sender, PropertyChangedEventArgs e)
        {
            base.FramingPropertiesChanged(sender, e);
            switch (e.PropertyName)
            {
                case "FramingInfo":
                case "FramingType":
                case "QuoteLength":
                    Helper.RegenerationFramingName(FramingReference.FramingSheet.Joists.ToList());
                    var orderedEnumerable = FramingReference.FramingSheet.Joists.OrderBy(framing => framing.Name);
                    var sortedList = orderedEnumerable.ToList();
                    FramingReference.FramingSheet.Joists.Clear();
                    FramingReference.FramingSheet.Joists.AddRange(sortedList);
                    break;
                default:break;
            }
        }

        protected override void SetFramingColor(int thickness)
        {
            if (FramingReference==null)
            {
                return;
            } 
            if(FramingReference.FramingType == FramingTypes.DeckJoist)
            {
                Color = System.Drawing.Color.FromArgb(255, 127, 159);
                return;
            }    
            if(FramingReference.FramingType == FramingTypes.HipRafter)
            {
                Color = System.Drawing.Color.FromArgb(255, 0, 255);
                return;
            }   
            if(FramingReference.FramingType== FramingTypes.Fascia)
            {
                Color = System.Drawing.Color.FromArgb(63,111,127);
                return;
            }    
            else if(FramingReference.FramingType == FramingTypes.CeilingJoist)
            {
                Color = System.Drawing.Color.FromArgb(0,127,0);
                return;
            }    
            switch (thickness)
            {
                case 35:
                    if (FramingReference.FramingType == FramingTypes.FloorJoist || FramingReference.FramingType == FramingTypes.RafterJoist)
                    {
                        Color = System.Drawing.Color.FromArgb(255, 127, 159);
                    }
                    break;
                case 45:
                    if (FramingReference.FramingType == FramingTypes.FloorJoist || FramingReference.FramingType == FramingTypes.RafterJoist)
                    {
                        Color = System.Drawing.Color.FromArgb(82, 165, 0);
                    }
                    break;
                case 50:
                    if (FramingReference.FramingType == FramingTypes.FloorJoist || FramingReference.FramingType == FramingTypes.RafterJoist)
                    {
                        Color = System.Drawing.Color.White;
                    }
                    break;
                case 63:
                    if (FramingReference.FramingType == FramingTypes.FloorJoist || FramingReference.FramingType == FramingTypes.RafterJoist)
                    {
                        Color = System.Drawing.Color.FromArgb(0, 76, 0);
                    }
                    break;
                case 90:
                    if (FramingReference.FramingType == FramingTypes.FloorJoist || FramingReference.FramingType == FramingTypes.RafterJoist)
                    {
                        Color = System.Drawing.Color.FromArgb(0, 63, 255);
                    }
                    break;
                default:
                    break;
            }
            
        }
    }
}
