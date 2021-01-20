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
using AppModels.ResponsiveData.Framings.FloorAndRafters.Beam;
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
                if (JobModel.ActiveFloorSheet== null)
                {
                    continue;
                }
                LevelWall level = JobModel.ActiveFloorSheet.Level;

                if (level != null && this.JobModel.ActiveFloorSheet!=null)
                {
                    var beamId = JobModel.ActiveFloorSheet.Beams.Count + 1;
                    var beamType = FramingTypes.FloorBeam;
                    if (JobModel.ActiveFloorSheet.FramingSheetType == FramingSheetTypes.RafterFraming)
                    {
                        beamType = FramingTypes.RafterBeam;
                    }

                    var newBeam = new FBeam(JobModel.ActiveFloorSheet)
                    {
                        Index = beamId, FramingType = beamType, FullLength = (int) (startPoint.DistanceTo(endPoint))
                    };
                    var beamThickness = 45;
                    if (JobModel.SelectedJoitsMaterial != null)
                    {
                        newBeam.FramingInfo = JobModel.SelectedJoitsMaterial;
                        beamThickness = newBeam.FramingInfo.NoItem * newBeam.FramingInfo.Depth;
                    }
                    this.JobModel.ActiveFloorSheet.Beams.Add(newBeam);
                    var beam = new Beam2D(startPoint, endPoint, newBeam, beamThickness)
                    {
                        ColorMethod = colorMethodType.byEntity
                    };
                    this.EntitiesManager.AddAndRefresh(beam, LayerManager.SelectedLayer.Name);
                    
                }
                
            }
        }
    }
}
