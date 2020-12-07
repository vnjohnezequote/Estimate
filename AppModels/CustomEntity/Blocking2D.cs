using System;
using System.ComponentModel;
using System.Drawing;
using AppModels.CustomEntity.CustomEntitySurrogate;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings.Blocking;
using AppModels.ViewModelEntity;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Serialization;

namespace AppModels.CustomEntity
{
    public sealed class Blocking2D : Text,IEntityVmCreateAble,IFraming2D
    {
        #region Private

        private IFraming _framingReference;
        #endregion

        #region Properties
        #endregion
        public Guid Id { get; set; }
        public Guid LevelId { get; set; }
        public Guid FramingSheetId { get; set; }

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
            if (e.PropertyName == "BlockingType")
            {
                if (((Blocking)FramingReference).BlockingType == BlockingTypes.SingleBlocking)
                {
                    this.TextString = FramingReference.Name;
                }
                else
                {
                    this.TextString = FramingReference.Name+"-"+FramingReference.Name;
                }
            }

            if (e.PropertyName == "Name")
            {
                if (((Blocking)FramingReference).BlockingType == BlockingTypes.SingleBlocking)
                {
                    this.TextString = FramingReference.Name;
                }
                else
                {
                    this.TextString = FramingReference.Name + "-" + FramingReference.Name;
                }
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
