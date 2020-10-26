using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationService;
using AppModels.CustomEntity;
using AppModels.ResponsiveData.Framings.FloorAndRafters.Floor;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;

namespace AppAddons.DrawingTools
{
    public class DrawJoist: DrawingLine
    {
        public override string ToolName => "Drawing Single Joist";
        public DrawJoist() : base()
        {
            //IsUsingInsideLength = true;
            //InsideLengthDistance = 90;
        }

        [CommandMethod("Joist")]
        public void DrawJoistCommand()
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
                var joistMember = new Joist();
                if (this.JobModel.ActiveFloorSheet != null)
                {
                    var index = this.JobModel.ActiveFloorSheet.Joists.Count;
                    joistMember.Id = index;
                    joistMember.SheetName = this.JobModel.ActiveFloorSheet.Name;
                    this.JobModel.ActiveFloorSheet.Joists.Add(joistMember);
                    var joists = new Joist2D(Plane.XY, startPoint, endPoint, joistMember,45);
                    //var beam = new Beam2D(Plane.XY, startPoint, endPoint, 45, false, true);
                    joists.Color = this.LayerManager.SelectedLayer.Color;
                    joists.ColorMethod = colorMethodType.byEntity;
                    this.EntitiesManager.AddAndRefresh(joists, LayerManager.SelectedLayer.Name);
                }
                
            }
        }

    }
}
