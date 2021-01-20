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
        private bool _outTriggerAFlipped;
        private bool _outTriggerBFlipped;

        public Joist2D(Point3D outerStartPoint, Point3D outerEndPoint, Joist joistReference,
            int thickness = 90) :
            base(outerStartPoint,outerEndPoint,joistReference,thickness)
        {
            
        }

        public Joist2D(Point3D outerStartPoint, Point3D outerEndPoint, int thickness,bool flipped) : base(outerStartPoint,
            outerEndPoint, thickness,flipped)
        {

        }

        public Joist2D(Joist2D another) : base(another)
        {

        }

        
        public void SetFlippedOutriggerA(bool outTriggerAFlipped)
        {
            _outTriggerAFlipped = outTriggerAFlipped;
        }
        public void SetFlippedOutriggerB(bool outTriggerBFlipped)
        {
            _outTriggerBFlipped = outTriggerBFlipped;
        }
       
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
            
                switch (thickness)
                {
                    case 45:
                        if (FramingReference.FramingType == FramingTypes.FloorJoist)
                        {
                            Color = System.Drawing.Color.FromArgb(82, 165, 0);
                        }
                        break;
                    case 50:
                        if (FramingReference.FramingType == FramingTypes.FloorJoist)
                        {
                            Color = System.Drawing.Color.White;
                        }
                        break;
                    case 63:
                        if (FramingReference.FramingType == FramingTypes.FloorJoist)
                        {
                            Color = System.Drawing.Color.FromArgb(0, 76, 0);
                        }
                        break;
                    case 90:
                        if (FramingReference.FramingType == FramingTypes.FloorJoist)
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
