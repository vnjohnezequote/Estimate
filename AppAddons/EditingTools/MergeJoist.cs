using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels;
using AppModels.CustomEntity;
using AppModels.EntityCreator;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace AppAddons.EditingTools
{
    public class MergeJoist: ToolBase
    {
        public override Point3D BasePoint { get; protected set; }
        public override string ToolName => "Merge Joist";
        private bool _processingTool = true;

        public MergeJoist()
        {
            EntityUnderMouseDrawingType = UnderMouseDrawingType.ByObject;
            IsUsingOrthorMode = false;
            IsSnapEnable = false;
        }

        [CommandMethod("Merge Joists")]
        public void MergeJ()
        {
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            while (_processingTool)
            {
                ToolMessage = "Please select Joist to Merge";
                var promptEntities = new PromptEntityOptions();
                var result = acDoc.Editor.GetEntities(promptEntities, false);
                if (result.Status == PromptStatus.Cancel) {
                    return;
                }
                if (result.Status == PromptStatus.OK)
                {
                    //var i = 0;
                    var joistDic = new Dictionary<Joist2D, Joist2D>();
                    while (result.Entities.Count>0)
                    {
                        if (result.Entities[0] is Joist2D joist)
                        {
                            var isExistColinearJoist = false;
                            for (int j = 1; j < result.Entities.Count; j++)
                            {
                                if (result.Entities[j] is Joist2D joisttest)
                                {
                                    if (joist.IsColinearTouchWithOther(joisttest))
                                    {
                                        result.Entities.Remove(joist);
                                        result.Entities.Remove(joisttest);
                                        j--;
                                        joistDic.Add(joist,joisttest);
                                        isExistColinearJoist = true;
                                    }
                                }
                                else
                                {
                                    result.Entities.Remove(result.Entities[j]);
                                }
                            }

                            if (!isExistColinearJoist)
                            {
                                result.Entities.Remove(joist);
                            }
                        }
                        else
                        {
                            result.Entities.Remove(result.Entities[0]);
                        }
                    }

                    if (joistDic.Count>0)
                    {
                        foreach (var joist in joistDic)
                        {
                            var listPoints = new List<Point3D>();
                            if (!listPoints.Contains(joist.Key.StartCenterLinePoint))
                            {
                                listPoints.Add(joist.Key.StartCenterLinePoint);
                            }

                            if (!listPoints.Contains(joist.Key.EndCenterLinePoint))
                            {
                                listPoints.Add(joist.Key.EndCenterLinePoint);
                            }

                            if (!listPoints.Contains(joist.Value.StartCenterLinePoint))
                            {
                                listPoints.Add(joist.Value.StartCenterLinePoint);
                            }

                            if (!listPoints.Contains(joist.Value.EndCenterLinePoint))
                            {
                                listPoints.Add(joist.Value.EndCenterLinePoint);
                            }

                            if (listPoints.Count<3)
                            {
                                return;
                            }
                            var newSegment2D = FindMaxSegment(listPoints);
                            if (newSegment2D!=null)
                            {
                                var joiseCreator = new Joist2DCreator(joist.Key,
                                    newSegment2D.P0.ConvertPoint2DtoPoint3D(),
                                    newSegment2D.P1.ConvertPoint2DtoPoint3D(),true);
                                 this.EntitiesManager.RemoveEntity(joist.Key);
                                this.EntitiesManager.RemoveEntity(joist.Value);
                                EntitiesManager.AddAndRefresh((Joist2D)joiseCreator.GetFraming2D(),joist.Key.LayerName);
                            }
                        }
                    }
                    return;
                }
            }
        }
        private Segment2D ProjectSegmentOn(Segment2D segment1, Segment2D sourceSegment)
        {
            var disc1 = segment1.Project(sourceSegment.P0);
            var projectPt1 = segment1.PointAt(disc1);
            var disc2 = segment1.Project(sourceSegment.P1);
            var projectPt2 = segment1.PointAt(disc2);
            return new Segment2D(projectPt1, projectPt2);
        }
        private Segment2D FindMaxSegment(List<Point3D> segments)
        {
            var sortedSegments = Helper.SortPointInLine(segments);
            return new Segment2D(sortedSegments[0], sortedSegments[2]);

        }

    }
}
