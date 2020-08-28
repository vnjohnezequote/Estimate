using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot.Entities;
using Prism.Events;

namespace ApplicationService
{
    public class EntityService: PubSubEvent<Entity>
    {
    }
}
