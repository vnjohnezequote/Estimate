using System;
using AppModels.Enums;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Serialization;
using Attribute = devDept.Eyeshot.Entities.Attribute;

namespace AppModels.CustomEntity.CustomEntitySurrogate
{
    public class BeamEntitySurrogate: BlockReferenceSurrogate
    {
        #region Properties
        public Line BeamLine { get; set; }
        public Attribute BeamNameAttribute { get; set; }
        public Attribute BeamQtyAttribute { get; set; }
        public Attribute LintelAttribute { get; set; }
        public Attribute ContinuesAttribute { get; set; }
        public Attribute TreatmentAttribute { get; set; }
        public Attribute SupportWallAttribute { get; set; }
        public Attribute CustomAttribute { get; set; }
        public Point3D BaseAttributePoint { get; set; }
        public Leader BeamLeader { get; set; }
        public Block BeamBlock { get; set; }
        public string ClientName { get; set; }
        public Guid FramingReferenceId { get; set; }
        public string LevelName { get; set; }
        public BeamMarkedLocation BeamMarkedLocation { get; set; }
        public bool ShowBeamNameOnly { get; set; }

        public bool ContinuesBeam { get; set; }

        public bool SupportWallOver { get; set; }

        public string CustomAtrributeString { get; set; }
        #endregion
        public BeamEntitySurrogate(BlockReference blockReference) : base(blockReference)
        {
        }

        protected override Entity ConvertToObject()
        {
            Entity ent;
            ent = new BeamEntity(new BlockReference(new Point3D(0, 0, 0),BlockName,0));
            CopyDataToObject(ent);
            return ent;

        }

        
        protected override void CopyDataToObject(Entity entity)
        {
            if (entity is BeamEntity beam)
            {
                beam.BeamLine= BeamLine;
                beam.BeamNameAttribute= BeamNameAttribute ;
                beam.BeamQtyAttribute = BeamQtyAttribute ;
                beam.LintelAttribute= LintelAttribute ;
                beam.ContinuesAttribute = ContinuesAttribute;
                beam.TreatmentAttribute =TreatmentAttribute;
                beam.SupportWallAttribute = SupportWallAttribute;
                beam.ContinuesAttribute= CustomAttribute;
                beam.BaseAttributePoint=BaseAttributePoint ;
                beam.BeamLeader= BeamLeader;
                beam.BeamBlock= BeamBlock;
                beam.ClientName= ClientName ;
                beam.FramingReferenceId = FramingReferenceId;
                beam.LevelName=LevelName ;
                beam.BeamMarkedLocation= BeamMarkedLocation;
                beam.ShowBeamNameOnly=ShowBeamNameOnly;
                beam.ContinuesBeam = ContinuesBeam;
                beam.SupportWallOver= SupportWallOver;
                beam.CustomAtrributeString=CustomAtrributeString ;
            }
            base.CopyDataToObject(entity);
        }
        protected override void CopyDataFromObject(Entity entity)
        {
            if (entity is BeamEntity beam)
            {
                BeamLine = beam.BeamLine;
                BeamNameAttribute = beam.BeamNameAttribute;
                BeamQtyAttribute = beam.BeamQtyAttribute;
                LintelAttribute = beam.LintelAttribute;
                ContinuesAttribute = beam.ContinuesAttribute;
                TreatmentAttribute = beam.TreatmentAttribute;
                SupportWallAttribute = beam.SupportWallAttribute;
                CustomAttribute = beam.ContinuesAttribute;
                BaseAttributePoint = beam.BaseAttributePoint;
                BeamLeader = beam.BeamLeader;
                BeamBlock = beam.BeamBlock;
                ClientName = beam.ClientName;
                FramingReferenceId = beam.FramingReference.Id;
                LevelName = beam.LevelName;
                BeamMarkedLocation = beam.BeamMarkedLocation;
                ShowBeamNameOnly = beam.ShowBeamNameOnly;
                ContinuesBeam = beam.ContinuesBeam;
                SupportWallOver = beam.SupportWallOver;
                CustomAtrributeString = beam.CustomAtrributeString;

            }
            
            base.CopyDataFromObject(entity);
        }
    }
}
