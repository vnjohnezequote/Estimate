﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;
using Prism.Mvvm;

namespace AppModels.ViewModelEntity
{
    [Serializable]
    public class EntityVm: EntityVmBase
    {
        public EntityVm(Entity entity): base(entity)
        {
        }

    }
}
