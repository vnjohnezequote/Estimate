using System;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace AppModels.CustomEntity
{
    [Serializable]
    public class PaperBolderReference: BlockReference
    {
        public string FloorName
        {
            get => this.Attributes.ContainsKey("Title") ? Attributes["Title"].Value : string.Empty;
            set
            {
                if (this.Attributes.ContainsKey("Title"))
                {
                    this.Attributes["Title"].Value = value;
                }
                else
                {
                    Attributes.Add("Title", value);
                }
            }
        }
        public PaperBolderReference(Point3D insPoint, string blockName, double rotationAngleInRadians) : base(insPoint, blockName, rotationAngleInRadians)
        {
        }
    }
}
