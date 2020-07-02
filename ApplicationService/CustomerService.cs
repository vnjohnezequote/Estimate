using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using AppModels;
using Prism.Events;

namespace ApplicationService
{
    public class CustomerService : PubSubEvent<Client>
    {
    }
}
