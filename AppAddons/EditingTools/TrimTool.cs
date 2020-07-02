using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using ApplicationInterfaceCore;
using AppModels.EventArg;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using DrawingModule.CommandClass;
using DrawingModule.DrawInteractiveUtilities;
using DrawingModule.DrawToolBase;
using GeometryGym.Ifc;

namespace AppAddons.EditingTools
{
    public class TrimTool : ToolBase
    {
        public override Point3D BasePoint { get; protected set; }
        public override string ToolName => "Trim";
        private Entity _trimedEntity;
        private Point3D _trimPoint;
        private bool _processingTool;
        private Entity _firstSelectedEntity;
        private Entity _secondSelectedEntity;
        private int _selectedEntityIndex;
        public TrimTool()
        {
            _processingTool = true;
            IsSnapEnable = false;
            IsUsingOrthorMode = false;
            _selectedEntityIndex = -1;
        }

        [CommandMethod("Trim")]
        public void Trim()
        {
            
            OnProcessCommand();
        }
        protected virtual void OnProcessCommand()
        {
            while (_processingTool)
            {
                
            }
            
        }

        private void ProcessTrimedEntities()
        {
            
        }


        public override void OnJigging(object sender, DrawInteractiveArgs e)
        {
            DrawInteractiveTrim((ICadDrawAble)sender,e);

        }

        public override void NotifyMouseDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = (ICadDrawAble)sender;
            var mousePosition = RenderContextUtility.ConvertPoint(canvas.GetMousePosition(e));
            var index = canvas.GetEntityUnderMouseCursor(mousePosition);
            if (_firstSelectedEntity != null && _secondSelectedEntity != null) return;
            if (index >-1)
            {
                _selectedEntityIndex = index;
            }

            if (_secondSelectedEntity != null) return;
            canvas.ScreenToPlane(mousePosition, canvas.DrawingPlane, out var clickPoint);
            _trimPoint = clickPoint;
        }

        public override void NotifyPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this._processingTool = false;
                e.Handled = true;
            }
        }

        private void DrawInteractiveTrim(ICadDrawAble canvas, DrawInteractiveArgs e)
        {

            if (_firstSelectedEntity == null)
            {
                DrawInteractiveUntilities.DrawSelectionMark(canvas,e.MousePosition);
                ToolMessage = "Please select trim entity";
                if (_selectedEntityIndex != -1)
                {
                    _firstSelectedEntity = EntitiesManager.GetEntity(_selectedEntityIndex);
                    _selectedEntityIndex = -1;
                    //return;
                }
            }
            else if (_secondSelectedEntity == null)
            {
                if (_selectedEntityIndex != -1)
                {
                    _secondSelectedEntity = EntitiesManager.GetEntity(_selectedEntityIndex);
                }
                else
                {
                    DrawInteractiveUntilities.DrawSelectionMark(canvas,e.MousePosition);
                    ToolMessage = "Please select Trimed Entity";
                }
            }
            else if (_firstSelectedEntity != null && _secondSelectedEntity != null)
            {
                if (_firstSelectedEntity is ICurve trimmingCurve && _secondSelectedEntity is ICurve curve)
                {
                   
                    Point3D[] intersetionPoints = Curve.Intersection(trimmingCurve, curve);
                    if (intersetionPoints.Length > 0 && _trimPoint !=null)
                    {
                        List<double> parameters = new List<double>();
                        for (int i = 0; i < intersetionPoints.Length; i++)
                        {
                            var intersetionPoint = intersetionPoints[i];
                            double t = ((InterPoint)intersetionPoint).s;
                            parameters.Add(t);
                        }

                        double distSelected = 1;

                        ICurve[] trimmedCurves = null;
                        if (parameters != null)
                        {
                            parameters.Sort();
                            double u;
                            curve.ClosestPointTo(_trimPoint, out u);
                            distSelected = Point3D.Distance(_trimPoint, curve.PointAt(u));
                            distSelected += distSelected / 1e3;

                            if (u <= parameters[0])
                            {
                                curve.SplitBy(new Point3D[] { curve.PointAt(parameters[0]) }, out trimmedCurves);
                            }
                            else if (u > parameters[parameters.Count - 1])
                            {
                                curve.SplitBy(new Point3D[] { curve.PointAt(parameters[parameters.Count - 1]) },
                                              out trimmedCurves);
                            }
                            else
                            {
                                for (int i = 0; i < parameters.Count - 1; i++)
                                {
                                    if (u > parameters[i] && u <= parameters[i + 1])
                                    {
                                        curve.SplitBy(
                                            new Point3D[] { curve.PointAt(parameters[i]), curve.PointAt(parameters[i + 1]) },
                                            out trimmedCurves);
                                    }
                                }
                            }
                        }

                        bool success = false;
                        //Decide which portion of curve to be deleted
                        for (int i = 0; i < trimmedCurves.Length; i++)
                        {
                            ICurve trimmedCurve = trimmedCurves[i];
                            double t;

                            trimmedCurve.ClosestPointTo(_trimPoint, out t);
                            {

                                if ((t < trimmedCurve.Domain.t0 || t > trimmedCurve.Domain.t1)
                                    || Point3D.Distance(_trimPoint, trimmedCurve.PointAt(t)) > distSelected)
                                {
                                    var tempEntity = (Entity) trimmedCurve;
                                    EntitiesManager.AddAndRefresh(tempEntity,tempEntity.LayerName);
                                    success = true;
                                }
                            }
                        }

                        // Delete original entity to be trimmed
                        if (success)
                            EntitiesManager.RemoveEntity(_secondSelectedEntity);
                    }
                    _firstSelectedEntity = null;
                    _secondSelectedEntity = null;
                    _trimPoint = null;

                }
            }


        }
    }
}
