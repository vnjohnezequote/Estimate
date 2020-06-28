﻿using System.ComponentModel;
using System.Drawing;
using ApplicationInterfaceCore;
using devDept.Eyeshot;
using devDept.Geometry;
using devDept.Graphics;

namespace DrawingModule.Interface
{
    public interface ICadDrawAble : INotifyPropertyChanged
    {
        ViewportList Viewports { get; }
        int ActiveViewport { get; }
        Size Size{ get; }
        Point3D LastClickPoint { get; }
        Plane DrawingPlane { get; }
        int PickBoxSize { get; }
        int[] GetAllEntitiesUnderMouseCursor(System.Drawing.Point mousePosition, bool selectTableOnly = true);
        EntityList Entities { get; }
        Point3D WorldToScreen(Point3D point);
        RenderContextBase renderContext { get; }
        double CurrentLengthDimension { get; }
        double CurrentAngleDimension { get; }
        IDynamicInputView DynamicInput { get; }

        void UpdateCurrentPointByLengthAndAngle(double length, double angle);
        bool ScreenToPlane(System.Drawing.Point mousePosition,Plane sketchPlant, out Point3D point);
    }
}