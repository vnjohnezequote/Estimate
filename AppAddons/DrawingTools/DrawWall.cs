using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ApplicationInterfaceCore;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels;
using AppModels.CustomEntity;
using AppModels.EventArg;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;
using Region = devDept.Eyeshot.Entities.Region;

namespace AppAddons.DrawingTools
{
    public class DrawWall: DrawingLine
    {
        //public DrawInteractiveDelegate DrawInteractiveHandler { get; private set; }
        public override string ToolName => "Drawing 2D Wall";
        #region Constructor

        public DrawWall() : base()
        {
            IsUsingInsideLength = true;
            InsideLengthDistance = 90;
        }
        #endregion
        [CommandMethod("Wall2D")]
        public void DrawWallLine()
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
                //var wallCenterSegment = new Segment2D(startPoint,endPoint);
                //Utility.OffsetPoint(startPoint, endPoint, 90, out var endWallPoint);
                var wall = new WallRegion(Plane.XY, startPoint,startPoint,endPoint);
                wall.Color = Color.FromArgb(127, Color.CornflowerBlue);
                wall.ColorMethod = colorMethodType.byEntity;
                wall.LineTypeName = "Dash Space";
                wall.LineTypeMethod = colorMethodType.byEntity;
                wall.IsLoadBearingWall = false;
                //wall.LineTypeName = "";
                this.EntitiesManager.AddAndRefresh(wall,this.LayerManager.SelectedLayer.Name);
                //Region region = WallRegion.CreateRectangle(Plane.XY, 2000, 90,true);
                //this.EntitiesManager.AddAndRefresh(region,this.LayerManager.SelectedLayer.Name);
                //var wall = new Wall2D(startPoint,endPoint,startPoint,);

                //wall.LineTypeMethod = colorMethodType.byLayer;
                //{
                //    if (LayerManager.SelectedLayer.LineTypeName != "Continues")
                //    {
                //        wall.LineTypeName = LayerManager.SelectedLayer.LineTypeName;
                //    }
                //}
                //wall.Color = Color.FromArgb(127,Color.CadetBlue);
                //wall.ColorMethod = colorMethodType.byEntity;
                //wall.LineTypeMethod = colorMethodType.byEntity;
                //wall.LineWeight = 1;
                //this.EntitiesManager.AddAndRefresh(wall, this.LayerManager.SelectedLayer.Name);
            }
        }
        #region Implement IDrawInteractive

        #endregion
    }
}
