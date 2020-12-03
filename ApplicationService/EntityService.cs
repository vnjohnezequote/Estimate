using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Interaface;
using Prism.Events;

namespace ApplicationService
{
    public class EntityService: PubSubEvent<IEntityVm>
    {
    }
}
