using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using AppModels.CustomEntity.CustomEntitySurrogate;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings.Blocking;
using AppModels.Undo;
using AppModels.ViewModelEntity;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Serialization;

namespace AppModels.CustomEntity
{
    public sealed class Blocking2D : Text,IEntityVmCreateAble,IFraming2D,ICloneAbleToUndo
    {
        #region Private

        private IFraming _framingReference;
        #endregion

        #region Properties
        #endregion
        public Guid Id { get; set; }
        public Guid LevelId { get; set; }
        public Guid FramingSheetId { get; set; }
        public bool IsRotate { get; set; }
        public IFraming FramingReference
        {
            get => _framingReference;
            set
            {
                _framingReference = value;
                FramingReference.PropertyChanged += BlockingReferenceOnPropertyChanged;
            }
        }

        public Guid FramingReferenceId { get; set; }

        public double FullLength
        {
            get
            {
                if (FramingReference!=null)
                {
                    return FramingReference.FullLength;
                }

                return 0.0;
            }
            set
            {
                if (FramingReference!=null)
                {
                    FramingReference.FullLength = (int)value;
                }
            }
        }

        public Blocking2D(Text another) : base(another)
        {

        }
        public Blocking2D(Point3D insPoint,Blocking blockingRef, string textString = "B-B", double height = 225) : base(insPoint, textString, height)
        {
            Id = Guid.NewGuid();
            LevelId = blockingRef.LevelId;
            FramingSheetId = blockingRef.FramingSheetId;
            Color = Color.DarkGoldenrod;
            ColorMethod = colorMethodType.byEntity;
            this.Alignment = alignmentType.MiddleCenter;
            FramingReference = blockingRef;
            FramingReferenceId = blockingRef.Id;
        }

        private void BlockingReferenceOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "BlockingType" when ((Blocking)FramingReference).BlockingType == BlockingTypes.SingleBlocking:
                    this.TextString = FramingReference.Name;
                    break;
                case "BlockingType":
                    this.TextString = FramingReference.Name+"-"+FramingReference.Name;
                    break;
                case "Name" when ((Blocking)FramingReference).BlockingType == BlockingTypes.SingleBlocking:
                    this.TextString = FramingReference.Name;
                    break;
                case "Name":
                    this.TextString = FramingReference.Name + "-" + FramingReference.Name;
                    break;
                case "FramingInfo":
                    Helper.RegenerationFramingName(FramingReference.FramingSheet.Blockings.ToList());
                    var items = FramingReference.FramingSheet.Blockings.OrderBy(blocking => blocking.Name);
                    var sortedList = items.ToList();
                    FramingReference.FramingSheet.Blockings.Clear();
                    FramingReference.FramingSheet.Blockings.AddRange(sortedList);
                    break;
            }
        }

        public IEntityVm CreateEntityVm(IEntitiesManager entitiesManager)
        {
            return new BlockingVm(this,entitiesManager);
        }

        public override EntitySurrogate ConvertToSurrogate()
        {
            return new Blocking2DSurrogate(this);
        }

    }
}
