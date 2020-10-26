using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationService;
using AppModels.CustomEntity;
using AppModels.Enums;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Openings;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;

namespace AppAddons.DrawingTools
{
    public class Draw2DBeam: DrawingLine
    {
        public override string ToolName => "Drawing 2D Beam";
        public Draw2DBeam() : base()
        {
            //IsUsingInsideLength = true;
            //InsideLengthDistance = 90;
        }

        [CommandMethod("Beam2D")]
        public void DrawBeam2D()
        {
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            DynamicInput?.FocusLength();
            while (true)
            {
                this.PromptPointOp.Message = ToolMessage;
                var res = acDoc.Editor.GetPoint(this.PromptPointOp);
                if (res.Status == PromptStatus.Cancel)
                {
                    return;
                }

                Points.Add(res.Value);
                if (Points.Count > 0)
                {
                    var index = Points.Count - 1;
                    BasePoint = (Point3D)Points[index]; ;
                    IsUsingOrthorMode = true;
                }
                if (this.Points.Count < 2) continue;
                var index2 = Points.Count - 1;
                var startPoint = (Point3D)Points[index2 - 1].Clone();
                var endPoint = (Point3D)Points[index2].Clone();
                LevelWall level = null;
                Beam newBeam = null;
                if (this.JobModel != null)
                {
                    foreach (var jobModelLevel in JobModel.Levels)
                    {
                        if (jobModelLevel.LevelName == ActiveLevel)
                        {
                            level = jobModelLevel;
                        }
                    }
                }

                if (level != null && this.JobModel.ActiveFloorSheet!=null)
                {
                    var beamId = level.RoofBeams.Count + 1;
                    newBeam = new Beam(BeamType.FloorBeam, level.LevelInfo) { Id = beamId };
                    newBeam.Quantity = 1;
                    newBeam.SpanLength = (int)(startPoint.DistanceTo(endPoint))-90;
                    this.JobModel.ActiveFloorSheet.Beams.Add(newBeam);
                    var beamBlockName = newBeam.Name + ActiveLevel;
                    var beam = new Beam2D(Plane.XY, startPoint, endPoint, newBeam,45, false, true);
                    beam.Color = Color.Brown;
                    beam.ColorMethod = colorMethodType.byEntity;
                    beam.LineTypeName = "Dash Space";
                    beam.LineTypeMethod = colorMethodType.byEntity;
                    this.EntitiesManager.AddAndRefresh(beam, LayerManager.SelectedLayer.Name);
                    //this.EntitiesManager.AddAndRefresh(beamEntity, "Beam");
                }
                
            }
        }
    }
}
