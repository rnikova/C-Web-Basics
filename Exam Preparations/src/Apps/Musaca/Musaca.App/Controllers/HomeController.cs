using Musaca.App.ViewModels.Orders;
using Musaca.App.ViewModels.Products;
using Musaca.Models;
using Musaca.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Mapping;
using SIS.MvcFramework.Result;

namespace Musaca.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOrdersService orderService;

        public HomeController(IOrdersService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet(Url = "/")]
        public IActionResult IndexSlash()
        {
            return this.Index();
        }

        public IActionResult Index()
        {
            var orderHomeViewModel = new OrderHomeViewModel();

            if (this.IsLoggedIn())
            {
                var order = this.orderService
                    .GetCurrentActiveOrderByCashierId(this.User.Id);

                orderHomeViewModel = order.To<OrderHomeViewModel>();

                orderHomeViewModel.Products.Clear();

                foreach (var orderProduct in order.Products)
                {
                    var productHomeViewModel = orderProduct.Product.To<ProductHomeViewModel>();

                    orderHomeViewModel.Products.Add(productHomeViewModel);
                }
            }

            return this.View(orderHomeViewModel);
        }
    }
}