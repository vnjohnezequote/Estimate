﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.AppData;
using Prism.Events;

namespace ApplicationService
{
    public class AddNewLayerEventService : PubSubEvent<LayerItem>
    {
    }
}
