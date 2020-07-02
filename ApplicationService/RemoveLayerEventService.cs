using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AppModels.AppData;
using Prism.Events;

namespace ApplicationService
{
    public class RemoveLayerEventService : PubSubEvent<LayerItem>
    {
    }
}
