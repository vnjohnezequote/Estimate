using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            LinearDim line = null;
        }

        protected override void FillModel()
        {
            base.FillModel();
            Model[typeof(Line)]
                .AddSubType(1001, typeof(WallLine2D));
            Model[typeof(LineSurrogate)]
                .AddSubType(1001, typeof(Wall2DSurrogate));
            Model[typeof(Wall2DSurrogate)]
                .Add(1, "WallLevelName")
                .UseConstructor = false;
            Model[typeof(BlockReference)]
                .AddSubType(1001, typeof(BeamEntity));
            Model[typeof(BlockReferenceSurrogate)]
                .AddSubType(1001, typeof(BeamEntitySurrogate));
            Model[typeof(BeamEntitySurrogate)]
                .Add(1, "BeamLine")
                .Add(2, "BeamNameAttribute")
                .Add(3, "BeamQtyAttribute")
                .Add(4, "LintelAttribute")
                .Add(5, "ContinuesAttribute")
                .Add(6, "TreatmentAttribute")
                .Add(7, "SupportWallAttribute")
                .Add(8, "CustomAttribute")
                .Add(9, "BaseAttributePoint")
                .Add(10, "BeamLeader")
                .Add(11, "BeamBlock")
                .Add(12, "ClientName")
                .Add(13, "BeamReferenceId")
                .Add(14, "LevelName")
                .Add(15, "BeamMarkedLocation")
                .Add(16, "ShowBeamNameOnly")
                .Add(17, "ContinuesBeam")
                .Add(18, "SupportWallOver")
                .Add(19, "CustomAtrributeString")
                .UseConstructor = false;
            Model[typeof(Text)]
                .AddSubType(1001, typeof(DoorCountEntity));
            Model[typeof(TextSurrogate)]
                .AddSubType(1001, typeof(DoorCountEntitySurrogate));
            Model[typeof(DoorCountEntitySurrogate)]
                .Add(1, "LevelName")
                .Add(2, "DoorReferenceId")
                .UseConstructor = false;
            Model[typeof(PlanarEntity)]
                .AddSubType(1001, typeof(Joist2D));

        }

    }
}
