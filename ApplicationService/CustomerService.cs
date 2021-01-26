using AppModels.PocoDataModel;
using Prism.Events;

namespace ApplicationService
{
    public class CustomerService : PubSubEvent<ClientPoco>
    {
    }
}
