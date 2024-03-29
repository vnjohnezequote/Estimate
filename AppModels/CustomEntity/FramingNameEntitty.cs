﻿using System;
using System.ComponentModel;
using System.Drawing;
using AppModels.CustomEntity.CustomEntitySurrogate;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.Undo;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Serialization;

namespace AppModels.CustomEntity
{
    public sealed class FramingNameEntity : Text,IDependencyUndoEntity
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

        public FramingNameEntity(FramingNameEntity another,bool isCloneToUndo = false):base(another)
        {
            this.ColorMethod = colorMethodType.byEntity;
            if (isCloneToUndo)
            {
                Id = another.Id;
                FramingReference = another.FramingReference;
                FramingReferenceId = another.FramingReferenceId;
            }
            else
            {
                Id = Guid.NewGuid();
                FramingReference = (IFraming)another.FramingReference.Clone();
                FramingReferenceId = FramingReference.Id;
            }
            LevelId = another.LevelId;
            FramingSheetId = another.FramingSheetId;
        }

        private void SetFramingNameColor()
        {
            if (FramingReference.FramingInfo != null)
            {
                if (FramingReference.FramingType == FramingTypes.FloorJoist || FramingReference.FramingType == FramingTypes.RafterJoist)
                {
                    var thickness = FramingReference.FramingInfo.Depth * FramingReference.FramingInfo.NoItem;
                    switch (thickness)
                    {
                        case 35:
                            Color = System.Drawing.Color.FromArgb(255, 127, 159);
                            break;
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
            if (e.PropertyName == nameof(FramingReference.Name) 
                || e.PropertyName==nameof(FramingReference.Index)
                ||e.PropertyName==nameof(FramingReference.NamePrefix))
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

        public override object Clone()
        {
            return new FramingNameEntity(this);
        }
        public object CloneToUndo()
        {
            var framingNameEntity = new FramingNameEntity(this,true);
            framingNameEntity.Id = this.Id;
            return framingNameEntity;
        }

        public void RollBackDependency(UndoList undoItem,IEntitiesManager entitiesManager)
        {
            if (undoItem.DependencyEntitiesDictionary.TryGetValue(this,
                                 out var dependencyEntity))
            {
                if (dependencyEntity is FramingRectangleContainHangerAndOutTrigger framing)
                {
                    framing.FramingName = this;
                    framing.FramingNameId = Id;
                    framing.IsShowFramingName = true;
                }

                if (dependencyEntity is JoistArrowEntity joistArrow)
                {
                    entitiesManager.AddAndRefresh(joistArrow, joistArrow.LayerName);
                }
            }
        }
    }
}
