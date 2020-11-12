using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationInterfaceCore;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels.Enums;
using AppModels.EventArg;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.Application;
using DrawingModule.CommandClass;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.DrawToolBase;
using DrawingModule.Helper;
using DrawingModule.UserInteractive;

namespace AppAddons.DrawingTools
{
    public class DrawLeader : DrawTextBase,ILeaderTool
    {
        public override string ToolName => "Draw Leader";
        public override Point3D BasePoint { get; protected set; }
        private List<Point3D> _clickPoints;
        private int _leaderSegment = 1;
        private int _arrowSize = 500;
        public int LeaderSegment
        {
            get => _leaderSegment <= 0 ? 1 : _leaderSegment;
            set => SetProperty(ref _leaderSegment, value);
        }

        public int ArrowSize
        {
            get => _arrowSize<=0 ? 500 : _arrowSize;
            set => SetProperty(ref _arrowSize, value);
        }

        public DrawLeader()
        {
            _clickPoints = new List<Point3D>();
            IsUsingLeaderSegmentTextBox = true;
            IsUsingArrowHeadSizeTextBox = true;
            IsUsingTextStringAngleTextBox = false;
            IsSnapEnable = true;
            IsUsingOrthorMode = true;
        }

        [CommandMethod("Leader")]
        public void DrawingLeader()
        {
            while (true)
            {
                this.DynamicInput?.FocusDynamicInputTextBox(FocusType.TextContent);
                var acadEditor = Application.DocumentManager.MdiActiveDocument.Editor;
                for (var i = 0; i <= _leaderSegment; i++)
                {
                    ToolMessage = _clickPoints.Count == 0 ? "Please enter firstPoint" : "Please enter nextPoint";
                    var promptPointOption = new PromptPointOptions(ToolMessage);
                    var result = acadEditor.GetPoint(promptPointOption);
                    if (result.Status == PromptStatus.Cancel)
                    {
                        return;
                    }

                    var point = result.Value.Clone() as Point3D;
                    BasePoint = point;
                    _clickPoints.Add(point);
                }

                var leader = new Leader(Plane.XY, _clickPoints) { ArrowheadSize = this.ArrowSize };
                EntitiesManager.AddAndRefresh(leader, "BeamMarked");
                var text = Utils.CreateNewTextLeader(this._clickPoints[_clickPoints.Count - 2], this._clickPoints[_clickPoints.Count - 1],TextInput,TextHeight);
                EntitiesManager.AddAndRefresh(text, "BeamMarked");
                _clickPoints.Clear();
                BasePoint = null;
                    //EntitiesManager.AddAndRefresh(text, LayerManager.SelectedLayer.Name);
            }
            

        }

        protected override void OnMoveNextTab()
        {
            if (this.DynamicInput == null) return;
            switch (DynamicInput.PreviusDynamicInputFocus)
            {
                case FocusType.TextContent:
                    DynamicInput.FocusTextHeight();
                    break;
                case FocusType.TextHeight:
                    DynamicInput.FocusLeaderSegment();
                    break;
                case FocusType.LeaderSegment:
                    DynamicInput.FocusTextArrowSize();
                    break;
                case FocusType.ArrowSize:
                    DynamicInput.FocusTextContent();
                    break;
            }
        }

        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {
           DrawInteractiveLeader((ICadDrawAble)sender,e);
        }

        private void DrawInteractiveLeader(ICadDrawAble canvas, DrawInteractiveArgs e)
        {
            //canvas.renderContext.EnableXOR(false);
            if (_clickPoints.Count == 0)
            {
                return;
            }

            if (e.CurrentPoint == null)
            {
                return;
            }
            
            canvas.renderContext.EnableXOR(true);
            
            var index = _clickPoints.Count - 1;
            var startPoint = _clickPoints[index];
            DrawInteractiveUntilities.DrawInteractiveLines(_clickPoints,canvas,e.CurrentPoint);
            DrawInteractiveUntilities.DrawInteractiveTextForLeader(canvas,e.CurrentPoint,startPoint,TextInput,TextHeight);

            if (this._leaderSegment == 1)
            {
                DrawInteractiveUntilities.DrawInteractiveArrowHeader(canvas,_clickPoints[0], e.CurrentPoint, _arrowSize);
            }
            else if (_clickPoints.Count < 2)
            {
                return;
            }
            else
            {
                DrawInteractiveUntilities.DrawInteractiveArrowHeader(canvas,_clickPoints[0], _clickPoints[1], ArrowSize);
            }
        }

        
    }
}
