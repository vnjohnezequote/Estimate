﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;

namespace AppModels.Undo.EntityRollBack
{
    public abstract class FramingRectangle2DRollBack: Framing2DRollBack
    {
        public FramingRectangle2DRollBack(Entity entity) : base(entity)
        {
        }
    }
}