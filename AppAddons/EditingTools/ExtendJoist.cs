using System.Collections.Generic;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels.CustomEntity;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.Application;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace AppAddons.EditingTools
{
    public class ExtendJoist : ToolBase
    {
        private List<FramingRectangle2D> _framingList = new List<FramingRectangle2D>();
        private Beam2D _boundaryFraming;
        private Line _boundaryLine;
        public override string ToolName => "Extend Joist";
        public override Point3D BasePoint { get; protected set; }

        public ExtendJoist()
        {
            EntityUnderMouseDrawingType = UnderMouseDrawingType.ByObject;
            IsUsingOrthorMode = false;
            IsSnapEnable = false;
        }

        [CommandMethod("Extend Framing")]
        public void ExtendJ()
        {
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            if (ChoosingBoundaryExtendEntity(acDoc) == PromptStatus.OK)
            {
                if (ChoosingExtendJoist(acDoc )== PromptStatus.OK)
                {
                    if (_boundaryFraming!=null)
                    {
                        ProcessExtendJoist(_boundaryFraming);
                    }
                    else
                    {
                        ProcessExtendJoist(_boundaryLine);
                    }
                    EntitiesManager.EntitiesRegen();
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        private void ProcessExtendJoist(Beam2D boundaryBeam)
        {
            foreach (var framing in _framingList)
            {
                if (framing.IntersectWith(boundaryBeam,out var farPoint, out var closerPoint))
                {
                    var distance1 = framing.OuterStartPoint.DistanceTo(farPoint);
                    var distance2 = framing.OuterEndPoint.DistanceTo(closerPoint);
                    if (distance1>distance2)
                    {
                        if (boundaryBeam.IsBeamUnder)
                        {
                            TranslateOutTriggerBToo(framing,farPoint);
                            framing.OuterEndPoint = farPoint;
                        }
                        else
                        {
                            TranslateOutTriggerBToo(framing, closerPoint);
                            framing.OuterEndPoint = closerPoint;
                        }
                    }
                    else
                    {
                        if (boundaryBeam.IsBeamUnder)
                        {
                            TranslateOutTriggerAToo(framing, farPoint);
                            framing.OuterStartPoint = farPoint;
                        }
                        else
                        {
                            TranslateOutTriggerAToo(framing, closerPoint);
                            framing.OuterStartPoint = closerPoint;
                        }
                    }
                }

            }
        }

        private void TranslateOutTriggerAToo(IRectangleSolid framing,Point3D newPointMove)
        {
            if (framing is IFraming2DContaintHangerAndOutTrigger frame2D && frame2D.OutTriggerA != null)
            {
                var startP = framing.OuterStartPoint;
                var endP = newPointMove;
                var movement = new Vector3D(startP, endP);
                frame2D.OutTriggerA.Translate(movement);
            }
        }
        private void TranslateOutTriggerBToo(IRectangleSolid framing,Point3D newPointMove)
        {
            if (framing is IFraming2DContaintHangerAndOutTrigger frame2D && frame2D.OutTriggerB != null)
            {
                var startP = framing.OuterEndPoint;
                var endP = newPointMove;
                var movement = new Vector3D(startP, endP);
                frame2D.OutTriggerB.Translate(movement);
            }
        }

        private void ProcessExtendJoist(Line boundaryLine)
        {
            foreach (var framing in _framingList)
            {
                if (framing.IntersectWith(boundaryLine, out var intersectPoint))
                {
                    var distance1 = framing.OuterStartPoint.DistanceTo(intersectPoint);
                    var distance2 = framing.OuterEndPoint.DistanceTo(intersectPoint);
                    if (distance1>distance2)
                    {
                       TranslateOutTriggerBToo((IRectangleSolid)framing,intersectPoint);
                        
                        framing.OuterEndPoint = intersectPoint;
                        
                    }
                    else
                    {
                        TranslateOutTriggerAToo((IRectangleSolid)framing,intersectPoint);
                        framing.OuterStartPoint = intersectPoint;
                    }
                }
            }

        }

        private PromptStatus ChoosingExtendJoist(Document acDoc)
        {
            while(true)
            {
                _framingList.Clear();
                ToolMessage = "Please select Joists to Extend";
                var promptEntities = new PromptEntityOptions();
                var result = acDoc.Editor.GetEntities(promptEntities);
                if (result.Status == PromptStatus.OK)
                {
                    foreach (var entity in result.Entities)
                    {
                        if (entity is FramingRectangleContainHangerAndOutTrigger framing2D)
                        {
                            _framingList.Add(framing2D);
                        }
                    }

                    if (_framingList.Count > 0)
                    {
                        return PromptStatus.OK;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    return PromptStatus.Cancel;
                }
            }
        }

        private PromptStatus ChoosingBoundaryExtendEntity(Document acDoc)
        {
            while (true)
            {
                ToolMessage = "Please select Boundary Beam/Line to extend Joist";
                var promptSelection = new PromptSelectionOptions();
                var resutl = acDoc.Editor.GetSelection(promptSelection);
                if (resutl.Status == PromptStatus.OK)
                {
                    if (resutl.Value is Beam2D beam)
                    {
                        _boundaryFraming = beam;
                        return PromptStatus.OK;
                    }

                    else if (resutl.Value is Line line)
                    {
                        _boundaryLine = line;
                        return PromptStatus.OK;
                    }
                    else
                    {
                        continue;
                    }

                }
                else
                {
                    return PromptStatus.Cancel;
                }
            }
        }
    }
}
