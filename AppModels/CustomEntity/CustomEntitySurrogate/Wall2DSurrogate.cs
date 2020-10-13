using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;
using devDept.Serialization;

namespace AppModels.CustomEntity.CustomEntitySurrogate
{
    [Serializable]
    public class Wall2DSurrogate: LineSurrogate
    {
        #region Properties
        public string WallLevelName { get; set; }
        #endregion
        public Wall2DSurrogate(WallLine2D wallLine2D) : base(wallLine2D)
        {
        }
        protected override Entity ConvertToObject()
        {
            Entity ent;
            ent = new WallLine2D(new Line(Vertices[0],Vertices[1]));

            CopyDataToObject(ent);

            return ent;
        }
        protected override void CopyDataToObject(Entity entity)
        {
            var wall2D = entity as WallLine2D;
            if (wall2D != null)
                wall2D.WallLevelName = WallLevelName;

            base.CopyDataToObject(entity);
        }

        /// <summary>
        /// Copies all data from the object to its surrogate.
        /// </summary>
        /// <remarks>Use this method to fill ALL the properties of this surrogate. It is called by the empty constructor to initialize the surrogates properties.</remarks>        
        protected override void CopyDataFromObject(Entity entity)
        {
            var wall2D = entity as WallLine2D;
            if (wall2D != null)
                WallLevelName = wall2D.WallLevelName;

            base.CopyDataFromObject(entity);
        }

        /// <summary>
        /// Integrity check according to the content type.
        /// </summary>                
        /// <remarks>        
        /// During the serialization process, this method is called internally before serializing the surrogate.        
        /// During the deserialization process, it can be used in the ConvertToObject method.
        ///  </remarks>
        protected override bool CheckSurrogateData(contentType content, string logMessage = null)
        {
            if (content == contentType.Tessellation)
            {
                if (Vertices == null || Vertices.Length == 0)
                {
                    WriteLog(logMessage != null ? logMessage : "Warning wallLine2D with no vertices.");
                    return false;
                }
            }

            return true;
        }

    }
}
