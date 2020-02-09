using Musaca.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Result;

namespace Musaca.App.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrdersService orderService;

        public OrdersController(IOrdersService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public IActionResult Cashout()
        {
            var currentActiveOrder = this.orderService.GetCurrentActiveOrderByCashierId(this.User.Id);

            this.orderService.CompleteOrder(currentActiveOrder.Id, this.User.Id);

            return this.Redirect("/");
        }
    }
}
