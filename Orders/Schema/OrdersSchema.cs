using GraphQL;

namespace Orders.Schema
{
    public class OrdersSchema : GraphQL.Types.Schema
    {
        public OrdersSchema(OrderQuery query, IDependencyResolver resolver, OrdersMutation mutation, OrdersSubscription ordersSubscription)
        {
            Query = query;
            DependencyResolver = resolver;
            Mutation = mutation;
            Subscription = ordersSubscription;
        }
    }
}
