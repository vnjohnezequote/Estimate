using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.CustomEntity;
using AppModels.CustomEntity.CustomEntitySurrogate;
using devDept.Eyeshot.Entities;
using devDept.Serialization;

namespace AppModels.FileSerializer
{
    public class EzequoteFileSerializer: devDept.Serialization.FileSerializer
    {

        public EzequoteFileSerializer()
        {

        }

        public EzequoteFileSerializer(contentType contentType) : base(contentType)
        {
        }

        protected override void FillModel()
        {
            base.FillModel();
            Model[typeof(Line)]
                .AddSubType(1001, typeof(Wall2D));
            Model[typeof(LineSurrogate)]
                .AddSubType(1001, typeof(Wall2DSurrogate));
            Model[typeof(Wall2DSurrogate)]
                .Add(1, "WallLevelName")
                .UseConstructor = false;
            
        }

    }
}
