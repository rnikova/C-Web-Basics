using System.Linq;
using System.Text;
using IRunes.Models;
using IRunes.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Result;
using System.Security.Cryptography;
using SIS.MvcFramework.Attributes.Http;
using SIS.MvcFramework.Attributes.Action;

namespace IRunes.App.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [NonAction]
        private string HashPassword(string password)
        {
            using (SHA256 sha256hash = SHA256.Create())
            {
                return Encoding.UTF8.GetString(sha256hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        public ActionResult Login()
        {
            return this.View();
        }

        [HttpPost(ActionName = "Login")]
        public ActionResult LoginConfirm()
        {
            string username = this.Request.FormData["username"].FirstOrDefault();
            string password = this.Request.FormData["password"].FirstOrDefault();

            User userFromDb = this.userService.GetUserByUsernameAndPassword(username, this.HashPassword(password));

            if (userFromDb == null)
            {
                return this.Redirect("/Users/Login");
            }

            this.SignIn(userFromDb.Id, userFromDb.Username, userFromDb.Email);

            return this.Redirect("/");
        }

        public ActionResult Register()
        {
            return this.View();
        }

        [HttpPost(ActionName = "Register")]
        public ActionResult RegisterConfirm()
        {

            string username = (this.Request.FormData["username"]).FirstOrDefault();
            string password = (this.Request.FormData["password"]).FirstOrDefault();
            string confirmPassword = (this.Request.FormData["confirmPassword"]).FirstOrDefault();
            string email = (this.Request.FormData["email"]).FirstOrDefault();

            if (password != confirmPassword)
            {
                return this.Redirect("/Users/Register");
            }

            User user = new User
            {
                Username = username,
                Password = this.HashPassword(password),
                Email = email
            };

            this.userService.CreateUser(user);

            return this.Redirect("/Users/Login");
        }

        public ActionResult Logout()
        {
            this.SingOut();

            return this.Redirect("/");
        }
    }
}