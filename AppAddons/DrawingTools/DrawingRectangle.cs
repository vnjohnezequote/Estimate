﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ApplicationInterfaceCore;
using ApplicationInterfaceCore.Enums;
using ApplicationService;
using AppModels.CustomEntity;
using AppModels.Enums;
using AppModels.EventArg;
using AppModels.Interaface;
using AppModels.Undo;
using AppModels.Undo.Backup;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;
using DrawingModule.DrawToolBase;
using DrawingModule.UserInteractive;

namespace AppAddons.DrawingTools
{
    public class DrawingRectangle : ToolBase
    {
        //public DrawInteractiveDelegate DrawInteractiveHandler { get; private set; }
        private double _currentWidth;
        private double _currentHeight;
        public override string ToolName => "Draw Rectangle";
        public sealed override string ToolMessage => BasePoint==null
            ? "Please enter start point. Escape to break tool"
            : "Please enter end point. Escape to break tool";

        public override double CurrentWidth { get=>_currentWidth; set=>SetProperty(ref _currentWidth,value); }
        public override double CurrentHeight { get=>_currentHeight; set=>SetProperty(ref _currentHeight,value); }
        public override Point3D BasePoint { get; protected set; }
        public Point3D EndPoint { get; protected set; }
        
        #region Constructor

        public DrawingRectangle()
        {
            IsUsingOrthorMode = false;
            IsUsingHeightTextBox = true;
            IsUsingWidthTextBox = true;
            DefaultDynamicInputTextBoxToFocus = FocusType.Width;

        }
        #endregion
        [CommandMethod("Rectangle")]
        public void DrawRectangle()
        {
            var acDoc = DrawingModule.Application.Application.DocumentManager.MdiActiveDocument;
            var promptPointOp = new PromptPointOptions(ToolMessage);
            //acDoc.Editor.FocusToLengthTextBox();
            DynamicInput?.FocusWidth();
            while (true)
            {
                var res = acDoc.Editor.GetPoint(promptPointOp);
                if (res.Status == PromptStatus.Cancel)
                {
                    //Application.DocumentManager.MdiActiveDocument.Editor.UnRegisterDrawInteractive(this);
                    return;
                }

                if (BasePoint == null)
                {
                    BasePoint = res.Value;
                }
                else
                {
                    EndPoint = res.Value;
                }

                if (BasePoint !=null && EndPoint !=null)
                {
                    var point0 = (Point3D)BasePoint.Clone();
                    var point2 = (Point3D)EndPoint.Clone();
                    var point1 = new Point3D(point2.X, point0.Y);
                    var point3 = new Point3D(point0.X, point2.Y);
                    var rectanglePoints = new List<Point3D>
                    {
                        point0,
                        point1,
                        point2,
                        point3,
                        (Point3D) point0.Clone()
                    };

                    var rectangle = new LinearPath(rectanglePoints);

                    rectangle.LineTypeMethod = colorMethodType.byLayer;
                    {
                        if (LayerManager.SelectedLayer.LineTypeName != "Continues")
                        {
                            rectangle.LineTypeName = LayerManager.SelectedLayer.LineTypeName;
                        }
                    }

                    rectangle.Color = LayerManager.SelectedLayer.Color;
                    var wall2D = new LinearPathWall2D(rectangle);
                    wall2D.WallLevelName = this.ActiveLevel;
                    wall2D.LineTypeScale = 50;
                    var undoList = new UndoList() { ActionType = ActionTypes.Add };
                    var backup = BackupEntitiesFactory.CreateBackup(wall2D, undoList, EntitiesManager);
                    backup?.Backup();
                    UndoEngineer.SaveSnapshot(undoList);
                    this.EntitiesManager.AddAndRefresh(wall2D, this.LayerManager.SelectedLayer.Name);
                    BasePoint = EndPoint = null;
                    CurrentWidth = 0;
                    CurrentHeight = 0;
                }
                
                //acDoc.Editor.CanvasDrawing.AddAndRefresh(line);
                //DynamicInput?.FocusDynamicInputTextBox(this.DefaultDynamicInputTextBoxToFocus);
            }
        }
        #region Implement IDrawInteractive
        public override void NotifyMouseMove(object sender, MouseEventArgs e)
        {
            DynamicInput?.FocusDynamicInputTextBox(FocusType.Previous);
        }
        protected override void OnMoveNextTab()
        {
            if (this.DynamicInput == null) return;
                switch (DynamicInput.PreviusDynamicInputFocus)
            {
                case FocusType.Width:
                    DynamicInput.FocusHeight();
                    break;
                case FocusType.Height:
                    DynamicInput.FocusWidth();
                    break;
            }
        }
        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {

            DrawInteractiveRectangle((ICadDrawAble)sender, e);
        }
        public void DrawInteractiveRectangle(ICadDrawAble drawTable, DrawInteractiveArgs e)
        {
            if (e.CurrentPoint == null || BasePoint == null)
            {
                return;
            }

            drawTable.renderContext.SetColorWireframe(LayerManager.SelectedLayer.Color);
            drawTable.renderContext.SetLineSize(LayerManager.SelectedLayer.LineWeight);

            //drawTable.renderContext.EnableXOR(true);

            var startPoint = drawTable.WorldToScreen(BasePoint);
            //var endPoint = WorldToScreen(this.GetEndPoint(_clickPoints[0], CurrentPoint));
            var endPoint = drawTable.WorldToScreen(e.CurrentPoint);
            //this.renderContext.DrawLine(startPoint, endPoint);
            double minX, minY, maxX, maxY;
            if (startPoint.X < endPoint.X)
            {
                minX = startPoint.X;
                maxX = endPoint.X;

            }
            else
            {
                minX = endPoint.X;
                maxX = startPoint.X;
            }

            if (startPoint.Y < endPoint.Y)
            {
                minY = startPoint.Y;
                maxY = endPoint.Y;
            }
            else
            {
                minY = endPoint.Y;
                maxY = startPoint.Y;
            }

            var bottomLeft = new Point3D(minX, minY);
            var bottomRight = new Point3D(maxX, minY);
            var topLeft = new Point3D(minX, maxY);
            var topRight = new Point3D(maxX, maxY);
            //double l = startPoint.X;
            //double r = endPoint.X;

            var pts = new List<Point3D>(new Point3D[]
            {
                //new Point3D(l, startPoint.Y), new Point3D(endPoint.X, startPoint.Y),
                //new Point3D(r, startPoint.Y), new Point3D(r, endPoint.Y),
                //new Point3D(r, endPoint.Y), new Point3D(l, endPoint.Y),
                //new Point3D(l, endPoint.Y), new Point3D(l, startPoint.Y)
                bottomLeft,bottomRight,topRight,topLeft,bottomLeft
            });
            drawTable.renderContext.EnableXOR(false);
            //renderContext.DrawLines(pts.ToArray());
            drawTable.renderContext.DrawLineStrip(pts.ToArray());
            //this.DrawSpotOffset(topLeft, topRight, this._textBlockWidth);
            //this.DrawSpotOffset(bottomLeft, topLeft, this._textBlockHeight);



        }
    #endregion
    }
}
