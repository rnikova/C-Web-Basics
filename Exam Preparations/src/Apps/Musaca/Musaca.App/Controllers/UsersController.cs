using Musaca.App.BindingModels.Users;
using Musaca.App.ViewModels.Orders;
using Musaca.App.ViewModels.Users;
using Musaca.Models;
using Musaca.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Action;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SIS.MvcFramework.Mapping;

namespace Musaca.App.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IOrdersService ordersService;

        public UsersController(IUsersService usersService, IOrdersService ordersService)
        {
            this.usersService = usersService;
            this.ordersService = ordersService;
        }

        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.Redirect("/Users/Login");
            }

            var user = this.usersService.GetUserByUsernameAndPassword(model.Username, model.Password);

            if (user == null)
            {
                return this.Redirect("/Users/Login");
            }

            this.SignIn(user.Id, user.Username, user.Email);

            return this.Redirect("/");
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(UserRegisterBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Users/Register");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.Redirect("/Users/Register");
            }

            var user = new User
            {
                Username = model.Username,
                Password = this.HashPassword(model.Password),
                Email = model.Email
            };

            this.usersService.CreateUser(user);
            this.ordersService.CreateOrder(new Order { CashierId = user.Id });

            return this.Redirect("/Users/Login");
        }

        [Authorize]
        public IActionResult Logout()
        {
            this.SignOut();

            return this.Redirect("/");
        }

        [NonAction]
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return Encoding.UTF8.GetString(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        public IActionResult Profile()
        {
            var userProfileViewModel = new UserProfileViewModel();
            var ordersFromDb = this.ordersService.GetAllCompletedOrdersByCashierId(this.User.Id);

            userProfileViewModel.Orders = ordersFromDb
                .To<OrderProfileViewModel>()
                .ToList();

            foreach (var order in userProfileViewModel.Orders)
            {
                order.CashierName = this.User.Username;

                order.Total = ordersFromDb.Where(orderF => orderF.Id == order.Id)
                    .SelectMany(orderF => orderF.Products)
                    .Sum(pr => pr.Product.Price)
                    .ToString();

                order.IssuedOnDate = ordersFromDb.SingleOrDefault(orderF => orderF.Id == order.Id).IssuedOn
                    .ToString("dd/MM/yyyy");
            }

            return this.View(userProfileViewModel);
        }
    }
}
