using Musaca.Models;
using System.Collections.Generic;

namespace Musaca.Services
{
    public interface IOrdersService
    {
        bool AddProductToCurrentActiveOrder(string productId, string userId);

        Order CreateOrder(Order order);

        Order CompleteOrder(string orderId, string userId);

        List<Order> GetAllCompletedOrdersByCashierId(string userId);

        Order GetCurrentActiveOrderByCashierId(string userId);
    }
}
