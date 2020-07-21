using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orders.Models;


namespace Orders.Services
{
    public class OrderService : IOrderService
    {
        public IList<Order> _orders;

        public readonly IOrderEventService _events;
        public OrderService(IOrderEventService orderEventService)
        {
            _orders = new List<Order>();

            _orders.Add(new Order("1000", "250 Conference Brochures", DateTime.Now, 1, "76BACF8A - EA9D - 4C27 - A5BC - 2A698757B548"));
            _orders.Add(new Order("2000", "250 T-Shirts", DateTime.Now, 2, "7E49CD02-400E-427B-8DC9-A979D0AC168C"));
            _orders.Add(new Order("3000", "500 Stickers", DateTime.Now, 3, "7B8BC9EF-EDAA-4583-9F4C-B43B88488DC4"));
            _orders.Add(new Order("4000", "10 Posters", DateTime.Now, 4, "B27764F7-E3B2-49BE-9173-EAD6B5D1E811"));

            this._events = orderEventService;
        }
        private Order GetById(string id)
        {
            var order = _orders.SingleOrDefault(o => Equals(o.Id, id));
            if (order == null)
            {
                throw new ArgumentException(string.Format("Order ID '{0}' is invalid", id));
            }
            return order;
        }

        public Task<Order> CreateAsync(Order order)
        {
            _orders.Add(order);
            var orderEvent = new OrderEvent(order.Id, order.Name, OrderStatuses.CREATED, DateTime.Now);

            _events.AddEvent(orderEvent);

            return Task.FromResult(order);
        }

        public Task<Order> GetOrderByIdAsync(string id)
        {
            return Task.FromResult(_orders.Single(o => Equals(o.Id, id)));
        }

        public Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return Task.FromResult(_orders.AsEnumerable());
        }

        public Task<Order> StartAsync(string orderId)
        {
            var order = GetById(orderId);
            order.Start();
            var orderEvent = new OrderEvent(order.Id, order.Name, OrderStatuses.PROCESSING, DateTime.Now);

            _events.AddEvent(orderEvent);

            return Task.FromResult(order);
        }
    }

    public interface IOrderService
    {
        Task<Order> GetOrderByIdAsync(string id);
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order> CreateAsync(Order order);
        Task<Order> StartAsync(string orderId);
    }
}
