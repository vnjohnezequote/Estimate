using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Configuration;
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
using DrawingModule.CustomControl.CanvasControl;
using GeometryGym.Ifc;

namespace AppAddons.DrawingTools
{
    public class DrawBeamCommand: DrawingWallLine
    {
        public override string ToolName => "Drawing Beam";

        public DrawBeamCommand() : base()
        {

        }

        [CommandMethod("Beam")]
        public void DrawBeam()
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
                if (this.JobModel!=null)
                {
                    foreach (var jobModelLevel in JobModel.Levels)
                    {
                        if (jobModelLevel.LevelName == ActiveLevel)
                        {
                            level = jobModelLevel;
                        }
                    }
                }

                if (level !=null)
                {
                    var beamId = level.RoofBeams.Count + 1;
                    newBeam = new Beam(BeamType.TrussBeam, level.LevelInfo) { Id = beamId };
                    newBeam.Quantity = 1;
                    newBeam.SpanLength = (int) (startPoint.DistanceTo(endPoint));
                    level.RoofBeams.Add(newBeam);
                    var beamBlockName = newBeam.Name + ActiveLevel;
                    while (EntitiesManager.Blocks.Contains(beamBlockName))
                    {
                        beamBlockName = beamBlockName + "s";
                    }
                    var beamEntity = new BeamEntity(new Point3D(0, 0, 0), beamBlockName, (Point3D)startPoint.Clone(), (Point3D)endPoint.Clone(), "Prenail", "GroundFloor", 0);
                    EntitiesManager.Blocks.Add(beamEntity.BeamBlock);
                    beamEntity.BeamReference = newBeam;
                    beamEntity.LevelName = ActiveLevel;
                    if (string.IsNullOrEmpty(newBeam.SizeGrade))
                    {
                        var beamString = newBeam.SizeGrade + " @ " + newBeam.Quantity + "/" + newBeam.QuoteLength;
                        beamEntity.Attributes.Add("Name", beamString);
                    }
                    else
                    {
                        beamEntity.Attributes.Add("Name", newBeam.Name);
                    }
                    beamEntity.Attributes.Add("Continues","");
                    beamEntity.Attributes.Add("Support", "");
                    beamEntity.Attributes.Add("Treatment", "");
                    beamEntity.Attributes.Add("Custom", "");
                    beamEntity.Attributes.Add("Lintel", "");
                    this.EntitiesManager.AddAndRefresh(beamEntity, "Beam");
                }
            }
        }

        
    }
}
