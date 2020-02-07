using SIS.MvcFramework;
using SIS.MvcFramework.Result;
using IRunes.App.ViewModels.Users;
using SIS.MvcFramework.Attributes.Http;

namespace IRunes.App.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet(Url = "/")]
        public IActionResult IndexSlash()
        {
            return Index();
        }

        public IActionResult Index()
        {
            if (this.IsLoggedIn())
            {
               

                return this.View(new UserHomeViewModel {Username = this.User.Username }, "Home");
            }

            return this.View();
        }
    }
}
