using System.Collections.Generic;
using ApplicationService;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace DrawingModule.UserInteractive
{
    public class PromptEntityResult : PromptResult
    {
        public List<Entity> Entities { get; private set; }
        public Point3D LastPickedPoint { get; private set; }
        internal PromptEntityResult(PromptStatus promptStatus, string stringResult) : base(promptStatus, stringResult)
        {
        }

        internal PromptEntityResult(PromptStatus promptStatus, string stringResult, Point3D pickedPoint,
            List<Entity> entities) : base(promptStatus, stringResult)
        {
            if (promptStatus != PromptStatus.OK) return;
            this.LastPickedPoint = pickedPoint;
            this.Entities = entities;
        }
    }
}
