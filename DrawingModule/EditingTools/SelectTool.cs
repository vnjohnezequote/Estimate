using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Input;
using ApplicationInterfaceCore;
using AppModels.EventArg;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using DrawingModule.Application;
using DrawingModule.CustomControl.CanvasControl;
using DrawingModule.DrawToolBase;
using DrawingModule.Enums;
using DrawingModule.Interface;

namespace DrawingModule.EditingTools
{
    public sealed class SelectTool: ToolBase
    {
        //private ObservableCollection<Entity> _selectedEntities;
        public PickState CurrentPickState { get; set; }
        public Point3D StartPoint { get; set; }
        public Point3D EndPoint { get; set; }
        //public ObservableCollection<Entity> SelectedEntities => this._selectedEntities;
        public DrawInteractiveDelegate DrawInteractiveHandler { get; private set; }
        public SelectTool()
        {
            this.CurrentPickState = PickState.Pick;
            //this._selectedEntities = new ObservableCollection<Entity>();
            IsSnapEnable = false;
            DrawInteractiveHandler += DrawInteractiveSelect;
        }
        public void ProcessMouseDownForSelection(MouseButtonEventArgs e, bool isSelected, CanvasDrawing canvasDrawing)
        {
            if (isSelected)
            {
                this.ProcessSelection(e,true,canvasDrawing);
                return;
            }
            if (this.StartPoint == null)
            {
                canvasDrawing.ScreenToPlane(RenderContextUtility.ConvertPoint(canvasDrawing.GetMousePosition(e)), canvasDrawing.DrawingPlane, out var clickPoint);
                StartPoint = clickPoint;
            }
            else
            {
                canvasDrawing.ScreenToPlane(RenderContextUtility.ConvertPoint(canvasDrawing.GetMousePosition(e)), canvasDrawing.DrawingPlane, out var clickPoint);
                this.EndPoint = clickPoint;
                this.ProcessSelection(e, false,canvasDrawing);
                
            }
        }
        public void ProcessMouseMoveForSelection(MouseEventArgs e, CanvasDrawing canvas)
        {
            canvas.ScreenToPlane(RenderContextUtility.ConvertPoint(canvas.GetMousePosition(e)), canvas.DrawingPlane, out var currentPoint);
            
            int diffX = (int)(currentPoint.X - this.StartPoint.X);
            if (diffX > 10)
            {
                this.CurrentPickState = PickState.Enclose;
            }
            else if (diffX < -10)
            {
                this.CurrentPickState = PickState.Cross;
            }
            else
            {
                this.CurrentPickState = PickState.Pick;
            }
        }

        public void ProcessEscapeTool()
        {
            this.BasePoint = null;
            this.CurrentPickState = PickState.Enclose;
            this.StartPoint = null;
        }
        public void ProcessSelection(MouseButtonEventArgs e,bool isSelected, CanvasDrawing canvas)
        {
            var currentLocation = RenderContextUtility.ConvertPoint(canvas.GetMousePosition(e));
            var added = new List<int>();
            var removed = new List<int>();


            IList<Entity> myEnts = canvas.Entities.CurrentBlockReference != null
                ? canvas.Blocks[canvas.Entities.CurrentBlockReference.BlockName].Entities
                : new List<Entity>(canvas.Entities);

            int[] ents;

            //if (Keyboard.Modifiers == ModifierKeys.Shift)
            //{
            //    for (int i = 0; i < myEnts.Count; i++)
            //    {
            //        if (myEnts[i].Selected)
            //            removed.Add(i);

            //        myEnts[i].Selected = false;
            //    }
            //}

            if (isSelected)
            {
                var ent = canvas.GetEntityUnderMouseCursor(currentLocation);

                if (ent >= 0)
                {
                    ManageSelection(ent, myEnts, added, removed);
                }
                return;
            }

            var startPoint = canvas.WorldToScreen(this.StartPoint);
            var endPoint = canvas.WorldToScreen(this.EndPoint);

            var startPointForRec = new Point3D(startPoint.X, canvas.Size.Height - startPoint.Y);
            var endPointForRec = new Point3D(endPoint.X, canvas.Size.Height - endPoint.Y);

            int dx = (int)(endPointForRec.X - startPointForRec.X);
            int dy = (int)(endPointForRec.Y - startPointForRec.Y);
           

            NormalizeBox(ref startPointForRec, ref endPointForRec);
            var p1 = new System.Drawing.Point((int)startPointForRec.X, (int)startPointForRec.Y);

            switch (CurrentPickState)
            {
                case PickState.Cross:
                    if (dx != 0 && dy != 0)
                    {
                        ents =
                            canvas.GetAllCrossingEntities(new System.Drawing.Rectangle(p1,
                                new System.Drawing.Size(Math.Abs(dx), Math.Abs(dy))));

                        foreach (var ent1 in ents)
                        {
                            ManageSelection(ent1, myEnts, added, removed);
                        }
                    }
                    break;
                case PickState.Enclose:
                    if (dx != 0 && dy != 0)
                    {
                        ents =
                            canvas.GetAllEnclosedEntities(new System.Drawing.Rectangle(p1,
                                new System.Drawing.Size(Math.Abs(dx), Math.Abs(dy))));

                        foreach (var ent2 in ents)
                        {
                            ManageSelection(ent2, myEnts, added, removed);
                        }
                    }
                    break;
            }

            this.ResetSelectionTool();
            
        }
        private void ResetSelectionTool()
        {
            this.CurrentPickState = PickState.Pick;
            this.StartPoint = null;
            this.EndPoint = null;
        }
        internal static void NormalizeBox(ref Point3D p1, ref Point3D p2)
        {

            var firstX = (int)Math.Min(p1.X, p2.X);
            var firstY = (int)Math.Min(p1.Y, p2.Y);
            var secondX = (int)Math.Max(p1.X, p2.X);
            var secondY = (int)Math.Max(p1.Y, p2.Y);

            p1.X = firstX;
            p1.Y = firstY;
            p2.X = secondX;
            p2.Y = secondY;
        }
        private void ManageSelection(int ent, IList<Entity> myEnts, List<int> added, List<int> removed)
        {
            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                myEnts[ent].Selected = false;
                //this.SelectedEntities.Remove(myEnts[ent]);
                this.EntitiesManager.SelectedEntities.Remove(myEnts[ent]);
                return;
            }
            myEnts[ent].Selected = true;
            this.EntitiesManager.SelectedEntities.Add(myEnts[ent]);
            //this.SelectedEntities.Add(myEnts[ent]);
        }
        public void DrawInteractiveSelect(ICadDrawAble drawTable, DrawInteractiveArgs drawInteractiveArgs)
        {
            if (this.StartPoint==null || this.StartPoint == drawInteractiveArgs.CurrentPoint ) return;
            switch (this.CurrentPickState)
            {
                case PickState.Cross:
                    DrawSelectionBox(StartPoint, drawInteractiveArgs.CurrentPoint, Color.DarkBlue, true, true,drawTable);
                    break;
                case PickState.Enclose:
                    DrawSelectionBox(StartPoint, drawInteractiveArgs.CurrentPoint, Color.DarkRed, true, false,drawTable);
                    break;
                default:
                    break;
            }
        }
        private void DrawSelectionBox(Point3D p1, Point3D p2, Color transparentColor, bool drawBorder, bool dottedBorder, ICadDrawAble canvas)
        {
            //var pp1 = p1;
            //var pp2 = p2;
            //pp1.Y = (int)(Size.Height - pp1.Y);
            //pp2.Y = (int)(Size.Height - pp2.Y);

            var startPoint = canvas.WorldToScreen(p1);
            var endPoint = canvas.WorldToScreen(p2);

            var startPointForRec = new Point3D(startPoint.X, (startPoint.Y));
            var endPointForRec = new Point3D(endPoint.X, (endPoint.Y));

            NormalizeBox(ref startPointForRec, ref endPointForRec);
            
            //Adjust the bounds so that it doesn't exit from the current viewport frame

            int[] viewFrame = canvas.Viewports[canvas.ActiveViewport].GetViewFrame();
            int left = viewFrame[0];
            int top = viewFrame[1] + viewFrame[3];
            int right = left + viewFrame[2];
            int bottom = viewFrame[1];

            if (endPointForRec.X > right - 1)
                endPointForRec.X = right - 1;

            if (endPointForRec.Y > top - 1)
                endPointForRec.Y = top - 1;

            if (startPointForRec.X < left + 1)
                startPointForRec.X = left + 1;

            if (startPointForRec.Y < bottom + 1)
                startPointForRec.Y = bottom + 1;




            canvas.renderContext.SetState(blendStateType.Blend);
            canvas.renderContext.SetColorWireframe(System.Drawing.Color.FromArgb(40, transparentColor.R, transparentColor.G, transparentColor.B));
            canvas.renderContext.SetState(rasterizerStateType.CCW_PolygonFill_CullFaceBack_NoPolygonOffset);

            //int w = (int)(pp2.X - pp1.X);
            //int h = (int)(pp2.Y - pp1.Y);

            var w1 = (int)(endPointForRec.X - startPointForRec.X);
            var h1 = (int)(endPointForRec.Y - startPointForRec.Y);




            canvas.renderContext.DrawQuad(new System.Drawing.RectangleF((float)(startPointForRec.X + 1), (float)(startPointForRec.Y + 1), w1 - 1, h1 - 1));
            canvas.renderContext.SetState(blendStateType.NoBlend);

            if (drawBorder)
            {
                canvas.renderContext.SetColorWireframe(System.Drawing.Color.FromArgb(255, transparentColor.R, transparentColor.G, transparentColor.B));

                List<Point3D> pts = null;

                if (dottedBorder)
                {
                    canvas.renderContext.SetLineStipple(1, 0x0F0F, canvas.Viewports[0].Camera);
                    canvas.renderContext.EnableLineStipple(true);
                }

                var l = (int)startPoint.X;
                var r = (int)endPoint.X;
                

                pts = new List<Point3D>(new Point3D[]
                {
                    new Point3D(l, startPoint.Y), new Point3D(endPoint.X, startPoint.Y),
                    new Point3D(r, startPoint.Y), new Point3D(r, endPoint.Y),
                    new Point3D(r, endPoint.Y), new Point3D(l, endPoint.Y),
                    new Point3D(l, endPoint.Y), new Point3D(l, startPoint.Y),
                });


                canvas.renderContext.DrawLines(pts.ToArray());
                
                if (dottedBorder)
                    canvas.renderContext.EnableLineStipple(false);
            }
        }

        public override Point3D BasePoint { get; protected set; }
    }
}
