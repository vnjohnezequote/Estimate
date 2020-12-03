using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Framings;
using devDept.Eyeshot.Entities;
using devDept.Serialization;

namespace AppModels.CustomEntity.CustomEntitySurrogate
{
    public class FramingBase2DSurrogate: TextSurrogate
    {
        public Guid Id { get; set; }
        public Guid FramingReferenceId { get; set; }
        public double FullLength { get; set;  }
        
        public FramingBase2DSurrogate(Text text) : base(text)
        {
        }
        protected override void CopyDataToObject(Entity entity)
        {
            base.CopyDataToObject(entity);
            if (!(entity is IFraming2D joist2D)) return;
            joist2D.Id = Id;
            joist2D.FramingReferenceId = FramingReferenceId;
            joist2D.FullLength = FullLength;
        }
        protected override void CopyDataFromObject(Entity entity)
        {
            if (entity is IFraming2D joist2D)
            {
                Id = joist2D.Id;
                FramingReferenceId = joist2D.FramingReference.Id;
                FullLength = joist2D.FullLength;

            }
            base.CopyDataFromObject(entity);
        }
    }
}
