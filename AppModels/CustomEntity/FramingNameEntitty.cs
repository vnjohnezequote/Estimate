using System;
using System.ComponentModel;
using System.Drawing;
using AppModels.CustomEntity.CustomEntitySurrogate;
using AppModels.Enums;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Serialization;

namespace AppModels.CustomEntity
{
    public sealed class FramingNameEntity : Text
    {
        private IFraming _framingReference;
        public IFraming FramingReference
        {
            get => _framingReference;
            set
            {
                _framingReference = value;
                this.SetFramingName();
                this.SetFramingNameColor();
                _framingReference.PropertyChanged+=FramingReferenceOnPropertyChanged;
            }
        }

        public Guid Id { get; set; }
        public Guid LevelId { get; set; }
        public Guid FramingSheetId { get; set; }
        public Guid FramingReferenceId { get; set; }

        public FramingNameEntity(Point3D insPoint, string textString, double height, alignmentType alignment,IFraming framingReference) : base(insPoint, textString, height, alignment)
        {
            Id = Guid.NewGuid();
            LevelId = framingReference.LevelId;
            FramingSheetId = framingReference.FramingSheetId;
            FramingReference = framingReference;
            FramingReferenceId = framingReference.Id;
        }

        public FramingNameEntity(Text another) : base(another)
        {
            this.ColorMethod = colorMethodType.byEntity;
        }

        private void SetFramingNameColor()
        {
            if (FramingReference.FramingInfo != null)
            {
                if (FramingReference.FramingType == FramingTypes.FloorJoist)
                {
                    var thickness = FramingReference.FramingInfo.Depth * FramingReference.FramingInfo.NoItem;
                    switch (thickness)
                    {
                        case 45:
                            this.Color = System.Drawing.Color.FromArgb(82, 165, 0);
                            break;
                        case 50:
                            Color = Color.White;
                            break;
                        case 63:
                            Color = System.Drawing.Color.FromArgb(0, 76, 0);
                            break;
                        case 90:
                            Color = System.Drawing.Color.FromArgb(0, 63, 255);
                            break;
                    }

                }
            }
            
        }

        private void SetFramingName()
        {
            if (FramingReference!=null)
            this.TextString = FramingReference.Name;
        }
        private void FramingReferenceOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FramingReference.Name))
            {
                SetFramingName();
            }

            if (e.PropertyName == nameof(FramingReference.FramingInfo))
            {
               SetFramingNameColor();
            }
            
        }

        public override EntitySurrogate ConvertToSurrogate()
        {
            return new FramingNameSurrogate(this);
        }

        
    }
}
