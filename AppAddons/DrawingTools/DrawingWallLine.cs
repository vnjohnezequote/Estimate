// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawingLine.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the CanvasDrawing type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using ApplicationService;
using AppModels.CustomEntity;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawingModule.CommandClass;

namespace AppAddons.DrawingTools
{
    /// <summary>
    /// The my model.
    /// </summary>
    public  class DrawingWallLine: DrawingLine
    {
        public override string ToolName => "Drawing Wall Line";
        #region Constructor

        public DrawingWallLine(): base()
        {
        }
        #endregion

        [CommandMethod("WallLine")]
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
                if (Points.Count>0)
                {
                    var index = Points.Count - 1;
                    BasePoint = (Point3D)Points[index]; ;
                    IsUsingOrthorMode = true;
                }
                if (this.Points.Count < 2) continue;
                var index2 = Points.Count - 1;
                var startPoint = (Point3D)Points[index2 - 1].Clone();
                var endPoint = (Point3D) Points[index2].Clone();
                var line = new Line(startPoint,endPoint);
                line.LineTypeMethod = colorMethodType.byLayer;
                {
                    if (LayerManager.SelectedLayer.LineTypeName!="Continues")
                    {
                        line.LineTypeName = LayerManager.SelectedLayer.LineTypeName;
                    }
                }
                line.Color = LayerManager.SelectedLayer.Color;
                var wall2D = new WallLine2D(line);
                wall2D.WallLevelName = this.ActiveLevel;
                wall2D.LineTypeScale = 1;
                //wall2D.WallLevelName = "Level 1";
                this.EntitiesManager.AddAndRefresh(wall2D,this.LayerManager.SelectedLayer.Name);
            }
        }
        #region Implement IDrawInteractive

        #endregion

    }
}
