using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Orders.Services;

namespace Orders.Schema
{
    public class OrderQuery:ObjectGraphType<object>
    {
        public OrderQuery(IOrderService orders, ICustomerService customers)
        {
            Name = "Query";
            Field<ListGraphType<OrderType>>(
                "orders",
                resolve: context => orders.GetOrdersAsync()
            );

            FieldAsync<OrderType>(
                "orderById",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "orderId" }),
                resolve: async context => {
                    return await context.TryAsyncResolve(
                        async c => await orders.GetOrderByIdAsync(c.GetArgument<String>("orderId"))
                    );
                }
            );

            Field<ListGraphType<CustomerType>>(
                "customers",
                resolve: context => customers.GetCustomersAsync()
            );

        }
    }
}
