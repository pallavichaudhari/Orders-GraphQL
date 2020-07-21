using GraphQL.Types;
using Orders.Models;
using Orders.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orders.Schema
{
    public class OrderType : ObjectGraphType<Order>
    {
        public OrderType(ICustomerService customers)
        {
            Field(o => o.Id);
            Field(o => o.Name);
            Field(o => o.Description);
            Field<CustomerType>("customers", 
                resolve: context => customers.GetCustomerByIdAsync(
                    context.Source.Customerid));
            Field(o => o.Created);
            Field<OrderStatusEnumType>("status",
                resolve: context => context.Source.Status);
        }
    }
}
