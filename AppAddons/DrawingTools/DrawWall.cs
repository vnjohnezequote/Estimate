﻿using System;
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
using AppModels.NewReposiveData;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Labels;
using devDept.Geometry;
using devDept.Graphics;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;
using GeometryGym.Ifc;
//using TriangleNet.Geometry;
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
            
        }
        #endregion
        [CommandMethod("Wall2D")]
        public void DrawWallLine()
        {
            
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            DynamicInput?.FocusLength();
            while (true)
            {
                InsideLengthDistance = this.JobModel.SelectedWallThickness;
                if (JobModel.CCMode)
                {
                    this.IsUsingInsideLength = true;
                }
                else
                {
                    this.IsUsingInsideLength = false;
                }
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
                var wallThickness = 90;
                if (JobModel.SelectedWallThickness!=0)
                {
                    wallThickness = JobModel.SelectedWallThickness;
                }
                var wall = new Wall2D(Plane.XY, startPoint,endPoint,wallThickness,JobModel.CurrentIsLoadBearingWall,JobModel.CCMode,true);
                //wall.Color = Color.FromArgb(127, Color.CornflowerBlue);
                wall.Color = Color.CornflowerBlue;
                //wall.Color = Color.CadetBlue;
                wall.ColorMethod = colorMethodType.byEntity;
                wall.LineTypeName = "Dash Space";
                wall.LineTypeMethod = colorMethodType.byEntity;
                //var wall2 = new Wall2D(wall);
                //wall.IsBeamUnder = false;
                //wall.LineTypeName = "";
                this.EntitiesManager.AddAndRefresh(wall,this.LayerManager.SelectedLayer.Name);
            }
        }
        #region Implement IDrawInteractive



        #endregion
    }
}
