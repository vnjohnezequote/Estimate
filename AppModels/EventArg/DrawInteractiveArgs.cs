﻿using System.Collections.Generic;
using devDept.Eyeshot;
using devDept.Geometry;

namespace AppModels.EventArg
{
    public class DrawInteractiveArgs
    {
        public Point3D CurrentPoint { get; private set; }
        public System.Drawing.Point MousePosition { get; private set; }
        public Point3D LastClickPoint { get; private set; }
        public List<Point3D> ClickPoints { get; private set; }
        public Environment.DrawSceneParams DrawData { get; private set; }

        public DrawInteractiveArgs(Environment.DrawSceneParams data)
        {
            this.DrawData = data;
        }
        public DrawInteractiveArgs(Point3D currentPoint, Point3D lastClickPoint, Environment.DrawSceneParams data, System.Drawing.Point mousePosition)
        {
            this.CurrentPoint = currentPoint;
            this.LastClickPoint = lastClickPoint;
            this.DrawData = data;
            this.MousePosition = mousePosition;
        }

    }
}
