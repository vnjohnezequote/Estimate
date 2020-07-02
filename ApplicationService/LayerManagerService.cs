using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using AppModels.AppData;
using Prism.Events;

namespace ApplicationService
{
    public class LayerManagerService: PubSubEvent<LayerItem>
    {
    }
}
