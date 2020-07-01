using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationInterfaceCore;
using ApplicationService;
using AppModels.EventArg;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.Application;
using DrawingModule.CommandClass;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace AppAddons.DrawingTools
{
    public class DrawLeader : ToolBase
    {
        public override Point3D BasePoint { get; protected set; }
        public List<Point3D> _clickPoints;
        private int _leaderSegment = 1;
        private int _arrowSize = 10;

        public DrawLeader()
        {
            _clickPoints = new List<Point3D>();
        }

        [CommandMethod("Leader")]
        public void DrawingLeader()
        {
            var acadEditor = Application.DocumentManager.MdiActiveDocument.Editor;
            for (var i = 0; i <= _leaderSegment; i++)
            {
                ToolMessage = _clickPoints.Count ==0 ? "Please enter firstPoint" : "Please enter nextPoint";
                var promptPointOption = new PromptPointOptions(ToolMessage);
                var result = acadEditor.GetPoint(promptPointOption);
                if (result.Status == PromptStatus.Cancel)
                {
                    return;
                }

                var point = result.Value.Clone() as Point3D;
                _clickPoints.Add(point);
            }

            var leader = new Leader(Plane.XY, _clickPoints) { ArrowheadSize = _arrowSize };
            EntitiesManager.AddAndRefresh(leader,LayerManager.SelectedLayer.Name);

        }

        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {
           DrawInteractiveLeader((ICadDrawAble)sender,e);
        }

        private void DrawInteractiveLeader(ICadDrawAble canvas, DrawInteractiveArgs e)
        {
            canvas.renderContext.EnableXOR(false);

            string text;
            if (_clickPoints.Count == 0)
            {
                return;
            }

            canvas.renderContext.EnableXOR(true);
            
            //var index = _clickPoints.Count - 1;
            //var startPoint = _clickPoints[index];
            DrawInteractiveUntilities.DrawInteractiveLines(_clickPoints,canvas,e.CurrentPoint);
            //DrawInteractiveUntilities.DrawInteractiveTextForLeader(canvas,e.CurrentPoint,startPoint);

            //if (this._leaderSegment == 1)
            //{
            //    DrawInteractiveUntilities.DrawInteractiveArrowHeader(_clickPoints[0], e.CurrentPoint, _arrowSize);
            //}
            //else if (_clickPoints.Count < 2)
            //{
            //    return;
            //}
            //else
            //{
            //    this.DrawInteractiveArrowHeader(_clickPoints[0], _clickPoints[1], ArrowSize);
            //}
        }

    }
}
