using Microsoft.EntityFrameworkCore;
using Musaca.Data;
using Musaca.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Musaca.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly MusacaDbContext context;

        public OrdersService(MusacaDbContext dbContext)
        {
            this.context = dbContext;
        }

        public bool AddProductToCurrentActiveOrder(string productId, string userId)
        {
            return true;
        }

        public Order CreateOrder(Order order)
        {
            this.context.Orders.Add(order);
            this.context.SaveChanges();

            return order;
        }

        public Order CompleteOrder(string orderId, string userId)
        {
            var orderFromDb = this.context.Orders.SingleOrDefault(order => order.Id == orderId);

            orderFromDb.IssuedOn = DateTime.UtcNow;
            orderFromDb.Status = OrderStatus.Completed;

            this.context.Update(orderFromDb);
            this.context.SaveChanges();

            this.CreateOrder(new Order { CashierId = userId });

            return orderFromDb;
        }

        public List<Order> GetAllCompletedOrdersByCashierId(string userId)
        {
            return new List<Order>();
        }

        public Order GetCurrentActiveOrderByCashierId(string userId)
        {
            return new Order();
        }
    }
}
