using Musaca.App.BindingModels.Products;
using Musaca.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using SIS.MvcFramework.Mapping;
using Musaca.Models;
using System.Linq;
using Musaca.App.ViewModels.Products;

namespace Musaca.App.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService productsService;
        private readonly IOrdersService ordersService;

        public ProductsController(IProductsService productsService, IOrdersService ordersService)
        {
            this.productsService = productsService;
            this.ordersService = ordersService;
        }

        public IActionResult All()
        {
            return this.View(this.productsService.GetAll().To<ProductsAllViewModel>().ToList());
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(ProductCreateBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View();
            }

            this.productsService.CreateProduct(model.To<Product>());

            return this.Redirect("All");
        }

        [HttpPost]
        public IActionResult Order(ProductsOrderBindingModel productOrderBindingModel)
        {
            var productToOrder = this.productsService.GetByName(productOrderBindingModel.Product);

            this.ordersService.AddProductToCurrentActiveOrder(productToOrder.Id, this.User.Id);

            return this.Redirect("/");
        }
    }
}
