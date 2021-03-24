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
            //LinearDim line = null;
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
                .Add(13, "FramingReferenceId")
                .Add(14, "LevelName")
                .Add(15, "BeamMarkedLocation")
                .Add(16, "ShowBeamNameOnly")
                .Add(17, "ContinuesBeam")
                .Add(18, "SupportWallOver")
                .Add(19, "CustomAtrributeString")
                .UseConstructor = false;
            Model[typeof(Text)]
                .AddSubType(1001, typeof(DoorCountEntity))
                .AddSubType(1002, typeof(Hanger2D))
                .AddSubType(1003, typeof(Blocking2D))
                .AddSubType(1004, typeof(FramingNameEntity));
                //.AddSubType(1004,typeof(Beam2D));
            Model[typeof(TextSurrogate)]
                .AddSubType(1001, typeof(DoorCountEntitySurrogate))
                .AddSubType(1002, typeof(FramingBase2DSurrogate))
                .AddSubType(1003, typeof(FramingNameSurrogate));
            Model[typeof(FramingBase2DSurrogate)]
                .AddSubType(1001, typeof(Framing2DSurrogate))
                .AddSubType(1002, typeof(Hanger2DSurrogate));
            Model[typeof(Framing2DSurrogate)]
                //.AddSubType(1001, typeof(Beam2DSurrogate))
                .AddSubType(1001, typeof(Blocking2DSurrogate));
            Model[typeof(DoorCountEntitySurrogate)]
                .Add(1, "LevelName")
                .Add(2, "DoorReferenceId")
                .UseConstructor = false;
            Model[typeof(FramingBase2DSurrogate)]
                .Add(1, "Id")
                .Add(2, "FramingReferenceId")
                .Add(3, "FullLength")
                .Add(4,"LevelId")
                .Add(5,"FramingSheetId")
                .UseConstructor = false;
            Model[typeof(FramingNameSurrogate)]
                .Add(1, "Id")
                .Add(2, "LevelId")
                .Add(3, "FramingSheetId")
                .Add(4, "FramingReferenceId")
                .UseConstructor = false;
            Model[typeof(Hanger2DSurrogate)].UseConstructor = false;
            Model[typeof(Blocking2DSurrogate)]
                .Add(1,"IsRotate")
                .UseConstructor = false;
            Model[typeof(PlanarEntity)]
                .AddSubType(1001, typeof(FramingRectangle2D));
            Model[typeof(FramingRectangle2D)]
                .AddSubType(1001, typeof(FramingRectangleContainHangerAndOutTrigger))
                .AddSubType(1002,typeof(OutTrigger2D));
            Model[typeof(FramingRectangleContainHangerAndOutTrigger)]
                .AddSubType(1001, typeof(Beam2D))
                .AddSubType(1002, typeof(Joist2D));
            Model[typeof(PlanarEntitySurrogate)]
                .AddSubType(1001, typeof(FramingRectangle2DSurrogate));
            Model[typeof(FramingRectangle2DSurrogate)]
                .AddSubType(1001, typeof(FramingRectangleContainHangerAndOutTriggerSurrogate))
                .AddSubType(1002, typeof(OutTrigger2DSurrogate));
            Model[typeof(FramingRectangleContainHangerAndOutTriggerSurrogate)]
                .AddSubType(1002, typeof(Beam2DSurrogate))
                .AddSubType(1001, typeof(Joist2DSurrogate));
            Model[typeof(FramingRectangle2DSurrogate)]
                .Add(1, "Thickness")
                .Add(2, "Depth")
                .Add(3, "Id")
                .Add(4, "LevelId")
                .Add(5, "FramingSheetId")
                .Add(6, "Flipped")
                .Add(7, "OuterStartPoint")
                .Add(8, "OuterEndPoint")
                .Add(9, "FramingReferenceId")
                .UseConstructor = false;
            Model[typeof(FramingRectangleContainHangerAndOutTriggerSurrogate)]
                .Add(10, "HangerAId")
                .Add(11, "HangerBId")
                .Add(12, "IsHangerA")
                .Add(13, "IsHangerB")
                .Add(14, "OutTriggerAId")
                .Add(15, "OutTriggerBId")
                .Add(16, "IsOutTriggerA")
                .Add(17, "IsOutTriggerB")
                .Add(18, "OutTriggerAFlipped")
                .Add(19, "OutTriggerBFlipped")
                .Add(20, "FramingNameId")
                .Add(21, "IsShowFramingName")
                .UseConstructor = false;
            Model[typeof(Joist2DSurrogate)]
                .UseConstructor = false;
            Model[typeof(Beam2DSurrogate)]
                .Add(22, "IsBeamUnder")
                .Add(23, "ShowDimension")
                //.Add(26, "DimensionLine")
                .UseConstructor = false;
            Model[typeof(OutTrigger2DSurrogate)]
                .Add(10, "InsideLength")
                .Add(11, "OutSideLength")
                .UseConstructor = false;

        }

    }
}
