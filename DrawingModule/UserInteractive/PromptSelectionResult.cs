using ApplicationService;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

namespace DrawingModule.UserInteractive
{
    public class PromptSelectionResult : PromptResult
    {
        public Entity Value { get; private set; }

        public Point3D ClickedPoint { get; private set; }
        internal PromptSelectionResult(PromptStatus promptStatus, string stringResult) : base(promptStatus, stringResult)
        {
        }
        internal PromptSelectionResult(PromptStatus promptStatus, string stringResult,Point3D clickedPoint ,Entity entity) : base(promptStatus, stringResult)
        {
            if (promptStatus != PromptStatus.OK) return;
            this.Value= entity;
            ClickedPoint = clickedPoint;
        }
    }
}
