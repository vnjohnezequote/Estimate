﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Geometry;
using DrawingModule.DrawToolBase;

namespace AppAddons.EditingTools
{
    public class MirrorTool : ToolBase
    {
        public override Point3D BasePoint { get; protected set; }
    }
}
